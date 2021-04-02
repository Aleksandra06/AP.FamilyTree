using AP.FamilyTree.Web.PageModels.Interfaces;
using AP.FamilyTree.Web.PageModels.User;

namespace AP.FamilyTree.Web.Pages.User
{
    public class EditAccessTableViewModel : IEditModel
    {
        public bool DialogIsOpen { get; set; } = false;
        public AccessItemViewModel Model { get; set; }
        public bool IsConcurrencyError { get; set; }
        public string ErrorString { get; set; }
    }
}
