using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using cSharpTest.Models;
// For Password Hashing :
using Microsoft.AspNetCore.Identity;
// EF Core
using Microsoft.EntityFrameworkCore;
using System.Linq;



namespace cSharpTest.Controllers
{
    public class HomeController : Controller
    {

        private cSharpTestContext _context;
        public HomeController(cSharpTestContext context)
        {
            _context = context;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(LoginRegViewModel model)
        {
            if (ModelState.IsValid)
            {
                User userEmailFound = _context.Users.FirstOrDefault(user => user.Email == model.registerVM.Email);

                if (userEmailFound != null)
                {
                    ViewBag.emailExistsError = "Email already registered, please use a different email.";
                    return View("index");
                }
                else
                {
                    PasswordHasher<RegisterViewModel> Hasher = new PasswordHasher<RegisterViewModel>();
                    model.registerVM.Password = Hasher.HashPassword(model.registerVM, model.registerVM.Password);

                    User NewUser = new User
                    {
                        FirstName = model.registerVM.FirstName,
                        LastName = model.registerVM.LastName,
                        Email = model.registerVM.Email,
                        Password = model.registerVM.Password,
                        CreatedAt = DateTime.Now
                    };


                    _context.Add(NewUser);
                    _context.SaveChanges();

                    int newUserId = (Int32)NewUser.id;
                    // User newUser = _context.Users.FirstOrDefault(user => user.Email == model.registerVM.Email);

                    HttpContext.Session.SetString("currentUserName", model.registerVM.FirstName);
                    HttpContext.Session.SetInt32("currentUserId", newUserId);

                    return RedirectToAction("dashboard", new { id = newUserId });
                    // Handle success

                }
            }
            return View("index", model);
        }


        [HttpPost]
        [Route("login")]
        public IActionResult login(LoginRegViewModel model)
        {
            if (ModelState.IsValid)
            {
                User userFound = _context.Users.FirstOrDefault(user => user.Email == model.loginVM.Email);

                if (userFound == null)
                {
                    ViewBag.loginErrors = "Email not found, please register";
                    return View("index");
                }
                else
                {
                    var Hasher = new PasswordHasher<User>();
                    if (0 != Hasher.VerifyHashedPassword(userFound, userFound.Password, model.loginVM.Password))
                    {
                        HttpContext.Session.SetString("currentUserName", userFound.FirstName);
                        HttpContext.Session.SetInt32("currentUserId", userFound.id);
                        return RedirectToAction("dashboard", new { id = userFound.id });
                    }
                    else
                    {
                        ViewBag.loginErrors = "Password incorrect";
                        return View("index");
                    }
                }
            }
            else
            {
                return View("index", model);
            }
        }

        [HttpGet]
        [Route("dashboard/{id}")]
        public IActionResult dashboard(int id)
        {
            int? loggedInUser = HttpContext.Session.GetInt32("currentUserId");
            if (loggedInUser != null && id == (Int32)loggedInUser)
            {

                List<Activity> allActivities = _context.Activities
                    .Include(i => i.Participants)
                        .OrderBy(x => x.Date)
                            .ThenBy(y => y.Time)
                                .Where(z => z.Date >= DateTime.Now)
                                    .ToList();

                ViewBag.Activities = allActivities;

                ViewBag.username = HttpContext.Session.GetString("currentUserName");
                ViewBag.userId = id;
                return View("dashboard");
            }
            else
            {
                HttpContext.Session.Clear();
                return RedirectToAction("index");
            }
        }


        [HttpGet]
        [Route("planActivity")]
        public IActionResult planActivity()
        {
            int? loggedInUser = HttpContext.Session.GetInt32("currentUserId");
            ViewBag.userId = (Int32)loggedInUser;

            return View("planActivity");
        }


        [HttpPost]
        [Route("createActivity")]
        public IActionResult createActivity(ActivityViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Date <= DateTime.Now)
                {
                    ViewBag.activityError = "Activities cannot be dated prior to tomorrow.";
                    return View("planActivity", model);
                }
                else
                {

                    int? loggedInUser = HttpContext.Session.GetInt32("currentUserId");

                    Activity newActivity = new Activity
                    {
                        OwnerId = (Int32)loggedInUser,
                        OwnerName = HttpContext.Session.GetString("currentUserName"),
                        Title = model.Title,
                        Date = model.Date,
                        Time = model.Time,
                        Duration = model.Duration,
                        DurationScope = model.DurationScope,
                        Desc = model.Desc,
                        CreatedAt = DateTime.Now
                    };

                    _context.Add(newActivity);
                    _context.SaveChanges();

                    int newWeddingId = (Int32)newActivity.id;


                    return RedirectToAction("dashboard", new { id = (Int32)loggedInUser });
                    // Handle success
                }
            }
            else
            {
                return View("planActivity", model);

            }
        }


        [HttpPost]
        [Route("deleteActivity/{id}")]
        public IActionResult deleteActivity(int id)
        {
            Activity activity = _context.Activities.SingleOrDefault(i => i.id == id);

            _context.Activities.Remove(activity);
            _context.SaveChanges();

            return RedirectToAction("dashboard", new { id = (int)HttpContext.Session.GetInt32("currentUserId") });
        }


        [HttpPost]
        [Route("attend/{id}")]
        public IActionResult attend(int id)
        {
            // Conflicting Schedule Function
            // Activity selectedActivity = _context.Activities.SingleOrDefault(i => i.id == id);
            // DateTime activityStart = selectedActivity.Date;

            // if(selectedActivity.DurationScope == "Hours"){
            //     // TimeSpan time = new TimeSpan()
            // }
            // if(selectedActivity.DurationScope == "Minutes"){
                
            // }
            // if(selectedActivity.DurationScope == "Days"){
                
            // }
            // DateTime activityEnd = selectedActivity.Date;

            Participant newParticipant = new Participant();
            newParticipant.UserId = (int)HttpContext.Session.GetInt32("currentUserId");
            newParticipant.ActivityId = id;

            _context.Participants.Add(newParticipant);
            _context.SaveChanges();

            return RedirectToAction("dashboard", new { id = newParticipant.UserId });
        }

        [HttpPost]
        [Route("unAttend/{id}")]
        public IActionResult unAttend(int id)
        {
            Participant currentParticipant = _context.Participants.SingleOrDefault(i => i.UserId == id);
            _context.Participants.Remove(currentParticipant);
            _context.SaveChanges();

            return RedirectToAction("dashboard", new { id = (int)HttpContext.Session.GetInt32("currentUserId") });
        }


        [HttpGet]
        [Route("viewActivity/{id}")]
        public IActionResult viewActivity(int id)
        {

            Activity selectedActivity = _context.Activities.SingleOrDefault(i => i.id == id);
            List<Participant> activityParticipants = _context.Participants
                .Include(i => i.User)
                    .Where(a => a.ActivityId == id)
                        .ToList();

            ViewBag.ActivityId = selectedActivity.id;
            ViewBag.OwnerId = selectedActivity.OwnerId;
            ViewBag.OwnerName = selectedActivity.OwnerName;
            ViewBag.Desc = selectedActivity.Desc;
            ViewBag.Participants = activityParticipants;

            ViewBag.username = HttpContext.Session.GetString("currentUserName");
            ViewBag.userId = (int)HttpContext.Session.GetInt32("currentUserId");
            return View("viewActivity");

        }





        [HttpGet]
        [Route("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index");
        }






        //END OF CONTROLLER
    }
}
