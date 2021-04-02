using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AP.FamilyTree.Web.Data;
using AP.FamilyTree.Web.Data.Services.UserServices;
using AP.FamilyTree.Web.PageModels;
using AP.FamilyTree.Web.PageModels.User;
using Microsoft.AspNetCore.Components;

namespace AP.FamilyTree.Web.Pages.User
{
    public class AccessTableViewModel : BaseViewModel
    {
        [Inject] public AccessService Service { get; set; }
        protected List<AccessItemViewModel> Model { get; set; } = new List<AccessItemViewModel>();
        protected AccessItemViewModel mCurrentModel;
        protected EditAccessTableViewModel mEditViewModel = new EditAccessTableViewModel();
        protected bool mConfirmDialogIsOpen = false;
        protected override Task OnInitializedAsync()
        {
            mInformationDialog.Btn = "Ок";
            SetPageName("AccessTable");
            return base.OnInitializedAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                try
                {
                    Model = await Service.GetAccessForUser();
                    StateHasChanged();
                }
                catch (Exception e)
                {
                    ExceprionProcessing(e, FunctionModelEnum.OnAfterRenderAsync, null, null);
                    StateHasChanged();
                }
            }
        }

        protected void EditAccess(AccessItemViewModel item)
        {
            mCurrentModel = item;
            mEditViewModel = new EditAccessTableViewModel();
            mEditViewModel.Model = mCurrentModel;
            mEditViewModel.DialogIsOpen = true;
        }

        protected void Create()
        {
            mCurrentModel = new AccessItemViewModel();
            mEditViewModel = new EditAccessTableViewModel();
            mEditViewModel.Model = mCurrentModel;
            mEditViewModel.DialogIsOpen = true;
        }

        protected void SaveAccess(AccessItemViewModel item)
        {
            try
            {
                if (item.Id == 0)
                {
                    var newItem = Service.Create(item);
                    if (newItem != null)
                    {
                        Model.Add(newItem);
                    }
                }
                else
                {
                    var upItem = Service.Update(item);
                    if (upItem != null)
                    {
                        var index = Model.FindIndex(x => x.Id == item.Id);
                        Model[index] = upItem;
                    }
                }
                mEditViewModel.DialogIsOpen = false;
                StateHasChanged();
            }
            catch (Exception e)
            {
                ExceprionProcessing(e, FunctionModelEnum.Save, mCurrentModel, mEditViewModel);
                StateHasChanged();
            }
        }

        protected void RemoveOpenDialog(AccessItemViewModel item)
        {
            mCurrentModel = item;
            mConfirmDialogIsOpen = true;
        }

        protected void Remove(bool answer)
        {
            try
            {
                if (answer)
                {
                    Service.Delete(mCurrentModel);
                    Model.Remove(mCurrentModel);
                }
            }
            catch (Exception e)
            {
                ExceprionProcessing(e, FunctionModelEnum.Remove, mCurrentModel, null);
                StateHasChanged();
            }
            finally
            {
                mCurrentModel = null;
                mConfirmDialogIsOpen = false;
                StateHasChanged();
            }
        }

        public void ReloadItem(AccessItemViewModel item)
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
                mCurrentModel = item;
                ExceprionProcessing(e, FunctionModelEnum.Reload, mCurrentModel, mEditViewModel);
            }
        }

        protected void CloseInformationDialog()
        {
            mInformationDialog.IsOpenDialog = false;
            mInformationDialog.Btn = "Ок";
            if (mEditViewModel?.DialogIsOpen == true)
            {
                mEditViewModel.DialogIsOpen = false;
            }
        }

        protected void Sort(KeyValuePair<string, string> pair)
        {
            Model = pair.Value == "desc" ? Model.OrderByDescending(x => x.GetType().GetProperty(pair.Key).GetValue(x, null)).ToList()
                : Model.OrderBy(x => x.GetType().GetProperty(pair.Key).GetValue(x, null)).ToList();
            StateHasChanged();
        }

        protected override void Dispose(bool disposing)
        {
            Model = null;
            Service = null;
        }
    }
}
