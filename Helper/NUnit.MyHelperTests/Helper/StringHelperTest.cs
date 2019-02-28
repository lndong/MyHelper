using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHelper.Extensions;
using MyHelper.Helper;

namespace NUnit.MyHelperTests.Helper
{
    [TestFixture]
    public class StringHelperTest
    {
        [Test]
        public void Base64EncodeReturnString()
        {
            // TODO: Add your test code here
            var str = "aaa";
            var res = str.Base64Encode();
            Console.WriteLine(res);
            var resStr = res.Base64Decode();
            Console.WriteLine(resStr);
            Assert.AreEqual(str,resStr);
        }

        [Test]
        public void VerfyCodeReturnInt()
        {
            bool b = false;
            for (int i = 0; i < 20; i++)
            {
                var code = CaptchaHelper.GeneratorIntCode(4);
                Console.WriteLine(code);
                b = int.TryParse(code, out var res);
                var newCode = CaptchaHelper.GeneratorMixtedCode(4);
                Console.WriteLine(newCode);
            }
           
            Assert.IsTrue(b);
        }
    }
}
