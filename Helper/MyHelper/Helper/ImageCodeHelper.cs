using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace MyHelper.Helper
{
    /// <summary>
    /// 图片验证码
    /// </summary>
    public class ImageCodeHelper
    {
        /// <summary>
        /// 单个字体宽度范围（默认为16）设置大于16的值
        /// </summary>
        public int OneWidth { get; set; } = 16;

        /// <summary>
        /// 单个字体高度范围（默认为20）
        /// </summary>
        public int OneHeight { get; set; } = 20;

        #region 使用RNGCryptoServiceProvider生成随机数

        /// <summary>
        /// 随机值序列填充字节数组
        /// </summary>
        private static readonly byte[] RandomBytes = new byte[4];

        private static readonly RNGCryptoServiceProvider ServiceProvider = new RNGCryptoServiceProvider();

        /// <summary>
        /// 获取一个随机数字，数字范围从0到max包含0和max
        /// </summary>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        private static int Next(int max)
        {
            ServiceProvider.GetBytes(RandomBytes);
            var value = BitConverter.ToInt32(RandomBytes, 0);
            var res = Math.Abs(value % (max + 1));
            return res;
        }

        /// <summary>
        /// 获取一个随机数字，数字范围从min到max包含min和max
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        private static int Next(int min, int max)
        {
            return Next(max - min) + min;
        }
        #endregion

        /// <summary>
        /// Font对象集合
        /// </summary>
        public List<Font> FontList { get; set; } = new List<Font>
        {
            new Font(new FontFamily("Times New Roman"), 10 +Next(2) , FontStyle.Regular),
            new Font(new FontFamily("Georgia"), 10 + Next(2), FontStyle.Regular),
            new Font(new FontFamily("Arial"), 10 + Next(2), FontStyle.Regular),
            new Font(new FontFamily("Comic Sans MS"), 10 + Next(2), FontStyle.Regular)
        };

        /// <summary>
        /// 字体随机颜色
        /// </summary>
        /// <returns></returns>
        public Color GetRandomColor()
        {
            var intRed = Next(180);
            var intGreen = Next(180);
            var intBlue = intRed + intGreen > 300 ? 0 : 400 - intRed - intGreen;
            intBlue = intBlue > 255 ? 255 : intBlue;
            return Color.FromArgb(intRed, intGreen, intBlue);
        }

        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bxDir">如果扭曲则选择为True</param>
        /// <param name="dMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI]</param>
        /// <returns></returns>
        public Bitmap TwistImage(Bitmap srcBmp, bool bxDir, double dMultValue, double dPhase)
        {
            var PI2 = 6.283185307179586476925286766559;
            var destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            var graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            var dBaseAxisLen = bxDir ? (double)destBmp.Height : (double)destBmp.Width;
            for (var i = 0; i < destBmp.Width; i++)
            {
                for (var j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bxDir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    var dy = Math.Sin(dx);
                    int nOldX = 0, nOldY = 0;
                    nOldX = bxDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bxDir ? j : j + (int)(dy * dMultValue);

                    var color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                                   && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            srcBmp.Dispose();
            return destBmp;
        }

        /// <summary>
        /// 绘制图片验证码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Bitmap CreateImageCode(string code)
        {
            var codeLength = code.Length;
            var imageWidth = codeLength * OneWidth;
            Bitmap image = new Bitmap(imageWidth, OneHeight);
            var g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //画线
            for (var i = 0; i < 2; i++)
            {
                var x1 = Next(image.Width - 1);
                var x2 = Next(image.Width - 1);
                var y1 = Next(image.Height - 1);
                var y2 = Next(image.Height - 1);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }

            //写入验证码
            int _x = -12, _y = 0;           
            for (var index = 0; index < code.Length; index++)
            {
                _x += Next(12, 16);
                _y = Next(-2, 2);
                var str_char = code.Substring(index, 1); //获取验证码的每个字符
                str_char = Next(1) == 1 ? str_char.ToLower() : str_char.ToUpper();
                Brush newBrush = new SolidBrush(GetRandomColor());
                var thePos = new Point(_x, _y);
                g.DrawString(str_char, FontList[Next(FontList.Count - 1)], newBrush, thePos);
            }

            //画点
            for (int i = 0; i < 10; i++)
            {
                var x = Next(image.Width - 1);
                var y = Next(image.Height - 1);
                image.SetPixel(x,y,Color.FromArgb(Next(255),Next(255),Next(255)));
            }

            image = TwistImage(image, true, Next(1, 3), Next(4, 6));
            g.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, imageWidth - 1, OneHeight - 1);
            g.Dispose();
            return image;
        }
    }
}
