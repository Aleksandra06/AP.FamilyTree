using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AP.FamilyTree.Web.Data;
using AP.FamilyTree.Web.Data.Services.TreesServices;
using AP.FamilyTree.Web.PageModels;
using AP.FamilyTree.Web.PageModels.Trees;
using Microsoft.AspNetCore.Components;

namespace AP.FamilyTree.Web.Pages.Trees
{
    public class TreesViewModel : BaseViewModel
    {
        [Inject] protected TreesService Service { get; set; }
        protected List<TreeCardItemViewModel> Model { get; set; }
        protected TreeCardItemViewModel mCurrentItem;
        protected EditTreeDialogViewModel mEditViewModel = new EditTreeDialogViewModel();
        protected bool mIsItemLoaded = true;
        protected override Task OnInitializedAsync()
        {
            mInformationDialog.Btn = "Ок";
            SetPageName("Trees");
            return base.OnInitializedAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                try
                {
                    Model = await Service.GetAllForUser();
                    StateHasChanged();
                }
                catch (Exception e)
                {
                    ExceprionProcessing(e, FunctionModelEnum.OnAfterRenderAsync, null, null);
                }
            }
        }

        protected void CreateTree()
        {
            mCurrentItem = new TreeCardItemViewModel();
            mEditViewModel.Model = mCurrentItem;
            mEditViewModel.ErrorString = "";
            mEditViewModel.DialogIsOpen = true;
            StateHasChanged();
        }

        protected void Save(TreeCardItemViewModel item)
        {
            try
            {
                if (item.Id > 0)
                {
                    item = Service.Update(item);
                    var index = Model.FindIndex(x => x.Id == item.Id);
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
        protected void Edit(TreeCardItemViewModel item)
        {
            mCurrentItem = item;
            mEditViewModel.Model = mCurrentItem;
            mEditViewModel.DialogIsOpen = true;
            StateHasChanged();
        }
        protected void Reload(TreeCardItemViewModel item)
        {
            try
            {
                var reloadItem = Service.Reload(item);
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

                    var index = Model.FindIndex(x => x.Id == item.Id);
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
        protected void Remove(TreeCardItemViewModel item)
        {
            try
            {
                Service.Remove(item);
                Model.Remove(item);
            }
            catch (Exception e)
            {
                mCurrentItem = item;
                ExceprionProcessing(e, FunctionModelEnum.Remove, mCurrentItem, mEditViewModel?.DialogIsOpen == true ? mEditViewModel : null);
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
