using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AP.FamilyTree.Db;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Web.Data.Exceptions;
using AP.FamilyTree.Web.Globals;
using AP.FamilyTree.Web.PageModels.Review;
using Microsoft.AspNetCore.Components.Authorization;

namespace AP.FamilyTree.Web.Data.Services.UserServices
{
    public class ReviewService
    {
        private readonly EFRepository<Review> mRepoReview;
        private readonly EFRepository<ViewUserModel> mRepoUser;

        private string mUserName = "";
        private string mUserEmail = "";
        private string mUserId;

        private FamilyTreeDbContext mContext;
        public ReviewService(FamilyTreeDbContext context, AuthenticationStateProvider authenticationStateProvider)
        {
            var state = authenticationStateProvider.GetAuthenticationStateAsync().Result;
            if (state != null && state.User != null)
            {
                var user = state.User?.Identity;
                var name = user?.Name ?? "";
                mUserEmail = name;
                var index = name.IndexOf("@");
                if (index > 0)
                {
                    mUserName = name.Substring(0, index);
                }

                var u = authenticationStateProvider.GetAuthenticationStateAsync().Result.User.Identity;
                var claimsIdentity = u as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    // the principal identity is a claims identity.
                    // now we need to find the NameIdentifier claim
                    var userIdClaim = claimsIdentity.Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                    if (userIdClaim != null)
                    {
                        mUserId = userIdClaim.Value;
                    }
                }
            }
            mRepoReview = new EFRepository<Review>(context);
            mRepoUser = new EFRepository<ViewUserModel>(context);
            mContext = context;
        }

        public void Create(Review review)
        {
            review.InsertDate = DateTime.Now.Date;
            review.UserId = mUserId;

            mRepoReview.Create(review);
        }

        public async Task<List<ReviewItemViewModel>> GetModel()
        {
            var list = mRepoReview.Get().Select(x=> Convert(x)).ToList();

            return await Task.FromResult(list);
        }

        public ReviewItemViewModel ReloadItem(ReviewItemViewModel item)
        {
            var x = mRepoReview.Reload(item.ReviewId);
            if (x == null)
            {
                return null;
            }
            return Convert(x);
        }

        public ReviewItemViewModel Update(ReviewItemViewModel item)
        {
            var model = mRepoReview.FindById(item.ReviewId);
            if (model == null)
            {
                throw new ExceptionByType(ExeptionTypeEnum.OldData);
            }

            model.Accepted = item.Accepted;

            var result = mRepoReview.Update(model);

            return Convert(result);
        }

        private ReviewItemViewModel Convert(Review item)
        {
            var model = new ReviewItemViewModel(item);

            model.UserName = mRepoUser.Get(x => x.Id == item.UserId).SingleOrDefault()?.UserName ?? "";

            return model;
        }
    }
}
