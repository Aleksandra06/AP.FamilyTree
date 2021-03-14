using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AP.FamilyTree.Web.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace AP.FamilyTree.Web.PageModels
{
    public class BaseViewModel : ComponentBase, IDisposable
    {
        [Inject] protected ILogger Logger { get; set; }
        [Inject] protected IJSRuntime Js { get; set; }

        protected bool IsFailed { get; set; } = false;
        bool disposed = false;

        public ErrorComponentViewModel ErrorModel { get; set; } = new ErrorComponentViewModel();
        protected string browserInfo = string.Empty;

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
            Logger.LogError(e, $"158*Error: {additionalInfo}*{browserInfo}");
            ErrorModel.IsOpen = true;
            ErrorModel.ErrorContext = e.StackTrace;
            ErrorModel.ErrorMessage = e.Message;
            IsFailed = true;
        }
    }
}
