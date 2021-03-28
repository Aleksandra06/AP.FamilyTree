using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AP.FamilyTree.Db.Models
{
    [Table("AspNetUsers")]
    public class ViewUserModel
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
