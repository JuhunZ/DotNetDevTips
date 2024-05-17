using DotNetDevDB.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDevDB {
    public sealed class ModelContext : DbContext {
        

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options) {
        }

        public DbSet<Student> Students { get; set; }

        //#region new的方式 web不用
        //public ModelContext() {

        //}
        ///// <summary>
        ///// new的方式 web不用
        ///// </summary>
        ///// <param name="optionsBuilder"></param>
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        //    optionsBuilder.UseSqlServer(@"Data Source=127.0.0.1,1433;Initial Catalog=Demo;Persist Security Info=True;User ID=sa;password=123456;TrustServerCertificate=true");
        //}
        //#endregion

    }


}
