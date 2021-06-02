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
            Human = new HumanModel();
        }
        public NodeItemViewModel(NodeModel model, HumanModel human)
        {
            _item = model;
            Human = human;
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
        [MaxLength(20)]
        [Required]
        public string Name
        {
            get => Human.Name;
            set => Human.Name = value;
        }
        [MaxLength(20)]
        [Required]
        public string Surname
        {
            get => Human.Surname;
            set => Human.Surname = value;
        }
        [MaxLength(20)]
        public string MiddleName
        {
            get => Human.MiddleName;
            set => Human.MiddleName = value;
        }
        public DateTime? BirthDate
        {
            get => Human.BirthDate;
            set => Human.BirthDate = value;
        }
        public DateTime? DeathDate
        {
            get => Human.DeathDate;
            set => Human.DeathDate = value;
        }
        [Required]
        public int Gender
        {
            get => Human.Gender;
            set => Human.Gender = value;
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
