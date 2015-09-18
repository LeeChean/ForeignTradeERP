using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FTERPWeb.Common;
using System.IO;
using PetaPoco;
using System.Web.Routing;
using System.Text;
using FTERPWeb.Models;

namespace System.Web.Mvc
{
    public static class HtmlExtend
    {
        /// <summary>
        /// 生成脚本引用html
        /// </summary>
        /// <param name="html"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static HtmlString StaticFor(this HtmlHelper html, string path, bool auto = true)
        {
            string url = SysConfig.ResourceServer + path + "?token=" + SysConfig.StaticToken;
            string ext = Path.GetExtension(path);
            string tmp = url;
            if (auto)
            {
                if (".css".Equals(ext, StringComparison.CurrentCultureIgnoreCase))
                {
                    tmp = "<link href=\"" + url + "\" type=\"text/css\" rel=\"stylesheet\" />";
                }
                else if (".js".Equals(ext, StringComparison.CurrentCultureIgnoreCase))
                {
                    tmp = "<script language=\"JavaScript\" src=\"" + url + "\" type=\"text/javascript\"></script>";
                }
            }
            return new HtmlString(tmp);
        }

        /// <summary>
        /// 分页控件 上一页 1 2 3 4 下一页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="page"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public static HtmlString RenderHomePage<T>(this HtmlHelper html, Page<T> page, string area = "Home") where T : new()
        {
            RouteValueDictionary vs = html.ViewContext.RouteData.Values;
            var queryString = html.ViewContext.HttpContext.Request.QueryString;
            foreach (string key in queryString.Keys)
                if (queryString[key] != null && !string.IsNullOrEmpty(key))
                    vs[key] = queryString[key];

            var formString = html.ViewContext.HttpContext.Request.Form;
            foreach (string key in formString.Keys)
                vs[key] = formString[key];
            vs.Remove("CurrentPage");

            var builder = new StringBuilder();
            var classDict = new Dictionary<string, object>();

            builder.Append("<div class=\"m-page m-page-sr m-page-sm\">");

            vs.Add("CurrentPage", "");

            vs["CurrentPage"] = (page.CurrentPage - 1) > 0 ? (page.CurrentPage - 1) : 1;
            //如果当前是第一页 上一页不可用
            if (page.CurrentPage == 1)
            {
                builder.AppendFormat("<a href=\"javascript:;\" class=\"{0}\"><span class=\"pagearr\">&lt;</span>上一页</a>", "first pageprv z-dis");
            }
            else
            {
                //上一页不是第一页 上一页的url参数
                var list = (from o in vs where o.Key != "controller" && o.Key != "action" select o.Key + "=" + o.Value).ToList();
                builder.AppendFormat("<a href=\"/{0}?{1}\" class=\"{2}\"><span class=\"pagearr\">&lt;</span>上一页</a>"
                    , string.Join("/", new[] { area, vs["controller"].ToString(), vs["action"].ToString() })
                    , string.Join("&", list)
                    , "first pageprv");
            }

            classDict["class"] = "";
            int i = 0;
            //如果本页到第一页的距离小于3 生成1 2 3 4格式的页码
            if (page.CurrentPage - 3 > 1)
            {
                vs["CurrentPage"] = 1;
                builder.AppendFormat(Html.LinkExtensions.ActionLink(html, "1", vs["action"].ToString(), vs, classDict).ToString());
                builder.Append("<i>...</i>");

                for (i = (int)page.CurrentPage - 2; i <= page.CurrentPage; i++)
                {
                    vs["CurrentPage"] = i;
                    if (i == page.CurrentPage)
                    {
                        builder.AppendFormat("<a href=\"javascript:;\"  class=\"z-crt\">{0}</a>", i);
                    }
                    else
                    {
                        classDict["class"] = "";
                        builder.AppendFormat(Html.LinkExtensions.ActionLink(html, i.ToString(), vs["action"].ToString(), vs, classDict).ToString());
                    }
                }
            }
            //如果本页到第一页的距离大于3 生成1 ... 3 4 5格式的页码
            else
            {
                for (i = 1; i <= page.CurrentPage; i++)
                {
                    vs["CurrentPage"] = i;
                    //如果是当前页面 当前页不可以点中
                    if (i == page.CurrentPage)
                    {
                        builder.AppendFormat("<a href=\"javascript:;\" class=\"z-crt\">{0}</a>", i);
                    }
                    //如果不是当前页面 可以选中
                    else
                    {
                        classDict["class"] = "";
                        builder.AppendFormat(Html.LinkExtensions.ActionLink(html, i.ToString(), vs["action"].ToString(), vs, classDict).ToString());
                    }
                }
            }

            classDict["class"] = "";
            //如果当前页距离尾页大于3 生成 5 6 7 ... 10的样式
            if (page.CurrentPage + 3 < page.TotalPages)
            {
                for (i = (int)(page.CurrentPage + 1); i < page.CurrentPage + 3; i++)
                {
                    vs["CurrentPage"] = i;
                    builder.AppendFormat(Html.LinkExtensions.ActionLink(html, i.ToString(), vs["action"].ToString(), vs, classDict).ToString());
                }
                builder.Append("<i>...</i>");
                vs["CurrentPage"] = page.TotalPages;
                builder.Append(
                    Html.LinkExtensions.ActionLink(html, page.TotalPages.ToString(), vs["action"].ToString(), vs, classDict)
                        .ToString());
            }
            //如果当前页到尾页小于3 生成 5 6 7 8的样式
            else
            {
                for (i = (int)(page.CurrentPage + 1); i <= page.TotalPages; i++)
                {
                    vs["CurrentPage"] = i;
                    builder.AppendFormat(Html.LinkExtensions.ActionLink(html, i.ToString(), vs["action"].ToString(), vs, classDict).ToString());
                }
            }

            vs["CurrentPage"] = (page.CurrentPage + 1) < page.TotalPages ? (page.CurrentPage + 1) : page.TotalPages;
            //如果没有数据的情况下 下一页不可用
            if (page.CurrentPage == page.TotalPages || page.TotalPages == 0 || page.TotalPages == 1)
            {
                builder.AppendFormat("<a href=\"javascript:;\" class=\"{0}\">下一页<span class=\"pagearr\">&gt;</span></a>", "last pagenxt z-dis");
            }
            else
            {
                //生成下一页的参数
                var list = (from o in vs where o.Key != "controller" && o.Key != "action" select o.Key + "=" + o.Value).ToList();
                builder.AppendFormat("<a href=\"/{0}?{1}\" class=\"{2}\">下一页<span class=\"pagearr\">&gt;</span></a>"
                    , string.Join("/", new[] { area, vs["controller"].ToString(), vs["action"].ToString() })
                    , string.Join("&", list)
                    , "last pagenxt");
            }

            builder.Append("</div>");

            return new HtmlString(builder.ToString());
        }

