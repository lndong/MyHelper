using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MyHelper.Helper;

namespace NUnit.MyHelperTests.Helper
{
    [TestFixture]
    public class EncryptHelperTest
    {
        private string _publicKey;
        private string _privateKey;

        [SetUp]
        public void Setup()
        {
            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                _publicKey = rsaProvider.ToXmlString(false);
                _privateKey = rsaProvider.ToXmlString(true);
            }
        }

        [Test]
        public void Md5EncryptReturnString()
        {
            var res = EncryptHelper.Md5Encrypt("123456", Encoding.UTF8);
            Console.WriteLine(res);
            Assert.AreEqual("e10adc3949ba59abbe56e057f20f883e", res.ToLower());
        }

        [Test]
        public void DesEncryptReturnString()
        {
            var key = "01234567";
            byte[] iv = {0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF};
            var input = "aaa";

            var encryptStr = EncryptHelper.DesEncrypt(input, key, iv);

            Console.WriteLine(encryptStr);

            var decryptStr = EncryptHelper.DesDecrypt(encryptStr, key, iv);

            Assert.AreEqual(input, decryptStr);
        }

        [Test]
        public void GenerateRsaKeysOutTest()
        {
            EncryptHelper.GenerateRsaKeys(out var publicKey,out var privateKey);

            Console.WriteLine(publicKey);
            Console.WriteLine("==========");
            Console.WriteLine(privateKey);
            Assert.IsTrue(true);
        }

        [Test]
        public void RsaEncryptAndDecrypt()
        {
            Console.WriteLine(_publicKey);
            var input = "aaaaaaa";
            var inputEncrypt = EncryptHelper.RsaEncrypt(input, _publicKey);
            Console.WriteLine("==============");
            Console.WriteLine(inputEncrypt);
            Console.WriteLine("==============");
            Console.WriteLine(_privateKey);
            var inputDecrypt = EncryptHelper.RsaDecrypt(inputEncrypt, _privateKey);
            Console.WriteLine("==============");
            Console.WriteLine(inputDecrypt);

            Assert.AreEqual(input, inputDecrypt);
        }

        [Test]
        public void RsaSignAndVerifyTest()
        {
            var input = "aaaaaaa";
            var signStr = EncryptHelper.Sign(input, _privateKey);
            Console.WriteLine(signStr);
            var b = EncryptHelper.Verify(input, signStr, _publicKey);
            Assert.IsTrue(b);
        }
    }
}
