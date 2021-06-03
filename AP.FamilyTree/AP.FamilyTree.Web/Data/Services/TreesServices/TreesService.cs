using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AP.FamilyTree.Db;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Db.Views;
using AP.FamilyTree.Web.Data.Exceptions;
using AP.FamilyTree.Web.Globals;
using AP.FamilyTree.Web.PageModels.Trees;
using Microsoft.AspNetCore.Components.Authorization;

namespace AP.FamilyTree.Web.Data.Services.TreesServices
{
    public class TreesService
    {
        private readonly EFRepository<TreesModel> mRepo;
        private readonly EFRepository<UserTree> mRepoUserTree;
        private readonly EFRepository<Access> mRepoAccess;
        private readonly EFRepository<NodeModel> mRepoNodeModel;
        private readonly EFRepository<HumanModel> mRepoHumanModel;

        private string mUserName = "";
        private string mUserId;

        private readonly FamilyTreeDbContext mContex;
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
            mRepoNodeModel = new EFRepository<NodeModel>(context);
            mRepoHumanModel = new EFRepository<HumanModel>(context);
            mContex = context;
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
                throw new ExceptionByType(ExeptionTypeEnum.OldData);
            }

            return await Task.FromResult(new TreeCardItemViewModel(result));
        }

        public async Task<string> GetNameById(int id)
        {
            var result = mRepo.FindById(id);
            if (result == null)
            {
                throw new ExceptionByType(ExeptionTypeEnum.OldData);
            }

            return await Task.FromResult(result.Name);
        }

        public TreeCardItemViewModel Create(TreeCardItemViewModel item)
        {
            TreesModel model = new TreesModel();
            model.EndDate = item.EndDate;
            model.StartDate = item.StartDate;
            model.Name = item.Name;
            model.Surnames = item.Surnames;

            var newTree = mRepo.Create(model);

            mRepoUserTree.Create(new UserTree()
            {
                TreeId = newTree.Id,
                UserId = mUserId
            });

            return new TreeCardItemViewModel(newTree, true, true);
        }

        public TreeCardItemViewModel Update(TreeCardItemViewModel item)
        {
            var model = mRepo.FindById(item.Id);
            if (model == null)
            {
                throw new ExceptionByType(ExeptionTypeEnum.OldData);
            }
            model.EndDate = item.EndDate;
            model.StartDate = item.StartDate;
            model.Name = item.Name;
            model.Surnames = item.Surnames;

            var newItem = mRepo.Update(model, item.Item.RowVersion);

            return new TreeCardItemViewModel(model, item.Admin, item.Edit);
        }

        public TreeCardItemViewModel Reload(TreeCardItemViewModel item)
        {
            var model = mRepo.Reload(item.Id);
            if (model == null)
            {
                return null;
            }

            var isAdmin = mRepoUserTree.Get().Any(x => x.TreeId == model.Id && x.UserId == mUserId);
            var isEdit = isAdmin == true || (mRepoAccess.Get().Where(x => x.UserId == mUserId && x.TreeId == model.Id)?.FirstOrDefault()?.Edit ?? false);
            return new TreeCardItemViewModel(model, isAdmin, isEdit);
        }

        public void Remove(TreeCardItemViewModel item)
        {
            var userTree = mRepoUserTree.Get(x => x.TreeId == item.Id && x.UserId == mUserId)?.FirstOrDefault();
            if (userTree == null)
            {
                throw new ExceptionByType(ExeptionTypeEnum.OldData);
            }
            var userTreeItem = mRepoUserTree.FindById(userTree.Id);
            if (userTreeItem == null)
            {
                throw new ExceptionByType(ExeptionTypeEnum.OldData);
            }
            mRepoUserTree.Remove(userTreeItem);
            
            var listAccesses = mRepoAccess.Get(x => x.TreeId == item.Id).ToList();
            foreach (var access in listAccesses)
            {
                var itemAccess = mRepoAccess.FindById(access.Id);
                if (itemAccess == null)
                {
                    throw new ExceptionByType(ExeptionTypeEnum.OldData);
                }
                mRepoAccess.Remove(itemAccess);
            }

            var listNode = mRepoNodeModel.Get(x => x.TreeId == item.Id).ToList();
            foreach (var node in listNode)
            {
                var itemNode = mRepoNodeModel.FindById(node.Id);
                if (itemNode == null)
                {
                    throw new ExceptionByType(ExeptionTypeEnum.OldData);
                }
                mRepoNodeModel.Remove(itemNode);
            }

            var listHuman = mRepoHumanModel.Get(x => x.TreeId == item.Id).ToList();
            foreach (var human in listHuman)
            {
                var itemNode = mRepoHumanModel.FindById(human.Id);
                if (itemNode == null)
                {
                    throw new ExceptionByType(ExeptionTypeEnum.OldData);
                }
                mRepoHumanModel.Remove(itemNode);
            }

            var model = mRepo.FindById(item.Id);
            if (model == null)
            {
                throw new ExceptionByType(ExeptionTypeEnum.OldData);
            }
            mRepo.Remove(model);
        }

        public DostupEnum GetDostup(int id)
        {
            var isAdmin = mRepoUserTree.Get().Any(x => x.TreeId == id && x.UserId == mUserId);
            if (isAdmin)
            {
                return DostupEnum.Admin;
            }

            var userTree = mRepoAccess.Get().Where(x => x.UserId == mUserId && x.TreeId == id);
            if (userTree == null || userTree?.Count() == 0)
            {
                return DostupEnum.Not;
            }

            if (userTree.FirstOrDefault().Edit)
            {
                return DostupEnum.Edit;
            }

            return DostupEnum.Look;
        }

        public async Task<List<ViewNameId>> GetTreesForThisUser()
        {
            var list = mContex.GetListTree(mUserId);

            return await Task.FromResult(list);
        }
    }
}
