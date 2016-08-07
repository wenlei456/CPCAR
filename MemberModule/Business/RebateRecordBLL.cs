using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtils;
using AOP;
namespace MemberModule.Business
{
    public class RebateRecordBLL:BaseBusiness
    {
        /// <summary>
        /// 给上级返利
        /// </summary>
        /// <param name="cuid"></param>
        /// <param name="puid"></param>
        /// <param name="orderNumber"></param>
        /// <param name="orderPrice"></param>
        /// <returns></returns>
        public bool GiveParentRebate(string cuid,string cuName,int puid,string orderNumber, decimal orderPrice)
        { 
            //MemberBLL mbBll=new MemberBLL ();
            //DAO.MemberBase mb= mbBll.GetUserByID(pUid.ToString());

            MemberLevelBLL mlBll=new MemberLevelBLL ();
            DAO.MemberLevel ml= mlBll.GetLeveByUid(puid.ToString());//上级会员等级

            DAO.RebateRecord rr=new DAO.RebateRecord ();
            rr.UID=puid;
            rr.Price = decimal.Parse(ConfigurationApp.PV) * orderPrice * ml.Income;
            rr.Source = "推广返利";
            rr.PType = "收入";
            rr.LowerUID = int.Parse(cuid);
            rr.LowerOrder = orderNumber;
            rr.OrderPrice = orderPrice;
            rr.Datetime = DateTime.Now;
            rr.Status = 0;
            rr.InCome = ml.Income;
            rr.LowerLoginName = cuName;
            if (orderPrice > 0 && rr.Price > 0)
            {
                db.RebateRecord.Add(rr);
                if (db.SaveChanges() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else {
                return false;
            }
        }
        /// <summary>
        /// 获取返利记录
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<DAO.RebateRecord> GetRebateRecordList(string uid)
        {
            string sql = "SELECT * FROM [RebateRecord] where Uid={0} ";
            List<DAO.RebateRecord> expList = db.Database.SqlQuery<DAO.RebateRecord>(string.Format(sql, uid)).ToList();
            return expList;
        }
    }
}
