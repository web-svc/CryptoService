namespace CryptoService.Interface
{
    public interface IBase64
    {
        string Encrypt(string CipherText);
        string Decrypt(string CipherText);
    }
}
