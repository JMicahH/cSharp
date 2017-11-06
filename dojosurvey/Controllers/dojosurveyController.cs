using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dojosurvey.Controllers
{
    public class dojosurveyController : Controller
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


        [HttpPost]
        [Route("submit")]
        public IActionResult Submit(string name, string location, string language, string comment)
        {

            @ViewBag.name = name;
            @ViewBag.location = location;
            @ViewBag.language = language;
            @ViewBag.comment = comment;
            

            return View("Submitted");
            //Both of these returns will render the same view (You 			only need one!)
        }
    }
}