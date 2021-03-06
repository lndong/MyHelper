﻿using NUnit.Framework;
using MyHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
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

        [Test]
        public void AndOr()
        {
            Expression<Func<A, bool>> exA = x => false;
            var exD = exA.And(x => x.Id > 5);
            exA = exA.Or(x => x.Id > 5);
            Expression<Func<A, bool>> exB = x => x.Age > 5;
            var exf = exB.And(x => true);
            exD = exD.Or(exf);
            Console.WriteLine(exD);
            var exc = exA.And(x => x.Age > 5);
            Console.WriteLine(exc);
            var ex = exA.And(exB);
            Console.WriteLine(ex);
            Expression<Func<A, bool>> exp = parameter => (false || parameter.Id > 5) && parameter.Age > 5;
            Assert.AreEqual(exp.ToString(), ex.ToString());
        }

        /// <summary>
        /// Func<T,bool> add
        /// </summary>
        [Test]
        public void FuncTest()
        {
            Func<A, bool> funcA = (x => true);
            funcA += (x => x.Id > 10);
            funcA += (x => x.Age > 5);
            var aRes = aList.Where(funcA).ToList();
            var bRes = aList.Where(x => x.Id > 10 && x.Age > 5).ToList();

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