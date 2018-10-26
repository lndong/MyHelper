using NUnit.Framework;
using MyHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NUnit.MyHelperTests
{
    [TestFixture()]
    public class ExpressionExtensionsTest
    {
        private IList<A> aList = null;

        [Test()]
        public void AndReturn()
        {
            Expression<Func<A, bool>> exA = parameter => true;
            exA = exA.And(parameter => parameter.Id > 5);
            exA = exA.And(parameter => parameter.Age > 5);
            Expression<Func<A, bool>> exB = parameter => true && parameter.Id > 5 && parameter.Age > 5;

            Assert.AreEqual(exB.ToString(), exA.ToString());
        }

        [Test]
        public void OrExpression()
        {
            Expression<Func<A, bool>> exA = x => true;
            exA = exA.Or(x => x.Id > 5);
            exA = exA.Or(x => x.Age > 5);
            Expression<Func<A, bool>> exB = parameter => true || parameter.Id > 5 || parameter.Age > 5;
            Assert.AreEqual(exB.ToString(), exA.ToString());
        }

        /// <summary>
        /// Func<T,bool> add
        /// </summary>
        [Test]
        public void FuncTest()
        {
            Func<A, bool> funcA = (x => true);
            funcA += (x => x.Id > 10);
            var aRes = aList.Where(funcA).ToList();

            var bRes = aList.Where(x => x.Id > 10).ToList();

            Assert.AreEqual(aRes.Count(), bRes.Count());
        }

        [SetUp]
        public void Setup()
        {
            aList = new List<A>();
            for (var i = 0; i < 15; i++)
            {
                var a = new A
                {
                    Id = i,
                    Name = "aa" + i.ToString(),
                    Age = i + 1
                };
                aList.Add(a);
            }
        }

    }


}