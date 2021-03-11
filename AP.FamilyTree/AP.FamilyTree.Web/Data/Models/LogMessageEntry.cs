using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AP.FamilyTree.Web.Data.Models
{
    public class LogMessageEntry
    {
        public string ErrorMsg { get; set; }
        public string ErrorContext { get; set; }
        public string ErrorMsgUser { get; set; }
        public DateTime InsertDate { get; set; }
        public string UserData { get; set; }
        public int? ErrorLevel { get; set; }
        public string BrowserInfo { get; set; }
        public string AppVersion { get; set; }
    }
}
