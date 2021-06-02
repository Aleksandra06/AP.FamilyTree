using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AP.FamilyTree.Web.Data
{
    public static class GlobalFunction
    {
        public static string ConvertToLiveYear(DateTime? start, DateTime? finish)
        {
            string str = string.Empty;
            if (start != null)
            {
                str += start.Value.ToShortDateString();
            }

            str += " - ";
            if (finish != null)
            {
                str += finish.Value.ToShortDateString();
            }

            return str;
        }
    }
}
