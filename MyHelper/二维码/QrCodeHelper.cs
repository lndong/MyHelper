using System.Drawing;
using QRCoder;

namespace MyHelper.二维码
{
    /// <summary>
    /// 使用QRCoder生成二维码，可自定义logo
    /// 此类只能生成二维码，不能解析二维码
    /// </summary>
    public class QrCodeHelper
    {
        #region 生成二维码

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="msg">文本内容</param>
        /// <param name="pixel">二维码像素大小</param>
        /// <returns></returns>

        public static Bitmap GetCodeImage(string msg, int pixel)
        {
            return CodeImage(msg, QRCodeGenerator.ECCLevel.Q, pixel, Color.Black, Color.White);
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="eccLevel"></param>
        /// <param name="pixel"></param>
        /// <param name="foreColor"></param>
        /// <param name="backColor"></param>
        /// <returns></returns>
        public static Bitmap GetCodeImage(string msg, QRCodeGenerator.ECCLevel eccLevel, int pixel, Color foreColor,
            Color backColor)
        {
            return CodeImage(msg, eccLevel, pixel, foreColor, backColor);
        }

        /// <summary>
        /// 使用QRCode生成二维码
        /// 主要实现方法，其余方法可随机定义参数
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="version">版本1-40</param>
        /// <param name="eccLevel">容错率</param>
        /// <param name="isUtf8">是否使用UTF8编码(部分编码不支持中文，带中文时使用)</param>
        /// <param name="pixel">像素点大小</param>
        /// <param name="darkColor">暗色 （前景）一般设置为Color.Black 黑色</param>
        /// <param name="lightColor">亮色（背景） 一般设置为Color.White  白色</param>
        /// <param name="iconPath">中心水印图标地址，默认为NULL ，加上这个二维码中间会显示一个图标</param>
        /// <param name="iconSize">水印图标大小比例</param>
        /// <param name="iconBorder">水印的边框</param>
        /// <param name="whiteDege">静止区，位于二维码某一边的空白边界,用来阻止读者获取
        /// 与正在浏览的二维码无关的信息 即是否绘画二维码的空白边框区域 默认为true</param>
        /// <returns></returns>
        public static Bitmap CodeImage(string msg, QRCodeGenerator.ECCLevel eccLevel, int pixel, Color darkColor,
            Color lightColor, bool isUtf8 = false, int version = -1,
            string iconPath = null, int iconSize = 15,
            int iconBorder = 6,
            bool whiteDege = true)
        {
            var qrCodeData = GetQrCodeData(msg, eccLevel, version, isUtf8);
            var qrCode = new QRCoder.QRCode(qrCodeData);
            Bitmap iconImage = null;
            if (!string.IsNullOrEmpty(iconPath))
            {
                iconImage = new Bitmap(iconPath);
            }

            var qrCodeImg = qrCode.GetGraphic(pixel, darkColor, lightColor, iconImage, iconSize, iconBorder, whiteDege);
            return qrCodeImg;
        }

        #endregion



        /// <summary>
        /// 生成二维码base64字符串
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="pixel"></param>
        /// <returns></returns>
        public static string GetCodeBase64String(string msg, int pixel)
        {
            return CodeBase64String(msg, QRCodeGenerator.ECCLevel.Q, pixel, Color.Black, Color.White);
        }
       

       

        /// <summary>
        /// 生成二维码base64字符串
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="eccLevel"></param>
        /// <param name="pixel"></param>
        /// <param name="darkColor"></param>
        /// <param name="lightColor"></param>
        /// <param name="version"></param>
        /// <param name="isUtf8"></param>
        /// <param name="iconPath"></param>
        /// <param name="iconSize"></param>
        /// <param name="iconBorder"></param>
        /// <param name="whiteDege"></param>
        /// <returns></returns>
        public static string CodeBase64String(string msg, QRCodeGenerator.ECCLevel eccLevel, int pixel,
            Color darkColor,
            Color lightColor, bool isUtf8 = false, int version = -1,
            string iconPath = null, int iconSize = 15,
            int iconBorder = 6,
            bool whiteDege = true)
        {
            var qrCodeData = GetQrCodeData(msg, eccLevel, version, isUtf8);
            var qrCode = new QRCoder.Base64QRCode(qrCodeData);
            Bitmap iconImage = null;
            if (!string.IsNullOrEmpty(iconPath))
            {
                iconImage = new Bitmap(iconPath);
            }

            var qrCodeImg = qrCode.GetGraphic(pixel, darkColor, lightColor, iconImage, iconSize, iconBorder, whiteDege);
            return qrCodeImg;
        }

        public static QRCodeGenerator.ECCLevel GetErrLevel(string text)
        {
            var error = QRCodeGenerator.ECCLevel.M;
            switch (text)
            {
                case "L":
                    error = QRCodeGenerator.ECCLevel.L;
                    break;
                case "M":
                    error = QRCodeGenerator.ECCLevel.M;
                    break;
                case "Q":
                    error = QRCodeGenerator.ECCLevel.Q;
                    break;
                case "H":
                    error = QRCodeGenerator.ECCLevel.H;
                    break;
            }
            return error;
        }

        #region 内部使用方法

        /// <summary>
        /// 生成QRCodeData
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="eccLevel"></param>
        /// <param name="version"></param>
        /// <param name="isUtf8"></param>
        /// <returns></returns>
        private static QRCodeData GetQrCodeData(string msg, QRCodeGenerator.ECCLevel eccLevel, int version = -1,
            bool isUtf8 = false)
        {
            var qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData;
            if (isUtf8)
            {
                qrCodeData = qrGenerator.CreateQrCode(msg, eccLevel, true, true,
                    QRCodeGenerator.EciMode.Utf8, version);
            }
            else
            {
                qrCodeData = qrGenerator.CreateQrCode(msg, eccLevel, false, false,
                    QRCodeGenerator.EciMode.Default, version);
            }

            return qrCodeData;
        }

        #endregion
    }
}
