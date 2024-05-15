using DotNetDevDB.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDevDB {
    public class ModelContext : DbContext {
        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options) {
        }

        public DbSet<Student> Students { get; set; }
    }
}
