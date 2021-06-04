using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AP.FamilyTree.Db;
using AP.FamilyTree.Db.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace AP.FamilyTree.Web.Data.Services.UserServices
{
    public class ReviewService
    {
        private readonly EFRepository<Review> mRepoReview;

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
            mContext = context;
        }

        public void Create(Review review)
        {
            review.InsertDate = DateTime.Now.Date;
            review.UserId = mUserId;

            mRepoReview.Create(review);
        }
    }
}
