using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
