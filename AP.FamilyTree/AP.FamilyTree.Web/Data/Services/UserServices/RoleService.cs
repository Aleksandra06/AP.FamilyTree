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
    public class RoleService
    {
        private readonly EFRepository<ViewUserModel> mRepoUser;

        private readonly UserManager<IdentityUser> mUserManager;
        private readonly RoleManager<IdentityRole> mRoleManager;

        private string mUserName = "";
        private string mUserEmail = "";
        private string mUserId;

        private FamilyTreeDbContext mContext;
        public RoleService(FamilyTreeDbContext context, AuthenticationStateProvider authenticationStateProvider, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
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
            mRepoUser = new EFRepository<ViewUserModel>(context);
            mUserManager = userManager;
            mRoleManager = roleManager;
            mContext = context;
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

        public async Task<RoleItemViewModel> ReloadItem(RoleItemViewModel item)
        {
            var u = await mUserManager.FindByEmailAsync(item.Login);
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
    }
}
