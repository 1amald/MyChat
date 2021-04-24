using System.ComponentModel.DataAnnotations;

namespace MyChat.Models.Account
{
    public class ChangePasswordPartial
    {

        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Status { get; set; }

    }
}
