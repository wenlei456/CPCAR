using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
//分页
public static class ExtMethod
{
    //string SelectByCate,int pageIndex=1
    public static HtmlString ShowPageNavigate(this HtmlHelper htmlHelper, int currentPage, int pageSize, int totalCount)
    {
        var redirectTo = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
        pageSize = pageSize == 0 ? 3 : pageSize;
        var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数
        var output = new StringBuilder();
        if (totalPages > 1)
        {
            output.AppendFormat("<a class='pageLink' href='{0}?pageIndex=1'>首页</a> ", redirectTo);
            if (currentPage > 1)
            {//处理上一页的连接
                output.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}'>上一页</a> ", redirectTo, currentPage - 1);
            }

            output.Append(" ");
            int currint = 5;
            for (int i = 0; i <= 10; i++)
            {//一共最多显示10个页码，前面5个，后面5个
                if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                {
                    if (currint == i)
                    {//当前页处理                           
                        output.AppendFormat("<a class='cpb' href='{0}?pageIndex={1}'>{2}</a> ", redirectTo, currentPage, currentPage);
                    }
                    else
                    {//一般页处理
                        output.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}'>{2}</a> ", redirectTo, currentPage + i - currint, currentPage + i - currint);
                    }
                }
                output.Append(" ");
            }
            if (currentPage < totalPages)
            {//处理下一页的链接
                output.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}'>下一页</a> ", redirectTo, currentPage + 1);
            }

            output.Append(" ");
            if (currentPage != totalPages)
            {
                output.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}'>末页</a> ", redirectTo, totalPages);
            }
            output.Append(" ");
        }
        output.AppendFormat("<label>第{0}页 / 共{1}页</label>", currentPage, totalPages);//这个统计加不加都行

        return new HtmlString(output.ToString());
    }

    public static HtmlString navGoon(this HtmlHelper htmlHelper, int pid, List<DAO.PartsCategory> list,string className)
    {
        return new HtmlString(navGoonFor(pid, list, className));
    }
    private static string navGoonFor(int pid, List<DAO.PartsCategory> list, string className)
    {
        StringBuilder output = new StringBuilder("");
        var clist = list.Where(m => m.ParentID == pid);
        if (clist.Count() > 0)
        {
            output.Append("  <ul class='dropdown-menu'>");
            foreach (var item in clist)
            {               
                var clist2 = list.Where(m => m.ParentID == item.ID);
                if (clist2.Count() > 0)
                {
                    output.Append("<li class='dropdown-submenu'><a href='javascript:;' class='" + className + "' val='" + item.ID + "'>" + item.CategoryName + "</a>");
                    output.Append(navGoonFor(item.ID, list, className));    
                    output.Append("</li> <li class='divider'></li>");
                }
                else {
                    output.Append(" <li><a href='javascript:;' class='" + className + "' val='" + item.ID + "'>" + item.CategoryName + "</a></li> <li class='divider'></li>");
                }
              
            }
            output.Append("  </ul>");
        }
        return output.ToString();
    }
}