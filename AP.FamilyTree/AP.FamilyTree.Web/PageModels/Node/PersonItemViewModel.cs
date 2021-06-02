using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AP.FamilyTree.Db.Models;

namespace AP.FamilyTree.Web.PageModels.Node
{
    public class PersonItemViewModel : ICloneable
    {
        private HumanModel _item;
        public HumanModel Item => _item;
        public PersonItemViewModel()
        {
            _item = new HumanModel();
        }
        public PersonItemViewModel(HumanModel model)
        {
            _item = model;
        }
        public int Id
        {
            get => _item.Id;
            set => _item.Id = value;
        }
        public string Name
        {
            get => _item.Name;
            set => _item.Name = value;
        }
        public string Surname
        {
            get => _item.Surname;
            set => _item.Surname = value;
        }
        public string MiddleName
        {
            get => _item.MiddleName;
            set => _item.MiddleName = value;
        }
        public DateTime? BirthDate
        {
            get => _item.BirthDate;
            set => _item.BirthDate = value;
        }
        public DateTime? DeathDate
        {
            get => _item.DeathDate;
            set => _item.DeathDate = value;
        }
        public int Gender
        {
            get => _item.Gender;
            set => _item.Gender = value;
        }
        public object Clone()
        {
            PersonItemViewModel tempObject = (PersonItemViewModel)this.MemberwiseClone();
            tempObject._item = (HumanModel)_item.Clone();
            return tempObject;
        }
    }
}
