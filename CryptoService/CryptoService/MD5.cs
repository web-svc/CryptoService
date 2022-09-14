namespace CryptoService
{
    using CryptoService.Interface;
    public class MD5: IAction
    {
        readonly IMD5 Algorithm = new Algorithms();
        
        public string Encrypt(ICipherInput cipherInput) => Algorithm.Encrypt(cipherInput: cipherInput);

        public string Decrypt(ICipherInput cipherInput) => Algorithm.Decrypt(cipherInput: cipherInput);
    }
}