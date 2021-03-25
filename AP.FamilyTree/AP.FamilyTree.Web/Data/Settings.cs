using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AP.FamilyTree.Web.Data
{
    public enum FunctionModelEnum
    {
        OnInitializedAsync,
        OnAfterRenderAsync,
        Save,
        Reload,
        Trash,
        Remove,
        Restore,
        Other
    }
    public enum ExeptionTypeEnum
    {
        Concurrency = 1,
        OldData = 2,
        RemoveItem = 3,
        Other
    }

    public enum DostupEnum
    {
        Admin = 1,
        Edit = 2,
        Look = 3,
        Not = 4
    }

    public enum GenderEnum
    {
        Man = 1,
        Woman = 2
    }
}
