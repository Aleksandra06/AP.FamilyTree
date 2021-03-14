using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AP.FamilyTree.Web.PageModels.System
{
    public class FilterErrorLogModel
    {
        public string UserFltr { get; set; }
        public string ErrorFltr { get; set; }
        public DateTime? StartDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        public DateTime? EndDate { get; set; } = DateTime.Now;
    }
}
