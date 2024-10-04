using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDevDB.Model {
    [Table("Blogs")]
    public class Blog {
        public int BlogId { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }
    }
}
