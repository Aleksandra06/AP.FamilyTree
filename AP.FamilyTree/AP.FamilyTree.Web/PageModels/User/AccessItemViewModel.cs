using System;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Web.PageModels.Interfaces;

namespace AP.FamilyTree.Web.PageModels.User
{
    public class AccessItemViewModel : ICloneable, IIsRefreshed
    {
        private Access _item;
        public Access Item => _item;
        public AccessItemViewModel()
        {
            _item = new Access();
        }
        public AccessItemViewModel(Access model)
        {
            _item = model;
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
        public int TreeId
        {
            get
            {
                return _item.TreeId;
            }
            set
            {
                _item.TreeId = value;
            }
        }
        public string UserId
        {
            get
            {
                return _item.UserId;
            }
            set
            {
                _item.UserId = value;
            }
        }
        public bool Edit
        {
            get
            {
                return _item.Edit;
            }
            set
            {
                _item.Edit = value;
            }
        }
        public string UserEmail { get; set; }
        public string TreeName { get; set; }
        public bool IsRefreshed { get; set; } = false;
        public object Clone()
        {
            AccessItemViewModel tempObject = (AccessItemViewModel)this.MemberwiseClone();
            tempObject._item = (Access)_item.Clone();
            return tempObject;
        }
    }
}
