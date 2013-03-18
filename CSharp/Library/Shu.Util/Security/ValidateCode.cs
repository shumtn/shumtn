using System;
using System.IO;
using System.Drawing;

namespace Shu.Util
{
    /// <summary>
    /// 随机编码
    /// </summary>
    public class ValidateCode
    {
        #region 中文验证
        /// <summary>
        /// 中文验证
        /// </summary>
        public class Chinese
        {
            /// <summary>
            /// 中文随机码
            /// </summary>
            /// <param name="codeCount">随机码长度</param>
            /// <returns>string</returns>
            public static string RndCode(int codeCount)
            {
                string allChar = "君,洁,东,魔,兽,天,门,八,佛,达,吉,祥,如,意,万,发,事";
                string[] allCharArray = allChar.Split(',');
                string randomCode = "";
                int temp = -1;

                Random rand = new Random();
                for (int i = 0; i < codeCount; i++)
                {
                    if (temp != -1)
                    {
                        rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                    }
                    int t = rand.Next(17);
                    if (temp == t)
                    {
                        return RndCode(codeCount);
                    }
                    temp = t;
                    randomCode += allCharArray[t];
                }
                return randomCode;
            }

            /// <summary>
            /// 生成图片流
            /// </summary>
            /// <param name="checkCode">要显示的随机码</param>
            /// <returns>byte[]</returns>
            public static byte[] Image(string checkCode)
            {
                byte[] imageArray = null;
                int iwidth = (int)(checkCode.Length * 20);
                Bitmap image = new Bitmap(iwidth, 20);
                Graphics g = Graphics.FromImage(image);
                Font f = new Font("黑体", 12, FontStyle.Bold);
                Brush b = new SolidBrush(Color.Green);
                g.FillRectangle(new SolidBrush(Color.Blue), 0, 0, image.Width, image.Height);
                g.Clear(Color.WhiteSmoke);

                g.DrawString(checkCode, f, b, 0, 0);

                /*Pen blackPen = new Pen(Color.Black, 0);
                Random rand = new Random();
                for (int i=0;i<5;i++)
                {
                    int y = rand.Next(image.Height);
                    g.DrawLine(blackPen,0,y,image.Width,y);
                }*/

                MemoryStream ms = new MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                imageArray = ms.ToArray();
                g.Dispose();
                image.Dispose();

                return imageArray;
            }
        }
        #endregion

        #region 数字+字母
        /// <summary>
        /// 数字+字母
        /// </summary>
        public class Number
        {
            /// <summary>
            /// 生成随机码
            /// </summary>
            /// <param name="VcodeNum">随机码的长度</param>
            /// <returns>string</returns>
            public static string RndCode(int VcodeNum)
            {
                string Vchar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
                string[] VcArray = Vchar.Split(',');
                string VNum = ""; //由于字符串很短，就不用StringBuilder了
                int temp = -1; //记录上次随机数值，尽量避免生产几个一样的随机数

                //采用一个简单的算法以保证生成随机数的不同
                Random rand = new Random();
                for (int i = 1; i < VcodeNum + 1; i++)
                {
                    if (temp != -1)
                    {
                        rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                    }
                    int t = rand.Next(VcArray.Length);
                    if (temp != -1 && temp == t)
                    {
                        return RndCode(VcodeNum);
                    }
                    temp = t;
                    VNum += VcArray[t];
                }
                return VNum;
            }

            /// <summary>
            /// 生成图片流
            /// </summary>
            /// <param name="checkCode">要实现的随机码</param>
            /// <returns>byte[]</returns>
            public static byte[] Image(string checkCode)
            {
                byte[] imageArray = null;
                int iwidth = (int)(checkCode.Length * 15);
                Bitmap image = new Bitmap(iwidth, 22);
                Graphics g = Graphics.FromImage(image);
                g.Clear(Color.White);
                Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };//定义颜色			
                string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };//定义字体 
                Random rand = new Random();
                //随机输出噪点
                for (int i = 0; i < 50; i++)
                {
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);
                    g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 0, 0);
                }

                //输出不同字体和颜色的验证码字符
                for (int i = 0; i < checkCode.Length; i++)
                {
                    int cindex = rand.Next(7);
                    int findex = rand.Next(5);

                    //Font f = new System.Drawing.Font(font[findex], 12, System.Drawing.FontStyle.Bold);
                    Font f = new System.Drawing.Font(font[findex], 12);
                    Brush b = new System.Drawing.SolidBrush(c[cindex]);
                    int ii = 4;
                    if ((i + 1) % 2 == 0)
                    {
                        ii = 2;
                    }
                    g.DrawString(checkCode.Substring(i, 1), f, b, 3 + (i * 12), ii);
                }
                //画一个边框
                g.DrawRectangle(new Pen(Color.Black,0),0,0,image.Width-1,image.Height-1);

                //输出到浏览器
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                imageArray = ms.ToArray();
                g.Dispose();
                image.Dispose();

                return imageArray;
            }
        }
        #endregion
    }
}