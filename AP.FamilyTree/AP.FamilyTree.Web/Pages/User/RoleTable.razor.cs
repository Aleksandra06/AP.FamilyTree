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
    public class RoleTableViewModel : BaseViewModel
    {
        [Inject] public RoleService Service { get; set; }
        protected List<RoleItemViewModel> Model { get; set; } = new List<RoleItemViewModel>();
        protected RoleItemViewModel mCurrentItem;
        protected EditRoleTableViewModel mEditViewModel = new EditRoleTableViewModel();
        protected override Task OnInitializedAsync()
        {
            mInformationDialog.Btn = "Ок";
            SetPageName("RoleTable");
            return base.OnInitializedAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                try
                {
                    Model = await Service.GetUserRoles();
                    StateHasChanged();
                }
                catch (Exception e)
                {
                    ExceprionProcessing(e, FunctionModelEnum.OnAfterRenderAsync, null, null);
                    StateHasChanged();
                }
            }
        }

        protected void EditRole(RoleItemViewModel item)
        {
            mCurrentItem = item;
            mEditViewModel = new EditRoleTableViewModel();
            mEditViewModel.Model = mCurrentItem;
            mEditViewModel.DialogIsOpen = true;
        }

        protected async Task SaveRole(RoleItemViewModel item)
        {
            try
            {
                var updateRole = await Service.UpdateRole(item);
                var index = Model.FindIndex(x => x.Login == item.Login);
                Model[index] = updateRole;
                mEditViewModel.DialogIsOpen = false;
                StateHasChanged();
            }
            catch (Exception e)
            {
                ExceprionProcessing(e, FunctionModelEnum.Other, mCurrentItem, mEditViewModel, "SaveRole");
                StateHasChanged();
            }
        }

        public async Task ReloadItem(RoleItemViewModel item)
        {
            try
            {
                var reloadItem = await Service.ReloadItem(item);
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

                    var index = Model.FindIndex(x => x.Login == item.Login);
                    if (string.IsNullOrEmpty(reloadItem.Login))
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
