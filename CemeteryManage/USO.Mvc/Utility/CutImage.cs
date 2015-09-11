using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace USO.Mvc.Utility
{
    /// <summary>
    /// 图片处理   
    /// 
    /// Ron 2013-04-18 创建
    /// 
    /// </summary>
    public class CutImage
    {
        #region 自定义裁剪并缩放

        /// <summary>
        /// 指定长宽裁剪
        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>     
        /// <param name="fromFile">原图Stream对象</param>
        /// <param name="fileSaveUrl">保存路径</param>
        /// <param name="maxWidth">最大宽(单位:px)</param>
        /// <param name="maxHeight">最大高(单位:px)</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForCustom(System.IO.Stream fromFile, string fileSaveUrl, int maxWidth, int maxHeight, int quality)
        {
            //从文件获取原始图片，并使用流中嵌入的颜色管理信息
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(fromFile, true);
           
            
            //原图宽高均小于模版，不作处理，直接保存
            
            if (initImage.Width <= maxWidth && initImage.Height <= maxHeight)
            {
                initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                //模版的宽高比例
                double templateRate = (double)maxWidth / maxHeight;
                //原图片的宽高比例
                double initRate = (double)initImage.Width / initImage.Height;

                //原图与模版比例相等，直接缩放
                if (templateRate == initRate)
                {
                    //按模版大小生成最终图片
                    System.Drawing.Image templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);
                    templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                //原图与模版比例不等，裁剪后缩放
                else
                {
                    //裁剪对象
                    System.Drawing.Image pickedImage = null;
                    System.Drawing.Graphics pickedG = null;

                    //定位
                    Rectangle fromR = new Rectangle(0, 0, 0, 0);//原图裁剪定位
                    Rectangle toR = new Rectangle(0, 0, 0, 0);//目标定位

                    //宽为标准进行裁剪
                    if (templateRate > initRate)
                    {
                        //裁剪对象实例化
                        pickedImage = new System.Drawing.Bitmap(initImage.Width, (int)System.Math.Floor(initImage.Width / templateRate));
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                        //裁剪源定位
                        fromR.X = 0;
                        fromR.Y = (int)System.Math.Floor((initImage.Height - initImage.Width / templateRate) / 2);
                        fromR.Width = initImage.Width;
                        fromR.Height = (int)System.Math.Floor(initImage.Width / templateRate);

                        //裁剪目标定位
                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = initImage.Width;
                        toR.Height = (int)System.Math.Floor(initImage.Width / templateRate);
                    }
                    //高为标准进行裁剪
                    else
                    {
                        pickedImage = new System.Drawing.Bitmap((int)System.Math.Floor(initImage.Height * templateRate), initImage.Height);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                        fromR.X = (int)System.Math.Floor((initImage.Width - initImage.Height * templateRate) / 2);
                        fromR.Y = 0;
                        fromR.Width = (int)System.Math.Floor(initImage.Height * templateRate);
                        fromR.Height = initImage.Height;

                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = (int)System.Math.Floor(initImage.Height * templateRate);
                        toR.Height = initImage.Height;
                    }

                    //设置质量
                    pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    //裁剪
                    pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);

                    //按模版大小生成最终图片
                    System.Drawing.Image templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(pickedImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, pickedImage.Width, pickedImage.Height), System.Drawing.GraphicsUnit.Pixel);

                    //关键质量控制
                    //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                    //ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                    //ImageCodecInfo ici = null;
                    //foreach (ImageCodecInfo i in icis)
                    //{
                    //    if (i.MimeType == "image/jpeg")
                    //    {
                    //        ici = i;
                    //    }
                    //}
                   

                    EncoderParameters ep = new EncoderParameters(1);
                    ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

                    //保存缩略图
                    templateImage.Save(fileSaveUrl,GetEncoder(ImageFormat.Jpeg), ep);
                    //templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //释放资源
                    templateG.Dispose();
                    templateImage.Dispose();

                    pickedG.Dispose();
                    pickedImage.Dispose();
                }
            }

            //释放资源
            initImage.Dispose();
        }
        #endregion
        static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.Single(codec => codec.FormatID == format.Guid);
        }

        #region 等比缩放
        /// <summary> 

        /// 图片等比缩放 

        /// </summary> 

        /// <param name="postedFile">原图HttpPostedFile对象</param> 

        /// <param name="savePath">缩略图存放地址</param> 

        /// <param name="targetWidth">指定的最大宽度</param> 

        /// <param name="targetHeight">指定的最大高度</param> 

        /// <param name="watermarkText">水印文字(为""表示不使用水印)</param> 

        /// <param name="watermarkImage">水印图片路径(为""表示不使用水印)</param> 

        public static void ZoomAuto(System.IO.Stream fromFile, string savePath, System.Double targetWidth, System.Double targetHeight, string watermarkText, string watermarkImage)
        {

            //从文件获取原始图片，并使用流中嵌入的颜色管理信息
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(fromFile, true);
           


            //原图宽高均小于模版，不作处理，直接保存 

            if (initImage.Width <= targetWidth && initImage.Height <= targetHeight)
            {

                //文字水印 

                if (watermarkText != "")
                {

                    using (System.Drawing.Graphics gWater = System.Drawing.Graphics.FromImage(initImage))
                    {

                        System.Drawing.Font fontWater = new Font("黑体", 10);

                        System.Drawing.Brush brushWater = new SolidBrush(Color.White);

                        gWater.DrawString(watermarkText, fontWater, brushWater, 10, 10);

                        gWater.Dispose();

                    }

                }



                //透明图片水印 

                if (watermarkImage != "")
                {

                    if (File.Exists(watermarkImage))
                    {

                        //获取水印图片 

                        using (System.Drawing.Image wrImage = System.Drawing.Image.FromFile(watermarkImage))
                        {

                            //水印绘制条件：原始图片宽高均大于或等于水印图片 

                            if (initImage.Width >= wrImage.Width && initImage.Height >= wrImage.Height)
                            {

                                Graphics gWater = Graphics.FromImage(initImage);



                                //透明属性 

                                ImageAttributes imgAttributes = new ImageAttributes();

                                ColorMap colorMap = new ColorMap();

                                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);

                                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

                                ColorMap[] remapTable = { colorMap };

                                imgAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);



                                float[][] colorMatrixElements = {  

                                   new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f}, 

                                   new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f}, 

                                   new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f}, 

                                   new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},//透明度:0.5 

                                   new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f} 

                                };



                                ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);

                                imgAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                                gWater.DrawImage(wrImage, new Rectangle(initImage.Width - wrImage.Width, initImage.Height - wrImage.Height, wrImage.Width, wrImage.Height), 0, 0, wrImage.Width, wrImage.Height, GraphicsUnit.Pixel, imgAttributes);



                                gWater.Dispose();

                            }

                            wrImage.Dispose();

                        }

                    }

                }



                //保存 

                initImage.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);

            }

            else
            {

                //缩略图宽、高计算 

                double newWidth = initImage.Width;

                double newHeight = initImage.Height;



                //宽大于高或宽等于高（横图或正方） 

                if (initImage.Width > initImage.Height || initImage.Width == initImage.Height)
                {

                    //如果宽大于模版 

                    if (initImage.Width > targetWidth)
                    {

                        //宽按模版，高按比例缩放 

                        newWidth = targetWidth;

                        newHeight = initImage.Height * (targetWidth / initImage.Width);

                    }

                }

                //高大于宽（竖图） 

                else
                {



                    //如果高大于模版 

                    if (initImage.Height > targetHeight)
                    {

                        //高按模版，宽按比例缩放 

                        newHeight = targetHeight;

                        newWidth = initImage.Width * (targetHeight / initImage.Height);

                    }

                }



                //生成新图 

                //新建一个bmp图片 

                System.Drawing.Image newImage = new System.Drawing.Bitmap((int)newWidth, (int)newHeight);

                //新建一个画板 

                System.Drawing.Graphics newG = System.Drawing.Graphics.FromImage(newImage);



                //设置质量 

                newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;



                //置背景色 

                newG.Clear(Color.White);

                //画图 

                newG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, newImage.Width, newImage.Height), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);



                //文字水印 

                if (watermarkText != "")
                {

                    using (System.Drawing.Graphics gWater = System.Drawing.Graphics.FromImage(newImage))
                    {

                        System.Drawing.Font fontWater = new Font("宋体", 10);

                        System.Drawing.Brush brushWater = new SolidBrush(Color.White);

                        gWater.DrawString(watermarkText, fontWater, brushWater, 10, 10);

                        gWater.Dispose();

                    }

                }



                //透明图片水印 

                if (watermarkImage != "")
                {

                    if (File.Exists(watermarkImage))
                    {

                        //获取水印图片 

                        using (System.Drawing.Image wrImage = System.Drawing.Image.FromFile(watermarkImage))
                        {

                            //水印绘制条件：原始图片宽高均大于或等于水印图片 

                            if (newImage.Width >= wrImage.Width && newImage.Height >= wrImage.Height)
                            {

                                Graphics gWater = Graphics.FromImage(newImage);



                                //透明属性 

                                ImageAttributes imgAttributes = new ImageAttributes();

                                ColorMap colorMap = new ColorMap();

                                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);

                                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

                                ColorMap[] remapTable = { colorMap };

                                imgAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);



                                float[][] colorMatrixElements = {  

                                   new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f}, 

                                   new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f}, 

                                   new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f}, 

                                   new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},//透明度:0.5 

                                   new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f} 

                                };



                                ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);

                                imgAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                                gWater.DrawImage(wrImage, new Rectangle(newImage.Width - wrImage.Width, newImage.Height - wrImage.Height, wrImage.Width, wrImage.Height), 0, 0, wrImage.Width, wrImage.Height, GraphicsUnit.Pixel, imgAttributes);

                                gWater.Dispose();

                            }

                            wrImage.Dispose();

                        }

                    }

                }



                //保存缩略图 

                newImage.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);



                //释放资源 

                newG.Dispose();

                newImage.Dispose();

                initImage.Dispose();

            }

        }
        #endregion


        #region 固定宽度
        /// <summary> 

        /// 图片固定宽度 

        /// </summary> 

        /// <param name="postedFile">原图HttpPostedFile对象</param> 

        /// <param name="savePath">缩略图存放地址</param> 

        /// <param name="targetWidth">指定的最大宽度</param> 
       

        public static void ZoomAuto1(System.IO.Stream fromFile, string savePath, System.Double targetWidth)
        {

            //从文件获取原始图片，并使用流中嵌入的颜色管理信息
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(fromFile, true);



            //原图宽高均小于模版，不作处理，直接保存 

            if (initImage.Width <= targetWidth)
            {
                //保存 
                initImage.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);

            }

            else
            {

                //缩略图宽、高计算 

                double newWidth = initImage.Width;

                double newHeight = initImage.Height;


                //如果宽大于模版 

                if (initImage.Width > targetWidth)
                {

                    //宽按模版，高按比例缩放 

                    newWidth = targetWidth;

                    newHeight = initImage.Height * (targetWidth / initImage.Width);

                }


                //生成新图 

                //新建一个bmp图片 

                System.Drawing.Image newImage = new System.Drawing.Bitmap((int)newWidth, (int)newHeight);

                //新建一个画板 

                System.Drawing.Graphics newG = System.Drawing.Graphics.FromImage(newImage);



                //设置质量 

                newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;



                //置背景色 

                newG.Clear(Color.White);

                //画图 

                newG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, newImage.Width, newImage.Height), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);




                //保存缩略图 

                newImage.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);



                //释放资源 

                newG.Dispose();

                newImage.Dispose();

                initImage.Dispose();

            }

        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbnail(System.IO.Stream fromFile, string thumbnailPath, int width, int height, string mode)
        {
            Image originalImage = System.Drawing.Image.FromStream(fromFile, true);
           
            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }


        #endregion

        #region 其它

        /// <summary>
        /// 判断文件类型是否为WEB格式图片
        /// (注：JPG,GIF,BMP,PNG)
        /// </summary>
        /// <param name="contentType">HttpPostedFile.ContentType</param>
        /// <returns></returns>
        public static bool IsWebImage(string contentType)
        {
            if (contentType == "image/pjpeg" || contentType == "image/jpeg" || contentType == "image/gif" || contentType == "image/bmp" || contentType == "image/png")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 取得随机文件名
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetRandomFileName(string filename, int size)
        {
            string[] files = filename.Split('.');
            string exfilename = "." + files.GetValue(files.Length - 1);

            //  char[] s = new char[]{'0','1', '2','3','4','5','6','7','8','9','A' 
            //,'B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q' 
            //,'R','S','T','U','V','W','X','Y','Z'};
            //  string num = "";
            //  Random r = new Random();
            //  for (int i = 0; i < 4; i++)
            //      num += s[r.Next(0, s.Length)].ToString();//51&aspx
            //  DateTime time = DateTime.Now;
            //string name = time.Year.ToString()
            //    + time.Month.ToString().PadLeft(2, '0')
            //    + time.Day.ToString().PadLeft(2, '0')
            //    + time.Hour.ToString().PadLeft(2, '0')
            //    + time.Minute.ToString().PadLeft(2, '0')
            //    + time.Second.ToString().PadLeft(2, '0')
            //    + num + exfilename;

            string name = size + exfilename;
            return name;
        }

        #endregion

    }
}
