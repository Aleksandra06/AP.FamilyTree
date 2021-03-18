using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AP.FamilyTree.Web.Data.Models
{
    public class MailItem
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
    }
}
