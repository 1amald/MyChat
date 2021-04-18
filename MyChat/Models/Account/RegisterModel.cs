using System.ComponentModel.DataAnnotations;

namespace MyChat.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Ваш email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        public string PasswordConfirm { get; set; }
    }
}