        /// <summary>
        /// 生成后台页面的二级面包屑导航 by姜正午
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString RenderNavHtml(this HtmlHelper helper)
        {
            //从Session中读取当前用户拥有权限的
            List<FuncModel> funcList = SysConfig.CurrentAuthInfo;
            if (null == funcList)
            {
                return MvcHtmlString.Create(string.Empty);
            }
            StringBuilder stringBuilder = new StringBuilder("<div class=\"m-tt\">");
            RouteValueDictionary vd = helper.ViewContext.RouteData.Values;
            string action = vd["action"].ToString().ToLower();
            string controller = vd["controller"].ToString().ToLower();
            var func = funcList.FirstOrDefault(s => s.Name.ToLower() == controller);
            var group = funcList.FirstOrDefault(f => f.Id == func.Pid.ToString());
            if (null == func)
            {
                stringBuilder.Append("</div>");
                return MvcHtmlString.Create(stringBuilder.ToString());
            }
            //stringBuilder.AppendFormat("{0}", group.Title);
            var node = funcList.FirstOrDefault(s => "" + s.Pid == func.Id && s.Name.ToLower() == action);
            if (null == node)
            {
                stringBuilder.Append("</div>");
                return MvcHtmlString.Create(stringBuilder.ToString());
            }
            //显示两级目录
            if ("Index".Equals(action, StringComparison.CurrentCultureIgnoreCase))
            {
                stringBuilder.AppendFormat("{0}&gt{1}&gt{2}", group.Title, func.Title, node.Title);
            }
            else
            {
                stringBuilder.AppendFormat("{0}&gt{1}&gt{2}", group.Title, func.Title, node.Title);
            }
            stringBuilder.Append("</div>");
            return MvcHtmlString.Create(stringBuilder.ToString());
        }
    }
}