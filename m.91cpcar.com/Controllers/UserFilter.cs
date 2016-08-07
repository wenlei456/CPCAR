using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace m.cpcar.com.Controllers.master
{
    /// <summary>
    /// 管理员
    /// </summary>
    public class UserFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //获取出现异常的controller名和action名，用于记录
            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];
            Dictionary<string, object> objs = (Dictionary<string, object>)filterContext.ActionParameters;

            // 如果用户没有登陆
            if (filterContext.HttpContext.Session["memberID"] == null)
            {
                if (objs.Count>0)
                {
                    filterContext.HttpContext.Session[controllerName + actionName] = objs;
                }
                else
                {
                    filterContext.HttpContext.Session[controllerName + actionName] = null;
                }

                if (controllerName == "Order" && actionName == "goOrder")
                {
                    filterContext.HttpContext.Session["ControllerUrl"] = "Product";
                    filterContext.HttpContext.Session["ActionUrl"] = "GetCar";
                } if (controllerName == "Order" && actionName == "Detail")
                {
                    filterContext.HttpContext.Session["ControllerUrl"] = "Member";
                    filterContext.HttpContext.Session["ActionUrl"] = "MyOrder";
                }
                else
                {
                    filterContext.HttpContext.Session["ControllerUrl"] = controllerName;
                    filterContext.HttpContext.Session["ActionUrl"] = actionName;
                }
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Member", Action = "Login" }));//这里是跳转到Member下的Login
            }
             
        }

    }
}