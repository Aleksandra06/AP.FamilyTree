using System;
using System.Threading.Tasks;
using AP.FamilyTree.Web.Data;
using AP.FamilyTree.Web.Data.Services.UserServices;
using AP.FamilyTree.Web.PageModels;
using AP.FamilyTree.Web.PageModels.User;
using Microsoft.AspNetCore.Components;

namespace AP.FamilyTree.Web.Pages.User
{
    public class PersonalOfficeViewModel : BaseViewModel
    {
        [Inject] public UserService Service { get; set; }
        protected UserItemViewModel Model { get; set; }

        protected override Task OnInitializedAsync()
        {
            mInformationDialog.Btn = "Ок";
            SetPageName("PersonalOffice");
            return base.OnInitializedAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                try
                {
                    Model = await Service.GetUserModel();
                    StateHasChanged();
                }
                catch (Exception e)
                {
                    ExceprionProcessing(e, FunctionModelEnum.OnAfterRenderAsync, null, null);
                }
            }
        }

        protected async Task SaveItem()
        {
            try
            {
                Model = await Service.Save(Model);
                StateHasChanged();
            }
            catch (Exception e)
            {
                ExceprionProcessing(e, FunctionModelEnum.OnAfterRenderAsync, null, null);
            }
        }
        protected void CloseInformationDialog()
        {
            mInformationDialog.IsOpenDialog = false;
            //if (mEditViewModel?.DialogIsOpen == true)
            //{
            //    mEditViewModel.DialogIsOpen = false;
            //}
        }

        protected override void Dispose(bool disposing)
        {
            Model = null;
            Service = null;
        }
    }
}