namespace CryptoService
{
    using CryptoService.Constant;
    using CryptoService.Interface;
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class Algorithms : IAlgorithms, IChecksum, IMD5, IAES
    {
        readonly IMD5 Algorithm = new Algorithms();
        string IChecksum.Generate(string CipherText)
        {
            using System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(CipherText))).Replace("-", string.Empty).ToLower();
        }

        byte[] IMD5.GetHashCode(string CipherKey)
        {
            using var objMd5CryptoServiceProvider = new MD5CryptoServiceProvider();
            var CipherKeyArray = objMd5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(CipherKey));
            objMd5CryptoServiceProvider.Clear();
            return CipherKeyArray;
        }

        string IMD5.Decrypt(ICipherInput cipherInput)
        {
            cipherInput.CipherKey = string.IsNullOrEmpty(cipherInput.CipherKey) ? Const.CipherKey : cipherInput.CipherKey;

            byte[] CipherTextArray = Convert.FromBase64String(cipherInput.CipherText);
            byte[] CipherKeyArray = cipherInput.UseHashing ? Algorithm.GetHashCode(cipherInput.CipherKey) : Encoding.UTF8.GetBytes(cipherInput.CipherKey);

            using var objTripleDesCryptoServiceProvider = new TripleDESCryptoServiceProvider
            {
                Key = CipherKeyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            using var objICryptoTransform = objTripleDesCryptoServiceProvider.CreateDecryptor();
            byte[] resultArray = objICryptoTransform.TransformFinalBlock(CipherTextArray, 0, CipherTextArray.Length);

            objTripleDesCryptoServiceProvider.Clear();
            return Encoding.UTF8.GetString(resultArray);
        }

        string IMD5.Encrypt(ICipherInput cipherInput)
        {
            cipherInput.CipherKey = string.IsNullOrEmpty(cipherInput.CipherKey) ? Const.CipherKey : cipherInput.CipherKey;

            byte[] CipherTextArray = Encoding.UTF8.GetBytes(cipherInput.CipherText);
            byte[] CipherKeyArray = cipherInput.UseHashing ? Algorithm.GetHashCode(cipherInput.CipherKey) : Encoding.UTF8.GetBytes(cipherInput.CipherKey);

            using var objTripleDesCryptoServiceProvider = new TripleDESCryptoServiceProvider
            {
                Key = CipherKeyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            using var objICryptoTransform = objTripleDesCryptoServiceProvider.CreateEncryptor();
            byte[] resultArray = objICryptoTransform.TransformFinalBlock(CipherTextArray, 0, CipherTextArray.Length);
            objTripleDesCryptoServiceProvider.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        string IAES.Encrypt(ICipherInput cipherInput)
        {
            using var rijAlg = new RijndaelManaged();
            rijAlg.Mode = CipherMode.CBC;
            rijAlg.Padding = PaddingMode.PKCS7;
            rijAlg.FeedbackSize = 128;

            rijAlg.Key = Encoding.UTF8.GetBytes(cipherInput.CipherKey);
            rijAlg.IV = Encoding.UTF8.GetBytes(cipherInput.CipherKey);

            var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(cipherInput.CipherText);
            }
            return Encoding.UTF8.GetString(msEncrypt.ToArray());
        }

        string IAES.Decrypt(ICipherInput cipherInput)
        {

            using var rijAlg = new RijndaelManaged();
            rijAlg.Mode = CipherMode.CBC;
            rijAlg.Padding = PaddingMode.PKCS7;
            rijAlg.FeedbackSize = 128;
            rijAlg.Key = Encoding.UTF8.GetBytes(cipherInput.CipherKey);
            rijAlg.IV = Encoding.UTF8.GetBytes(cipherInput.CipherKey);
            var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

            using var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherInput.CipherText));
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
    }
}
