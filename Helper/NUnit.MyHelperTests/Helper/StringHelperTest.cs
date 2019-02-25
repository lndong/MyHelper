using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHelper.Extensions;

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
    }
}
