using NUnit.Framework;
using MyHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Description = System.ComponentModel.DescriptionAttribute;

namespace MyHelper.Extensions.Tests
{
    [TestFixture]
    public class EnumExtensionsTest
    {
        [Test]
        public void GetDescriptionReturnStr()
        {
            var redDes = MyColor.Red.GetDescription();
            Console.WriteLine(redDes);
            Assert.AreEqual("红色", redDes);

            var greenDes = MyColor.Green.GetDescription();
            Assert.AreEqual("green", greenDes.ToLower());
            //Assert.Pass(greenDes);
        }

        [Test]
        public void EnumListDicReturnDic()
        {
            var dic = MyColor.Black.EnumListDic(null);
            foreach (var item in dic)
            {
                Console.WriteLine("key:" + item.Key + "  value:" + item.Value);
            }
            Assert.AreEqual(6, dic.Count());
        }

        [Test]
        public void EnumListDicDefaultReturnDic()
        {
            var dic = MyColor.Black.EnumListDic("-1", "请选择");
            foreach(var key in dic.Keys)
            {
                Console.WriteLine("key:" + key + "  value:" + dic[key]);
            }
            Assert.AreEqual(7, dic.Count());
        }
    }

    public enum MyColor
    {
        [@Description("红色")]
        Red,
        [@Description("蓝色")]
        Blue,
        [@Description("白色")]
        White,
        [@Description("黑色")]
        Black,
        [@Description("黄色")]
        Yellow,

        Green
    }
}