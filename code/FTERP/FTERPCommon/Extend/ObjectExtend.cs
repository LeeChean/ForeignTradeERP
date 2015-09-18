//*****************************************************************
//
// File Name:   ObjectExtend
//
// Description: Object扩展方法
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
using System.Text;

namespace FTERPCommon
{
    public static class ObjectExtend
    {
        public static DateTime ToDateTime(this object value)
        {
            DateTime dt;
            bool iRes = DateTime.TryParse(value.ToString(), out dt);
            if (!iRes)
            {
                dt = new DateTime();
            }
            return dt;
        }

        public static int ToInt(this object value)
        {
            int iTemp;
            if (null == value)
            {
                return 0;
            }
            bool iRes = int.TryParse(value.ToString(), out iTemp);
            if (!iRes)
            {
                iTemp = 0;
            }
            return iTemp;
        }

        public static long ToLong(this object value)
        {
            long lTemp;
            if (null == value)
            {
                return 0;
            }
            bool bRes = long.TryParse(value.ToString(), out lTemp);
            if (!bRes)
            {
                lTemp = 0;
            }
            return lTemp;
        }

        public static float ToFloat(this object value)
        {
            float fTemp;
            if (null == value)
            {
                return 0;
            }
            bool iRes = float.TryParse(value.ToString(), out fTemp);
            if (!iRes)
            {
                fTemp = 0;
            }
            return fTemp;
        }

        public static double ToDouble(this object value)
        {
            double dTemp;
            if (null == value)
            {
                return 0;
            }
            bool bRes = double.TryParse(value.ToString(), out dTemp);
            if (!bRes)
            {
                dTemp = 0;
            }
            return dTemp;
        }

        public static decimal ToDecimal(this object value)
        {
            decimal dTemp;
            if (null == value)
            {
                return 0;
            }
            bool bRes = decimal.TryParse(value.ToString(), out dTemp);
            if (!bRes)
            {
                dTemp = 0;
            }
            return dTemp;
        }

        public static bool ToBool(this object value)
        {
            bool dTemp;
            if (null == value)
            {
                return false;
            }
            bool bRes = bool.TryParse(value.ToString(), out dTemp);
            if (!bRes)
            {
                dTemp = false;
            }
            return dTemp;
        }
    }
}
