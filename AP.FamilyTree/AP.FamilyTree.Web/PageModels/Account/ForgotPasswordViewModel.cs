using System.ComponentModel.DataAnnotations;

namespace AP.FamilyTree.Web.PageModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
