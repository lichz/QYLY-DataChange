//========================================
// Copyright © 2017
// 
// CLR版本 	: 4.0.30319.42000
// 计算机  	: USER-20170420WC
// 文件名  	: Encryption.cs
// 创建人  	: kaifa5
// 创建时间	: 2017/9/25 10:21:03
// 文件版本	: 1.0.0
// 文件描述	: 
//========================================

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RoadFlow.Utility.New {
    /// <summary>
    /// 指示一种SHA签名算法模式。
    /// </summary>
    public enum ShaMode {
        /// <summary>
        /// SHA1模式（签名结果为40位）。
        /// </summary>
        SHA1 = 0x28,
        /// <summary>
        /// SHA256模式（签名结果为64位）。
        /// </summary>
        SHA256 = 0x40,
        /// <summary>
        /// SHA384模式（签名结果为96位）。
        /// </summary>
        SHA384 = 0x60,
        /// <summary>
        /// SHA512模式（签名结果为128位）。
        /// </summary>
        SHA512 = 0x80
    }

    /// <summary>
    /// 加解密静态封装类。
    /// </summary>
    public static class Encryption {
        /// <summary>
        /// AES加解密默认秘钥（16位）。
        /// </summary>
        private const string aesKey = "j0wVr%+BO~AQEg3A";
        /// <summary>
        /// DES加解密默认秘钥（8位）。
        /// </summary>
        private const string desKey = "=?Rs%F5f";

        /// <summary>
        /// 计算加解密秘钥字节数组。
        /// </summary>
        /// <param name="key">用于加解密的字符串秘钥。</param>
        /// <param name="length">加解密所需的秘钥长度。</param>
        /// <param name="encoding">字符串的编码方式。</param>
        /// <returns>加解密所需的秘钥字节数组。</returns>
        /// <exception cref="ArgumentException">参数key为空时将抛出该异常。</exception>
        /// <exception cref="ArgumentNullException">参数encoding为null时将抛出该异常。</exception>
        private static byte[] ComputeKey(string key, int length, Encoding encoding) {
            if (string.IsNullOrEmpty(key)) {
                throw new ArgumentException("秘钥不能为空!", "key");
            }
            if (encoding == null) {
                throw new ArgumentNullException("encoding");
            }
            byte[] result = encoding.GetBytes(key);
            int keyLength = result.Length;
            if (keyLength < length) {
                //若长度不足，循环拼接字节。
                byte[] buffer = new byte[length];
                for (int i = 0; i < length; i++) {
                    buffer[i] = result[i % keyLength];
                }
                result = buffer;
            } else if (keyLength > length) {
                //若长度超出，按平均步长取相应字节。
                int skip = keyLength / length;
                byte[] buffer = new byte[length];
                for (int i = 0; i < length; i++) {
                    buffer[i] = result[i * skip];
                }
                result = buffer;
            }
            return result;
        }

        /// <summary>
        /// 创建指定模式的SHA算法提供对象。
        /// </summary>
        /// <param name="mode">SHA算法模式。</param>
        /// <returns>指定模式的SHA算法提供对象。</returns>
        /// <exception cref="ArgumentException">传入不能识别的ShaMode时将抛出该异常。</exception>
        private static HashAlgorithm CreateShaProvider(ShaMode mode) {
            switch (mode) {
                case ShaMode.SHA1:
                    return new SHA1CryptoServiceProvider();
                case ShaMode.SHA256:
                    return new SHA256CryptoServiceProvider();
                case ShaMode.SHA384:
                    return new SHA384CryptoServiceProvider();
                case ShaMode.SHA512:
                    return new SHA512CryptoServiceProvider();
            }
            throw new ArgumentException("不存在该模式!", "mode");
        }

        #region ====对称加解密算法====

        #region ====AES加解密====
        /// <summary>
        /// 采用默认秘钥和UTF-8编码对字符串进行AES解密。
        /// </summary>
        /// <param name="input">需要解密的字符串。</param>
        /// <returns>解密后的字符串。</returns>
        public static string AesDecrypt(string input) {
            return Encryption.AesDecrypt(Encryption.aesKey, input);
        }

        /// <summary>
        /// 以指定的秘钥并采用UTF-8编码对字符串进行AES解密。
        /// </summary>
        /// <param name="key">解密使用的字符串秘钥。</param>
        /// <param name="value">需要解密的字符串。</param>
        /// <returns>解密后的字符串。</returns>
        /// <exception cref="ArgumentException">参数key为空时将抛出该异常。</exception>
        public static string AesDecrypt(string key, string value) {
            return Encryption.AesDecrypt(key, value, Encoding.UTF8);
        }

        /// <summary>
        /// 以指定的秘钥和编码方式对字符串进行AES解密。
        /// </summary>
        /// <param name="key">解密使用的字符串秘钥。</param>
        /// <param name="value">需要解密的字符串。</param>
        /// <param name="encoding">字符串使用的编码方式。</param>
        /// <returns>解密后的字符串。</returns>
        /// <exception cref="ArgumentException">参数key为空时将抛出该异常。</exception>
        /// <exception cref="ArgumentNullException">参数encoding为null时将抛出该异常。</exception>
        public static string AesDecrypt(string key, string value, Encoding encoding) {
            if (string.IsNullOrEmpty(key)) {
                return string.Empty;
            }
            byte[] keyBuffer = Encryption.ComputeKey(key, 16, encoding);
            string result;
            using (Aes provider = new AesCryptoServiceProvider()) {
                using (MemoryStream memory = new MemoryStream()) {
                    using (ICryptoTransform transform = provider.CreateDecryptor(keyBuffer, keyBuffer)) {
                        using (CryptoStream crypto = new CryptoStream(memory, transform, CryptoStreamMode.Write)) {
                            byte[] buffer = Convert.FromBase64String(value);
                            crypto.Write(buffer, 0, buffer.Length);
                        }
                    }
                    result = encoding.GetString(memory.ToArray());
                }
            }
            return result;
        }

        /// <summary>
        /// 采用默认秘钥和UTF-8编码对字符串进行AES加密。
        /// </summary>
        /// <param name="input">需要加密的字符串。</param>
        /// <returns>加密后的字符串。</returns>
        public static string AesEncrypt(string input) {
            return Encryption.AesEncrypt(Encryption.aesKey, input);
        }

        /// <summary>
        /// 以指定的秘钥并采用UTF-8编码对字符串进行AES加密。
        /// </summary>
        /// <param name="key">加密使用的字符串秘钥。</param>
        /// <param name="value">需要加密的字符串。</param>
        /// <returns>加密后的字符串。</returns>
        /// <exception cref="ArgumentException">参数key为空时将抛出该异常。</exception>
        public static string AesEncrypt(string key, string value) {
            return Encryption.AesEncrypt(key, value, Encoding.UTF8);
        }

        /// <summary>
        /// 以指定的秘钥和编码方式对字符串进行AES加密。
        /// </summary>
        /// <param name="key">加密使用的字符串秘钥。</param>
        /// <param name="value">需要加密的字符串。</param>
        /// <param name="encoding">字符串使用的编码方式。</param>
        /// <returns>加密后的字符串。</returns>
        /// <exception cref="ArgumentException">参数key为空时将抛出该异常。</exception>
        /// <exception cref="ArgumentNullException">参数encoding为null时将抛出该异常。</exception>
        public static string AesEncrypt(string key, string value, Encoding encoding) {
            if (string.IsNullOrEmpty(key)) {
                return string.Empty;
            }
            byte[] keyBuffer = Encryption.ComputeKey(key, 16, encoding);
            string result;
            using (Aes provider = new AesCryptoServiceProvider()) {
                using (MemoryStream memory = new MemoryStream()) {
                    using (ICryptoTransform transform = provider.CreateEncryptor(keyBuffer, keyBuffer)) {
                        using (CryptoStream crypto = new CryptoStream(memory, transform, CryptoStreamMode.Write)) {
                            byte[] buffer = encoding.GetBytes(value);
                            crypto.Write(buffer, 0, buffer.Length);
                        }
                    }
                    result = Convert.ToBase64String(memory.ToArray());
                }
            }
            return result;
        }
        #endregion

        #region ====Base64编码解码====
        /// <summary>
        /// 以UTF-8编码对字符串进行Base64解码。
        /// </summary>
        /// <param name="input">需要解码的字符串。</param>
        /// <returns>解码后的字符串。</returns>
        public static string Base64Decode(string input) {
            return Encryption.Base64Decode(input, Encoding.UTF8);
        }

        /// <summary>
        /// 以指定的编码方式对字符串进行Base64解码。
        /// </summary>
        /// <param name="input">需要解码的字符串。</param>
        /// <param name="encoding">原字符串的编码方式。</param>
        /// <returns>解码后的字符串。</returns>
        /// <exception cref="ArgumentNullException">参数encoding为null时将抛出该异常。</exception>
        public static string Base64Decode(string input, Encoding encoding) {
            if (string.IsNullOrEmpty(input)) {
                return string.Empty;
            }
            if (encoding == null) {
                throw new ArgumentNullException("encoding");
            }
            byte[] buffer = Convert.FromBase64String(input);
            return encoding.GetString(buffer);
        }

        /// <summary>
        /// 以UTF-8编码对字符串进行Base64编码。
        /// </summary>
        /// <param name="input">需要编码的字符串。</param>
        /// <returns>编码后的字符串。</returns>
        public static string Base64Encode(string input) {
            return Encryption.Base64Encode(input, Encoding.UTF8);
        }

        /// <summary>
        /// 以指定的编码方式对字符串进行Base64编码。
        /// </summary>
        /// <param name="input">需要编码的字符串。</param>
        /// <param name="encoding">字符串的编码方式。</param>
        /// <returns>编码后的字符串。</returns>
        /// <exception cref="ArgumentNullException">参数encoding为null时将抛出该异常。</exception>
        public static string Base64Encode(string input, Encoding encoding) {
            if (string.IsNullOrEmpty(input)) {
                return string.Empty;
            }
            if (encoding == null) {
                throw new ArgumentNullException("encoding");
            }
            byte[] buffer = encoding.GetBytes(input);
            return Convert.ToBase64String(buffer);
        }
        #endregion

        #region ====DES加解密====
        /// <summary>
        /// 采用默认秘钥和UTF-8编码对字符串进行DES解密。
        /// </summary>
        /// <param name="input">需要解密的字符串。</param>
        /// <returns>解密后的字符串。</returns>
        public static string DesDecrypt(string input) {
            return Encryption.DesDecrypt(Encryption.desKey, input);
        }

        /// <summary>
        /// 以指定的秘钥并采用UTF-8编码对字符串进行DES解密。
        /// </summary>
        /// <param name="key">解密使用的字符串秘钥。</param>
        /// <param name="value">需要解密的字符串。</param>
        /// <returns>解密后的字符串。</returns>
        /// <exception cref="ArgumentException">参数key为空时将抛出该异常。</exception>
        public static string DesDecrypt(string key, string value) {
            return Encryption.DesDecrypt(key, value, Encoding.UTF8);
        }

        /// <summary>
        /// 以指定的秘钥和编码方式对字符串进行DES解密。
        /// </summary>
        /// <param name="key">解密使用的字符串秘钥。</param>
        /// <param name="value">需要解密的字符串。</param>
        /// <param name="encoding">字符串的编码方式。</param>
        /// <returns>解密后的字符串。</returns>
        /// <exception cref="ArgumentException">参数key为空时将抛出该异常。</exception>
        /// <exception cref="ArgumentNullException">参数encoding为null时将抛出该异常。</exception>
        public static string DesDecrypt(string key, string value, Encoding encoding) {
            if (string.IsNullOrEmpty(key)) {
                return string.Empty;
            }
            byte[] keyBuffer = Encryption.ComputeKey(key, 8, encoding);
            string result;
            using (DES provider = new DESCryptoServiceProvider()) {
                using (MemoryStream memory = new MemoryStream()) {
                    using (ICryptoTransform transform = provider.CreateDecryptor(keyBuffer, keyBuffer)) {
                        using (CryptoStream crypto = new CryptoStream(memory, transform, CryptoStreamMode.Write)) {
                            byte[] buffer = Convert.FromBase64String(value);
                            crypto.Write(buffer, 0, buffer.Length);
                        }
                    }
                    result = encoding.GetString(memory.ToArray());
                }
            }
            return result;
        }

        /// <summary>
        /// 采用默认的秘钥和UTF-8编码对字符串进行DES加密。
        /// </summary>
        /// <param name="input">需要加密的字符串。</param>
        /// <returns>加密后的字符串。</returns>
        public static string DesEncrypt(string input) {
            return Encryption.DesEncrypt(Encryption.desKey, input);
        }

        /// <summary>
        /// 以指定的秘钥并采用UTF-8编码对字符串进行DES加密。
        /// </summary>
        /// <param name="key">加密使用的字符串秘钥。</param>
        /// <param name="value">需要加密的字符串。</param>
        /// <returns>加密后的字符串。</returns>
        /// <exception cref="ArgumentException">参数key为空时将抛出该异常。</exception>
        public static string DesEncrypt(string key, string value) {
            return Encryption.DesEncrypt(key, value, Encoding.UTF8);
        }

        /// <summary>
        /// 以指定的秘钥和编码方式对字符串进行DES加密。
        /// </summary>
        /// <param name="key">加密使用的字符串秘钥。</param>
        /// <param name="value">需要加密的字符串。</param>
        /// <param name="encoding">字符串使用的编码方式。</param>
        /// <returns>加密后的字符串。</returns>
        /// <exception cref="ArgumentException">参数key为空时将抛出该异常。</exception>
        /// <exception cref="ArgumentNullException">参数encoding为null时将抛出该异常。</exception>
        public static string DesEncrypt(string key, string value, Encoding encoding) {
            if (string.IsNullOrEmpty(key)) {
                return string.Empty;
            }
            byte[] keyBuffer = Encryption.ComputeKey(key, 8, encoding);
            string result;
            using (DES provider = new DESCryptoServiceProvider()) {
                using (MemoryStream memory = new MemoryStream()) {
                    using (ICryptoTransform transform = provider.CreateEncryptor(keyBuffer, keyBuffer)) {
                        using (CryptoStream crypto = new CryptoStream(memory, transform, CryptoStreamMode.Write)) {
                            byte[] buffer = encoding.GetBytes(value);
                            crypto.Write(buffer, 0, buffer.Length);
                        }
                    }
                    result = Convert.ToBase64String(memory.ToArray());
                }
            }
            return result;
        }
        #endregion

        #endregion

        #region ====非对称签名算法====

        #region ====计算MD5摘要====
        /// <summary>
        /// 以UTF-8编码方式计算字符串的32位MD5值。
        /// </summary>
        /// <param name="input">需要计算MD5值的字符串。</param>
        /// <returns>字符串对应的32位MD5值。</returns>
        public static string ComputeMd5(string input) {
            return Encryption.ComputeMd5(input, Encoding.UTF8);
        }

        /// <summary>
        /// 计算IO流的32位MD5值。
        /// </summary>
        /// <param name="stream">需要计算MD5值的IO流。</param>
        /// <returns>IO流对应的32位MD5值。</returns>
        /// <exception cref="ArgumentNullException">参数stream为null时将抛出该异常。</exception>
        public static string ComputeMd5(Stream stream) {
            if (stream == null) {
                throw new ArgumentNullException("stream");
            }
            StringBuilder builder = new StringBuilder(32);
            using (MD5 provider = new MD5CryptoServiceProvider()) {
                byte[] result = provider.ComputeHash(stream);
                for (int i = 0; i < result.Length; i++) {
                    builder.Append(result[i].ToString("X2"));
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 以指定的编码方式计算字符串的32位MD5值。
        /// </summary>
        /// <param name="input">需要计算MD5值的字符串。</param>
        /// <param name="encoding">字符串的编码方式。</param>
        /// <returns>字符串对应的32位MD5值。</returns>
        /// <exception cref="ArgumentNullException">参数encoding为null时将抛出该异常。</exception>
        public static string ComputeMd5(string input, Encoding encoding) {
            if (string.IsNullOrEmpty(input)) {
                return string.Empty.PadLeft(32, '0');
            }
            if (encoding == null) {
                throw new ArgumentNullException("encoding");
            }
            byte[] buffer = encoding.GetBytes(input);
            StringBuilder builder = new StringBuilder(32);
            using (MD5 provider = new MD5CryptoServiceProvider()) {
                byte[] result = provider.ComputeHash(buffer);
                for (int i = 0; i < result.Length; i++) {
                    builder.Append(result[i].ToString("X2"));
                }
            }
            return builder.ToString();
        }
        #endregion

        #region ====计算SHA摘要====
        /// <summary>
        /// 以UTF-8编码计算字符串的40位SHA1值。
        /// </summary>
        /// <param name="input">需要计算SHA1值的字符串。</param>
        /// <returns>字符串对应的40位SHA1值</returns>
        public static string ComputeSha(string input) {
            return Encryption.ComputeSha(input, Encoding.UTF8);
        }

        /// <summary>
        /// 计算IO流的128位SHA512值。
        /// </summary>
        /// <param name="stream">需要计算SHA512值的IO流。</param>
        /// <returns>IO流对应的128位SHA512值。</returns>
        /// <exception cref="ArgumentNullException">参数stream为null时将抛出该异常。</exception>
        public static string ComputeSha(Stream stream) {
            return Encryption.ComputeSha(stream, ShaMode.SHA512);
        }

        /// <summary>
        /// 以指定的编码方式计算字符串的40位SHA1值。
        /// </summary>
        /// <param name="input">需要计算SHA1值的字符串。</param>
        /// <param name="encoding">字符串的编码方式。</param>
        /// <returns>字符串对应的40位SHA1值。</returns>
        /// <exception cref="ArgumentNullException">参数encoding为null时将抛出该异常。</exception>
        public static string ComputeSha(string input, Encoding encoding) {
            return Encryption.ComputeSha(input, encoding, ShaMode.SHA1);
        }

        /// <summary>
        /// 以UTF-8编码计算字符串指定模式的SHA值。
        /// </summary>
        /// <param name="input">需要计算SHA值的字符串。</param>
        /// <param name="mode">计算SHA的模式。</param>
        /// <returns>字符串对应的SHA值。</returns>
        /// <exception cref="ArgumentException">传入不能识别的ShaMode时将抛出该异常。</exception>
        public static string ComputeSha(string input, ShaMode mode) {
            return Encryption.ComputeSha(input, Encoding.UTF8, mode);
        }

        /// <summary>
        /// 计算IO流指定模式的SHA值。
        /// </summary>
        /// <param name="stream">需要计算SHA值的IO流。</param>
        /// <param name="mode">计算SHA的模式。</param>
        /// <returns>IO流对应的SHA值。</returns>
        /// <exception cref="ArgumentNullException">参数stream为null时将抛出该异常。</exception>
        /// <exception cref="ArgumentException">传入不能识别的ShaMode时将抛出该异常。</exception>
        public static string ComputeSha(Stream stream, ShaMode mode) {
            if (stream == null) {
                throw new ArgumentNullException("stream");
            }
            int length = Convert.ToInt32(mode);
            StringBuilder builder = new StringBuilder(length);
            using (HashAlgorithm provider = Encryption.CreateShaProvider(mode)) {
                byte[] result = provider.ComputeHash(stream);
                for (int i = 0; i < result.Length; i++) {
                    builder.Append(result[i].ToString("X2"));
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 以指定的编码方式计算字符串指定模式的SHA值。
        /// </summary>
        /// <param name="input">需要计算SHA值的字符串。</param>
        /// <param name="encoding">字符串的编码方式。</param>
        /// <param name="mode">计算SHA的模式。</param>
        /// <returns>字符串对应的SHA值。</returns>
        /// <exception cref="ArgumentNullException">参数encoding为null时将抛出该异常。</exception>
        /// <exception cref="ArgumentException">传入不能识别的ShaMode时将抛出该异常。</exception>
        public static string ComputeSha(string input, Encoding encoding, ShaMode mode) {
            int length = Convert.ToInt32(mode);
            if (string.IsNullOrEmpty(input)) {
                return string.Empty.PadLeft(length, '0');
            }
            if (encoding == null) {
                throw new ArgumentNullException("encoding");
            }
            byte[] buffer = encoding.GetBytes(input);
            StringBuilder builder = new StringBuilder(length);
            using (HashAlgorithm provider = Encryption.CreateShaProvider(mode)) {
                byte[] result = provider.ComputeHash(buffer);
                for (int i = 0; i < result.Length; i++) {
                    builder.Append(result[i].ToString("X2"));
                }
            }
            return builder.ToString();
        }
        #endregion

        #endregion
    }
}