using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Web.Data;
using AP.FamilyTree.Web.PageModels.Interfaces;

namespace AP.FamilyTree.Web.PageModels.Trees
{
    public class TreeCardItemViewModel : ICloneable, IIsRefreshed
    {
        private TreesModel _item;
        public TreesModel Item => _item;
        public TreeCardItemViewModel()
        {
            _item = new TreesModel();
        }
        public TreeCardItemViewModel(TreesModel model)
        {
            _item = model;
        }
        public TreeCardItemViewModel(TreesModel model, bool admin, bool edit)
        {
            _item = model;
            Admin = admin;
            Edit = edit;
        }
        public int Id
        {
            get
            {
                return _item.Id;
            }
            set
            {
                _item.Id = value;
            }
        }
        [Required(ErrorMessage = "Это поле должно быть заполнено")]
        public string Name
        {
            get
            {
                return _item.Name;
            }
            set
            {
                _item.Name = value;
            }
        }
        public DateTime? StartDate
        {
            get
            {
                return _item.StartDate;
            }
            set
            {
                _item.StartDate = value;
            }
        }
        public DateTime? EndDate
        {
            get
            {
                return _item.EndDate;
            }
            set
            {
                _item.EndDate = value;
            }
        }

        public string DateString
        {
            get
            {
                return GlobalFunction.ConvertToLiveYear(StartDate, EndDate);
            }
        }
        [Required(ErrorMessage = "Это поле должно быть заполнена")]
        public string Surnames
        {
            get
            {
                return _item.Surnames;
            }
            set
            {
                _item.Surnames = value;
            }
        }

        public bool Edit = false;
        public bool Admin = false;
        public bool IsRefreshed { get; set; } = false;
        public object Clone()
        {
            TreeCardItemViewModel tempObject = (TreeCardItemViewModel)this.MemberwiseClone();
            tempObject._item = (TreesModel)_item.Clone();
            return tempObject;
        }
    }
}
