using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var res = StringHelper.Base64Encode(str);
            Console.WriteLine(res);
            var resStr = StringHelper.Base64Decode(res);
            Console.WriteLine(resStr);
            Assert.AreEqual(str,resStr);
        }
    }
}
