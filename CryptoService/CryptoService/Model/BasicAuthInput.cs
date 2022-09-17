namespace CryptoService.Model
{
    using CryptoService.Interface;
    using System.ComponentModel.DataAnnotations;

    public class BasicAuthInput : IBasicAuthInput
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}