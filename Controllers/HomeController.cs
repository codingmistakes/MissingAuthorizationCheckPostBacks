using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MissingAuthorizationCheckPostBacks.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using MissingAuthorizationCheckPostBacks.Utility;

namespace MissingAuthorizationCheckPostBacks.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            if (HttpContext.Session.Get<User>("User") == null)
            {
                HttpContext.Session.Set<User>("User", new User()
                {
                    ID = 1,
                    Username = "burgun",
                    FullName = "Bedirhan Urgun"
                });
            }
        }

        public ActionResult Index()
        {
            User currentUser = HttpContext.Session.Get<User>("User");
            if (!currentUser.IsAdmin)
            {
                ViewBag.Error = "Only Administrators can create new offers";
            }
            return View();
        }

        [HttpPost]
        public ActionResult Add(Offer offer)
        {
            // process the offer and add it to the inventory
            ViewBag.Result = "A new offer is created and registered into the inventory!";
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
