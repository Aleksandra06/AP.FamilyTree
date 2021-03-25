using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Web.Data;
using AP.FamilyTree.Web.Data.Services.NodeServices;
using AP.FamilyTree.Web.PageModels;
using AP.FamilyTree.Web.PageModels.Node;
using Microsoft.AspNetCore.Components;

namespace AP.FamilyTree.Web.Pages.Node
{
    public class NodeTableViewModel : BaseViewModel
    {
        [Inject] protected NodeService Service { get; set; }
        [Parameter] public int TreeId { get; set; }

        protected List<NodeItemViewModel> Model { get; set; }
        protected NodeItemViewModel mCurrentItem;

        protected EditNodeDialogViewModel mEditViewModel { get; set; } = new EditNodeDialogViewModel();

        protected override Task OnInitializedAsync()
        {
            mInformationDialog.Btn = "Ок";
            SetPageName("NodeTable");
            return base.OnInitializedAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                try
                {
                    Model = await Service.GetByTreeId(TreeId);
                    StateHasChanged();
                }
                catch (Exception e)
                {
                    ExceprionProcessing(e, FunctionModelEnum.OnAfterRenderAsync, null, null);
                }
            }
        }

        protected void EditNodeOpenDialog(NodeItemViewModel item)
        {
            mEditViewModel.DialogIsOpen = true;
            mEditViewModel.Model = item;
        }
        protected void Create()
        {
            mEditViewModel.DialogIsOpen = true;
            mEditViewModel.Model = new NodeItemViewModel();
            mEditViewModel.Model.Human = new HumanModel();
            mEditViewModel.Model.TreeId = TreeId;
        }
        protected void Save(NodeItemViewModel item)
        {
            try
            {
                if (item.NodeId > 0)
                {
                    item = Service.Update(item);
                    var index = Model.FindIndex(x => x.NodeId == item.NodeId);
                    Model[index] = item;

                }
                else
                {
                    item = Service.Create(item);
                    Model.Add(item);
                }

                mEditViewModel.DialogIsOpen = false;
                StateHasChanged();
            }
            catch (Exception e)
            {
                mCurrentItem = item;
                ExceprionProcessing(e, FunctionModelEnum.Save, mCurrentItem, mEditViewModel);
            }
        }
    }
}
