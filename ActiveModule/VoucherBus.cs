using AOP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActiveModule
{
    public class VoucherBus : BaseBusiness
    {
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
       public List<DAO.Voucher> GetVoucherByUid(int uid,int state)
       {
           return db.Voucher.Where(m => m.Uid == uid && m.IsState==state).ToList();          
       }
       /// <summary>
       /// 获得一个对象
       /// </summary>
       /// <param name="uid"></param>
       /// <param name="vnumber"></param>
       /// <param name="state"></param>
       /// <returns></returns>
       public DAO.Voucher GetVoucherModel(int uid,string vnumber,int state)
    {
           DateTime now=DateTime.Now;
           return db.Voucher.Where(m=>m.Uid==uid && m.VoucherNumber==vnumber && m.IsState==state && now<=m.EndTime).FirstOrDefault();
    }
       public DAO.Voucher GetVoucherModel( string vnumber, int state)
       {
           DateTime now = DateTime.Now;
           return db.Voucher.Where(m => m.VoucherNumber == vnumber && m.IsState == state ).FirstOrDefault();
       }
        /// <summary>
        /// 使用优惠券
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="vnumber"></param>
        /// <returns></returns>
        public bool UseVoucher(string vnumber)
        {
            string sql = "UPDATE [Voucher]   SET  [IsState] =1  WHERE  VoucherNumber='{0}' ";
           int i= db.Database.ExecuteSqlCommand(string.Format(sql,vnumber));
           if (i > 0)
           {
               return true;
           }
           else {
               return false;              
           }
        }
        /// <summary>
        /// 给一张注册优惠券
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool RegGiveAVoucher(int uid)
        {
            DAO.Voucher m = new DAO.Voucher();
            m.Uid = uid;
            m.EndTime = DateTime.Now.AddDays(int.Parse(CommonUtils.ConfigurationApp.regVoucherEnd));
            m.InsertTime = DateTime.Now;
            m.IsState = 0;
            m.Price = decimal.Parse(CommonUtils.ConfigurationApp.regVoucherPrice);
            m.TypeName = "注册优惠券";
            m.VoucherNumber = CommonUtils.RandomCode.GetRandomCode(8);
            db.Voucher.Add(m);
            int i = db.SaveChanges();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 给一张介绍人注册优惠券
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool RegGiveAVoucherJie(int uid)
        {
            DAO.Voucher m = new DAO.Voucher();
            m.Uid = uid;
            m.EndTime = DateTime.Now.AddDays(int.Parse(CommonUtils.ConfigurationApp.regVoucherEnd));
            m.InsertTime = DateTime.Now;
            m.IsState = 0;
            m.Price = decimal.Parse(CommonUtils.ConfigurationApp.regVoucherPriceJie);
            m.TypeName = "介绍人注册优惠券";
            m.VoucherNumber = CommonUtils.RandomCode.GetRandomCode(8);
            db.Voucher.Add(m);
            int i = db.SaveChanges();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddVoucher(DAO.Voucher model)
        {
            db.Voucher.Add(model);
           int i= db.SaveChanges();
           if (i > 0)
           {
               return true;
           }else{
               return false;
           }
        }
    }
}
