using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Domain.Model
{
    public class Blog
    {
        [Key]
        public long BlogId { get; set; }
        public long CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BlogTitle { get; set; }
        public string BlogDescription { get; set; }
        public DateTime publishedDate { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("CreatedUserId")]
        public virtual User User { get; set; }
    }
}
