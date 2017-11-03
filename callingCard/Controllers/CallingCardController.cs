using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace callingCard.Controllers
{
    public class CallingCardController : Controller
    {
        // [HttpGetAttribute]
        // public string Index()
        // {
        //     return "Hello World!";
        // }

        [HttpGet]
        [Route("/{fname}/{lname}/{age}/{favColor}")]
        public JsonResult Method(string fname, string lname, string age, string favColor)
        {
            var returnObj = new
            {
                firstname = fname,
                lastname = lname,
                age = age,
                favColor = favColor
            };

            return Json(returnObj);
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
