using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace Shu.Util
{
    public class WaterMark
    {
        private static string _WatermarkType = null;
        private static string _WatermarkPosition = null;
        private static bool _ShowWatermark = true;
        private static string _Watermarkimgpath = null;
        private static string _WatermarkText = null;

        public static string Watermarkimgpath
        {
            get { return _Watermarkimgpath; }
            set { _Watermarkimgpath = value; }
        }

        /// <summary>
        /// 是否显示水印
        /// </summary>
        public static bool ShowWatermark
        {
            get { return _ShowWatermark; }
            set { _ShowWatermark = value; }
        }

        /// <summary>
        /// 添加文字水印
        /// </summary>
        public static string WatermarkText
        {
            get { return _WatermarkText; }
            set { _WatermarkText = value; }
        }

        /// <summary>
        /// 水印位置
        /// </summary>
        public static string WatermarkPosition
        {
            get { return _WatermarkPosition; }
            set { _WatermarkPosition = value; }
        }

        /// <summary>
        /// 水印类型
        /// </summary>
        public static string WatermarkType
        {
            get { return _WatermarkType; }
            set { _WatermarkType = value; }
        }

        ///// <summary>
        ///// 上传图片代码
        ///// </summary>
        ///// <param name="image_file">HtmlInputFile控件</param>
        ///// <param name="ImgPath">存放的文件夹绝对位置</param>
        ///// <param name="ImgLink">生成的图片的名称带后缀</param>
        ///// <returns>是否成功</returns>
        //public bool UpImage(System.Web.UI.WebControls.FileUpload image_file, string ImgPath, string ImgLink)
        //{
        //    if (image_file.PostedFile.FileName != null && image_file.PostedFile.FileName.Trim() != "")
        //    {
        //        try
        //        {
        //            if (!System.IO.Directory.Exists(ImgPath))
        //            {
        //                System.IO.Directory.CreateDirectory(ImgPath);
        //            }

        //            //如果显示水印
        //            if (ShowWatermark)
        //            {
        //                image_file.PostedFile.SaveAs(ImgPath + "\\" + "old_" + ImgLink);
        //                //加水印
        //                this.addWaterMark((ImgPath + "\\" + "old_" + ImgLink), (ImgPath + "\\" + ImgLink), WatermarkType);
        //            }
        //            else
        //            {
        //                image_file.PostedFile.SaveAs(ImgPath + "\\" + ImgLink);
        //            }
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}


        ///// <summary>
        ///// 上传图片代码
        ///// </summary>
        ///// <param name="image_file">HtmlInputFile控件</param>
        ///// <param name="ImgPath">存放的文件夹绝对位置</param>
        ///// <param name="ImgLink">生成的图片的名称带后缀</param>
        ///// <returns>是否成功</returns>
        //public bool UpImage(System.Web.UI.HtmlControls.HtmlInputFile image_file, string ImgPath, string ImgLink)
        //{
        //    if (image_file.PostedFile.FileName != null && image_file.PostedFile.FileName.Trim() != "")
        //    {
        //        try
        //        {
        //            if (!System.IO.Directory.Exists(ImgPath))
        //            {
        //                System.IO.Directory.CreateDirectory(ImgPath);
        //            }
        //            //如果显示水印
        //            if (ShowWatermark)
        //            {
        //                image_file.PostedFile.SaveAs(ImgPath + "\\" + "old_" + ImgLink);
        //                //加水印
        //                this.addWaterMark((ImgPath + "\\" + "old_" + ImgLink), (ImgPath + "\\" + ImgLink), WatermarkType);
        //            }
        //            else
        //            {
        //                image_file.PostedFile.SaveAs(ImgPath + "\\" + ImgLink);
        //            }
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// 添加图片水印
        /// 填加图片函数，需要下面两个函数的支持，当然也可以写到一起，不过那看起来就很冗长了。 添加图片水印
        /// </summary>
        /// <param name="oldpath">原图片绝对地址</param>
        /// <param name="newpath">新图片放置地址</param>
        public static void AddWaterMark(string oldpath, string newpath, string type)
        {
            try
            {
                Image image = Image.FromFile(oldpath);
                Bitmap b = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(b);
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.High;
                g.DrawImage(image, 0, 0, image.Width, image.Height);
                if (ShowWatermark)
                {
                    switch (type)
                    {
                        //是图片的话               
                        case "图片":
                            AddWatermarkImage(g, Watermarkimgpath, WatermarkPosition, image.Width, image.Height);
                            break;
                        //如果是文字                    
                        case "文字":
                            AddWatermarkText(g, WatermarkText, WatermarkPosition, image.Width, image.Height);
                            break;
                    }
                    b.Save(newpath, ImageFormat.Jpeg);
                    b.Dispose();
                    image.Dispose();
                }
            }
            catch
            {
                if (File.Exists(oldpath))
                {
                    File.Delete(oldpath);
                }
            }
            finally
            {
                if (File.Exists(oldpath))
                {
                    File.Delete(oldpath);
                }
            }
        }

        /// <summary>
        ///  加水印文字
        /// </summary>
        /// <param name="picture">imge 对象</param>
        /// <param name="_watermarkText">水印文字内容</param>
        /// <param name="_watermarkPosition">水印位置</param>
        /// <param name="_width">被加水印图片的宽</param>
        /// <param name="_height">被加水印图片的高</param>
        private static void AddWatermarkText(Graphics picture, string _watermarkText, string _watermarkPosition, int _width, int _height)
        {
            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };
            Font crFont = null;
            SizeF crSize = new SizeF();
            for (int i = 0; i < 7; i++)
            {
                crFont = new Font("arial", sizes[i], FontStyle.Bold);
                crSize = picture.MeasureString(_watermarkText, crFont);
                if ((ushort)crSize.Width < (ushort)_width)
                {
                    break;
                }
            }

            float xpos = 0;
            float ypos = 0;

            switch (_watermarkPosition)
            {
                case "上左":
                    xpos = ((float)_width * (float).01) + (crSize.Width / 2);
                    ypos = (float)_height * (float).01;
                    break;
                case "上右":
                    xpos = ((float)_width * (float).99) - (crSize.Width / 2);
                    ypos = (float)_height * (float).01;
                    break;
                case "下左":
                    xpos = ((float)_width * (float).01) + (crSize.Width / 2);
                    ypos = ((float)_height * (float).99) - crSize.Height;
                    break;
                case "下右":
                    xpos = ((float)_width * (float).99) - (crSize.Width / 2);
                    ypos = ((float)_height * (float).99) - crSize.Height;
                    break;
            }

            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));
            picture.DrawString(_watermarkText, crFont, semiTransBrush2, xpos + 1, ypos + 1, StrFormat);

            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));
            picture.DrawString(_watermarkText, crFont, semiTransBrush, xpos, ypos, StrFormat);
            semiTransBrush2.Dispose();
            semiTransBrush.Dispose();
        }

        /// <summary>
        ///  加水印图片
        /// </summary>
        /// <param name="picture">imge 对象</param>
        /// <param name="WaterMarkPicPath">水印图片的地址</param>
        /// <param name="_watermarkPosition">水印位置</param>
        /// <param name="_width">被加水印图片的宽</param>
        /// <param name="_height">被加水印图片的高</param>
        private static void AddWatermarkImage(Graphics picture, string WaterMarkPicPath, string _watermarkPosition, int _width, int _height)
        {
            Image watermark = new Bitmap(WaterMarkPicPath);
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };
            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
            float[][] colorMatrixElements = {
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                            };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;

            switch (_watermarkPosition)
            {
                case "上左":
                    xpos = 10;
                    ypos = 10;
                    break;
                case "上右":
                    xpos = ((_width - watermark.Width) - 10);
                    ypos = 10;
                    break;
                case "下左":
                    xpos = 10;
                    ypos = _height - watermark.Height - 10;
                    break;
                case "下右":
                    xpos = ((_width - watermark.Width) - 10);
                    ypos = _height - watermark.Height - 10;
                    break;
            }
            picture.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);
            watermark.Dispose();
            imageAttributes.Dispose();
        }
    }
}
