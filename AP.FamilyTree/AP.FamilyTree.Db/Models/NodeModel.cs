using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AP.FamilyTree.Db.Models
{
    [Table("Node")]
    public class NodeModel : ICloneable
    {
        [Key]
        public int Id { get; set; }
        public int HumanId { get; set; }
        public int? MotherId { get; set; }
        public int? FatherId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActiv { get; set; }
        public int TreeId { get; set; }
        public object Clone()
        {
            NodeModel tempObject = (NodeModel)this.MemberwiseClone();
            return tempObject;
        }
    }
}
