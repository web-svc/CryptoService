namespace CryptoService
{
    using CryptoService.Constant;
    using CryptoService.Interface;
    using CryptoService.Model;
    using Helper;
    using Helper.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class AuthService : IAuthService
    {
        readonly IBase64 base64 = new Algorithms();
        readonly IUrlService urlService = new UrlService();

        string IAuthService.ConstructBearerAuth(string CipherText)
        {
            return $"{Const.BearerAuth} {CipherText}";
        }

        string IAuthService.DecodeBearerToken(string CipherText)
        {
            return CipherText.Replace($"{Const.BasicAuth} ", string.Empty);
        }

        IBasicAuthInput IAuthService.DecodeBasicAuth(string CipherText)
        {
            return new BasicAuthInput() { UserName = base64.Decrypt(CipherText).Split(':')[0], Password = base64.Decrypt(CipherText).Split(':')[1] };
        }

        IBasicAuthInput IAuthService.DecodeBasicAuthWithHeader(string BasicAuthText)
        {
            return new BasicAuthInput() { UserName = base64.Decrypt(BasicAuthText.Replace($"{Const.BasicAuth} ", BasicAuthText)).Split(':')[0], Password = base64.Decrypt(BasicAuthText.Replace($"{Const.BasicAuth} ", BasicAuthText)).Split(':')[1] };
        }

        string IAuthService.GenerateBasicAuth(IBasicAuthInput basicAuthInput)
        {
            return base64.Encrypt($"{basicAuthInput.UserName}:{basicAuthInput.Password}");
        }

        string IAuthService.GenerateBasicAuthWithHeader(IBasicAuthInput basicAuthInput)
        {
            return $"{Const.BasicAuth} {base64.Encrypt($"{basicAuthInput.UserName}:{basicAuthInput.Password}")}";
        }

        public string ConstructOAuth(Dictionary<string, string> OAuthInput)
        {
            string CipherText = string.Join(", ", OAuthInput.Select(x => $"{x.Key}=\"{Uri.EscapeDataString(x.Value)}\"").ToArray());
            return $"{Const.OAuth} {CipherText}";
        }

        public string GetOAuthSignature(string compositeKey, Dictionary<string, string> form, string BaseUrl, string UrlQueryString = null)
        {
            string baseString = string.Join("&", form.Select(x => $"{x.Key}={x.Value}").ToArray());
            string CipherText = $"GET&{Uri.EscapeDataString(BaseUrl)}{(string.IsNullOrEmpty(UrlQueryString) ? $"&" : $"&{UrlQueryString}{urlService.UrlEncode("&")}")}{Uri.EscapeDataString(baseString)}";
            using HMACSHA1 hasher = new HMACSHA1(Encoding.ASCII.GetBytes(compositeKey));
            return Convert.ToBase64String(hasher.ComputeHash(Encoding.ASCII.GetBytes(CipherText)));
        }
    }
}
