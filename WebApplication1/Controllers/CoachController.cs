using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CoachController : Controller
    {
        // GET: Coach
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Login()
        {
            LoginViewModel2 loginModel = new LoginViewModel2();
            return View(loginModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel2 loginModel)
        {
            if (ModelState.IsValid)
            {
                using (Model1 dbModel = new Model1())
                {
                    var user = dbModel.Coaches.FirstOrDefault(u => u.CoachMail == loginModel.CoachMail);

                    if (user != null && user.CoachPassword == loginModel.CoachPassword)
                    {
                        Session["CurrentCoach"] = user;

                        ViewBag.SuccessMessage = "Login successful";
                        return RedirectToAction("Profile", "Coach");

                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password");
                    }
                }
            }

            return View(loginModel);
        }
        public ActionResult Profile()
        {
            // Ensure the coach is logged in
            if (Session["CurrentCoach"] == null)
            {
                return RedirectToAction("Login");
            }

            // Retrieve the current coach from the session
            Coach currentCoach = (Coach)Session["CurrentCoach"];
            using (Model1 dbModel = new Model1())
            {
                currentCoach.AssignedUsers = dbModel.Users.Where(u => u.CoachId == currentCoach.CoachId).ToList();
            }

            // You may want to load additional data related to the coach here

            return View(currentCoach);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(Coach updatedCoach)
        {
            // Ensure the coach is logged in
            if (Session["CurrentCoach"] == null)
            {
                return RedirectToAction("Login");
            }

            // Retrieve the current coach from the session
            Coach currentCoach = (Coach)Session["CurrentCoach"];

            if (ModelState.IsValid)
            {
                using (Model1 dbModel = new Model1())
                {
                    // Update the coach's information in the database
                    currentCoach.CoachName = updatedCoach.CoachName;
                    currentCoach.CoachMail = updatedCoach.CoachMail;
                    // Update other properties as needed

                    dbModel.Entry(currentCoach).State = EntityState.Modified;
                    dbModel.SaveChanges();

                    ViewBag.SuccessMessage = "Profile updated successfully";
                }
            }

            return View("Profile", currentCoach);
        }
        public ActionResult AddExerciseProgram(int userId)
        {
            // Ensure the coach is logged in
            if (Session["CurrentCoach"] == null)
            {
                return RedirectToAction("Login");
            }

            // Retrieve the current coach from the session
            Coach currentCoach = (Coach)Session["CurrentCoach"];

            // You may want to load additional data related to the coach here

            // Create a new ExerciseProgram object with default values
            ExerciseProgram newExerciseProgram = new ExerciseProgram
            {
                UserId = userId,
                // Set other default values as needed
            };

            return View(newExerciseProgram);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExerciseProgram(ExerciseProgram newExerciseProgram)
        {
            // Ensure the coach is logged in
            if (Session["CurrentCoach"] == null)
            {
                return RedirectToAction("Login");
            }

            // Retrieve the current coach from the session
            Coach currentCoach = (Coach)Session["CurrentCoach"];

            // You may want to load additional data related to the coach here

            if (ModelState.IsValid)
            {
                using (Model1 dbModel = new Model1())
                {
                    // Add the new exercise program to the database
                    dbModel.ExercisePrograms.Add(newExerciseProgram);
                    dbModel.SaveChanges();

                    ViewBag.SuccessMessage = "Exercise program added successfully";
                }
            }

            return View(newExerciseProgram);
        }
        public ActionResult AddNutritionPlan(int userId)
        {
            // Ensure the coach is logged in
            if (Session["CurrentCoach"] == null)
            {
                return RedirectToAction("Login");
            }

            // Retrieve the current coach from the session
            Coach currentCoach = (Coach)Session["CurrentCoach"];

            // You may want to load additional data related to the coach here

            // Create a new NutritionPlan object with default values
            NutritionPlan newNutritionPlan = new NutritionPlan
            {
                UserId = userId,
                // Set other default values as needed
            };

            return View(newNutritionPlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNutritionPlan(NutritionPlan newNutritionPlan)
        {
            // Ensure the coach is logged in
            if (Session["CurrentCoach"] == null)
            {
                return RedirectToAction("Login");
            }

            // Retrieve the current coach from the session
            Coach currentCoach = (Coach)Session["CurrentCoach"];

            // You may want to load additional data related to the coach here

            if (ModelState.IsValid)
            {
                using (Model1 dbModel = new Model1())
                {
                    // Add the new nutrition plan to the database
                    dbModel.NutritionPlans.Add(newNutritionPlan);
                    dbModel.SaveChanges();

                    ViewBag.SuccessMessage = "Nutrition plan added successfully";
                }
            }

            return View(newNutritionPlan);
        }
        
        
        public ActionResult SendMessageToUser(int userId)
        {
            // Ensure the coach is logged in
            if (Session["CurrentCoach"] == null)
            {
                return RedirectToAction("Login");
            }

            // Retrieve the current coach from the session
            Coach currentCoach = (Coach)Session["CurrentCoach"];

            using (Model1 dbModel = new Model1())
            {
                // Get the user information from the database
                User user = dbModel.Users.Find(userId);

                // Check if the user exists
                if (user == null)
                {
                    ViewBag.ErrorMessage = "User not found.";
                    return View();
                }

                // Pass user information to the view
                ViewBag.UserName = user.UserName;
                ViewBag.UserId = user.UserId;

                if (Request.HttpMethod == "POST")
                {
                    // Handle the message sending logic here
                    string messageContent = Request.Form["messageContent"];

                    // You can save the message to the database or perform any other necessary action

                    ViewBag.SuccessMessage = "Message sent successfully";
                }

                return View();
            }
        }

        public ActionResult ViewReceivedMessages()
        {
            // Ensure the coach is logged in
            if (Session["CurrentCoach"] == null)
            {
                return RedirectToAction("Login");
            }

            // Retrieve the current coach from the session
            Coach currentCoach = (Coach)Session["CurrentCoach"];

            using (Model1 dbModel = new Model1())
            {
                // Antrenörün kendisine gelen mesajları al
                var receivedMessages = dbModel.Messages
                    .Where(m => m.ReceiverID == currentCoach.CoachId)
                    .ToList();

                return View(receivedMessages);
            }
        }



    }

}