using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AP.FamilyTree.Db;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Web.PageModels.User;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using NPOI.OpenXmlFormats.Wordprocessing;

namespace AP.FamilyTree.Web.Data.Services.UserServices
{
    public class UserService
    {
        private readonly EFRepository<UserTree> mRepoUserTree;
        private readonly EFRepository<Access> mRepoAccess;

        private MailingService MailingService;
        private SignInManager<IdentityUser> mSignInManager;
        private readonly UserManager<IdentityUser> mUserManager;

        private string mUserName = "";
        private string mUserEmail = "";
        private string mUserId;
        public UserService(FamilyTreeDbContext context, AuthenticationStateProvider authenticationStateProvider, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, MailingService service)
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
            mRepoUserTree = new EFRepository<UserTree>(context);
            mRepoAccess = new EFRepository<Access>(context);
            mUserManager = userManager;
            mSignInManager = signInManager;
            MailingService = service;
        }

        public async Task<UserItemViewModel> GetUserModel()
        {
            var user = await mUserManager.FindByEmailAsync(mUserEmail);
            var model = new UserItemViewModel();
            model.Email = user.Email;

            return await Task.FromResult(model);
        }

        public async Task<UserItemViewModel> Save(UserItemViewModel model)
        {
            var changedEmail = model.Email;
            string changedPassword = model.Password;
            model.ErrorList = new List<string>();
            if (!string.IsNullOrEmpty(model.Password))
            {
                var user = await mUserManager.FindByEmailAsync(mUserEmail);

                var result = await mUserManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
                if (!result.Succeeded)
                {
                    changedPassword = model.OldPassword;
                    foreach (var error in result.Errors)
                    {
                        model.ErrorList.Add(error.Description);
                    }
                }
            }

            if (!string.IsNullOrEmpty(model.Email))
            {
                var user = await mUserManager.FindByEmailAsync(mUserEmail);
                if (mSignInManager.CheckPasswordSignInAsync(user, model.OldPassword, false)?.Result.Succeeded == true ||
                    mSignInManager.CheckPasswordSignInAsync(user, model.Password, false)?.Result.Succeeded == true)
                {
                    if (!model.Email.Equals(user.Email))
                    {
                        user.Email = model.Email;
                        user.UserName = model.Email;
                        user.EmailConfirmed = true;
                        var result = await mUserManager.UpdateAsync(user);
                        //var result = await mUserManager.SetEmailAsync(user, model.Email);
                        //var user2 = await mUserManager.FindByEmailAsync(model.Email);
                        //var result2 = await mUserManager.SetUserNameAsync(user2, mUserEmail);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                model.ErrorList.Add(error.Description);
                            }
                        }
                        else
                        {
                            changedEmail = model.Email;
                            mUserEmail = model.Email;
                        }

                    }
                }
                else
                {
                    model.ErrorList.Add("Неверен действующий пароль");
                    return await GetUserModel();
                }
            }

            if (model.ErrorList == null || model.ErrorList?.Count == 0)
            {
                var result = await mSignInManager.PasswordSignInAsync(changedEmail, changedPassword, false, false);
            }

            var newModel = await GetUserModel();
            if (model.ErrorList != null && model.ErrorList?.Count > 0)
            {
                newModel.ErrorList = model.ErrorList;
            }

            return await Task.FromResult(newModel);
        }
    }
}
