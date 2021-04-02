using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AP.FamilyTree.Db;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Web.Data.Exceptions;
using AP.FamilyTree.Web.PageModels.User;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AP.FamilyTree.Web.Data.Services.UserServices
{
    public class AccessService
    {
        private readonly EFRepository<Access> mRepoAccess;
        private readonly EFRepository<ViewUserModel> mRepoUserModel;

        private string mUserName = "";
        private string mUserEmail = "";
        private string mUserId;

        private FamilyTreeDbContext mContext;
        public AccessService(FamilyTreeDbContext context, AuthenticationStateProvider authenticationStateProvider)
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
            mRepoAccess = new EFRepository<Access>(context);
            mRepoUserModel = new EFRepository<ViewUserModel>(context);
            mContext = context;
        }
        public async Task<List<AccessItemViewModel>> GetAccessForUser()
        {
            var idUser = mRepoUserModel.Get(x => x.Email == mUserEmail).FirstOrDefault()?.Id;
            var users = mRepoAccess.Get(x => x.AdminUserId == idUser).ToList();
            if (users == null)
            {
                return null;
            }

            List<AccessItemViewModel> list = new List<AccessItemViewModel>();
            foreach (var user in users)
            {
                AccessItemViewModel item = new AccessItemViewModel(user);
                item.TreeName = mContext.GetTreeNameById(user.TreeId);
                item.UserEmail = mContext.GetUserEmailById(user.UserId);
                list.Add(item);
            }

            return await Task.FromResult(list);
        }

        public AccessItemViewModel Create(AccessItemViewModel item)
        {
            var isUser = mRepoUserModel.Get().FirstOrDefault(x => x.Email == item.UserEmail);
            if (isUser == null)
            {
                throw new ExceptionByType(ExeptionTypeEnum.Other, "Пользователь не найден");
            }
            var itemM = new Access();
            itemM.UserId = isUser.Id;
            itemM.Edit = item.Edit;
            itemM.TreeId = item.TreeId;
            itemM.AdminUserId = mUserId;

            var upItem = mRepoAccess.Create(itemM);
            return Convert(upItem);
        }

        public AccessItemViewModel Update(AccessItemViewModel item)
        {
            var itemM = mRepoAccess.FindById(item.Id);
            if (itemM == null)
            {
                throw new ExceptionByType(ExeptionTypeEnum.OldData);
            }

            itemM.Edit = item.Edit;

            var upItem = mRepoAccess.Update(itemM);
            return Convert(upItem);
        }

        private AccessItemViewModel Convert(Access model)
        {
            var item = new AccessItemViewModel(model);
            item.TreeName = mContext.GetTreeNameById(model.TreeId);
            item.UserEmail = mContext.GetUserEmailById(model.UserId);

            return item;
        }

        public void Delete(AccessItemViewModel currentModel)
        {
            var item = mRepoAccess.FindById(currentModel.Id);
            if (item == null)
            {
                throw new ExceptionByType(ExeptionTypeEnum.OldData);
            }

            mRepoAccess.Remove(item);
        }
    }
}
