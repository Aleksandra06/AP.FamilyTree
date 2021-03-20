using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AP.FamilyTree.Web.Shared
{
    public class ErrorComponentViewModel
    {
        public string ErrorMessage { get; set; }
        public string ErrorContext { get; set; }
        public string RedirectUrl { get; set; }
        public bool IsOpen { get; set; }
    }
}
