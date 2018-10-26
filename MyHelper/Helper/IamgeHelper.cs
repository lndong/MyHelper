using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper.Helper
{
    /// <summary>
    /// 图片处理类：包括固定宽高、限制宽高、限制长边、文字水印(透明)、图片水印(透明、任意角度旋转)
    /// </summary>
    public partial class IamgeHelper
    {
        #region 变量
        //优化良好的图片每个像素平均占用文件大小，经验值，可根据需要修改
        private static readonly double sizePerPx = 0.18;
        #endregion

        #region 生成高质量缩略图、优化图片      

        /// <summary>
        /// 生成高质量缩略图（固定宽高），不一定保持原宽高比
        /// </summary>
        /// <param name="destPath">目标保存路径</param>
        /// <param name="srcPath">源文件路径</param>
        /// <param name="destWidth">生成缩略图的宽度，设置为0，则与源图比处理</param>
        /// <param name="destHeight">生成缩略图的高度，设置为0，则与源图等比例处理</param>
        /// <param name="quality">1~100整数,无效值则取默认值95</param>
        /// <param name="error">out错误信息</param>
        /// <param name="mimeType">如 image/jpeg</param>    
        public bool GetThumbnailImage(string destPath, string srcPath, int destWidth, int destHeight, int quality, out string error, string mimeType = "image/jpeg")
        {
            var retVal = false;
            error = string.Empty;
            //宽高不能小于0
            if (destWidth < 0 || destHeight < 0)
            {
                error = "目标宽高不能小于0";
                return false;
            }
            //宽高不能同时为0
            if (destWidth == 0 && destHeight == 0)
            {
                error = "目标宽高不能同时为0";
                return false;
            }
            Image srcImage = null;
            Image destImage = null;
            Graphics graphics = null;
            try
            {
                //获取源图像
                srcImage = Image.FromFile(srcPath, false);
                //计算高宽比例
                var d = (float)srcImage.Height / srcImage.Width;
                //如果输入的宽为0，则按高度等比缩放
                if (destWidth == 0)
                {
                    destWidth = Convert.ToInt32(destHeight / d);
                }
                //如果输入的高为0，则按宽度等比缩放
                if (destHeight == 0)
                {
                    destHeight = Convert.ToInt32(destWidth * d);
                }
                //定义画布
                destImage = new Bitmap(destWidth, destHeight);
                //获取高清Graphics
                graphics = GetGraphics(destImage);
                //将源图像画到画布上，注意最后一个参数GraphicsUnit.Pixel
                graphics.DrawImage(srcImage, new Rectangle(0, 0, destWidth, destHeight), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                //如果是覆盖则先释放源资源
                if (destPath == srcPath)
                {
                    srcImage.Dispose();
                }
                //保存到文件，同时进一步控制质量
                SaveImage2File(destPath, destImage, quality, mimeType);
                retVal = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                srcImage?.Dispose();
                destImage?.Dispose();
                graphics?.Dispose();
            }
            return retVal;
        }

        /// <summary>
        /// 对图片进行压缩优化（限制宽高），始终保持原宽高比
        /// </summary>
        /// <param name="destPath">目标保存路径</param>
        /// <param name="srcPath">源文件路径</param>
        /// <param name="maxWidth">压缩后的图片宽度不大于这值，如果为0，表示不限制宽度</param>
        /// <param name="maxHeight">压缩后的图片高度不大于这值，如果为0，表示不限制高度</param>
        /// <param name="quality">1~100整数,无效值则取默认值95</param>
        /// <param name="mimeType">如 image/jpeg</param>
        /// <param name="error">out异常信息</param>
        public bool GetCompressImage(string destPath, string srcPath, int maxWidth, int maxHeight, int quality, out string error, string mimeType = "image/jpeg")
        {
            var retVal = false;
            error = string.Empty;
            //宽高不能小于0
            if (maxWidth < 0 || maxHeight < 0)
            {
                error = "目标宽高不能小于0";
                return false;
            }
            Image srcImage = null;
            Image destImage = null;
            Graphics graphics = null;
            try
            {
                //获取源图像
                srcImage = Image.FromFile(srcPath, false);
                FileInfo fileInfo = new FileInfo(srcPath);
                //目标宽度
                var destWidth = srcImage.Width;
                //目标高度
                var destHeight = srcImage.Height;
                //如果输入的最大宽为0，则不限制宽度
                //如果不为0，且原图宽度大于该值，则附值为最大宽度
                if (maxWidth != 0 && destWidth > maxWidth)
                {
                    destWidth = maxWidth;
                }
                //如果输入的最大宽为0，则不限制宽度
                //如果不为0，且原图高度大于该值，则附值为最大高度
                if (maxHeight != 0 && destHeight > maxHeight)
                {
                    destHeight = maxHeight;
                }
                float srcD = (float)srcImage.Height / srcImage.Width;
                float destD = (float)destHeight / destWidth;
                //目的高宽比 大于 原高宽比 即目的高偏大,因此按照原比例计算目的高度  
                if (destD > srcD)
                {
                    destHeight = Convert.ToInt32(destWidth * srcD);
                }
                else if (destD < srcD) //目的高宽比 小于 原高宽比 即目的宽偏大,因此按照原比例计算目的宽度  
                {
                    destWidth = Convert.ToInt32(destHeight / srcD);
                }
                //如果维持原宽高，则判断是否需要优化
                if (destWidth == srcImage.Width && destHeight == srcImage.Height &&
                    fileInfo.Length < destWidth * destHeight * sizePerPx)
                {
                    error = "图片不需要压缩优化";
                    return false;
                }
                //定义画布
                destImage = new Bitmap(destWidth, destHeight);
                //获取高清Graphics
                graphics = GetGraphics(destImage);
                //将源图像画到画布上，注意最后一个参数GraphicsUnit.Pixel
                graphics.DrawImage(srcImage, new Rectangle(0, 0, destWidth, destHeight), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                //如果是覆盖则先释放源资源
                if (destPath == srcPath)
                {
                    srcImage.Dispose();
                }
                //保存到文件，同时进一步控制质量
                SaveImage2File(destPath, destImage, quality, mimeType);
                retVal = true;

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                srcImage?.Dispose();
                destImage?.Dispose();
                graphics?.Dispose();
            }
            return retVal;
        }

        /// <summary>
        /// 对图片进行压缩优化，始终保持原宽高比，限制长边长度，常用场景：相片
        /// </summary>
        /// <param name="destPath">目标保存路径</param>
        /// <param name="srcPath">源文件路径</param>
        /// <param name="maxLength">压缩后的图片边（宽或者高）长变不大于这值，为0表示不限制</param>  
        /// <param name="quality">1~100整数,无效值，则取默认值95</param>
        /// <param name="mimeType">如 image/jpeg</param>
        /// <param name="error">out异常信息</param>
        public bool GetCompressImage(string destPath, string srcPath, int maxLength, int quality, out string error, string mimeType = "image/jpeg")
        {
            bool retVal = false;
            error = string.Empty;
            //最大边长不能小于0
            if (maxLength < 0)
            {
                error = "最大边长不能小于0";
                return false;
            }
            Image srcImage = null;
            Image destImage = null;
            Graphics graphics = null;
            try
            {
                //获取源图像
                srcImage = Image.FromFile(srcPath, false);
                FileInfo fileInfo = new FileInfo(srcPath);
                //目标宽度
                var destWidth = srcImage.Width;
                //目标高度
                var destHeight = srcImage.Height;
                //如果限制
                if (maxLength > 0)
                {
                    //原高宽比
                    float srcD = (float)srcImage.Height / srcImage.Width;
                    //如果宽>高，且大于 限制
                    if (destWidth > destHeight && destWidth > maxLength)
                    {
                        destWidth = maxLength;
                        destHeight = Convert.ToInt32(destWidth * srcD);
                    }
                    else
                    {
                        if (destHeight > maxLength)
                        {
                            destHeight = maxLength;
                            destWidth = Convert.ToInt32(destHeight / srcD);
                        }
                    }
                }
                //如果维持原宽高，则判断是否需要优化
                if (destWidth == srcImage.Width && destHeight == srcImage.Height &&
                    fileInfo.Length < destWidth * destHeight * sizePerPx)
                {
                    error = "图片不需要压缩优化";
                    return false;
                }
                //定义画布
                destImage = new Bitmap(destWidth, destHeight);
                //获取高清Graphics
                graphics = GetGraphics(destImage);
                //将源图像画到画布上，注意最后一个参数GraphicsUnit.Pixel
                graphics.DrawImage(srcImage, new Rectangle(0, 0, destWidth, destHeight), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                //如果是覆盖则先释放源资源
                if (destPath == srcPath)
                {
                    srcImage.Dispose();
                }
                //保存到文件，同时进一步控制质量
                SaveImage2File(destPath, destImage, quality, mimeType);
                retVal = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                srcImage?.Dispose();
                destImage?.Dispose();
                graphics?.Dispose();
            }
            return retVal;
        }

        #endregion

        #region 从图片中挖取一块区域

        /// <summary>
        /// 从源图片中挖取一块区域保存为新图片
        /// </summary>
        /// <param name="destPath">保存路径</param>
        /// <param name="srcPath">源路径，两个路径可以相同，相同则会覆盖源图片</param>
        /// <param name="x">挖取区域左上角x值</param>
        /// <param name="y">挖取区域左上角y值</param>
        /// <param name="width">挖取区域的宽度</param>
        /// <param name="height">挖取区域的高度</param>
        /// <param name="quality">1~100整数,无效值，则取默认值95</param>
        /// <param name="error"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public bool GetDigImage(string destPath, string srcPath, int x, int y, int width, int height, int quality, out string error, string mimeType = "image/jpeg")
        {
            var retVal = false;
            error = string.Empty;
            Image srcImage = null;
            Image destImage = null;
            Graphics graphics = null;
            try
            {
                //获取源图像
                srcImage = Image.FromFile(srcPath, false);
                //定义画布                                
                destImage = new Bitmap(width, height);
                //获取高清Graphics
                graphics = GetGraphics(destImage);
                //将源图像的某区域画到新画布上，注意最后一个参数GraphicsUnit.Pixel
                graphics.DrawImage(srcImage, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
                //如果是覆盖则先释放源资源
                if (destPath == srcPath)
                {
                    srcImage.Dispose();
                }
                //保存到文件，同时进一步控制质量
                SaveImage2File(destPath, destImage, quality, mimeType);
                retVal = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                srcImage?.Dispose();
                destImage?.Dispose();
                graphics?.Dispose();
            }
            return retVal;
        }

        #endregion

        #region 在图片加入文字水印或者图片水印,并控制水印透明度，图片水印还可以控制旋转角度
        /// <summary>
        /// 给图片加入文字水印,且设置水印透明度
        /// </summary>
        /// <param name="destPath">保存地址</param>
        /// <param name="srcPath">源文件地址，如果想覆盖源图片，两个地址参数一定要一样</param>
        /// <param name="text">文字</param>
        /// <param name="font">字体，为空则使用默认，注意，在创建字体时 GraphicsUnit.Pixel </param>
        /// <param name="brush">刷子，为空则使用默认</param>
        /// <param name="pos">设置水印位置，1左上，2中上，3右上
        ///                                 4左中，5中，  6右中
        ///                                 7左下，8中下，9右下</param>
        /// <param name="padding">跟css里的padding一个意思</param>
        /// <param name="quality">1~100整数,无效值，则取默认值95</param>
        /// <param name="opcity">不透明度  100 为完全不透明，0为完全透明</param>
        /// <param name="error"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public bool DrawWaterText(string destPath, string srcPath, string text, Font font, Brush brush, int pos, int padding, int quality, int opcity, out string error, string mimeType = "image/jpeg")
        {
            var retVal = false;
            error = string.Empty;
            Image srcImage = null;
            Image destImage = null;
            Graphics graphics = null;
            if (font == null)
            {
                font = new Font("微软雅黑", 20, FontStyle.Bold, GraphicsUnit.Pixel);//统一尺寸
            }
            if (brush == null)
            {
                brush = new SolidBrush(Color.White);
            }
            try
            {
                //获取源图像
                srcImage = Image.FromFile(srcPath, false);
                //定义画布，大小与源图像一样
                destImage = new Bitmap(srcImage);
                //获取高清Graphics
                graphics = GetGraphics(destImage);
                //将源图像画到画布上，注意最后一个参数GraphicsUnit.Pixel
                graphics.DrawImage(srcImage, new Rectangle(0, 0, destImage.Width, destImage.Height), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                //如果水印字不为空，且不透明度大于0，则画水印
                if (!string.IsNullOrEmpty(text) && opcity > 0)
                {
                    //获取可以用来绘制水印图片的有效区域
                    Rectangle validRect = new Rectangle(padding, padding, srcImage.Width - padding * 2, srcImage.Height - padding * 2);
                    //获取绘画水印文字的格式,即文字对齐方式
                    StringFormat format = GetStringFormat(pos);
                    //如果不透明度==100,那么直接将字画到当前画布上.
                    if (opcity == 100)
                    {
                        graphics.DrawString(text, font, brush, validRect, format);
                    }
                    else
                    {
                        //如果不透明度在0到100之间,就要实现透明效果，文字没法透明，图片才能透明
                        //则先将字画到一块临时画布，临时画布与destImage一样大，先将字写到这块画布上，再将临时画布画到主画布上，同时设置透明参数
                        Bitmap transImg = null;
                        Graphics gForTransImg = null;
                        try
                        {
                            //定义临时画布
                            transImg = new Bitmap(destImage);
                            //获取高清Graphics
                            gForTransImg = GetGraphics(transImg);
                            //绘制文字
                            gForTransImg.DrawString(text, font, brush, validRect, format);
                            //**获取带有透明度的ImageAttributes
                            ImageAttributes imageAtt = GetAlphaImgAttr(opcity);
                            //将这块临时画布画在主画布上
                            graphics.DrawImage(transImg, new Rectangle(0, 0, destImage.Width, destImage.Height), 0, 0, transImg.Width, transImg.Height, GraphicsUnit.Pixel, imageAtt);
                        }
                        catch (Exception ex)
                        {
                            error = ex.Message;
                            return false;
                        }
                        finally
                        {
                            transImg?.Dispose();
                            gForTransImg?.Dispose();
                        }
                    }
                }
                //如果两个地址相同即覆盖，则提前Dispose源资源
                if (destPath.ToLower() == srcPath.ToLower())
                {
                    srcImage.Dispose();
                }
                //保存到文件，同时进一步控制质量
                SaveImage2File(destPath, destImage, quality, mimeType);
                retVal = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                srcImage?.Dispose();
                destImage?.Dispose();
                graphics?.Dispose();
            }
            return retVal;
        }

        /// <summary>
        /// 给图片加入图片水印,且设置水印透明度，旋转角度
        /// </summary>
        /// <param name="destPath">保存地址</param>
        /// <param name="srcPath">源文件地址，如果想覆盖源图片，两个地址参数一定要一样</param>
        /// <param name="waterPath">水印图片地址</param>      
        /// <param name="pos">设置水印位置，1左上，2中上，3右上
        ///                                 4左中，5中，  6右中
        ///                                 7左下，8中下，9右下</param>
        /// <param name="padding">跟css里的padding一个意思</param>
        /// <param name="quality">1~100整数,无效值，则取默认值95</param>
        /// <param name="opcity">不透明度  100 为完全不透明，0为完全透明</param>
        /// <param name="angle">顺时针旋转角度</param>
        /// <param name="error"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public bool DrawWaterImage(string destPath, string srcPath, string waterPath, int pos, int padding, int quality, int opcity, int angle, out string error, string mimeType = "image/jpeg")
        {
            var retVal = false;
            error = string.Empty;
            Image srcImage = null;
            Image waterImage = null;
            Image destImage = null;
            Graphics graphics = null;
            try
            {
                //获取原图
                srcImage = Image.FromFile(srcPath, false);
                //获取水印图片
                waterImage = Image.FromFile(waterPath, false);
                var waterRect = new Rectangle(0, 0, waterImage.Width, waterImage.Height);
                //定义画布
                destImage = new Bitmap(srcImage);
                //获取高清Graphics
                graphics = GetGraphics(destImage);
                //将源图画到画布上
                graphics.DrawImage(srcImage, new Rectangle(0, 0, destImage.Width, destImage.Height), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                //不透明度大于0，则画水印
                if (opcity > 0)
                {
                    //获取可以用来绘制水印图片的有效区域
                    Rectangle validRect = new Rectangle(padding, padding, srcImage.Width - padding * 2, srcImage.Height - padding * 2);
                    //如果要进行旋转
                    if (angle != 0)
                    {
                        Image rotateImage = null;
                        try
                        {
                            //获取水印图像旋转后的图像
                            rotateImage = GetRotateImage(waterImage, angle);
                            if (rotateImage != null)
                            {
                                //旋转后图像的矩形区域
                                var rotateRect = new Rectangle(0, 0, rotateImage.Width, rotateImage.Height);
                                //计算水印图片的绘制位置
                                var destRect = GetRectangleByPostion(validRect, rotateRect, pos);
                                //如果不透明度>=100,那么直接将水印画到当前画布上.
                                if (opcity == 100)
                                {
                                    graphics.DrawImage(rotateImage, destRect, rotateRect, GraphicsUnit.Pixel);
                                }
                                else
                                {
                                    //如果不透明度在0到100之间，设置透明参数
                                    ImageAttributes imageAtt = GetAlphaImgAttr(opcity);
                                    //将旋转后的图片画到画布上
                                    graphics.DrawImage(rotateImage, destRect, 0, 0, rotateRect.Width, rotateRect.Height, GraphicsUnit.Pixel, imageAtt);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            error = ex.Message;
                            return false;

                        }
                        finally
                        {
                            rotateImage?.Dispose();
                        }
                    }
                    else
                    {
                        //计算水印图片的绘制位置
                        var destRect = GetRectangleByPostion(validRect, waterRect, pos);
                        //如果不透明度=100,那么直接将水印画到当前画布上.
                        if (opcity == 100)
                        {
                            graphics.DrawImage(waterImage, destRect, waterRect, GraphicsUnit.Pixel);
                        }
                        else
                        {
                            //如果不透明度在0到100之间，设置透明参数
                            ImageAttributes imageAtt = GetAlphaImgAttr(opcity);
                            //将水印图片画到画布上
                            graphics.DrawImage(waterImage, destRect, 0, 0, waterRect.Width, waterRect.Height, GraphicsUnit.Pixel, imageAtt);

                        }
                    }

                }
                //如果两个地址相同即覆盖，则提前Dispose源资源
                if (destPath.ToLower() == srcPath.ToLower())
                {
                    srcImage.Dispose();
                }
                SaveImage2File(destPath, destImage, quality, mimeType);
                retVal = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                srcImage?.Dispose();
                destImage?.Dispose();
                graphics?.Dispose();
                waterImage?.Dispose();
            }
            return retVal;
        }
        #endregion

        #region 共用方法     
        
        /// <summary>
        /// 将Image实例保存到文件,注意此方法不执行 img.Dispose()
        /// 图片保存时本可以直接使用destImage.Save(path, ImageFormat.Jpeg)，但是这种方法无法进行进一步控制图片质量
        /// </summary>
        /// <param name="path"></param>
        /// <param name="destImage"></param>
        /// <param name="quality">1~100整数,无效值，则取默认值95</param>
        /// <param name="mimeType"></param>
        public void SaveImage2File(string path, Image destImage, int quality, string mimeType = "image/jpeg")
        {
            if (quality <= 0 || quality > 100) quality = 95;
            //创建保存的文件夹
            FileInfo fileInfo = new FileInfo(path);
            if (!Directory.Exists(fileInfo.DirectoryName))
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }
            //设置保存参数，保存参数里进一步控制质量
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qua = new long[] { quality };
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            //获取指定mimeType的mimeType的ImageCodecInfo
            var codecInfo = ImageCodecInfo.GetImageEncoders().FirstOrDefault(ici => ici.MimeType == mimeType);
            destImage.Save(path, codecInfo, encoderParams);
        }

        /// <summary>
        /// 获取高清的Graphics
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public Graphics GetGraphics(Image img)
        {
            var g = Graphics.FromImage(img);
            //设置质量
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            //InterpolationMode不能使用High或者HighQualityBicubic,如果是灰色或者部分浅色的图像是会在边缘处出一白色透明的线
            //用HighQualityBilinear却会使图片比其他两种模式模糊（需要肉眼仔细对比才可以看出）
            g.InterpolationMode = InterpolationMode.Default;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            return g;
        }

        /// <summary>
        /// 获取文字水印位置  
        /// </summary>
        /// <param name="pos">
        ///         1左上，2中上，3右上
        ///         4左中，5中，  6右中
        ///         7左下，8中下，9右下
        /// </param>
        /// <returns></returns>
        public StringFormat GetStringFormat(int pos)
        {
            StringFormat format = new StringFormat();
            switch (pos)
            {
                case 1: format.Alignment = StringAlignment.Near; format.LineAlignment = StringAlignment.Near; break;
                case 2: format.Alignment = StringAlignment.Center; format.LineAlignment = StringAlignment.Near; break;
                case 3: format.Alignment = StringAlignment.Far; format.LineAlignment = StringAlignment.Near; break;
                case 4: format.Alignment = StringAlignment.Near; format.LineAlignment = StringAlignment.Center; break;
                case 6: format.Alignment = StringAlignment.Far; format.LineAlignment = StringAlignment.Center; break;
                case 7: format.Alignment = StringAlignment.Near; format.LineAlignment = StringAlignment.Far; break;
                case 8: format.Alignment = StringAlignment.Center; format.LineAlignment = StringAlignment.Far; break;
                case 9: format.Alignment = StringAlignment.Far; format.LineAlignment = StringAlignment.Far; break;
                default: format.Alignment = StringAlignment.Center; format.LineAlignment = StringAlignment.Center; break;
            }
            return format;
        }

        /// <summary>
        /// 获取图片水印位置，及small在big里的位置
        /// 如果small的高度大于big的高度，返回big的高度
        /// 如果small的宽度大于big的宽度，返回big的宽度
        /// </summary>
        /// <param name="pos">
        ///         1左上，2中上，3右上
        ///         4左中，5中，  6右中
        ///         7左下，8中下，9右下
        /// </param>
        /// <returns></returns>
        public Rectangle GetRectangleByPostion(Rectangle big, Rectangle small, int pos)
        {
            if (big.Width < small.Width)
            {
                small.Width = big.Width;
            }
            if (big.Height < small.Height)
            {
                small.Height = big.Height;
            }
            Rectangle retVal = small;
            switch (pos)
            {
                case 1: retVal.X = 0; retVal.Y = 0; break;
                case 2: retVal.X = (big.Width - small.Width) / 2; retVal.Y = 0; break;
                case 3: retVal.X = big.Width - small.Width; retVal.Y = 0; break;
                case 4: retVal.X = 0; retVal.Y = (big.Height - small.Height) / 2; break;
                case 6: retVal.X = big.Width - small.Width; retVal.Y = (big.Height - small.Height) / 2; break;
                case 7: retVal.X = 0; retVal.Y = big.Height - small.Height; break;
                case 8: retVal.X = (big.Width - small.Width) / 2; retVal.Y = big.Height - small.Height; break;
                case 9: retVal.X = big.Width - small.Width; retVal.Y = big.Height - small.Height; break;
                default: retVal.X = (big.Width - small.Width) / 2; retVal.Y = (big.Height - small.Height) / 2; break;
            }
            retVal.X += big.X;
            retVal.Y += big.Y;
            return retVal;
        }

        /// <summary>
        /// 获取多个水印坐标
        /// </summary>
        /// <param name="imgSrc">原图</param>
        /// <param name="imgWm">水印图</param>
        /// <param name="offsetX">X间距</param>
        /// <param name="offsetY">Y间距</param>
        /// <param name="countX">X水印数量</param>
        /// <param name="countY">Y水印数量</param>
        /// <param name="firstOffsetX">第一个水印X偏移量</param>
        /// <param name="firstOffsetY">第一个水印Y偏移量</param>
        /// <returns></returns>
        public Rectangle[] GetCalculateRectangles(Image imgSrc, Image imgWm, int offsetX, int offsetY, int countX,
            int countY,int firstOffsetX,int firstOffsetY)
        {
            var imgSize = imgSrc.Size;
            var wmSize = imgWm.Size;

            var imgWidth = imgSize.Width;
            var imgHeight = imgSize.Height;

            var wmWidth = wmSize.Width;
            var wmHeight = wmSize.Height;

            var x = (int) Math.Ceiling((double) imgWidth / (wmWidth + offsetX)); //获取X水印数量
            x = x > countX ? x : countX;

            var y = (int) Math.Ceiling((double) imgHeight / (wmHeight + offsetY)); //获取Y水印数量
            y = y > countY ? y : countY;

            var rectangles = new Rectangle[x * y];
            var index = 0;
            for (var i = 0; i < x; i++)
            {
                for (var j = 0; j < y; j++)
                {
                    var point = new Point(i * (wmWidth + offsetX) + firstOffsetX,
                        j * (wmHeight + offsetY) + firstOffsetY);
                    rectangles[index] = new Rectangle(point, wmSize);
                    index++;
                }   
            }

            return rectangles;
        }

        /// <summary>
        /// 获取一个带有透明度的ImageAttributes
        /// </summary>
        /// <param name="opcity"></param>
        /// <returns></returns>
        public ImageAttributes GetAlphaImgAttr(int opcity)
        {
            if (opcity < 0 || opcity > 100)
            {
                throw new ArgumentOutOfRangeException("opcity 值为 0~100");
            }
            //颜色矩阵
            float[][] matrixItems =
            {
                new float[]{1,0,0,0,0},
                new float[]{0,1,0,0,0},
                new float[]{0,0,1,0,0},
                new float[]{0,0,0,(float)opcity / 100,0},
                new float[]{0,0,0,0,1}
            };
            ColorMatrix colorMatrix = new ColorMatrix(matrixItems);
            ImageAttributes imageAtt = new ImageAttributes();
            imageAtt.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return imageAtt;
        }

        /// <summary>
        /// 计算矩形绕中心任意角度旋转后所占区域矩形宽高
        /// </summary>
        /// <param name="width">原矩形的宽</param>
        /// <param name="height">原矩形高</param>
        /// <param name="angle">顺时针旋转角度</param>
        /// <returns></returns>
        public Rectangle GetRotateRectangle(int width, int height, float angle)
        {
            double radian = angle * Math.PI / 180; ;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            //只需要考虑到第四象限和第三象限的情况取大值(中间用绝对值就可以包括第一和第二象限)
            int newWidth = (int)(Math.Max(Math.Abs(width * cos - height * sin), Math.Abs(width * cos + height * sin)));
            int newHeight = (int)(Math.Max(Math.Abs(width * sin - height * cos), Math.Abs(width * sin + height * cos)));
            return new Rectangle(0, 0, newWidth, newHeight);
        }

        /// <summary>
        /// 获取原图像绕中心任意角度旋转后的图像
        /// </summary>
        /// <param name="rawImg"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public Image GetRotateImage(Image srcImage, int angle)
        {
            angle = angle % 360;
            //原图的宽和高
            int srcWidth = srcImage.Width;
            int srcHeight = srcImage.Height;
            //图像旋转之后所占区域宽和高
            Rectangle rotateRec = GetRotateRectangle(srcWidth, srcHeight, angle);
            int rotateWidth = rotateRec.Width;
            int rotateHeight = rotateRec.Height;
            //目标位图
            Bitmap destImage = null;
            Graphics graphics = null;
            try
            {
                //定义画布，宽高为图像旋转后的宽高
                destImage = new Bitmap(rotateWidth, rotateHeight);
                //graphics根据destImage创建，因此其原点此时在destImage左上角
                graphics = Graphics.FromImage(destImage);
                //要让graphics围绕某矩形中心点旋转N度，分三步
                //第一步，将graphics坐标原点移到矩形中心点,假设其中点坐标（x,y）
                //第二步，graphics旋转相应的角度(沿当前原点)
                //第三步，移回（-x,-y）
                //获取画布中心点
                Point centerPoint = new Point(rotateWidth / 2, rotateHeight / 2);
                //将graphics坐标原点移到中心点
                graphics.TranslateTransform(centerPoint.X, centerPoint.Y);
                //graphics旋转相应的角度(绕当前原点)
                graphics.RotateTransform(angle);
                //恢复graphics在水平和垂直方向的平移(沿当前原点)
                graphics.TranslateTransform(-centerPoint.X, -centerPoint.Y);
                //此时已经完成了graphics的旋转

                //计算:如果要将源图像画到画布上且中心与画布中心重合，需要的偏移量
                Point Offset = new Point((rotateWidth - srcWidth) / 2, (rotateHeight - srcHeight) / 2);
                //将源图片画到rect里（rotateRec的中心）
                graphics.DrawImage(srcImage, new Rectangle(Offset.X, Offset.Y, srcWidth, srcHeight));
                //重至绘图的所有变换
                graphics.ResetTransform();
                graphics.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                graphics?.Dispose();
            }
            return destImage;
        }
        #endregion

        #region 文字转图片

        /// <summary>
        /// 把文字画到画板上生成图片
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">文本格式</param>
        /// <returns></returns>
        public Image DrawIamge(string text, Font font)
        {
            if (font == null)
            {
                font = new Font("微软雅黑", 20, FontStyle.Bold, GraphicsUnit.Pixel);//统一尺寸
            }

            var size = GetTextSize(text, font); //获取文字宽高
            var bitmap = new Bitmap(size.Width+1,size.Height+1);
            using (var gb = GetGraphics(bitmap))
            {
                var semiTransBrush = new SolidBrush(Color.Black);
                gb.DrawString(text, font, semiTransBrush, 0, bitmap.Height - size.Height);
            }

            return bitmap;
        }

        /// <summary>
        /// 根据文字内容字符串和尺寸计算宽和高
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">文本格式</param>
        /// <returns></returns>
        private Size GetTextSize(string text, Font font)
        {
            var size = new Size(0,0);
            if (font == null)
            {
                font = new Font("微软雅黑", 20, FontStyle.Bold, GraphicsUnit.Pixel); //统一尺寸
            }

            if (!string.IsNullOrEmpty(text))
            {
                var bitmap = new Bitmap(800,800); //初始化bitmap实例，大小根据场景定义
                using (var gr = GetGraphics(bitmap))
                {
                    var s = gr.MeasureString(text, font); //测量字符串
                    size.Width = (int) s.Width;
                    size.Height = (int) s.Height;
                }
                bitmap.Dispose();
            }

            return size;
        }

        #endregion
    }
}
