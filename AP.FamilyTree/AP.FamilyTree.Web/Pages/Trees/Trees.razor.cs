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
        protected TreeCardItemViewModel mCurrentItem;
        protected EditTreeDialogViewModel EditDialog { get; set; } = new EditTreeDialogViewModel();
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

        protected void CreateTree()
        {
            mCurrentItem = new TreeCardItemViewModel();
            EditDialog.Model = mCurrentItem;
            EditDialog.IsOpenDialog = true;
            StateHasChanged();
        }

        protected void Save(TreeCardItemViewModel item)
        {
            if (item.Id > 0)
            {
                item = Service.Update(item);
                var index = ModelList.FindIndex(x => x.Id == item.Id);
                ModelList[index] = item;

            }
            else
            {
                item = Service.Create(item);
                ModelList.Add(item);
            }

            EditDialog.IsOpenDialog = false;
        }
        protected void Edit(TreeCardItemViewModel item)
        {
            mCurrentItem = item;
            EditDialog.Model = mCurrentItem;
            EditDialog.IsOpenDialog = true;
            StateHasChanged();
        }
        protected void Reload(TreeCardItemViewModel item)
        {
            item = Service.Reload(item);
            var index = ModelList.FindIndex(x => x.Id == item.Id);
            ModelList[index] = item;
        }
        protected void Remove(TreeCardItemViewModel item)
        {
            Service.Remove(item);
            ModelList.Remove(item);
        }
    }
}
