using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using bankAccounts.Models;

// For Password Hashing :
using Microsoft.AspNetCore.Identity;
// EF Core
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace bankAccounts.Controllers
{
    public class HomeController : Controller
    {

        private bankAccountsContext _context;

        public HomeController(bankAccountsContext context)
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
                    // OR _context.Users.Add(NewPerson);
                    _context.SaveChanges();


                    HttpContext.Session.SetString("currentUserName", model.registerVM.FirstName);
                    User newUser = _context.Users.FirstOrDefault(user => user.Email == model.registerVM.Email);
                    HttpContext.Session.SetInt32("currentUserId", newUser.UserId);

                    Account NewAccount = new Account
                    {
                        UserId = newUser.UserId,
                        Balance = 0,
                        CreatedAt = DateTime.Now
                    };

                    _context.Add(NewAccount);
                    // OR _context.Users.Add(NewPerson);
                    _context.SaveChanges();


                    return RedirectToAction("account", new { id = newUser.UserId });
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
                        HttpContext.Session.SetInt32("currentUserId", userFound.UserId);
                        return RedirectToAction("account", new { id = userFound.UserId });
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
        [Route("account/{id}")]
        public IActionResult account(int id)
        {
            ViewBag.transactErrors = TempData["transactErrors"];
            int? loggedInUser = HttpContext.Session.GetInt32("currentUserId");
            if (loggedInUser != null && id == (Int32)loggedInUser)
            {

                Account userAccount = _context.Accounts.Include(account => account.Transactions).SingleOrDefault(account => account.UserId == loggedInUser);

                // foreach()
                // ViewBag.Balance = userAccount.Balance;
                // ViewBag.Transactions = userAccount.Transactions;
                ViewBag.username = HttpContext.Session.GetString("currentUserName");
                return View("dashboard", userAccount);
            }
            else
            {
                HttpContext.Session.Clear();
                return RedirectToAction("index");
            }
        }


        [HttpPost]
        [Route("transaction")]
        public IActionResult transact(int amount)
        {
            int? loggedInUser = HttpContext.Session.GetInt32("currentUserId");
            int loggedInUserId = (Int32)loggedInUser;

            Account userAccount = _context.Accounts.FirstOrDefault(account => account.UserId == loggedInUserId);

            userAccount.Balance += amount;

            if(userAccount.Balance < 0){
                TempData["transactErrors"] = "You do not have enough funds to make that withdrawal.";
                return RedirectToAction("account", new { id = loggedInUserId });
            }
            else{
                _context.SaveChanges();

                Transaction newTransaction = new Transaction
                {
                    AccountId = userAccount.AccountId,
                    Amount = (Double)amount,
                    CreatedAt = DateTime.Now,
                };

                _context.Add(newTransaction);
                _context.SaveChanges();

                return RedirectToAction("account", new { id = loggedInUserId });

            }
        }


        [HttpGet]
        [Route("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index");
        }
    }
}
