using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.FamilyTree.Db.Models
{
    [Table("Review")]
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public string UserId { get; set; }
        public string ReviewText { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
