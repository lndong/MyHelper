using NUnit.Framework;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MyHelper.Helper;

namespace NUnit.MyHelperTests.Helper
{
    [TestFixture]
    public class AppSettingHelperTest
    {
        [Test]
        public void GetInt32ReturnInt()
        {
            var i = AppSettingHelper.GetInt32("id");
            Assert.AreEqual(1, i);
            // TODO: Add your test code here
            //Assert.Pass("Your first passing test");
        }

        [Test]
        public void GetValueReturnStruct()
        {
            var res = GetInt<int>((string g, out int s) => int.TryParse(g, out s));
            Assert.AreEqual(1, res);
        }

        [Test]
        public void GetValueReturnDecmail()
        {
            var res = AppSettingHelper.GetValue<decimal>("decimal",
                (string s, out decimal v) => decimal.TryParse(s, out v), null);
            Console.WriteLine(res);
            var exp = decimal.Parse("0.2");
            Assert.AreEqual(exp, res);
        }

        [Test]
        public void GetValueReturnBool()
        {
            var res = AppSettingHelper.GetValue<bool>("bool", (string s, out bool v) => bool.TryParse(s, out v), null);
            Console.WriteLine(res);
            Assert.IsFalse(res);
        }


        private T GetInt<T>(CustomFunc<T> func) where T : struct
        {
            var s = "1";
            var i = default(T);
            if (func(s, out i))
            {
                return i;
            }

            return i;
        }


    }

    public delegate bool CustomFunc<T>(string arg1, out T arg2) where T : struct;
}
