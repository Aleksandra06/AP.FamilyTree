using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AP.FamilyTree.Db;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Web.Globals;
using AP.FamilyTree.Web.PageModels.Node;

namespace AP.FamilyTree.Web.Data.Services.NodeServices
{
    public class HumanService
    {
        private readonly EFRepository<HumanModel> mHumanRepo;

        public HumanService(FamilyTreeDbContext context)
        {
            mHumanRepo = new EFRepository<HumanModel>(context);
        }

        public List<PersonItemViewModel> GetBySettings(GenderEnum genderEnum, int treeId)
        {
            int gender = genderEnum.GetHashCode();
            var list = mHumanRepo.Get(x => x.Gender == gender && x.TreeId == treeId).ToList();
            return list.Select(x=> Convert(x)).ToList();
        }

        public PersonItemViewModel Convert(HumanModel model)
        {
            var item = new PersonItemViewModel(model);
            return item;
        }
    }
}
