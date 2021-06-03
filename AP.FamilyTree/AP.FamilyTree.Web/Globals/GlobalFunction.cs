using System;

namespace AP.FamilyTree.Web.Globals
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
