using DAO;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;

namespace AOP
{
    public class AopProxy : RealProxy
    {

        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AopProxy(Type serverType)
            : base(serverType) { }

        public override IMessage Invoke(IMessage msg)
        {
            DateTime startTime = DateTime.Now;//方法启动时间
            //记录方法返回结果
            string returnvalue = "null";
            //Model.sys_AccessLog log = new Model.sys_AccessLog();
            ////log.Uri = msg.Properties["__Uri"].ToString();
            //log.IP = Safe.Instance().ClientIp();
            //log.Mac = Safe.Instance().ClientPort();
            //log.MethodName = msg.Properties["__MethodName"].ToString();
            //log.MethodSignature = msg.Properties["__MethodSignature"].ToString();
            //log.TypeName = msg.Properties["__TypeName"].ToString();
            //log.CallContext = msg.Properties["__CallContext"].ToString();
            //log.Project = "XAUTOA";
            object returnIMessage = null;
            
            if (msg is IConstructionCallMessage)
            {
                //构造函数调用
                IConstructionCallMessage constructCallMsg = msg as IConstructionCallMessage;
                IConstructionReturnMessage constructionReturnMessage = this.InitializeServerObject((IConstructionCallMessage)msg);
                RealProxy.SetStubData(this, constructionReturnMessage.ReturnValue);
                //Console.WriteLine("Call constructor");
                object[] args = constructCallMsg.Args;
                for (int i = 0; i < args.Length; i++)
                {
                    log.Debug("__Args:" + args[i].ToString());
                }
                returnIMessage = constructionReturnMessage;
            }
            else
            {
                log.Debug("__MethodName:" + msg.Properties["__MethodName"].ToString());
                log.Debug("__MethodSignature:" + msg.Properties["__MethodSignature"].ToString());
                log.Debug("__TypeName:" + msg.Properties["__TypeName"].ToString());
                log.Debug("__CallContext:" + msg.Properties["__CallContext"].ToString());
                
                //非构造函数调用
                IMethodCallMessage callMsg = msg as IMethodCallMessage;
                IMessage message;
                CarPartsEntities db = new CarPartsEntities();
                
                DbConnection con = null;
                DbTransaction tran = null;

                try
                {
                    object[] args = callMsg.Args;
                    #region 记录方法输入参数
                    //log.IP = args[args.Length-2].ToString();
                    //log.Mac = args[args.Length - 1].ToString();
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i] != null)
                        {
                            log.Debug("__Args:" + args[i].ToString());
                        }

                    }
                    #endregion

                    //BaseBusiness bb = (BaseBusiness)GetUnwrappedServer();
                    BaseBusiness bb = new BaseBusiness();
                    bb.db = db;

                    //打开数据库连接
                    con = ((IObjectContextAdapter)db).ObjectContext.Connection;
                    con.Open();
                    //开启事务
                    tran = con.BeginTransaction();

                    object o = callMsg.MethodBase.Invoke(GetUnwrappedServer(), args);
                    message = new ReturnMessage(o, args, args.Length, callMsg.LogicalCallContext, callMsg);

                    //业务逻辑处理完毕，提交事务
                    db.SaveChanges();
                    tran.Commit();
                }              
                catch (Exception e)
                {
                    //发生异常，事务回滚
                    tran.Rollback();
                    message = new ReturnMessage(e, callMsg);
                    returnvalue = e.InnerException.ToString();
                }
                finally
                {
                    //关闭数据库连接
                    con.Close();
                }
                if (message.Properties["__Return"] != null)
                {
                    returnvalue = message.Properties["__Return"].ToString();
                }
                log.Info("__ReturnValue:" + returnvalue);
                //Console.WriteLine(returnvalue);
                returnIMessage = message;

            }
            DateTime endTime = DateTime.Now;//方法结束时间
            log.Debug("__UseTime:" + (endTime - startTime).ToString());
            //SysLogService.GetInstance().InsertAccessLog(log);//写入日志到数据库
            log.Debug(string.Format("{0}:{1}", msg.Properties["__MethodName"].ToString(), returnvalue));//写入日志到本地文件
            return returnIMessage as IMessage;
        }
    }
}
