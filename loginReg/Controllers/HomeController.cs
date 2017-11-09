using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using loginReg;

using loginReg.Models;

namespace loginReg.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbConnector _dbConnector;

        public HomeController(DbConnector connect)
        {
            _dbConnector = connect;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if (TempData["registerErrors"] != null)
            {
                ViewBag.registerErrors = TempData["registerErrors"];
            }
            if (TempData["loginErrors"] != null)
            {
                ViewBag.loginErrors = TempData["loginErrors"];
            }
            return View();
        }


        [HttpPost]
        [Route("/register")]
        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                ViewBag.FirstName = newUser.FirstName;
                ViewBag.LastName = newUser.LastName;
                ViewBag.Email = newUser.Email;

                var emailExists = _dbConnector.Query($"SELECT * FROM USER WHERE user.email='{newUser.Email}'");

                if (emailExists.Count != 0)
                {
                    TempData["registerErrors"] = "Email already in use, please register with another.";
                    return RedirectToAction("Index");
                }
                else
                {
                    string insertString = $"INSERT INTO user (firstName, lastName, email, password) VALUES ('{newUser.FirstName}', '{newUser.LastName}', '{newUser.Email}', '{newUser.Password}')";
                    _dbConnector.Execute(insertString);

                    return View("Success");

                }
            }
            else
            {
                return View("Index", newUser);
            }


            // return RedirectToAction("Index");
        }


        [HttpPost]
        [Route("/login")]
        public IActionResult Login(string email, string password)
        {
            string loginEmailString = $"SELECT * FROM USER WHERE user.email='{email}'";
            var emailFound = _dbConnector.Query(loginEmailString);

            if (emailFound.Count == 0)
            {
                TempData["loginErrors"] = "This email has not registered yet.";
                return RedirectToAction("Index");

            }

            else
            {
                var passwordQuery = $"SELECT firstName, lastName, email, password FROM USER WHERE email='{email}' and password = '{password}'";
                var passwordMatch = _dbConnector.Query(passwordQuery);

                if (passwordMatch.Count == 0)
                {
                    TempData["loginErrors"] = "Password incorrect.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.userData = passwordMatch;
                    return View("success");
                }

            }



        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {

            
            return RedirectToAction("Index");
        }



    }
}
