namespace CryptoService
{
    using CryptoService.Interface;
    public class Base64: IAction
    {
        readonly IBase64 Algorithm = new Algorithms();
        
        public string Encrypt(ICipherInput cipherInput) => Algorithm.Encrypt(cipherInput.CipherText);

        public string Decrypt(ICipherInput cipherInput) => Algorithm.Decrypt(cipherInput.CipherText);
    }
}