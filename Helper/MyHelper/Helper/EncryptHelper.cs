using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace MyHelper.Helper
{
    /// <summary>
    ///     加密帮助类
    /// </summary>
    public class EncryptHelper
    {
        #region 通用hash加密算法

        /// <summary>
        ///     哈希加密算法
        /// </summary>
        /// <param name="hashAlgorithm"> 所有加密哈希算法实现均必须从中派生的基类 </param>
        /// <param name="input"> 待加密的字符串 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        private static string HashEncrypt(HashAlgorithm hashAlgorithm, string input, Encoding encoding)
        {
            var data = hashAlgorithm.ComputeHash(encoding.GetBytes(input));

            return BitConverter.ToString(data).Replace("-", "");
        }

        /// <summary>
        ///     验证哈希值
        /// </summary>
        /// <param name="hashAlgorithm"> 所有加密哈希算法实现均必须从中派生的基类 </param>
        /// <param name="unhashedText"> 未加密的字符串 </param>
        /// <param name="hashedText"> 经过加密的哈希值 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        private static bool VerifyHashValue(HashAlgorithm hashAlgorithm, string unhashedText, string hashedText,
            Encoding encoding)
        {
            return string.Equals(HashEncrypt(hashAlgorithm, unhashedText, encoding), hashedText,
                StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region 哈希加密算法（此算法结果是不可逆的）

        #region MD5 加密

        /// <summary>
        ///     MD5加密
        /// </summary>
        /// <param name="input">待加密字符串</param>
        /// <param name="encodeType">字符编码</param>
        /// <returns>加密后字符串</returns>
        public static string Md5Encrypt(string input, Encoding encodeType)
        {
            return HashEncrypt(MD5.Create(), input, encodeType);
        }

        #endregion

        #region SHA1 加密(SHA256,SHA384,SHA512 方式一样)

        /// <summary>
        ///     SHA1 算法
        /// </summary>
        /// <param name="input">待加密字符串</param>
        /// <param name="encoding">字符编码</param>
        /// <returns>机密后字符串</returns>
        public static string Sha1Encrypt(string input, Encoding encoding)
        {
            return HashEncrypt(SHA1.Create(), input, encoding);
        }

        #endregion

        #region  计算文件的hash值(使用sha1)

        /// <summary>
        /// 计算文件的hash值
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ComputeFileHash(string filePath)
        {
            var hashRes = string.Empty;
            if (File.Exists(filePath))
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var sha1 = SHA1.Create();
                    var buff = sha1.ComputeHash(fs);
                    hashRes = BitConverter.ToString(buff).Replace("-", "");
                }
            }

            return hashRes;
        }

        #endregion

        #region HMAC-MD5 加密(HMAC-SHA1,HMAC-SHA256,HMAC-SHA384,HMAC-SHA512等同)

        /// <summary>
        ///     HMAC-MD5 加密
        /// </summary>
        /// <param name="input"> 要加密的字符串 </param>
        /// <param name="key"> 密钥 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public static string HmacMd5Encrypt(string input, string key, Encoding encoding)
        {
            return HashEncrypt(new HMACMD5(encoding.GetBytes(key)), input, encoding);
        }

        #endregion

        #endregion

        #region 对称加密算法

        #region DES 加解密

        /// <summary>
        ///     Des加密
        /// </summary>
        /// <param name="source">待加密字符串</param>
        /// <param name="key">des秘钥，长度必须8位(超出截取，不够补齐)</param>
        /// <param name="iv">秘钥向量</param>
        /// 加密单位长度为8字节（向量为8位）
        /// private static readonly byte[] IvBytes = {0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF};
        /// <returns></returns>
        public static string DesEncrypt(string source, string key, byte[] iv)
        {
            using (var desProvider = new DESCryptoServiceProvider())
            {
                var keyByte = Encoding.UTF8.GetBytes(key);
                var inputByteArray = Encoding.UTF8.GetBytes(source);
                using (var ms = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(ms, desProvider.CreateEncryptor(keyByte, iv),
                        CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                        cryptoStream.FlushFinalBlock();

                        //1.第一种
                        return Convert.ToBase64String(ms.ToArray());

                        // 2.第二种
                        //StringBuilder result = new StringBuilder();
                        //foreach (byte b in memoryStream.ToArray())
                        //{
                        //    result.AppendFormat("{0:X2}", b);
                        //}
                        //return result.ToString();
                    }
                }
            }
        }

        /// <summary>
        ///     Des解密
        /// </summary>
        /// <param name="source">加密后的base64字符串</param>
        /// <param name="key">des密钥，长度必须8位</param>
        /// <param name="iv">密钥向量</param>
        /// <returns></returns>
        public static string DesDecrypt(string source, string key, byte[] iv)
        {
            using (var desProvider = new DESCryptoServiceProvider())
            {
                var byteKey = Encoding.UTF8.GetBytes(key);
                var inputByteArray = Convert.FromBase64String(source);

                using (var ms = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(ms, desProvider.CreateDecryptor(byteKey, iv),
                        CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                        cryptoStream.FlushFinalBlock();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
        }

        #endregion

        #region AES 加解密

        /// <summary>
        ///     AES加密
        /// </summary>
        /// <param name="input">待加密明文</param>
        /// <param name="key">密钥(16位/24位/32位)</param>
        /// <param name="iv">向量(16字节)</param>
        /// byte[] AES_IV ={ 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90,0xAB, 0xCD, 0xEF };
        /// <returns></returns>
        public static string AesEncrypt(string input, string key, byte[] iv)
        {
            var aes = new AesCryptoServiceProvider();
            var byteKey = Encoding.UTF8.GetBytes(key);
            var inputByteArray = Encoding.UTF8.GetBytes(input);
            using (var crytoTransform = aes.CreateEncryptor(byteKey, iv))
            {
                var res = crytoTransform.TransformFinalBlock(inputByteArray, 0, inputByteArray.Length);
                aes.Clear();
                return Convert.ToBase64String(res, 0, res.Length).Replace("+", "%2B");
                ;
            }
        }

        /// <summary>
        ///     AES 解密
        /// </summary>
        /// <param name="input">待解密密文</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns></returns>
        public static string AesDecrypt(string input, string key, byte[] iv)
        {
            var inputByteArray = Convert.FromBase64String(input.Replace("%2B", "+"));
            var aesProvider = new AesCryptoServiceProvider();
            var byteKey = Encoding.UTF8.GetBytes(key);
            using (var crytoTransform = aesProvider.CreateDecryptor(byteKey, iv))
            {
                var res = crytoTransform.TransformFinalBlock(inputByteArray, 0, inputByteArray.Length);
                aesProvider.Clear();
                return Encoding.UTF8.GetString(res).Replace("\0", "");
            }
        }

        #endregion

        #endregion

        #region RSA非对称加密

        /// <summary>
        ///     生成RSA 公钥和私钥
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="privatekey">私钥</param>
        public static void GenerateRsaKeys(out string publicKey, out string privatekey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                publicKey = rsa.ToXmlString(false);
                privatekey = rsa.ToXmlString(true);
            }
        }

        /// <summary>
        ///     把公钥导出
        /// </summary>
        /// <param name="fileName">文件目录</param>
        public static void SaveKeyFile(string fileName)
        {
            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                var publicKey = rsaProvider.ToXmlString(false); //公钥
                var keyByte = Encoding.UTF8.GetBytes(publicKey);
                fs.Write(keyByte, 0, keyByte.Length);
                fs.Dispose();
            }
        }

        /// <summary>
        ///     导入公钥
        /// </summary>
        /// <param name="fileName">文件目录</param>
        public static void LoadKeyFromFile(string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var data = new byte[fs.Length];
            fs.Read(data, 0, (int)fs.Length);
            fs.Dispose();
            var rsaProvider = new RSACryptoServiceProvider();
            rsaProvider.FromXmlString(Encoding.UTF8.GetString(data));
        }

        #region 使用证书生成RSACryptoServiceProvider对象

        //var myX509 = new X509Certificate2();
        //var deRsaProvider = myX509.PrivateKey as RSACryptoServiceProvider;
        //var RsaRrovider = myX509.PublicKey.Key as RSACryptoServiceProvider;

        #endregion

        #region 公钥加密私钥解密

        /// <summary>
        ///     RSA加密 (公钥/分块加密)
        /// </summary>
        /// <param name="input">待加密明文</param>
        /// <param name="publicKey">加密密钥</param>
        /// 对于加密较长的数据采用分块加密.net 规定每次加密的字节数，不能超过密钥的长度值减去11
        /// <returns>加密后的base64字符串</returns>
        public static string RsaEncrypt(string input, string publicKey)
        {
            var inputByte = Encoding.UTF8.GetBytes(input);
            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.FromXmlString(publicKey); //载入密钥
                var bufferSize = rsaProvider.KeySize / 8 - 11; //单块最大长度
                var buffer = new byte[bufferSize];
                using (MemoryStream inputStream = new MemoryStream(inputByte), outputStream = new MemoryStream())
                {
                    //分段加密
                    var readSize = inputStream.Read(buffer, 0, bufferSize);
                    while (readSize > 0)
                    {
                        var temp = new byte[bufferSize];
                        Array.Copy(buffer, 0, temp, 0, readSize);
                        var encryptedByte = rsaProvider.Encrypt(temp, false); //加密
                        outputStream.Write(encryptedByte, 0, encryptedByte.Length);
                        readSize = inputStream.Read(buffer, 0, bufferSize);
                    }

                    return Convert.ToBase64String(outputStream.ToArray());
                    // return Convert.ToBase64String(outputStream.ToArray()).Replace("+", "%2B");
                }
            }
        }

        /// <summary>
        ///     RSA 解密(私钥解密)
        /// </summary>
        /// <param name="input">加密密文</param>
        /// <param name="privateKey">私钥</param>
        /// 加密后得到密文的字节数，正好是密钥的长度值除以 8
        /// <returns>解密后明文</returns>
        public static string RsaDecrypt(string input, string privateKey)
        {
            //var inputType = Convert.FromBase64String(input.Replace("%2B", "+"));
            var inputBype = Convert.FromBase64String(input);
            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.FromXmlString(privateKey);

                var bufferSize = rsaProvider.KeySize / 8; //解密块最大长度限制
                var buffer = new byte[bufferSize];
                using (MemoryStream inputStream = new MemoryStream(inputBype), outputStream = new MemoryStream())
                {
                    while (true)
                    {
                        var readSize = inputStream.Read(buffer, 0, bufferSize);
                        if (readSize <= 0) break;

                        var temp = new byte[readSize];
                        Array.Copy(buffer, 0, temp, 0, readSize);
                        var rawBytes = rsaProvider.Decrypt(temp, false);
                        outputStream.Write(rawBytes, 0, rawBytes.Length);
                    }

                    return Encoding.UTF8.GetString(outputStream.ToArray()).Replace("\0", "");
                }
            }
        }

        #endregion

        #region 私钥签名，公钥验签

        /// <summary>
        ///     RSA私钥签名
        /// </summary>
        /// <param name="input">需要签名内容</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static string Sign(string input, string privateKey)
        {
            var inputByte = Encoding.UTF8.GetBytes(input);
            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.FromXmlString(privateKey);
                var signByte = rsaProvider.SignData(inputByte, "SHA1");
                return Convert.ToBase64String(signByte);
            }
        }

        /// <summary>
        ///     RSA公钥验签
        /// </summary>
        /// <param name="input">需要验证的内容</param>
        /// <param name="signedString">要验证的签名base64字符串</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static bool Verify(string input, string signedString, string publicKey)
        {
            var inputByte = Encoding.UTF8.GetBytes(input);
            var signedByte = Convert.FromBase64String(signedString);
            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.FromXmlString(publicKey);
                var res = rsaProvider.VerifyData(inputByte, "SHA1", signedByte);
                return res;
            }
        }

        #endregion

        #region XML密钥与PEM格式密钥互转未验证先收集待验证

        /// <summary>
        ///     XML格式密钥转PEM格式
        /// </summary>
        /// <param name="xmlPath">xml格式密钥文件路径</param>
        /// <param name="pemPath">生成pem格式密钥文件路径</param>
        public static void XmlConvertToPem(string xmlPath, string pemPath)
        {
            var rsa = new RSACryptoServiceProvider();
            using (var sr = new StreamReader(xmlPath))
            {
                rsa.FromXmlString(sr.ReadToEnd());
            }

            var p = rsa.ExportParameters(true);
            var key = new RsaPrivateCrtKeyParameters(
                new BigInteger(1, p.Modulus), new BigInteger(1, p.Exponent), new BigInteger(1, p.D),
                new BigInteger(1, p.P), new BigInteger(1, p.Q), new BigInteger(1, p.DP), new BigInteger(1, p.DQ),
                new BigInteger(1, p.InverseQ));
            using (var sw = new StreamWriter(pemPath))
            {
                var pemWriter = new PemWriter(sw);
                pemWriter.WriteObject(key);
            }
        }

        /// <summary>
        ///     PEM格式密钥转XML密钥(适用于私钥)
        /// </summary>
        /// <param name="pemPath"></param>
        /// <param name="xmlPath"></param>
        public static void PemPrivateConvertToXml(string pemPath, string xmlPath)
        {
            AsymmetricCipherKeyPair keyPair;
            using (var sr = new StreamReader(pemPath))
            {
                var pemReader = new PemReader(sr);
                keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();
            }

            var key = (RsaPrivateCrtKeyParameters)keyPair.Private;
            var p = new RSAParameters
            {
                Modulus = key.Modulus.ToByteArrayUnsigned(),
                Exponent = key.PublicExponent.ToByteArrayUnsigned(),
                D = key.Exponent.ToByteArrayUnsigned(),
                P = key.P.ToByteArrayUnsigned(),
                Q = key.Q.ToByteArrayUnsigned(),
                DP = key.DP.ToByteArrayUnsigned(),
                DQ = key.DQ.ToByteArrayUnsigned(),
                InverseQ = key.QInv.ToByteArrayUnsigned()
            };
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(p);
            using (var sw = new StreamWriter(xmlPath))
            {
                sw.Write(rsa.ToXmlString(true));
            }
        }

        /// <summary>
        ///     PEM格式密钥转XML密钥(适用于公钥)
        /// </summary>
        /// <param name="pemPath"></param>
        /// <param name="xmlPath"></param>
        public static void PemPublicConvertToXml(string pemPath, string xmlPath)
        {
            var sb = new StringBuilder();
            var sr = new StreamReader(pemPath);
            string line;
            while ((line = sr.ReadLine()) != null) sb.Append(line);
            var publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(sb.ToString()));
            var str =
                $"<RSAKeyValue><Modulus>{Convert.ToBase64String(publicKeyParam.Modulus.ToByteArrayUnsigned())}</Modulus><Exponent>{Convert.ToBase64String(publicKeyParam.Exponent.ToByteArrayUnsigned())}</Exponent></RSAKeyValue>";
            using (var sw = new StreamWriter(xmlPath))
            {
                sw.Write(str);
            }
        }

        #endregion

        #endregion
    }
}