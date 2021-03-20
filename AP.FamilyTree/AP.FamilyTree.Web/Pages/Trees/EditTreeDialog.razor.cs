using AP.FamilyTree.Web.PageModels.Interfaces;
using AP.FamilyTree.Web.PageModels.Trees;

namespace AP.FamilyTree.Web.Pages.Trees
{
    public class EditTreeDialogViewModel : IEditModel
    {
        public bool DialogIsOpen { get; set; } = false;
        public TreeCardItemViewModel Model { get; set; }
        public bool IsConcurrencyError { get; set; }
        public string ErrorString { get; set; }
    }
}
