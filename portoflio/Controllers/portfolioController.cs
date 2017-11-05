using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace portfolio.Controllers
{
    public class portfolioController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            // return View();
            //OR
            return View("Index");
            //Both of these returns will render the same view (You 			only need one!)
        }

        [HttpGet]
        [Route("portfolio")]
        public IActionResult Portfolio()
        {
            // return View();
            //OR
            return View("Portfolio");
            //Both of these returns will render the same view (You 			only need one!)
        }


        [HttpGet]
        [Route("contact")]
        public IActionResult Contact()
        {
            // return View();
            //OR
            return View("Contact");
            //Both of these returns will render the same view (You 			only need one!)
        }

    }
}