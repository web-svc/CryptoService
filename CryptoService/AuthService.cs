namespace CryptoService
{
    using CryptoService.Constant;
    using CryptoService.Interface;
    using CryptoService.Model;

    public class AuthService : IAuthService
    {
        readonly IBase64 base64 = new Algorithms();

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

        public string ConstructOAuth(string CipherText)
        {
            return $"{Const.OAuth} {CipherText}";
        }
    }
}
