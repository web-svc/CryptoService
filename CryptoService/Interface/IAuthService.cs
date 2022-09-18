namespace CryptoService.Interface
{
    public interface IAuthService
    {
        string GenerateBasicAuthWithHeader(IBasicAuthInput basicAuthInput);
        string GenerateBasicAuth(IBasicAuthInput basicAuthInput);
        IBasicAuthInput DecodeBasicAuthWithHeader(string BasicAuthText);
        IBasicAuthInput DecodeBasicAuth(string CipherText);
    }
}
