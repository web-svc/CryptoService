namespace CryptoService
{
    using CryptoService.Interface;
    public class AES : IAction
    {
        readonly IAES Algorithm = new Algorithms();

        public string Decrypt(ICipherInput cipherInput) => Algorithm.Decrypt(cipherInput: cipherInput);

        public string Encrypt(ICipherInput cipherInput) => Algorithm.Encrypt(cipherInput: cipherInput);
    }
}
