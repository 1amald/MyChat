using System.ComponentModel.DataAnnotations;

namespace MyChat.Models.Account
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Логин или Email")]
        public string LoginOrEmail { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
