using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace timeDisplay.Controllers
{
    public class timeDisplayController : Controller
    {
        // A GET method
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            DateTime CurrentTime = DateTime.Now;
            // return View();
            //OR
            return View("Index");
            //Both of these returns will render the same view (You only need one!)
        }

        // // A POST method
        // [HttpPost]
        // [Route("")]
        // public IActionResult Other()
        // {
        //     // Return a view (We'll learn how soon!)
        // }
    }
}
