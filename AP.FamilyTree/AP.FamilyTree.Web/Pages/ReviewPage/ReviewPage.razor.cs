using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Web.Data.Services.UserServices;
using AP.FamilyTree.Web.Globals;
using AP.FamilyTree.Web.PageModels;
using AP.FamilyTree.Web.PageModels.Review;
using AP.FamilyTree.Web.Shared;
using Microsoft.AspNetCore.Components;

namespace AP.FamilyTree.Web.Pages.ReviewPage
{
    public class ReviewPageViewModel : BaseViewModel
    {
        [Inject] protected ReviewService Service { get; set; }
        protected List<ReviewItemViewModel> Model { get; set; }
        protected ReviewItemViewModel mCurrentItem;

        protected bool mIsOpenConfirmationDialog = false;

        protected InformarionDialogViewModel mPreviewViewModel = new InformarionDialogViewModel();

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
                    Model = await Service.GetModel();
                    mPreviewViewModel.Btn = "Закрыть"; 
                    StateHasChanged();
                }
                catch (Exception e)
                {
                    ExceprionProcessing(e, FunctionModelEnum.OnAfterRenderAsync, null, null);
                }
            }
        }
        protected void CloseInformationDialog()
        {
            mInformationDialog.IsOpenDialog = false;
        }

        protected void ChangeStatusOpenDialog(ReviewItemViewModel item)
        {
            mCurrentItem = item;
            mIsOpenConfirmationDialog = true;
        }
        protected void ChangeStatusCloseDialog()
        {
            mIsOpenConfirmationDialog = false;
            ChangeStatus(mCurrentItem);
        }
        protected void ChangeStatus(ReviewItemViewModel item)
        {
            try
            {
                item.Accepted = !item.Accepted;
                var newItem = Service.Update(item);
                var index = Model.FindIndex(x => x.ReviewId == item.ReviewId);
                Model[index] = newItem;
            }
            catch (Exception e)
            {
                mCurrentItem = item;
                ExceprionProcessing(e, FunctionModelEnum.Other, mCurrentItem, null, "ChangeStatus");
            }
        }
        public void ReloadItem(ReviewItemViewModel item)
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
                    
                    var index = Model.FindIndex(x => x.ReviewId == item.ReviewId);
                    if (reloadItem.Item == null)
                    {
                        Model.RemoveAt(index);
                    }
                    else
                    {
                        Model[index] = reloadItem;
                    }
                }

                StateHasChanged();
            }
            catch (Exception e)
            {
                mCurrentItem = item;
                ExceprionProcessing(e, FunctionModelEnum.Reload, mCurrentItem, null);
            }
        }

        protected void OpenPreview(ReviewItemViewModel item)
        {
            mPreviewViewModel.IsOpenDialog = true;
            mPreviewViewModel.Title = item.UserName;
            mPreviewViewModel.Text = item.ReviewText;
        }

        protected void ClosePreviewDialog()
        {
            mPreviewViewModel.IsOpenDialog = false;
        }
    }
}
