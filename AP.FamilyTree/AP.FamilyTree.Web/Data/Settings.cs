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
}
