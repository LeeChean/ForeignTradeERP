using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace FTERPWeb.Common
{
    public class ImgHelper
    {
        #region 验证码图片相关方法

        /// <summary>
        /// 获取随机英文字符数字验证码
        /// </summary>
        /// <param name="validateLength"></param>
        /// <returns></returns>
        public static string GetRandomCharNumberString(int validateLength)
        {
            string vchar = "23456789ABCDFGHJKMNPPQRSTUVWXYZ";
            string vnum = "";
            System.Random rand = new Random();
            for (int i = 0; i < validateLength; i++)
            {
                int t = rand.Next(vchar.Length);
                vnum += vchar[t];
            }
            return vnum;
        }

        /// <summary>
        /// 根据验证字符串生成图象
        /// </summary>
        /// <param name="strValidateCode"></param>
        public static MemoryStream CreateImage(string strValidateCode)
        {
            int iImageWidth = strValidateCode.Length * 14;
            Random newRandom = new Random();
            //  图高20px
            Bitmap theBitmap = new Bitmap(iImageWidth, 36);
            Graphics theGraphics = Graphics.FromImage(theBitmap);

            //  白色背景
            theGraphics.Clear(Color.AliceBlue);

            //画图片的背景噪音线5条
            for (int i = 0; i < 5; i++)
            {
                int x1 = newRandom.Next(theBitmap.Width);
                int x2 = newRandom.Next(theBitmap.Width);
                int y1 = newRandom.Next(theBitmap.Height);
                int y2 = newRandom.Next(theBitmap.Height);
                //用银色画出噪音线
                theGraphics.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            //画图片的前景噪音点10个
            for (int i = 0; i < 10; i++)
            {
                int x = newRandom.Next(theBitmap.Width);
                int y = newRandom.Next(theBitmap.Height);
                theBitmap.SetPixel(x, y, Color.FromArgb(newRandom.Next()));
            }
            //画图片的框线
            theGraphics.DrawRectangle(new Pen(Color.SaddleBrown), 0, 0, 0, 0);
            ////获取系统已经安装的字体
            //InstalledFontCollection MyFont = new InstalledFontCollection();
            //FontFamily[] MyFontFamilies = MyFont.Families;
            //Font theFont = null;//new Font() //new Font("Arial", 10);
            //if (MyFontFamilies.Length > 0)
            //{
            //    FontFamily ff = MyFontFamilies[new Random().Next(MyFontFamilies.Length)];
            //    theFont = new Font(ff, 12);
            //}
            //else
            //{
            Font theFont = new Font("Arial", 12);
            // }
            for (int iindex = 0; iindex < strValidateCode.Length; iindex++)
            {
                string strchar = strValidateCode.Substring(iindex, 1);
                Brush newBrush = new SolidBrush(GetRandomColor());
                //1 + newRandom.Next(3)
                Point thePos = new Point(iindex * 13 + 1 + newRandom.Next(3), 10);
                theGraphics.DrawString(strchar, theFont, newBrush, thePos);
            }

            //  将生成的图片发回客户端
            MemoryStream ms = new MemoryStream();
            theBitmap.Save(ms, ImageFormat.Png);
            return ms;
        }

        /// <summary>
        /// 生成随机颜色
        /// </summary>
        /// <returns></returns>
        private static Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            //  对于C#的随机数
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            //  为了在白色背景上显示，尽量生成深色
            int iRed = RandomNum_First.Next(256);
            int iGreen = RandomNum_Sencond.Next(256);
            int iBlue = (iRed + iGreen > 400) ? 0 : 400 - iRed - iGreen;
            iRed = (iRed > 120) ? 120 : iRed;
            iGreen = (iGreen > 120) ? 120 : iGreen;
            iBlue = (iBlue > 120) ? 120 : iBlue;
            return Color.FromArgb(iRed, iGreen, iBlue);
        }

        #endregion
    }
}