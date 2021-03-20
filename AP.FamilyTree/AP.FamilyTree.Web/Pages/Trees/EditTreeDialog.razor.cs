using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AP.FamilyTree.Web.PageModels.Trees;

namespace AP.FamilyTree.Web.Pages.Trees
{
    public class EditTreeDialogViewModel
    {
        public bool IsOpenDialog { get; set; } = false;
        public TreeCardItemViewModel Model { get; set; }
        public bool IsConcurrencyError { get; set; }
    }
}
