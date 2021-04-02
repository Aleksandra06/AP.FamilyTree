using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AP.FamilyTree.Db.Models
{
    [Table("Access")]
    public class Access : ICloneable
    {
        [Key] 
        public int Id { get; set; }
        public int TreeId { get; set; }
        public string UserId { get; set; }
        public bool Edit { get; set; }
        public string AdminUserId { get; set; }
        public object Clone()
        {
            NodeModel tempObject = (NodeModel)this.MemberwiseClone();
            return tempObject;
        }
    }
}
