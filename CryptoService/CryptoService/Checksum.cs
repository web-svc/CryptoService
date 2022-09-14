namespace CryptoService
{
    using System;
    using System.Text;
    public class Checksum
    {
        public static string Generate(string text)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", String.Empty).ToLower();
            }
        }
    }
}
