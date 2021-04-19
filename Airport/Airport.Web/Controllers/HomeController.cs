namespace Airport.Web.Controllers
{
    using System.Diagnostics; 
    using Microsoft.AspNetCore.Mvc; 

    using Services.Contracts;
    using Web.Models;
    using Web.Models.Routes;

    public class HomeController : BaseController
    {
        public HomeController(ITownService towns)
            :base(towns) { }

        public IActionResult Index() =>
            View(new SearchRouteFormModel()
            {
                Towns = this.GenerateSelectListTowns()
            });

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
