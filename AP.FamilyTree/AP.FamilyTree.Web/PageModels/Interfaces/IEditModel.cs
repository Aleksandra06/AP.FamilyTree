using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AP.FamilyTree.Web.PageModels.Interfaces
{
    public interface IEditModel
    {
        public bool DialogIsOpen { get; set; }
        public string ErrorString { get; set; }
        public bool IsConcurrencyError { get; set; }
    }
}
