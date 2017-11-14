using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using theWall.Models;


namespace theWall.Controllers
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

            int? sessionUser = HttpContext.Session.GetInt32("userId");
            // int userLoggedIn = (Int32)sessionUser;
            if (sessionUser > 0)
            {
                string getCurrentUserString = $"Select first_name, concat(first_name, ' ', last_name) as username, email from users where id = {sessionUser}";
                var currentUser = _dbConnector.Query(getCurrentUserString);

                foreach (var user in currentUser)
                {
                    foreach (var kvp in user)
                    {
                        if (kvp.Key == "first_name")
                        {
                            ViewBag.userFirstName = kvp.Value;
                        }
                        if (kvp.Key == "username")
                        {
                            ViewBag.username = kvp.Value;
                        }
                    }
                }
                ViewBag.userId = sessionUser;

                string getAllMessagesString = $"SELECT messages.id, concat(users.first_name, ' ', users.last_name) as username, Date_format(messages.created_at, '%c/%e/%Y | %l:%i %p') as created_at,  messages.message, messages.users_id from messages JOIN users ON messages.users_id = users.id Order by messages.created_at Desc";
                var allMessages = _dbConnector.Query(getAllMessagesString);
                ViewBag.Messages = allMessages;

                string getAllCommentsString = $"Select comments.messages_id, comments.users_id, concat(users.first_name, ' ', users.last_name) as username, comments.created_at, comments.comment from comments join users on comments.users_id = users.id order by comments.created_at Asc;";
                var allComments = _dbConnector.Query(getAllCommentsString);
                ViewBag.Comments = allComments;


                return View();
            }

            return RedirectToAction("welcome");
        }

        [HttpGet]
        [Route("welcome")]
        public IActionResult welcome()
        {
            if (TempData["registerErrors"] != null)
            {
                ViewBag.registerErrors = TempData["registerErrors"];
            }
            if (TempData["loginErrors"] != null)
            {
                ViewBag.loginErrors = TempData["loginErrors"];
            }


            return View("LoginReg");
        }


        [HttpPost]
        [Route("login")]
        public IActionResult login(string email, string password)
        {
            string loginEmailString = $"SELECT * FROM USERs WHERE users.email='{email}'";
            var emailFound = _dbConnector.Query(loginEmailString);

            if (emailFound.Count == 0)
            {
                TempData["loginErrors"] = "This email has not registered yet.";
                return RedirectToAction("welcome");
            }
            else
            {
                var passwordQuery = $"SELECT id FROM USERs WHERE email='{email}' and password = '{password}'";
                var passwordMatch = _dbConnector.Query(passwordQuery);

                if (passwordMatch.Count == 0)
                {
                    TempData["loginErrors"] = "Password incorrect.";
                    return RedirectToAction("welcome");
                }
                else
                {
                    foreach (var user in passwordMatch)
                    {
                        foreach (var kvp in user)
                        {
                            if (kvp.Key == "id")
                            {
                                HttpContext.Session.SetInt32("userId", (Int32)kvp.Value);
                            }
                        }
                    }
                    return RedirectToAction("index");
                }
            }
        }


        [HttpPost]
        [Route("/register")]
        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                var emailExists = _dbConnector.Query($"SELECT * FROM USERs WHERE users.email='{newUser.Email}'");

                if (emailExists.Count != 0)
                {
                    TempData["registerErrors"] = "Email already in use, please register with another.";
                    return RedirectToAction("welcome");
                }
                else
                {
                    string insertString = $"INSERT INTO users (first_name, last_name, email, password, created_at) VALUES ('{newUser.FirstName}', '{newUser.LastName}', '{newUser.Email}', '{newUser.Password}', now())";
                    _dbConnector.Execute(insertString);

                    var newUserId = _dbConnector.Query($"SELECT LAST_INSERT_ID();");
                    // HttpContext.Session.SetInt32("userId", newUserId);
                    foreach (var id in newUserId)
                    {
                        foreach (var kvp in id)
                        {
                            if (kvp.Key == "LAST_INSERT_ID()")
                            {
                                int newId = Convert.ToInt32(kvp.Value);

                                // if (Int32.TryParse(9, out newId))
                                // {
                                //     System.Console.WriteLine("Parse Success");                          
                                HttpContext.Session.SetInt32("userId", (Int32)newId);
                                // }
                            }
                        }
                    }

                    return RedirectToAction("index");
                }
            }
            else
            {
                return View("LoginReg", newUser);
            }
        }



        [HttpGet]
        [Route("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.SetInt32("userId", 0);
            return RedirectToAction("welcome");
        }



        [HttpPost]
        [Route("postMessage")]
        public IActionResult postMessage(Message newPost)
        {
            int? authorId = HttpContext.Session.GetInt32("userId");
            newPost.authorId = (Int32)authorId;

            // DateTime postTime = DateTime.Now;
            // postTime.ToString("yyyy-MM-dd H:mm:ss");

            if (ModelState.IsValid)
            {

                string newMessageString = $"insert into messages (message, created_at, users_id) values ('{newPost.messageContent}', now(), {newPost.authorId})";
                _dbConnector.Execute(newMessageString);

                return RedirectToAction("Index");

            }
            else
            {
                return View("Index", newPost);

            }
        }



        [HttpPost]
        [Route("postaComment")]
        public IActionResult postComment(Comment newComment)
        {
            int authorId = (Int32)HttpContext.Session.GetInt32("userId");
            newComment.authorId = authorId;

            // DateTime postTime = DateTime.Now;
            // postTime.ToString("yyyy-MM-dd H:mm:ss");

            if (ModelState.IsValid)
            {

                string newCommentString = $"insert into comments (users_id, messages_id, comment, created_at) values ('{newComment.authorId}', '{newComment.messageId}', '{newComment.commentContent}', now())";
                _dbConnector.Execute(newCommentString);

                return RedirectToAction("Index");

            }
            else
            {
                return View("Index", newComment);

            }
        }


        [HttpGet]
        [Route("deleteMessage/{id}")]
        public IActionResult deleteMessage(string id)
        {

            string deleteMessageString = $"delete from messages where id = {id}";
            _dbConnector.Execute(deleteMessageString);

            System.Console.WriteLine("Message Deleted: ID: " + id);

            return RedirectToAction("Index");

        }

    }
}
