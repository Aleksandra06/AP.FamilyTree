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
            var human = mHumanRepo.FindById(nodeModel.HumanId);

            var item = new NodeItemViewModel(nodeModel, human);

            if (nodeModel.MotherId != 0)
            {
                item.Mother = mHumanRepo.FindById(nodeModel.MotherId);
            }

            if (nodeModel.FatherId != 0)
            {
                item.Father = mHumanRepo.FindById(nodeModel.FatherId);
            }

            return item;
        }
    }
}
