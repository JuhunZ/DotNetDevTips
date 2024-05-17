using DotNetDevDB;
using DotNetDevDB.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetDevWebTips.Apis {
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Data"),ApiController]
    public class DataController : ControllerBase {
        private static readonly Func<ModelContext, IAsyncEnumerable<Student>> _compiledQuery
        = EF.CompileAsyncQuery((ModelContext context) => context.Students.Where(b => b.Name.StartsWith("张")));



        [HttpGet]
        public async Task<ActionResult> Index([FromServices]ModelContext modelContext) {
            return new JsonResult(modelContext.Set<Student>().FirstOrDefault());
        }


        public async Task<int> GetCompile([FromServices] ModelContext modelContext) {
            var idSum = 0;
            await foreach (var blog in _compiledQuery(modelContext)) {
                idSum += blog.Id;
            }
            return idSum;
        }

        public async Task<int> GetCommon([FromServices] ModelContext modelContext) {
            var idSum = 0;
            foreach (var item in await modelContext.Students.Where(b => b.Name.StartsWith("张")).ToListAsync())
            {
                idSum += item.Id;
            }
            return idSum;
        }
    }
}
