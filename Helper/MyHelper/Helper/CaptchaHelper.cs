using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper.Helper
{
    /// <summary>
    /// 验证码帮助类
    /// </summary>
    public static class CaptchaHelper
    {
        private static readonly char[] NumList = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }; //纯数字

        /// <summary>
        /// 纯字母
        /// </summary>
        private static readonly char[] AlphabetList =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
            'V', 'W', 'X', 'Y', 'Z'
        };

        /// <summary>
        /// 数字+字母
        /// </summary>
        private static readonly char[] MixedList =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L',
            'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        /// <summary>
        /// Random对象
        /// </summary>
        private static readonly Random MyRandom = new Random(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// 生成纯数字验证码
        /// </summary>
        /// <param name="codeLength">验证码长度</param>
        /// <returns></returns>
        public static string GeneratorIntCode(int codeLength)
        {
            return GeneratorCode(NumList, codeLength);
        }

        /// <summary>
        /// 生成纯字母验证码
        /// </summary>
        /// <param name="codeLength">验证码长度</param>
        /// <returns></returns>
        public static string GeneratorACode(int codeLength)
        {
            return GeneratorCode(AlphabetList, codeLength);
        }

        /// <summary>
        /// 生成混合字符验证码（包含数字和字母）
        /// </summary>
        /// <param name="codeLength">验证码长度</param>
        /// <returns></returns>
        public static string GeneratorMixtedCode(int codeLength)
        {
            return GeneratorCode(MixedList, codeLength);
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="array">验证码可选字符数组</param>
        /// <param name="codeLength">验证码长度</param>
        /// <returns>返回验证码</returns>
        public static string GeneratorCode(char[] array, int codeLength)
        {
            var sb = new StringBuilder();
            var arrayLength = array.Length;
            for (var i = 0; i < codeLength; i++)
            {
                var index = MyRandom.Next(0,arrayLength);
                sb.Append(array[index]);
            }
            return sb.ToString();
        }
    }
}
