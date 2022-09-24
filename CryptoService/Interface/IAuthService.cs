using System.Collections.Generic;

namespace CryptoService.Interface
{
    public interface IAuthService
    {
        string GenerateBasicAuthWithHeader(IBasicAuthInput basicAuthInput);
        string GenerateBasicAuth(IBasicAuthInput basicAuthInput);
        string ConstructBearerAuth(string CipherText);
        string ConstructOAuth(Dictionary<string, string> OAuthInput);
        IBasicAuthInput DecodeBasicAuthWithHeader(string BasicAuthText);
        IBasicAuthInput DecodeBasicAuth(string CipherText);
        string DecodeBearerToken(string CipherText);
        string GetOAuthSignature(string compositeKey, Dictionary<string, string> form, string BaseUrl, string UrlQueryString = null);
    }
}
