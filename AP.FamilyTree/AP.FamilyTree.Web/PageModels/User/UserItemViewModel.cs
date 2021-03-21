using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AP.FamilyTree.Web.PageModels.User
{
    public class UserItemViewModel
    {
        [EmailAddress]
        public string Email { get; set; }

        //[StringLength(100, ErrorMessage = "Пароль должен содержать как минимум 6 символов", MinimumLength = 6)]
        [DataType(DataType.Password)] 
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = "";
        [Required(ErrorMessage = "Для поддверждения необходимо ввести действующий пароль.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; } = "";
        public List<string> ErrorList { get; set; }
    }
}
