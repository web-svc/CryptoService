namespace CryptoService
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    public class MD5
    {
        /// <summary>
        /// Encrypt a string using dual encryption method. Return a encrypted cipher Text
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra secirity</param>
        /// <param name="Key">key is mandatory, if key passed string empty then default key will be generated and used.</param>
        /// <returns>encrypted [string] text will be returned.</returns>       
        #region Encryption Function
        public static string Encrypt(string toEncrypt, string Key, bool useHashing = true)
        {
            byte[] KeyArray;
            byte[] ToEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            Key = string.IsNullOrEmpty(Key) ? Guid.NewGuid().ToString() : Key;
            if (useHashing)
            {
                var oMd5CryptoServiceProvider = new MD5CryptoServiceProvider();
                KeyArray = oMd5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(Key));
                oMd5CryptoServiceProvider.Clear();
            }
            else
                KeyArray = Encoding.UTF8.GetBytes(Key);

            var oTripleDesCryptoServiceProvider = new TripleDESCryptoServiceProvider
            {
                Key = KeyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var oICryptoTransform = oTripleDesCryptoServiceProvider.CreateEncryptor();
            byte[] resultArray = oICryptoTransform.TransformFinalBlock(ToEncryptArray, 0, ToEncryptArray.Length);
            oTripleDesCryptoServiceProvider.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        #endregion

        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// /// <param name="Key">key is mandatory, if not passed. Expected result will not be generated.</param>
        /// <returns>encrypted [string] text will be returned.</returns>
        #region Decryption Function
        public static string Decrypt(string cipherString, string Key, bool useHashing = true)
        {
            byte[] KeyArray;
            byte[] ToEncryptArray = Convert.FromBase64String(cipherString);
            if (useHashing)
            {
                var oMd5CryptoServiceProvider = new MD5CryptoServiceProvider();
                KeyArray = oMd5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(Key));
                oMd5CryptoServiceProvider.Clear();
            }
            else
                KeyArray = Encoding.UTF8.GetBytes(Key);

            var oTripleDesCryptoServiceProvider = new TripleDESCryptoServiceProvider
            {
                Key = KeyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform oICryptoTransform = oTripleDesCryptoServiceProvider.CreateDecryptor();
            byte[] resultArray = oICryptoTransform.TransformFinalBlock(ToEncryptArray, 0, ToEncryptArray.Length);

            oTripleDesCryptoServiceProvider.Clear();
            return Encoding.UTF8.GetString(resultArray);
        }
        #endregion
    }
}
