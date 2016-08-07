using Cparts_Service;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CParts_Offline.Controllers.master
{
    public class MasterMemberController : Controller
    {
        //
        // GET: /member/
        #region 会员等级
        
        public ActionResult memberLevel()
        {
            MemberLevelBll bll = new MemberLevelBll();
            List<MemberLevel> list= bll.GetList();
           // MemberLevel bll = new MemberLevel();
            return View(list);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(DAO.MemberLevel car)
        {
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            //添加汽车
            MemberLevelBll bll = new MemberLevelBll();
            bool r = bll.Add(car);
            if (r)
            {
                //map.Add("result", "ok");
            }
            else
            {
                //map.Add("result", "error");
            }
            return RedirectToAction("memberLevel");
           
        }
        /// <summary>
        /// 修改汽车
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(DAO.MemberLevel car)
        {
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            //修改汽车
            MemberLevelBll bll = new MemberLevelBll();
            bool r = bll.update(car);
            return RedirectToAction("memberLevel");
        }
        /// <summary>
        /// 删除汽车
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Del(string id)
        {
            //删除汽车
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            MemberLevelBll carbll = new MemberLevelBll();
            bool r = carbll.Del(id);
            if (r)
            {
                map.Add("result", "ok");
            }
            else
            {
                map.Add("result", "error");
            }
            return Json(map, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 会员管理
        public ActionResult memberManage()
        {
            return View();
        }
        #endregion

    }
}
