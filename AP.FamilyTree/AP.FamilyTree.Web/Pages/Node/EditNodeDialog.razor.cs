using AP.FamilyTree.Web.PageModels.Interfaces;
using AP.FamilyTree.Web.PageModels.Node;

namespace AP.FamilyTree.Web.Pages.Node
{
    public class EditNodeDialogViewModel : IEditModel
    {
        public NodeItemViewModel Model { get; set; }
        public bool DialogIsOpen { get; set; } = false;
        public bool IsConcurrencyError { get; set; }
        public string ErrorString { get; set; }
    }
}
