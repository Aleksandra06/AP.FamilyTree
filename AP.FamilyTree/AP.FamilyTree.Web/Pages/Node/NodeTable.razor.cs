using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Web.Data;
using AP.FamilyTree.Web.Data.Services.NodeServices;
using AP.FamilyTree.Web.Globals;
using AP.FamilyTree.Web.PageModels;
using AP.FamilyTree.Web.PageModels.Node;
using Microsoft.AspNetCore.Components;
using NPOI.SS.Formula.Functions;

namespace AP.FamilyTree.Web.Pages.Node
{
    public class NodeTableViewModel : BaseViewModel
    {
        [Inject] protected NodeService Service { get; set; }
        [Parameter] public int TreeId { get; set; }

        protected List<NodeItemViewModel> Model { get; set; }
        protected NodeItemViewModel mCurrentItem;

        protected EditNodeDialogViewModel mEditViewModel = new EditNodeDialogViewModel();

        protected bool mIsOpenConfirmationDialog = false;

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
                }
                catch (Exception e)
                {
                    ExceprionProcessing(e, FunctionModelEnum.OnAfterRenderAsync, null, null);
                }
                finally
                {
                    StateHasChanged();
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
            mEditViewModel.Model.Human = new PersonItemViewModel();
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
        public void ReloadItem(NodeItemViewModel item)
        {
            try
            {
                var reloadItem = Service.ReloadItem(item);
                if (reloadItem == null)
                {
                    Model.Remove(item);
                    mInformationDialog.IsOpenDialog = true;
                    mInformationDialog.Text = "Этот элемент был удален.";
                    mInformationDialog.Title = "Обновление";
                }
                else
                {
                    reloadItem.IsRefreshed = false;

                    if (mEditViewModel.DialogIsOpen)
                    {
                        mEditViewModel.Model = reloadItem;
                    }

                    var index = Model.FindIndex(x => x.NodeId == item.NodeId);
                    if (reloadItem.Item == null)
                    {
                        mEditViewModel.DialogIsOpen = false;
                        Model.RemoveAt(index);
                    }
                    else
                    {
                        mEditViewModel.IsConcurrencyError = false;
                        Model[index] = reloadItem;
                    }
                }

                StateHasChanged();
            }
            catch (Exception e)
            {
                mCurrentItem = item;
                ExceprionProcessing(e, FunctionModelEnum.Reload, mCurrentItem, mEditViewModel?.DialogIsOpen == true ? mEditViewModel : null);
            }
        }

        protected void RemoveOpenDialog(NodeItemViewModel item)
        {
            mCurrentItem = item;
            mIsOpenConfirmationDialog = true;
        }

        protected void CloseRemoveDialog(bool answer)
        {
            mIsOpenConfirmationDialog = false;
            if (answer)
            {
                Remove(mCurrentItem);
            }
        }

        protected void Remove(NodeItemViewModel item)
        {
            try
            {
                Service.Remove(item);
                var index = Model.FindIndex(x => x.NodeId == item.NodeId);
                Model.RemoveAt(index);
            }
            catch (Exception e)
            {
                mCurrentItem = item;
                ExceprionProcessing(e, FunctionModelEnum.Remove, mCurrentItem, null);
            }
        }

        protected void CloseInformationDialog()
        {
            mInformationDialog.IsOpenDialog = false;
            if (mEditViewModel?.DialogIsOpen == true)
            {
                mEditViewModel.DialogIsOpen = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            Model?.Clear();
            Model = null;
            mCurrentItem = null;
            mEditViewModel = null;
            Service = null;
        }
    }
}
