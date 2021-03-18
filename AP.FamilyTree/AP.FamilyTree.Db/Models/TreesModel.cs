using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AP.FamilyTree.Db.Models
{
    [Table("Trees")]
    public class TreesModel : ICloneable
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Surnames { get; set; }

        public object Clone()
        {
            TreesModel tempObject = (TreesModel)this.MemberwiseClone();
            return tempObject;
        }
    }
}
