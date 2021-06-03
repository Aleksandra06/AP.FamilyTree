using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AP.FamilyTree.Db.Interfaces;

namespace AP.FamilyTree.Db.Models
{
    [Table("Node")]
    public class NodeModel : ICloneable, IsConcurrency
    {
        [Key]
        public int Id { get; set; }
        public int HumanId { get; set; }
        public int MotherId { get; set; }
        public int FatherId { get; set; }
        public int TreeId { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public object Clone()
        {
            NodeModel tempObject = (NodeModel)this.MemberwiseClone();
            return tempObject;
        }
    }
}
