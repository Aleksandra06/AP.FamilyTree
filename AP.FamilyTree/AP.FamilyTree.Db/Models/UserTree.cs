using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AP.FamilyTree.Db.Models
{
    [Table("UserTree")]
    public class UserTree
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TreeId { get; set; }
    }
}
