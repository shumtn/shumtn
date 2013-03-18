using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Shu.Util
{
    /// <summary>
    /// 缩略图操作
    /// </summary>
    public class Breviary
    {
        #region 处理函数
        private static Size NewSize(int maxWidth, int maxHeight, int width, int height)
        {
            double w = 0.0;
            double h = 0.0;
            double sw = Convert.ToDouble(width);
            double sh = Convert.ToDouble(height);
            double mw = Convert.ToDouble(maxWidth);
            double mh = Convert.ToDouble(maxHeight);
            if (sw < mw && sh < mh)
            {
                w = sw;
                h = sh;
            }
            else if ((sw / sh) > (mw / mh))
            {
                w = maxWidth;
                h = (w * sh) / sw;
            }
            else
            {
                h = maxHeight;
                w = (h * sw) / sh;
            }
            return new Size(Convert.ToInt32(w), Convert.ToInt32(h));
        }
        #endregion

        #region 生成小图
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="pathpic">图片数据流</param>
        /// <param name="newspath">存储路径</param>
        /// <param name="newsname">新图片名</param>
        /// <param name="maxwidth">最大宽度</param>
        /// <param name="maxheight">最大高度</param>
        public static void SmallImage(Stream pathpic, string newspath, string newsname, int maxwidth, int maxheight)
        {
            if (!Directory.Exists(newspath))
            {
                Directory.CreateDirectory(newspath);
            }
            System.Drawing.Image img = System.Drawing.Image.FromStream(pathpic);
            System.Drawing.Imaging.ImageFormat thisFormat = img.RawFormat;

            Size newSize = NewSize(maxwidth, maxheight, img.Width, img.Height);
            Bitmap outBmp = new Bitmap(newSize.Width, newSize.Height);
            Graphics g = Graphics.FromImage(outBmp);

            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(img, new Rectangle(0, 0, newSize.Width, newSize.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
            g.Dispose();

            // 以下代码为保存图片时，设置压缩质量
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICI = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[x];//设置JPEG编码
                    break;
                }
            }

            if (jpegICI != null)
            {
                outBmp.Save(newspath + newsname, jpegICI, encoderParams);

            }
            else
            {
                outBmp.Save(newspath + newsname, thisFormat);
            }
            img.Dispose();
            outBmp.Dispose();
        }
        #endregion

        #region 生成大图
        public static void ToM(string PathPic, string NewPic, int maxWidth, int maxHeight, string Copyright, int Left, int Right, string StrFont, int FontSize)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(PathPic);
            System.Drawing.Imaging.ImageFormat thisFormat = img.RawFormat;

            Size newSize = NewSize(maxWidth, maxHeight, img.Width, img.Height);
            Bitmap outBmp = new Bitmap(newSize.Width, newSize.Height);
            Graphics g = Graphics.FromImage(outBmp);

            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(img, new Rectangle(0, 0, newSize.Width, newSize.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
            g.DrawString(Copyright, new Font(StrFont, FontSize, System.Drawing.FontStyle.Regular), new SolidBrush(Color.FloralWhite), Left, Right);
            g.Dispose();

            // 以下代码为保存图片时，设置压缩质量
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICI = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[x];//设置JPEG编码
                    break;
                }
            }

            if (jpegICI != null)
            {
                outBmp.Save(NewPic, jpegICI, encoderParams);
            }
            else
            {
                outBmp.Save(NewPic, thisFormat);
            }
            img.Dispose();
            outBmp.Dispose();
        }
        #endregion
    }
}