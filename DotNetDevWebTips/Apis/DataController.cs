using Bogus;
using DotNetDevDB;
using DotNetDevDB.Model;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;
using System.Transactions;

namespace DotNetDevWebTips.Apis {
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "Data"), ApiController]
    public class DataController : ControllerBase {
        private static readonly Func<ModelContext, IAsyncEnumerable<Student>> _compiledQuery
        = EF.CompileAsyncQuery((ModelContext context) => context.Students.Where(b => b.Name.StartsWith("张")));

        [HttpGet]
        public async Task<IActionResult> Bulk([FromServices] ModelContext modelContext) {
            try {
                var r0 = modelContext.Blogs.Where(p => p.Url == "BLACK.COM").ToList();
                var r1 = modelContext.Students.AsNoTracking().Where(p => true).ToList();


                r1.ForEach(p => p.Name = p.Name + "_桀桀桀");
                await modelContext.BulkInsertOrUpdateAsync(r1);
                await modelContext.BulkSaveChangesAsync();

                if (false) {
                    var stus = new Faker<Student>("zh_CN").RuleFor(o => o.Gender, f => f.PickRandom<Gender>()).
                       RuleFor(o => o.FirstName, f => f.Name.FirstName()).
                       RuleFor(o => o.LastName, f => f.Name.LastName()).
                       RuleFor(o => o.Name, (f, u) => u.LastName + u.FirstName).
                       RuleFor(o => o.Age, f => f.Random.Number(16, 50)).Generate(10000).OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ToList();

                    await modelContext.BulkInsertAsync(stus);
                    await modelContext.BulkSaveChangesAsync();
                }
            } catch (Exception ex) {
                return new JsonResult(new { statue = 0, msg = ex.Message }) { StatusCode = StatusCodes.Status500InternalServerError };
            }
            return new JsonResult(new { statue = 1 });
        }


        [HttpGet]
        public async Task<ActionResult> Index([FromServices] ModelContext modelContext) {
            //Console.WriteLine($"ip1:{HttpContext.Connection.RemoteIpAddress?.ToString()}");
            //Console.WriteLine($"ip2:{HttpContext.Request.Headers["HTTP_X_FORWARDED_FOR"].ToString()}");
            //var blogs = modelContext.Blogs
            //    .Join(modelContext.Students, p => p.BlogId, u => u.Id, (p, u) => new { p, u })
            //    .AsSplitQuery()
            //    .ToList();
            using (var scope0 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) {
                //modelContext.Add(new Blog() {
                //    //BlogId = 2,
                //    Title = "嘎嘎嘎",
                //    Url = "无"
                //});
                var blog = modelContext.Blogs.Single(b => b.BlogId == 20);
                blog.Title = "农民";
                modelContext.Update(blog);//因为注入上下文时禁用了对象追踪
                //modelContext.SaveChanges();

                blog = modelContext.Blogs.First();
                blog.Title = "鬼觉神知";
                modelContext.Update(blog);//因为注入上下文时禁用了对象追踪

                {
                    var blob = new Blog() {
                        BlogId = 3
                    };
                    modelContext.Attach(blob);
                    blob.Title = "无绝期·天雷杵";
                    blob.Url = "霓裳飘渺众神渡，觉来犹知梦成空 吾恨天穹无绝期，叹留人间一残生";
                    await modelContext.SaveChangesAsync();
                }

                //{
                //    var blob = new Blog() {
                //        BlogId = 3,
                //        Title = "嘶嘶嘶三"
                //    };
                //    modelContext.Update(blob);
                //    await modelContext.SaveChangesAsync();
                //}


                Console.WriteLine(modelContext.ChangeTracker.DebugView.ShortView);

                await modelContext.SaveChangesAsync();
                scope0.Complete();
            }
            //using (var scope1 = new CommittableTransaction()) {
            //    modelContext.Database.OpenConnection();
            //    modelContext.Database.EnlistTransaction(scope1);
            //    var blog = modelContext.Blogs.First();
            //    blog.Title = "呀哈哈";
            //    modelContext.Update(blog);
            //    await modelContext.SaveChangesAsync();
            //    scope1.Commit();
            //}
            return new JsonResult(modelContext.Blogs.First());
        }

        int a = 1;

        [HttpGet]
        public ActionResult<object> Demo() {

            Stu stu = new Stu();
            var rs = Newtonsoft.Json.JsonConvert.DeserializeObject(Newtonsoft.Json.JsonConvert.SerializeObject(new { ss = stu }));
            Task.FromResult(a);
            return CreatedAtAction(nameof(DDGet), new { id = 1 }, new Stu() {  name="哈哈哈"});
            return new JsonResult(new { ss = stu });
        }

        [HttpGet]
        public ActionResult<Stu> DDGet(int id) {
            return new Stu() { name = id + "" };
        }

        [Swashbuckle.AspNetCore.Annotations.SwaggerIgnore]
        public async Task<int> GetCompile([FromServices] ModelContext modelContext) {
            var idSum = 0;
            await foreach (var blog in _compiledQuery(modelContext)) {
                idSum += blog.Id;
            }
            return idSum;
        }

        [Swashbuckle.AspNetCore.Annotations.SwaggerIgnore]
        public async Task<int> GetCommon([FromServices] ModelContext modelContext) {
            var idSum = 0;
            foreach (var item in await modelContext.Students.Where(b => b.Name.StartsWith("张")).ToListAsync()) {
                idSum += item.Id;
            }
            return idSum;
        }
    }
}

public class Stu {
    public string name { get; set; } = "李四";

    public int age { get; set; } = 0;

    public int size { get; set; } = 11;

}
