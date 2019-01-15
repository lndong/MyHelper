using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHelper.Extensions;

namespace NUnit.MyHelperTests
{
    [TestFixture]
    public class DistinctExtensionsTest
    {
        private IList<A> aList = null;

        [Test]
        public void DisTinctByReturn()
        {
            Assert.AreEqual(9, aList.Count());
            foreach (var item in aList)
            {
                Console.WriteLine(item.Id + item.Name + item.Age);
            }
            var list = aList.DisTinctBy(x => x.Id).ToList();
            Console.WriteLine("b");
            var count = list.Count();
            foreach (var item in list)
            {
                Console.WriteLine(item.Id + item.Name + item.Age);
            }
            Assert.AreEqual(6, count);
        }

        [SetUp]
        public void Setup()
        {
            aList = new List<A>();
            for (var i = 0; i < 5; i++)
            {
                var a = new A
                {
                    Id = i,
                    Name = "aa" + i.ToString(),
                    Age = i + 1
                };
                aList.Add(a);
            }

            for (var i = 0; i < 4; i++)
            {
                var a = new A
                {
                    Id = i + 2,
                    Name = "bb" + i.ToString(),
                    Age = i + 2
                };
                aList.Add(a);
            }
            Console.WriteLine("a");
        }

        [TearDown]
        public void TearDown()
        {
            aList = null;
        }
    }

    public class A
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}
