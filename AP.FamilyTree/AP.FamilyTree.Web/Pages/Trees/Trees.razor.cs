using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AP.FamilyTree.Web.Data.Services.TreesServices;
using AP.FamilyTree.Web.PageModels;
using AP.FamilyTree.Web.PageModels.Trees;
using Microsoft.AspNetCore.Components;

namespace AP.FamilyTree.Web.Pages.Trees
{
    public class TreesViewModel : BaseViewModel
    {
        [Inject] protected TreesService Service { get; set; }
        protected List<TreeCardItemViewModel> ModelList { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //try
            //{
            ModelList = await Service.GetAllForUser();
            //}
            //catch (Exception e)
            //{

            //}
        }
    }
}
