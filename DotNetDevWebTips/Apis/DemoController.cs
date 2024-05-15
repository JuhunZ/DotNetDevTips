using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetDevWebTips.Apis {
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "Demo"), ApiController]
    public class DemoController : ControllerBase {

        [HttpGet]
        public ActionResult Index() {
            return new JsonResult(new { result = 1 });
        }
    }
}
