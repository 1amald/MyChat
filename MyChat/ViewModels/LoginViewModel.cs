using System.ComponentModel.DataAnnotations;


namespace MyChat.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Login")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Required field")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
