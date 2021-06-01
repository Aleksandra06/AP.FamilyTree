using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AP.FamilyTree.Db.Models
{
    [Table("Human")]
    public class HumanModel : ICloneable
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MiddleName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public int Gender { get; set; }
        public int TreeId { get; set; }

        public object Clone()
        {
            HumanModel tempObject = (HumanModel)this.MemberwiseClone();
            return tempObject;
        }
    }
}
