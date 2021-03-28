using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AP.FamilyTree.Web.Data.Services.SystemPageServices;
using AP.FamilyTree.Web.PageModels;
using AP.FamilyTree.Web.PageModels.System;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace AP.FamilyTree.Web.Pages.SystemPage
{
    public class SystemErrorLogPageViewModel : BaseViewModel
    {
        [Inject] protected SystemPageService Service { get; set; }
        [Inject] protected IMatDialogService MatDialogService { get; set; }
        protected List<ErrorLogModel> Model { get; set; }
        protected FilterErrorLogModel Filter { get; set; } = new FilterErrorLogModel();

        protected override async Task OnInitializedAsync()
        {
            //ErrorModel = new ErrorComponentModel()
            //{
            //    RedirectUrl = UriHelper.Uri
            //};

        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    GetLogs();
                    StateHasChanged();
                }
                catch (Exception e)
                {
                    Logger.LogError(e, "Error: SystemErrorLogPageViewModel/OnInitializedAsync");
                    ErrorModel.IsOpen = true;
                    ErrorModel.ErrorContext = e.StackTrace;
                    ErrorModel.ErrorMessage = e.Message + "   " + e.InnerException?.Message;
                    IsFailed = true;
                    StateHasChanged();
                }
            }
        }

        protected void GetLogs()
        {
            Model = Service.GetModel();
        }

        protected override void Dispose(bool disposing)
        {
            Model?.Clear();
            Model = null;
            Service = null;
            ErrorModel = null;

        }
        protected async Task DeleteErrors()
        {
            var result = await MatDialogService.AskAsync("Are you sure?", new string[] { "Yes", "No" });
            if (result == "Yes")
            {
                try
                {
                    List<ErrorLogModel> errors = Service.GetModel(Filter);

                    foreach (var error in errors)
                    {
                        Service.DeleteByErrorId(error.Id);
                    }

                    GetLogs();
                    StateHasChanged();
                }
                catch (Exception e)
                {
                    CatchException(e, "SystemErrorLogPage/DeleteErrors");
                    StateHasChanged();
                }
            }
        }

        protected async Task ExportErrors()
        {
            string FileName = $"export_{DateTime.Now.ToShortDateString()}.xlsx";
            var memory = new MemoryStream();
            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet($"Errors_{DateTime.Now.ToShortDateString()}");
            IRow row = excelSheet.CreateRow(0);
            int counter = 1;
            row.CreateCell(0).SetCellValue("#");
            row.CreateCell(1).SetCellValue("Insert");
            row.CreateCell(2).SetCellValue("User");
            row.CreateCell(3).SetCellValue("Level");
            row.CreateCell(4).SetCellValue("Error");
            row.CreateCell(5).SetCellValue("Stack Trace");

            List<ErrorLogModel> Model = Service.GetModel(Filter);

            foreach (var item in Model)
            {
                row = excelSheet.CreateRow(counter);
                row.CreateCell(0).SetCellValue(item.Id);
                row.CreateCell(1).SetCellValue(item.InsertDate.ToString("dd.MM.yy hh:mm:ss"));
                row.CreateCell(2).SetCellValue(item.UserData);
                row.CreateCell(3).SetCellValue(item.ErrorLevel.ToString());
                row.CreateCell(4).SetCellValue(item.ErrorMsg);
                row.CreateCell(5).SetCellValue(item.ErrorContext);
                counter++;
            }
            workbook.Write(memory);
            var fileData = memory.ToArray();

            await Js.InvokeAsync<object>(
                       "saveAsFile",
                       FileName,
                       fileData);
        }
        public async Task Remove(ErrorLogModel error)
        {
            try
            {
                Service.DeleteByErrorId(error.Id);
                Model.Remove(error);
                StateHasChanged();
            }
            catch (Exception e)
            {
                CatchException(e, "SystemErrorLogPage/Remove");
                StateHasChanged();
            }
        }

        protected bool IsOpenDialogStackTrace { get; set; } = false;
        protected string TextDialogStackTrace { get; set; }

        protected void Sorting(KeyValuePair<string, string> pair)
        {
            Model = pair.Value == "desc" ? Model.OrderByDescending(x => x.GetType().GetProperty(pair.Key).GetValue(x, null)).ToList()
                : Model.OrderBy(x => x.GetType().GetProperty(pair.Key).GetValue(x, null)).ToList();
            StateHasChanged();
        }

        protected string GetTitle(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (text.Length < 100)
                return text;

            return text.Substring(0, 100);
        }

        protected void ShowStackTraceDialog(string text)
        {
            TextDialogStackTrace = text;
            IsOpenDialogStackTrace = true;
        }

        protected static string GetLogUser(string logUser)
        {
            if (string.IsNullOrEmpty(logUser))
                return "IsNullOrEmpty";

            return logUser.IndexOf("@") > 0 ? logUser.Substring(0, logUser.IndexOf("@")) : logUser;
        }

        public async Task RemoveAsync(ErrorLogModel error)
        {
            var result = await MatDialogService.AskAsync("Are you sure?", new string[] { "Yes", "No" });

            if (result == "Yes")
            {
                await Remove(error);
            }
        }
    }
}
