using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace dojodachi.Controllers
{
    public class dojodachiController : Controller
    {


        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("happiness") == null)
            {
                HttpContext.Session.SetInt32("happiness", 20);
                HttpContext.Session.SetInt32("fullness", 20);
                HttpContext.Session.SetInt32("energy", 50);
                HttpContext.Session.SetInt32("meals", 3);
                HttpContext.Session.SetString("message", "");
                HttpContext.Session.SetInt32("winOrLose", 0);

            }

            if (HttpContext.Session.GetInt32("happiness") > 100 && HttpContext.Session.GetInt32("energy") > 100 && HttpContext.Session.GetInt32("fullness") > 100)
            {
                HttpContext.Session.SetInt32("winOrLose", 1);
                HttpContext.Session.SetString("message", "YOU WIN!!! Your Dojodachi is full, energized, and happy!");
            }

            if (HttpContext.Session.GetInt32("happiness") <= 0 || HttpContext.Session.GetInt32("fullness") <= 0)
            {
                HttpContext.Session.SetInt32("winOrLose", 1);
                HttpContext.Session.SetString("message", "You lose! Your Dojodachi is dead...or very depressed!");
            }

            @ViewBag.happiness = HttpContext.Session.GetInt32("happiness");
            @ViewBag.fullness = HttpContext.Session.GetInt32("fullness");
            @ViewBag.energy = HttpContext.Session.GetInt32("energy");
            @ViewBag.meals = HttpContext.Session.GetInt32("meals");
            @ViewBag.message = HttpContext.Session.GetString("message");
            @ViewBag.winOrLose = HttpContext.Session.GetInt32("winOrLose");
            // return View();
            //OR
            return View("index");
            //Both of these returns will render the same view (You 			only need one!)
        }

        [HttpGet]
        [Route("feed")]
        public IActionResult Feed()
        {
            int? meals = HttpContext.Session.GetInt32("meals");

            if (meals > 0)
            {
                int? fullness = HttpContext.Session.GetInt32("fullness");
                Random rand = new Random();
                int random = rand.Next(5, 10);

                fullness += random;
                meals -= 1;
                string feedMessage = "You fed your Dojodachi 1 Meal and gained " + random + " Fullness.";

                HttpContext.Session.SetInt32("fullness", fullness.GetValueOrDefault());
                HttpContext.Session.SetInt32("meals", meals.GetValueOrDefault());
                HttpContext.Session.SetString("message", feedMessage);
            }

            else
            {
                string feedMessage = "You have no meals to feed your Dojodachi! You should feel bad.";
                HttpContext.Session.SetString("message", feedMessage);
            }

            return RedirectToAction("Index");
        }



        [HttpGet]
        [Route("play")]
        public IActionResult Play()
        {
            int? energy = HttpContext.Session.GetInt32("energy");

            if (energy > 4)
            {
                int? happiness = HttpContext.Session.GetInt32("happiness");
                Random rand = new Random();
                int random = rand.Next(5, 10);

                happiness += random;
                energy -= 5;
                string playMessage = "You played with your Dojodachi, and it lost 5 energy but it gained " + random + " happiness.";

                HttpContext.Session.SetInt32("energy", energy.GetValueOrDefault());
                HttpContext.Session.SetInt32("happiness", happiness.GetValueOrDefault());
                HttpContext.Session.SetString("message", playMessage);
            }

            else
            {
                string playMessage = "Your Dojodachi doesn't have enough energy to play.";
                HttpContext.Session.SetString("message", playMessage);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("work")]
        public IActionResult Work()
        {
            int? energy = HttpContext.Session.GetInt32("energy");

            if (energy > 4)
            {
                int? meals = HttpContext.Session.GetInt32("meals");
                Random rand = new Random();
                int random = rand.Next(1, 3);

                meals += random;
                energy -= 5;
                string workMessage = "You made your Dojodachi work, and it lost 5 energy but it gained " + random + " meals.";

                HttpContext.Session.SetInt32("energy", (Int32)energy);
                HttpContext.Session.SetInt32("meals", (Int32)meals);
                HttpContext.Session.SetString("message", workMessage);
            }

            else
            {
                string workMessage = "Your Dojodachi doesn't have enough energy to work, he is not your slave.";
                HttpContext.Session.SetString("message", workMessage);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("sleep")]
        public IActionResult Sleep()
        {
            int? fullness = HttpContext.Session.GetInt32("fullness");
            int? happiness = HttpContext.Session.GetInt32("happiness");

            if (happiness > 4 && fullness > 4)
            {
                int? energy = HttpContext.Session.GetInt32("energy");
                energy += 15;
                happiness -= 5;
                fullness -= 5;
                string sleepMessage = "Your Dojodachi got some sleep. It lost 5 happiness and fullness, but it gained 15 energy.";

                HttpContext.Session.SetInt32("energy", (Int32)energy);
                HttpContext.Session.SetInt32("happiness", (Int32)happiness);
                HttpContext.Session.SetInt32("fullness", (Int32)fullness);
                HttpContext.Session.SetString("message", sleepMessage);
            }

            else
            {
                string workMessage = "Your Dojodachi doesn't have enough energy to work, he is not your slave.";
                HttpContext.Session.SetString("message", workMessage);
            }

            return RedirectToAction("Index");
        }



        [HttpGet]
        [Route("restart")]
        public IActionResult Restart()
        {
            HttpContext.Session.Clear();


            return RedirectToAction("Index");
        }

    }
}