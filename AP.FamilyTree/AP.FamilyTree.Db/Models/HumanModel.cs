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
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public int Gender { get; set; }
        public int TreeId { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime? WeddingDate { get; set; }
        public string PlaceOfDeath { get; set; }
        public string BurialPlace { get; set; }
        public string Nationality { get; set; }
        public string Works { get; set; }
        public string Biography { get; set; }
        public byte[] Photo { get; set; }

        public object Clone()
        {
            HumanModel tempObject = (HumanModel)this.MemberwiseClone();
            return tempObject;
        }
    }
}
