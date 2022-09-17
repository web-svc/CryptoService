namespace CryptoService
{
    using CryptoService.Constant;
    using CryptoService.Interface;
    using CryptoService.Model;

    public class AuthService : IAuthService
    {
        readonly IBase64 base64 = new Algorithms();
        public IBasicAuthInput DecodeBasicAuth(string CipherText)
        {
            return new BasicAuthInput() { UserName = base64.Decrypt(CipherText).Split(':')[0], Password = base64.Decrypt(CipherText).Split(':')[1] };
        }

        public IBasicAuthInput DecodeBasicAuthWithHeader(string BasicAuthText)
        {
            return new BasicAuthInput() { UserName = base64.Decrypt(BasicAuthText.Replace($"{Const.BasicAuth} ", BasicAuthText)).Split(':')[0], Password = base64.Decrypt(BasicAuthText.Replace($"{Const.BasicAuth} ", BasicAuthText)).Split(':')[1] };
        }

        public string GenerateBasicAuth(IBasicAuthInput basicAuthInput)
        {
            return base64.Encrypt($"{basicAuthInput.UserName}:{basicAuthInput.Password}");
        }

        public string GenerateBasicAuthWithHeader(IBasicAuthInput basicAuthInput)
        {
            return $"{Const.BasicAuth} {base64.Encrypt($"{basicAuthInput.UserName}:{basicAuthInput.Password}")}";
        }
    }
}
