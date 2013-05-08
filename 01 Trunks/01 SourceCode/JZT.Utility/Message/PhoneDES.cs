using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace JZT.Utility.Message
{
    public class DES
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public DES()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        //默认密钥初始化向量
        private static byte[] DESIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encriptString">待加密的字符串</param>
        /// <param name="encKey">加密密钥,要求为8字节</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string Encrypt(string encriptString, string encKey)
        {
            if (encKey.Trim().Length != 8)
                encKey = "87654321";

            try
            {
                byte[] bKey = Encoding.GetEncoding("GB2312").GetBytes(encKey.Substring(0, 8));
                byte[] bIV = DESIV;
                byte[] bEncContent = Encoding.GetEncoding("GB2312").GetBytes(encriptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                //用指定的 Key 和初始化向量 (IV) 创建对称数据加密标准 (DES) 加密器对象
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write);
                cStream.Write(bEncContent, 0, bEncContent.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
                // return Encoding.GetEncoding("GB2312").GetString(mStream.ToArray());
            }
            catch
            {
                return encriptString;
            }
        }
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8字节,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string Decrypt(string decryptString, string decryptKey)
        {
            if (decryptKey.Trim().Length != 8)
                decryptKey = "87654321";
            try
            {
                byte[] bKey = Encoding.GetEncoding("GB2312").GetBytes(decryptKey);
                byte[] bIV = DESIV;
                byte[] bDecContent = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write);
                cStream.Write(bDecContent, 0, bDecContent.Length);
                cStream.FlushFinalBlock();
                return Encoding.GetEncoding("GB2312").GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
    }
}
