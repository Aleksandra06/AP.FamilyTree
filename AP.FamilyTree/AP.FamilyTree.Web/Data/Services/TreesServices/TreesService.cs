using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AP.FamilyTree.Db;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Web.PageModels.Trees;
using Microsoft.AspNetCore.Components.Authorization;

namespace AP.FamilyTree.Web.Data.Services.TreesServices
{
    public class TreesService
    {
        private readonly EFRepository<TreesModel> mRepo;
        private readonly EFRepository<UserTree> mRepoUserTree;
        private readonly EFRepository<Access> mRepoAccess;
        private string mUserName = "";
        private string mUserId;
        public TreesService(FamilyTreeDbContext context, AuthenticationStateProvider authenticationStateProvider)
        {
            var state = authenticationStateProvider.GetAuthenticationStateAsync().Result;
            if (state != null && state.User != null)
            {
                var user = state.User?.Identity;
                var name = user?.Name ?? "";
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
            mRepo = new EFRepository<TreesModel>(context);
            mRepoUserTree = new EFRepository<UserTree>(context);
            mRepoAccess = new EFRepository<Access>(context);
        }
        public async Task<List<TreeCardItemViewModel>> GetAllForUser()
        {
            var myTreeIdList = mRepoUserTree.Get().Where(x => x.UserId == mUserId).Select(x => x.TreeId).ToList();
            var otherTreeIdList = mRepoAccess.Get().Where(x => x.UserId == mUserId)
                .Select(x => new Tuple<int, bool>(x.TreeId, x.Edit)).ToList();

            var list = new List<TreeCardItemViewModel>();
            foreach (var item in myTreeIdList)
            {
                list.Add(new TreeCardItemViewModel(mRepo.FindById(item), true, true));
            }
            foreach (var item in otherTreeIdList)
            {
                list.Add(new TreeCardItemViewModel(mRepo.FindById(item.Item1), false, item.Item2));
            }

           // var result = mRepo.Get().Select(r => new TreeCardItemViewModel(r)).ToList();
            return await Task.FromResult(list);
        }

        public async Task<TreeCardItemViewModel> GetById(int id)
        {
            var result = mRepo.FindById(id);
            if (result == null)
            {
                return null;
            }

            return await Task.FromResult(new TreeCardItemViewModel(result));
        }

        public async Task<string> GetNameById(int id)
        {
            var result = mRepo.FindById(id);
            if (result == null)//todo
            {
                return null;
            }

            return await Task.FromResult(result.Name);
        }
    }
}
