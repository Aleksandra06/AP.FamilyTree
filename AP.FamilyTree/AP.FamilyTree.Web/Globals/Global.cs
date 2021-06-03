using System.Collections.Generic;
using AP.FamilyTree.Web.Data;

namespace AP.FamilyTree.Web.Globals
{
    public class Global
    {
        public static string AppVersion = "0.2";
        public static Dictionary<ExeptionTypeEnum, string> ExceptionText => InitExceptionTextDictionary();

        private static Dictionary<ExeptionTypeEnum, string> InitExceptionTextDictionary()
        {
            var dic = new Dictionary<ExeptionTypeEnum, string>();

            dic.Add(ExeptionTypeEnum.Concurrency, "Ваши данные больше не актуальны. Обновите данные и снова внесите изменения.");
            dic.Add(ExeptionTypeEnum.OldData, "Запись данных, которую вы хотите обновить, не существует (больше). Пожалуйста, обновите свои данные.");
            dic.Add(ExeptionTypeEnum.RemoveItem, "Запись данных не может быть удалена, поскольку она связана с другими записями данных. Сначала удалите подключенные записи.");

            return dic;
        } 
    }
}
