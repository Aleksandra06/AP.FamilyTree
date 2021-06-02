using System;
using System.ComponentModel.DataAnnotations;
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
            Human = new PersonItemViewModel();
        }
        public NodeItemViewModel(NodeModel model, HumanModel human)
        {
            _item = model;
            Human = new PersonItemViewModel(human);
        }

        public int NodeId
        {
            get => _item.Id;
            set => _item.Id = value;
        }
        public int HumanId
        {
            get => _item.HumanId;
            set => _item.HumanId = value;
        }
        public int? MotherId
        {
            get => _item.MotherId;
            set => _item.MotherId = value ?? 0;
        }

        public string MotherIdToString
        {
            get => MotherId != null ? MotherId.ToString() : "";
            set => MotherId = int.Parse(value);
        }
        public int? FatherId
        {
            get => _item.FatherId;
            set => _item.FatherId = value ?? 0;
        }
        public string FatherIdToString
        {
            get => FatherId != null ? FatherId.ToString() : "";
            set => FatherId = int.Parse(value);
        }
        public int TreeId
        {
            get => _item.TreeId;
            set => _item.TreeId = value;
        }
        public PersonItemViewModel Human { get; set; }
        public HumanModel Mother { get; set; }
        public HumanModel Father { get; set; }

        public bool IsRefreshed { get; set; } = false;

        public object Clone()
        {
            NodeItemViewModel tempObject = (NodeItemViewModel)this.MemberwiseClone();
            tempObject._item = (NodeModel)_item.Clone();
            tempObject.Human = this.Mother == null ? null : (PersonItemViewModel)this.Mother.Clone();
            tempObject.Mother = this.Mother == null ? null : (HumanModel)this.Mother.Clone();
            tempObject.Father = this.Mother == null ? null : (HumanModel)this.Mother.Clone();
            return tempObject;
        }
    }
}
