using DotNetDevDB.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDevDB {
    public sealed class ModelContext : DbContext {
        ILogger<ModelContext> logger;

        public ModelContext(DbContextOptions<ModelContext> options, ILogger<ModelContext> _logger)
            : base(options) {
            this.logger = _logger;
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        #region 使用池化不能用这个
    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //=> optionsBuilder
    //    .LogTo(
    //        LogError,
    //        (eventId, logLevel) => logLevel >= LogLevel.Information
    //                               || eventId == RelationalEventId.ConnectionOpened
    //                               || eventId == RelationalEventId.ConnectionClosed);
        #endregion

        //#region new的方式 web不用1
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



        void LogError(string msg) {
            logger.LogError(msg);
        }
    }


}
