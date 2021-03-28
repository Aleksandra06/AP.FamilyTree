using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AP.FamilyTree.Db.Models
{
    [Table("Access")]
    public class Access
    {
        [Key] 
        public int Id { get; set; }
        public int TreeId { get; set; }
        public string UserId { get; set; }
        public bool Edit { get; set; }
    }
}
