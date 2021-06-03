using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AP.FamilyTree.Web.Data;
using AP.FamilyTree.Web.Data.Exceptions;
using AP.FamilyTree.Web.Globals;
using AP.FamilyTree.Web.PageModels.Interfaces;
using AP.FamilyTree.Web.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace AP.FamilyTree.Web.PageModels
{
    public class BaseViewModel : ComponentBase, IDisposable
    {
        [CascadingParameter] protected Task<AuthenticationState> authenticationStateTask { get; set; }
        [Inject] protected ILogger Logger { get; set; }
        [Inject] protected IJSRuntime Js { get; set; }

        protected bool IsFailed { get; set; } = false;
        bool disposed = false;

        public ErrorComponentViewModel ErrorModel { get; set; } = new ErrorComponentViewModel();
        protected string browserInfo = string.Empty;

        protected InformarionDialogViewModel mInformationDialog = new InformarionDialogViewModel() { Btn = "Schließen" };
        private string mNameUserAndPage;

        protected bool mIsItemLoaded = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (string.IsNullOrEmpty(browserInfo))
                {
                    browserInfo = await Js.InvokeAsync<string>("GetBrowserInfo");
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }
        public async Task<string> GetUserNameAsync()
        {
            var user = (await authenticationStateTask).User;
            return user?.Identity?.Name ?? "";
        }
        public string GetUserName()
        {
            var task = Task.Run(async () => await GetUserNameAsync());
            return task.Result;
        }

        //public async Task<bool> IsSysAdminRole()
        //{
        //    var user = (await authenticationStateTask).User;
        //    var isRole = user.IsInRole("Administrator");
        //    return isRole;
        //}
        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
            //GC.Collect(2);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        ~BaseViewModel()
        {
            Dispose(false);
        }

        protected void CatchException(Exception e, string additionalInfo)
        {
            Logger.LogError(e, $"{GetUserName()}*Error: {additionalInfo}*{browserInfo}");
            ExceptionOther(e);
        }

        public void SetPageName(string name)
        {
            mNameUserAndPage = $"{GetUserName()}*Error: {name}";
        }
        private void ExceptionType(ExeptionTypeEnum exeptionType, string message, FunctionModelEnum function, IEditModel editModel, IIsRefreshed currentItem)
        {
            if (editModel?.DialogIsOpen == true)//save//OldData Other
            {
                if (exeptionType != ExeptionTypeEnum.Other)
                {
                    mInformationDialog.IsOpenDialog = true;
                    mInformationDialog.Text = message;
                    mInformationDialog.Title = "Обновление данных";
                }
                else
                {
                    editModel.ErrorString = message;
                }

                return;
            }

            if (exeptionType == ExeptionTypeEnum.OldData && currentItem != null)//Restore Trash//OldData
            {
                currentItem.IsRefreshed = true;
            }

            StateHasChanged();
        }

        private void ExceptionDbUpdateConcurrency(FunctionModelEnum function, IEditModel editModel, IIsRefreshed currentItem)
        {
            if (editModel?.DialogIsOpen == true)
            {
                editModel.IsConcurrencyError = true;
            }

            if (currentItem != null)
            {
                currentItem.IsRefreshed = true;
            }

            StateHasChanged();
        }

        private void ExceptionOther(Exception e)
        {
            ErrorModel.IsOpen = true;
            ErrorModel.ErrorContext = e.StackTrace;
            ErrorModel.ErrorMessage = e.Message;
            IsFailed = true;
            StateHasChanged();
        }

        private void ExceptionDbUpdate(FunctionModelEnum function)
        {
            if (function == FunctionModelEnum.Remove)
            {
                mInformationDialog.IsOpenDialog = true;//remove
                mInformationDialog.Text = Global.ExceptionText[ExeptionTypeEnum.RemoveItem];
                mInformationDialog.Title = "Удаление";
            }
            else
            {
                mInformationDialog.IsOpenDialog = true;
                mInformationDialog.Text = "Ошибка базы данных. Обновите свои данные и попробуйте еще раз.";
                mInformationDialog.Title = "Обновление не удалось";
            }
            StateHasChanged();
        }

        public void ExceprionProcessing(Exception exception, FunctionModelEnum function, IIsRefreshed currentModel, IEditModel editModel, string functionName = null)
        {
            if (exception is DbUpdateConcurrencyException)
            {
                ExceptionDbUpdateConcurrency(function, editModel, currentModel);
                return;
            }

            if (exception is ExceptionByType)
            {
                ExceptionType(((ExceptionByType)exception).mExeptionType, exception.Message, function, editModel, currentModel);
                return;
            }

            string funName = function == FunctionModelEnum.Other ? functionName : function.ToString();
            var path = mNameUserAndPage + "/" + funName;
            Logger.LogError(exception, path + $"*{browserInfo}");

            if (exception is DbUpdateException)
            {
                ExceptionDbUpdate(function);
                return;
            }

            //exception
            ExceptionOther(exception);
        }
    }
}
