using DotNetDevDB;
using DotNetDevWebTips.Apis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TestProject {
    public class UnitTest1 {
        ILogger<UnitTest1> _logger;
        UnitTest1(ILogger<UnitTest1> logger) {
            _logger = logger;
        }

        static DbContextOptions<ModelContext> contextOptions = new DbContextOptionsBuilder<ModelContext>()
   .UseSqlServer(@"Data Source=127.0.0.1,1433;Initial Catalog=Demo;Persist Security Info=True;User ID=sa;password=123456;TrustServerCertificate=true")
   .Options;

        [Fact]
        public async Task Test1() {
            using var db = new ModelContext(contextOptions,_logger: null);
            var result = await new DataController().GetCommon(db);
            Assert.True(result > 0, "1 should not be prime");
        }

        [Fact]
        public async Task Test2() {
            using var db = new ModelContext(contextOptions, _logger: null);
            var result = await new DataController().GetCompile(db);
            Assert.True(result > 0, "1 should not be prime");
        }
    }
}
