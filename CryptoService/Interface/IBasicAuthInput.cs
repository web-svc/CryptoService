namespace CryptoService.Interface
{
    public interface IBasicAuthInput
    {
        string UserName { get; set; }
        string Password { get; set; }
    }
}
