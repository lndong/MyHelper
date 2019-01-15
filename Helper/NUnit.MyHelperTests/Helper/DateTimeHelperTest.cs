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
    public class DateTimeHelperTest
    {
        private readonly DateTime _dateTime = DateTime.Parse("2018-10-29 10:31:42");

        [Test]
        public void DateTimeToIntTest()
        {
            var dateTimeLong = DateTimeHelper.DateTimeToInt(_dateTime);
            Console.WriteLine(dateTimeLong);
            Assert.AreEqual(1540780302, dateTimeLong);
        }

        [Test]
        public void IntToDateTimeTest()
        {
            var timeLong = 1540780302;
            var dateTime = DateTimeHelper.IntToDateTime(timeLong);
            Console.WriteLine(dateTime);
            Assert.AreEqual(_dateTime, dateTime);
        }
    }
}
