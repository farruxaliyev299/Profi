using System.ComponentModel.DataAnnotations;

namespace Profi.ViewModels.Account
{
    public class RegisterVM
    {
        [Required]
        public string Username { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password),Compare(nameof(Password))]
        public string CheckPassword { get; set; }
    }
}
