using System;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Web.PageModels.Interfaces;

namespace AP.FamilyTree.Web.PageModels.Node
{
    public class NodeItemViewModel : ICloneable, IIsRefreshed
    {
        private NodeModel _item;
        public NodeModel Item => _item;
        public NodeItemViewModel()
        {
            _item = new NodeModel();
        }
        public NodeItemViewModel(NodeModel model)
        {
            _item = model;
        }

        public int NodeId
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
        public int HumanId
        {
            get
            {
                return _item.HumanId;
            }
            set
            {
                _item.HumanId = value;
            }
        }
        public int? MotherId
        {
            get
            {
                return _item.MotherId;
            }
            set
            {
                _item.MotherId = value;
            }
        }

        public string MotherIdToString
        {
            get
            {
                return MotherId != null ? MotherId.ToString() : "";
            }
            set
            {
                MotherId = int.Parse(value);
            }
        }
        public int? FatherId
        {
            get => _item.FatherId;
            set => _item.FatherId = value;
        }
        public string FatherIdToString
        {
            get => FatherId != null ? FatherId.ToString() : "";
            set => FatherId = int.Parse(value);
        }
        public bool IsDeleted
        {
            get
            {
                return _item.IsDeleted;
            }
            set
            {
                _item.IsDeleted = value;
            }
        }
        public bool IsActiv
        {
            get
            {
                return _item.IsActiv;
            }
            set
            {
                _item.IsActiv = value;
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

        public HumanModel Human { get; set; }
        public HumanModel Mother { get; set; }
        public HumanModel Father { get; set; }

        public bool IsRefreshed { get; set; } = false;

        public object Clone()
        {
            NodeItemViewModel tempObject = (NodeItemViewModel)this.MemberwiseClone();
            tempObject._item = (NodeModel)_item.Clone();
            tempObject.Human = this.Mother == null ? null : (HumanModel)this.Mother.Clone();
            tempObject.Mother = this.Mother == null ? null : (HumanModel)this.Mother.Clone();
            tempObject.Father = this.Mother == null ? null : (HumanModel)this.Mother.Clone();
            return tempObject;
        }
    }
}
