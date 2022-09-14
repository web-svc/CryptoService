namespace CryptoService
{
    using CryptoService.Interface;
    public class Checksum : IChecksum
    {
        readonly IChecksum Algorithm = new Algorithms();

        public string Generate(string CipherText) => Algorithm.Generate(CipherText: CipherText);
    }
}
