using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AP.FamilyTree.Db;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Web.PageModels.Node;

namespace AP.FamilyTree.Web.Data.Services.NodeServices
{
    public class NodeService
    {
        private readonly EFRepository<NodeModel> mNodeRepo;
        private readonly EFRepository<HumanModel> mHumanRepo;

        public NodeService(FamilyTreeDbContext context)
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

        public NodeItemViewModel Create(NodeItemViewModel item)
        {
            var modelHuman = mHumanRepo.Create(item.Human);
            var modelNode = new NodeModel();
            modelNode.TreeId = item.TreeId;
            modelNode.HumanId = modelHuman.Id;
            modelNode.IsActiv = item.IsActiv;
            modelNode.FatherId = item.FatherId;
            modelNode.MotherId = item.MotherId;

            modelNode = mNodeRepo.Create(modelNode);

            return new NodeItemViewModel()
            {
                Father = modelNode.FatherId != 0 ? mHumanRepo.FindById(modelNode.FatherId.GetValueOrDefault()) : null,
                FatherId = modelNode.FatherId,
                Human = modelHuman,
                HumanId = modelNode.HumanId,
                IsActiv = modelNode.IsActiv,
                IsDeleted = modelNode.IsDeleted,
                Mother = modelNode.MotherId != 0 ? mHumanRepo.FindById(modelNode.MotherId.GetValueOrDefault()) : null,
                MotherId = modelNode.MotherId,
                NodeId = modelNode.Id
            };
        }

        public NodeItemViewModel Update(NodeItemViewModel item)
        {
            var modelHuman = mHumanRepo.FindById(item.HumanId);
            modelHuman.Name = item.Human.Name;
            modelHuman.Surname = item.Human.Surname;
            modelHuman.BirthDate = item.Human.BirthDate;
            modelHuman.DeathDate = item.Human.DeathDate;
            modelHuman.MiddleName = item.Human.MiddleName;
            modelHuman.IsDeleted = item.Human.IsDeleted;
            modelHuman = mHumanRepo.Update(modelHuman);

            var modelNode = mNodeRepo.FindById(item.NodeId);
            modelNode.FatherId = item.FatherId;
            modelNode.IsActiv = item.IsActiv;
            modelNode.MotherId = item.MotherId;
            modelNode = mNodeRepo.Update(modelNode);

            return new NodeItemViewModel()
            {
                Father = modelNode.FatherId != 0 ? mHumanRepo.FindById(modelNode.FatherId.GetValueOrDefault()) : null,
                FatherId = modelNode.FatherId,
                Human = modelHuman,
                HumanId = modelNode.HumanId,
                IsActiv = modelNode.IsActiv,
                IsDeleted = modelNode.IsDeleted,
                Mother = modelNode.MotherId != 0 ? mHumanRepo.FindById(modelNode.MotherId.GetValueOrDefault()) : null,
                MotherId = modelNode.MotherId,
                NodeId = modelNode.Id
            };
        }

        public NodeItemViewModel ReloadItem(NodeItemViewModel item)
        {
            var x = mNodeRepo.Reload(item.NodeId);
            if (x == null) return null;
            return ConvertAndGetData(x);
        }
    }
}
