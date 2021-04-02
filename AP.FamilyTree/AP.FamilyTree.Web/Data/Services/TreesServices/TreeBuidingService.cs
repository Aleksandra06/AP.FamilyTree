using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AP.FamilyTree.Db;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Web.PageModels.Node;

namespace AP.FamilyTree.Web.Data.Services.TreesServices
{
    public class TreeBuidingService
    {
        private readonly EFRepository<NodeModel> mNodeRepo;
        private readonly EFRepository<HumanModel> mHumanRepo;

        public TreeBuidingService(FamilyTreeDbContext context)
        {
            mNodeRepo = new EFRepository<NodeModel>(context);
            mHumanRepo = new EFRepository<HumanModel>(context);
        }

        public async Task<List<NodeItemViewModel>> GetByTreeId(int treeId)
        {
            var result = mNodeRepo.Get(x => x.TreeId == treeId).Select(ConvertAndGetData).ToList();

            return await Task.FromResult(result);
        }
        private NodeItemViewModel ConvertAndGetData(NodeModel nodeModel)
        {
            var item = new NodeItemViewModel(nodeModel);

            item.Human = mHumanRepo.FindById(nodeModel.HumanId);

            if (nodeModel.MotherId != null)
            {
                item.Mother = mHumanRepo.FindById(nodeModel.MotherId.GetValueOrDefault());
            }

            if (nodeModel.FatherId != null)
            {
                item.Father = mHumanRepo.FindById(nodeModel.FatherId.GetValueOrDefault());
            }

            return item;
        }
    }
}
