using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;

namespace MyHelper.二维码
{
    /// <summary>
    /// ThoughtWorks.QRCode二维码生成帮助类
    /// 此类可生成二维码，也可以解析二维码
    /// 此类不能直接添加logo，添加logo的方法是先生成二维码，
    /// 在使用画板把logo画到二维码上面
    /// </summary>
    public class ThoughtWorksQrCodeHelper
    {
        #region MyRegion

        public static Bitmap GetCodeBitmap(string msg)
        {
            var qrCodeEncoder = GetQrCodeEncoder(QRCodeEncoder.ENCODE_MODE.BYTE, QRCodeEncoder.ERROR_CORRECTION.M,
                Color.Yellow, Color.Red, 4, 8);
            return qrCodeEncoder.Encode(msg);
        }



        /// <summary>
        /// 生成QRCodeEncoder类
        /// </summary>
        /// <param name="mode">二维码编码
        /// Numeric mode 数字编码，从 0 到9
        /// Alphanumeric mode 字符编码。包括 0-9，大写的A到Z（没有小写），以及符号$ % * + – . / : 包括空格</param>
        /// Byte mode, 字节编码，可以是0-255 的 ISO-8859-1 字符。
        /// <param name="error">容错处理
        /// level L : 最大 7% 的错误能够被纠正；
        ///level M : 最大 15% 的错误能够被纠正；
        ///level Q : 最大 25% 的错误能够被纠正；
        ///level H : 最大 30% 的错误能够被纠正</param>
        /// <param name="backgroundColor">背景色,默认值为Color.White</param>
        /// <param name="foregroundColor">前景色，默认值为Color.Black </param>
        /// <param name="qRCodeScale">二维码尺寸 设默认值为4</param>
        /// <param name="version">版本 设默认值为6</param>
        /// <returns></returns>
        private static QRCodeEncoder GetQrCodeEncoder(QRCodeEncoder.ENCODE_MODE mode,
            QRCodeEncoder.ERROR_CORRECTION error, Color backgroundColor, Color foregroundColor, int qRCodeScale = 4,
            int version = 6)
        {
            var qrCodeEncoder = new QRCodeEncoder
            {
                QRCodeEncodeMode = mode,
                QRCodeErrorCorrect = error,
                QRCodeBackgroundColor = backgroundColor,
                QRCodeForegroundColor = foregroundColor,
                QRCodeScale = qRCodeScale,
                QRCodeVersion = version
            };
            return qrCodeEncoder;
        }

        #endregion


        #region 解析二维码

        /// <summary>
        /// 解析二维码
        /// </summary>
        /// <param name="qRCodeImg"></param>
        /// <returns></returns>
        public static string DeQrCode(Image qRCodeImg)
        {
            return DeCoderQrCode(new Bitmap(qRCodeImg));
        }

        /// <summary>
        /// 解析二维码
        /// </summary>
        /// <param name="qRCodeImg"></param>
        /// <returns></returns>
        public static string DeQrCode(string qRCodeImg)
        {
            var bitmap = new Bitmap(qRCodeImg);
            return DeCoderQrCode(bitmap);
        }

        private static string DeCoderQrCode(Bitmap qBitmap)
        {
            var decoder = new QRCodeDecoder();
            try
            {
                return decoder.decode(new QRCodeBitmapImage(qBitmap));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

    }
}
