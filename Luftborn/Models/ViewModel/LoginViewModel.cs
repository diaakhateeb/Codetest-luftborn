using System.ComponentModel.DataAnnotations;

namespace Luftborn.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
