//*****************************************************************
//
// File Name:   SysConfig
//
// Description: 系统配置信息
//
// Coder:       LeeChean
//
// Date:        2015-04-15
//
// History:     1、2015-04-15 Create by LeeChean
//   
//*****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using FTERPWeb.Models;

namespace FTERPWeb.Common
{
    public static class SysConfig
    {
        /// <summary>
        /// 当前登录用户的信息
        /// </summary>
        public static UserModel CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session[UserInfo] != null)
                {
                    return HttpContext.Current.Session[UserInfo] as UserModel;
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session[UserInfo] = value;
            }
        }

        /// <summary>
        /// 当前登录用户所属部门
        /// </summary>
        public static string CurrentDepartment
        {
            get
            {
                DepartmentModel model = DepartmentModel.SingleOrDefault(CurrentUser.Departmentid);
                return model == null ? "" : model.FullName;
            }
        }

        /// <summary>
        /// 当前用户的角色数组
        /// </summary>
        public static List<RoleModel> CurrentRoleInfos
        {
            get
            {
                return HttpContext.Current.Session[RoleInfo] as List<RoleModel> ?? new List<RoleModel>();
            }
            set
            {
                HttpContext.Current.Session[RoleInfo] = value;
            }
        }

        /// <summary>
        /// 当前用户的所有角色Id
        /// </summary>
        public static string CurrentRoleIds
        {
            get
            {
                return string.Join(",", CurrentRoleInfos.Select(s => s.Id).ToArray());
            }
        }

        /// <summary>
        /// 当前用户的所有角色名称
        /// </summary>
        public static string CurrentRoleNames
        {
            get
            {
                return string.Join(",", CurrentRoleInfos.Select(s => s.Name).ToArray());
            }
        }

        /// <summary>
        /// 当前用户的所有模块权限
        /// </summary>
        public static List<FuncModel> CurrentAuthInfo
        {
            get
            {
                return HttpContext.Current.Session[AuthInfo] as List<FuncModel> ?? new List<FuncModel>();
            }
            set
            {
                HttpContext.Current.Session[AuthInfo] = value;
            }
        }

        /// <summary>
        /// 当前用户的所有部门权限
        /// </summary>
        public static string CurrentDepAuthInfo
        {
            get
            {
                return HttpContext.Current.Session[DepAuthInfo].ToString();
            }
            set
            {
                HttpContext.Current.Session[DepAuthInfo] = value;
            }
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            if (Array.IndexOf(System.Web.HttpContext.Current.Request.ServerVariables.AllKeys, "HTTP_X_Real_IP") == -1)
            {
                return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            else
            {
                return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_Real_IP"].ToString();
            }
        }

        /// <summary>
        /// 是否记录日志
        /// </summary>
        public static bool IsLog = bool.Parse(ConfigurationManager.AppSettings["IsLog"]);

        /// <summary>
        /// 资源服务器地址
        /// </summary>
        public static string ResourceServer = ConfigurationManager.AppSettings["ResourceServer"];

        /// <summary>
        /// 资源服务器版本戳
        /// </summary>
        public static string StaticToken = ConfigurationManager.AppSettings["StaticToken"];

        /// <summary>
        /// 每页条数
        /// </summary>
        public static int ItemsPerPage = int.Parse(ConfigurationManager.AppSettings["ItemsPerPage"]);

        /// <summary>
        /// 序列号类别
        /// </summary>
        public struct SerialNo
        {
            public const int ClientNo = 1;
            public const int SupplierNo = 2;
            public const int CommodityNo = 3;
        }

        #region Session相关

        /// <summary>
        /// 用户权限Session
        /// </summary>
        public const string AuthInfo = "AuthInfo";

        /// <summary>
        /// 用户权限Session
        /// </summary>
        public const string DepAuthInfo = "DepAuthInfo";

        /// <summary>
        /// session
        /// 存放用户信息的key
        /// </summary>
        public const string UserInfo = "UserInfo";

        /// <summary>
        /// session中验证码
        /// </summary>
        public const string VerifCodeKey = "VerifCode";

        /// <summary>
        /// session存放角色信息的key
        /// </summary>
        public const string RoleInfo = "RoleInfo";

        #endregion
    }
}