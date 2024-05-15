using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace DotNetDevWebTips.Controllers {
    public class HomeController : Controller {
        public IActionResult Index([FromServices] ILogger<HomeController> logger,int i=0) {
			try {
				var c = 1 / i;
			} catch (Exception ex) {
                logger.LogError(ex,"真的坏事了哦");
			}
            return View();
        }
    }
}
