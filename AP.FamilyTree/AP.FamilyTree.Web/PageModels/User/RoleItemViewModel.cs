using AP.FamilyTree.Web.PageModels.Interfaces;

namespace AP.FamilyTree.Web.PageModels.User
{
    public class RoleItemViewModel : IIsRefreshed
    {
        public string Login { get; set; }
        public string RoleName { get; set; }
        public bool IsRefreshed { get; set; } = false;
    }
}
