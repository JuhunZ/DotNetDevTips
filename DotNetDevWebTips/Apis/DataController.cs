using DotNetDevDB;
using DotNetDevDB.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetDevWebTips.Apis {
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Data"),ApiController]
    public class DataController : ControllerBase {

        [HttpGet]
        public ActionResult Index([FromServices]ModelContext modelContext) {
            return new JsonResult(modelContext.Set<Student>().FirstOrDefault());
        }
    }
}
