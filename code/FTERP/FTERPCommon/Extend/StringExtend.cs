//*****************************************************************
//
// File Name:   StringExtend
//
// Description: 字符串扩展方法
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
    public static class StringExtend
    {
        /// <summary>
        /// 生成32位MD5值
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static string MD5(this string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// 生成变异MD5
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns>加密后的MD5串</returns>
        public static String VariationMd5(this string password)
        {
            password = password.MD5();
            password = password.Substring(0, 14).MD5() + password.Substring(14);
            return password;
        }

        /// <summary>
        /// 字符串截取
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string SubStr(this string value, int length, string ext = "")
        {
            //扩展方法不会出现null
            if (null == value)
                return string.Empty;
            StringBuilder tmp = new StringBuilder();
            double c = 0.5;
            int strLen = value.Length;
            for (int i = 0; i < strLen; i++)
            {
                char ch = value[i];
                int t = (int)ch;
                if (t >= 0 && t <= 128)
                {
                    c += 0.5;
                }
                else
                {
                    c += 1;
                }
                if (c > length)
                {
                    tmp.Append(ext);
                    break;
                }
                tmp.Append(ch);
            }

            return tmp.ToString();
        }
    }
}
