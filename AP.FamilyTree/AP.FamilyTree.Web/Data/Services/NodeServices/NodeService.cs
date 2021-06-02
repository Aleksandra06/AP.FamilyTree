﻿using System.Collections.Generic;
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

        public NodeItemViewModel Create(NodeItemViewModel item)
        {
            item.Human.TreeId = item.TreeId;
            var modelHuman = mHumanRepo.Create(item.Human);
            var modelNode = new NodeModel();
            modelNode.TreeId = item.TreeId;
            modelNode.HumanId = modelHuman.Id;
            modelNode.FatherId = item.FatherId ?? 0;
            modelNode.MotherId = item.MotherId ?? 0;

            modelNode = mNodeRepo.Create(modelNode);

            return new NodeItemViewModel()
            {
                Father = modelNode.FatherId != 0 ? mHumanRepo.FindById(modelNode.FatherId) : null,
                FatherId = modelNode.FatherId,
                Human = modelHuman,
                HumanId = modelNode.HumanId,
                Mother = modelNode.MotherId != 0 ? mHumanRepo.FindById(modelNode.MotherId) : null,
                MotherId = modelNode.MotherId,
                NodeId = modelNode.Id
            };
        }

        public NodeItemViewModel Update(NodeItemViewModel item)
        {
            var modelHuman = mHumanRepo.FindById(item.HumanId);
            modelHuman.Name = item.Name;
            modelHuman.Surname = item.Surname;
            modelHuman.BirthDate = item.BirthDate;
            modelHuman.DeathDate = item.DeathDate;
            modelHuman.MiddleName = item.MiddleName;
            modelHuman.Gender = item.Gender;
            modelHuman = mHumanRepo.Update(modelHuman);

            var modelNode = mNodeRepo.FindById(item.NodeId);
            modelNode.FatherId = item.FatherId ?? 0;
            modelNode.MotherId = item.MotherId ?? 0;
            modelNode = mNodeRepo.Update(modelNode);

            return new NodeItemViewModel()
            {
                Father = modelNode.FatherId != 0 ? mHumanRepo.FindById(modelNode.FatherId) : null,
                FatherId = modelNode.FatherId,
                Human = modelHuman,
                HumanId = modelNode.HumanId,
                Mother = modelNode.MotherId != 0 ? mHumanRepo.FindById(modelNode.MotherId) : null,
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
