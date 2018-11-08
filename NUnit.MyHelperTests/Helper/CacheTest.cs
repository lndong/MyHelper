using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyHelper.Caches;

namespace NUnit.MyHelperTests.Helper
{
    [TestFixture]
    public class CacheTest
    {

        private ICache _cache;

        [Test]
        public void SystemCacheSetAbsoluteTest()
        {
            _cache = new SystemCache();
            var spanTime = new TimeSpan(0, 0, 3);
            _cache.SetCache("001","aaa",spanTime,ExpireType.Absolute);
            Thread.Sleep(2*1000);
            var value = _cache.GetCache("001");
            Console.WriteLine(value);
            Thread.Sleep(2 * 1000);
            var value2 = _cache.GetCache("001");
            Console.WriteLine("==========");
            Console.WriteLine(value2);
            Assert.IsNull(value2);
        }


        [Test]
        public void SystemCacheRelativeTest()
        {
            _cache = new SystemCache();
            var spanTime = new TimeSpan(0, 0, 3);
            _cache.SetCache("001", "aaa", spanTime, ExpireType.Relative);
            Thread.Sleep(2 * 1000);
            var value = _cache.GetCache("001");
            Console.WriteLine(value);
            Thread.Sleep(2 * 1000);
            var value2 = _cache.GetCache("001");
            Console.WriteLine("==========");
            Console.WriteLine(value2);
            Assert.AreEqual(value,value2);
        }

        [Test]
        public void HttpCacheAbsoluteTest()
        {
            _cache = new HttpCache();
            var spanTime = new TimeSpan(0, 0, 3);
            _cache.SetCache("001", "aaa", spanTime, ExpireType.Absolute);
            Thread.Sleep(2 * 1000);
            var value = _cache.GetCache("001");
            Console.WriteLine(value);
            Thread.Sleep(2 * 1000);
            var value2 = _cache.GetCache("001");
            Console.WriteLine("==========");
            Console.WriteLine(value2);
            Assert.IsNull(value2);
        }

        [Test]
        public void HttpCacheRelativeTest()
        {
            _cache = new HttpCache();
            var spanTime = new TimeSpan(0, 0, 3);
            _cache.SetCache("001", "aaa", spanTime, ExpireType.Relative);
            Thread.Sleep(2 * 1000);
            var value = _cache.GetCache("001");
            Console.WriteLine(value);
            Thread.Sleep(2 * 1000);
            var value2 = _cache.GetCache("001");
            Console.WriteLine("==========");
            Console.WriteLine(value2);
            Assert.AreEqual(value, value2);
        }

        [TearDown]
        public void TearDown()
        {
            _cache = null;
        }
    }
}
