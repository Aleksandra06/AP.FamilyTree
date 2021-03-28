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
        private readonly EFRepository<ViewUserModel> mRepoUser;

        private MailingService MailingService;
        private SignInManager<IdentityUser> mSignInManager;
        private readonly UserManager<IdentityUser> mUserManager;
        private readonly RoleManager<IdentityRole> mRoleManager;

        private string mUserName = "";
        private string mUserEmail = "";
        private string mUserId;

        private FamilyTreeDbContext mContext;
        public UserService(FamilyTreeDbContext context, AuthenticationStateProvider authenticationStateProvider, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, MailingService service, RoleManager<IdentityRole> roleManager)
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
            mRepoUser = new EFRepository<ViewUserModel>(context);
            mUserManager = userManager;
            mRoleManager = roleManager;
            mSignInManager = signInManager;
            MailingService = service;
            mContext = context;
        }

        public async Task<UserItemViewModel> GetUserModel()
        {
            var user = await mUserManager.FindByEmailAsync(mUserEmail);
            var model = new UserItemViewModel();
            model.Email = user.Email;

            return await Task.FromResult(model);
        }

        public async Task<List<string>> Save(UserItemViewModel model)
        {
            var messageError = new List<string>();
            if (!string.IsNullOrEmpty(model.Password))
            {
                var user = await mUserManager.FindByEmailAsync(mUserEmail);

                var result = await mUserManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        messageError.Add(error.Description);
                    }
                }
            }

            if (string.IsNullOrEmpty(model.Email)) return await Task.FromResult(messageError);

            var user2 = await mUserManager.FindByEmailAsync(mUserEmail);
            if (mSignInManager.CheckPasswordSignInAsync(user2, model.OldPassword, false)?.Result.Succeeded == true ||
                mSignInManager.CheckPasswordSignInAsync(user2, model.Password, false)?.Result.Succeeded == true)
            {
                if (!model.Email.Equals(user2.Email))
                {
                    user2.Email = model.Email;
                    user2.UserName = model.Email;
                    //user.EmailConfirmed = true;
                    var result = await mUserManager.UpdateAsync(user2);
                    //var result = await mUserManager.SetEmailAsync(user, model.Email);
                    //var user2 = await mUserManager.FindByEmailAsync(model.Email);
                    //var result2 = await mUserManager.SetUserNameAsync(user2, mUserEmail);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            messageError.Add(error.Description);
                        }
                    }
                }
            }
            else
            {
                messageError.Add("Неверен действующий пароль");
            }



            return await Task.FromResult(messageError);
        }

        public async Task<List<RoleItemViewModel>> GetUserRoles()
        {
            var users = mRepoUser.Get().ToList();
            if (users == null)
            {
                return null;
            }

            List<RoleItemViewModel> list = new List<RoleItemViewModel>();
            foreach (var user in users)
            {
                RoleItemViewModel item = new RoleItemViewModel();
                var u = await mUserManager.FindByEmailAsync(user.Email);
                var roles = await mUserManager.GetRolesAsync(u);
                var roleName = roles?.FirstOrDefault();
                if (!string.IsNullOrEmpty(roleName))
                {
                    var role = await mRoleManager.FindByNameAsync(roleName);
                    item.RoleName = role.Name;
                }
                item.Login = user.Email;
                list.Add(item);
            }

            return await Task.FromResult(list);
        }

        public async Task<RoleItemViewModel> UpdateRole(RoleItemViewModel model)
        {
            var users = await mUserManager.FindByEmailAsync(model.Login);
            if (users == null)
            {
                return null;
            }

            var result = await mUserManager.AddToRoleAsync(users, model.RoleName);
            if (result.Succeeded)
            {
                var newModel = new RoleItemViewModel();
                newModel.Login = users.Email;
                newModel.RoleName = model.RoleName;
                return await Task.FromResult(newModel);
            }
            else
            {
                var newModel = await GetRoleItemViewModelByUserName(users.UserName);
                return await Task.FromResult(newModel);
            }
        }

        public async Task<RoleItemViewModel> GetRoleItemViewModelByUserName(string usersUserName)
        {
            RoleItemViewModel item = new RoleItemViewModel();
            var u = await mUserManager.FindByEmailAsync(usersUserName);
            var roles = await mUserManager.GetRolesAsync(u);
            var roleName = roles?.FirstOrDefault();
            if (!string.IsNullOrEmpty(roleName))
            {
                var role = await mRoleManager.FindByNameAsync(roleName);
                item.RoleName = role.Name;
            }
            item.Login = u.Email;

            return await Task.FromResult(item);
        }
        public async Task<List<string>> GetRoles()
        {
            var list = mContext.GetAllRolesSort();

            return await Task.FromResult(list);
        }

        public async Task<bool> IsSysAdminRole()
        {
            var user = await mUserManager.FindByEmailAsync(mUserEmail);
            var roles = await mUserManager.GetRolesAsync(user);
            var roleName = roles?.FirstOrDefault();
            if (roleName == "Administrator")
            {
                return await Task.FromResult(true);
            }
            else
            {
                return await Task.FromResult(false);
            }
        }
    }
}
