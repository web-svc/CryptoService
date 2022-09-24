namespace CryptoService.Interface
{
    public interface IAuthService
    {
        string GenerateBasicAuthWithHeader(IBasicAuthInput basicAuthInput);
        string GenerateBasicAuth(IBasicAuthInput basicAuthInput);
        string ConstructBearerAuth(string CipherText);
        string ConstructOAuth(string CipherText);
        IBasicAuthInput DecodeBasicAuthWithHeader(string BasicAuthText);
        IBasicAuthInput DecodeBasicAuth(string CipherText);
        string DecodeBearerToken(string CipherText);
    }
}
