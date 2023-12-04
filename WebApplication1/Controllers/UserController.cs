﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        private User CurrentUser { get; set; }

        // GET: User
        public ActionResult Index()
        {
            // Check if the user is logged in
            if (Session["CurrentUser"] != null)
            {
                User currentUser = (User)Session["CurrentUser"];

                // Check if the current user is an admin (userRole=2)
                if (currentUser.UserRole == 2)
                {
                    using (Model1 dbModel = new Model1())
                    {
                        // Retrieve users with userRole equal to 1 or 3
                        List<User> usersToShow = dbModel.Users.Where(u => u.UserRole == 1 || u.UserRole == 3).ToList();

                        // You can pass the list of users to the view
                        return View("AdminIndex", usersToShow);
                    }
                }
                else
                {
                    // For users with roles other than admin, just show their own information

                    using (Model3 dbModel = new Model3())
                    {
                        List<Goal> goalsToShow = dbModel.Goal.ToList();
                        SelectList goalList = new SelectList(goalsToShow, "TargetId", "TargetName");
                        ViewBag.GoalList = goalList;
                    }

                    return View(currentUser);
                }


            }
            else
            {
                // Redirect to login if the user is not logged in
                return RedirectToAction("Login");
            }
        }


        // Other actions...


        public ActionResult AddOrEdit()
        {
            User newUserModel = new User();
            return View(newUserModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        
        public ActionResult SaveC(User userModel, HttpPostedFileBase UserPhotoFile)
        {
            if (ModelState.IsValid)
            {
                using (Model1 dbModel = new Model1())
                {
                    try
                    {
                        userModel.UserPass = HashPassword(userModel.UserPass);

                        userModel.UserRole = 1;
                        userModel.RecordStatus = 1;
                        if (UserPhotoFile != null && UserPhotoFile.ContentLength > 0)
                        {
                            // ... Resim dosyasını kaydetme işlemleri (önceki kodun devamı) ...
                        }

                        // Yeni bir kullanıcı ekleniyor
                        dbModel.Users.Add(userModel);
                        dbModel.SaveChanges();

                        ViewBag.SuccessMessage = "User added successfully";
                        ModelState.Clear();
                    }
              
               
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                Console.WriteLine($"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                            }
                        }
                    }
                    // Parolayı hash'le
                }
            }

            return View("AddOrEdit", new User());
        }

        // Parolayı hash'leyen yardımcı fonksiyon
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Parolayı byte dizisine çevir
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Byte dizisini string olarak dönüştür
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    stringBuilder.Append(hashedBytes[i].ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
            public ActionResult Edit()
        {
            // Check if the user is logged in
            if (Session["CurrentUser"] != null)
            {
                // Get the current user from the session
                User currentUser = (User)Session["CurrentUser"];
                return View(currentUser);
            }
            else
            {
                // Redirect to login if the user is not logged in
                return RedirectToAction("Login");
            }
        }
        public ActionResult Login()
        {
            LoginDto loginModel = new LoginDto();
            return View(loginModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User userModel, HttpPostedFileBase UserPhotoFile)
        {
            if (ModelState.IsValid)
            {
                using (Model1 dbModel = new Model1())
                {
                    // Diğer güncelleme işlemleri burada...

                    // Fotoğraf güncelleme işlemi
                    if (UserPhotoFile != null && UserPhotoFile.ContentLength > 0)
                    {
                        string uploadFolderPath = Server.MapPath("~/Uploads");

                        if (!Directory.Exists(uploadFolderPath))
                        {
                            Directory.CreateDirectory(uploadFolderPath);
                        }

                        string fileName = Path.GetFileName(UserPhotoFile.FileName);
                        string filePath = Path.Combine(uploadFolderPath, fileName);

                        UserPhotoFile.SaveAs(filePath);

                        userModel.UserPhoto = "~/Uploads/" + fileName;
                    }

                    // Diğer güncelleme işlemleri burada...

                    dbModel.Entry(userModel).State = EntityState.Modified;
                    dbModel.SaveChanges();

                    Session["CurrentUser"] = userModel;

                    ViewBag.SuccessMessage = "User information updated successfully";
                }
            }

            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginDto loginModel)
        {
            if (ModelState.IsValid)
            {
                using (Model1 dbModel = new Model1())
                {
                    var user = dbModel.Users.FirstOrDefault(u => u.UserMail == loginModel.UserMail);

                    if (user != null && user.UserPass == HashPassword(loginModel.UserPass))
                    {
                        // Set the CurrentUser property to the logged-in user
                        Session["CurrentUser"] = user;

                        ViewBag.SuccessMessage = "Login successful";
                        return RedirectToAction("Index", "User");
                       
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password");
                    }
                }
            }

            return View(loginModel);
        }

        [HttpPost]
        public ActionResult SaveTarget(int goal)
        {
            // Check if the user is logged in
            if (Session["CurrentUser"] != null)
            {
                // Get the current user from the session
                User currentUser = (User)Session["CurrentUser"];

                using (Model1 dbModel = new Model1())
                {

                    var user = dbModel.Users.FirstOrDefault(u => u.UserId == currentUser.UserId );

                    user.TargetId = goal;
                    
                    int affectedRows=dbModel.SaveChanges();

                    if (affectedRows > 0)
                    {

                                
                            var coaches =    dbModel.Coaches.FirstOrDefault(x=>x.TargetId==goal && x.CoachQuota>0 );

                        coaches.CoachQuota--;
                        user.CoachId = coaches.CoachId;

                        dbModel.SaveChanges();


                    }


                }



                // Redirect to the Index action
                return RedirectToAction("Index");
            }
            else
            {
                // Redirect to login if the user is not logged in
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult ForgotPassword(User userModel)
        {
            if (ModelState.IsValid)
            {
                using (Model1 dbModel = new Model1())
                {
                    var user = dbModel.Users.FirstOrDefault(u => u.UserMail == userModel.UserMail && u.SecurityQuestion == userModel.SecurityQuestion);

                    if (user != null)
                    {
                        // Set the new password for the user
                        user.UserPass = HashPassword(userModel.UserPass);

                        // Check if the entity is not already in a modified state
                        if (dbModel.Entry(user).State == EntityState.Detached)
                        {
                            // If detached, attach the entity and mark it as modified
                            dbModel.Users.Attach(user);
                            dbModel.Entry(user).State = EntityState.Modified;
                        }

                        // Save changes to the database
                        dbModel.SaveChanges();

                        Session["CurrentUser"] = user;

                        ViewBag.SuccessMessage = "Password reset successful";
                    }
                    else
                    {
                        // Handle the case when the user is not found
                    }
                }
            }

            return RedirectToAction("Login");
        }
        public ActionResult Disable(int userId)
        {
            using (Model1 dbModel = new Model1())
            {
                var user = dbModel.Users.Find(userId);

                if (user != null)
                {
                    user.RecordStatus = 0;
                    dbModel.SaveChanges();

                    ViewBag.SuccessMessage = "User disabled successfully";
                }
                else
                {
                    ViewBag.ErrorMessage = "User not found";
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Enable(int userId)
        {
            using (Model1 dbModel = new Model1())
            {
                var user = dbModel.Users.Find(userId);

                if (user != null)
                {
                    user.RecordStatus = 1;
                    dbModel.SaveChanges();

                    ViewBag.SuccessMessage = "User enabled successfully";
                }
                else
                {
                    ViewBag.ErrorMessage = "User not found";
                }
            }

            return RedirectToAction("Index");
        }
        public ActionResult ViewExerciseProgram()
        {
            // Ensure the user is logged in
            if (Session["CurrentUser"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            // Retrieve the current user from the session
            User currentUser = (User)Session["CurrentUser"];

            using (Model1 dbModel = new Model1())
            {
                // Retrieve all exercise programs for the current user
                List<ExerciseProgram> exercisePrograms = dbModel.ExercisePrograms
                    .Where(ep => ep.UserId == currentUser.UserId)
                    .ToList();

                if (exercisePrograms.Count > 0)
                {
                    // Pass the list of exercise programs to the view
                    return View(exercisePrograms);
                }
                else
                {
                    // If no exercise programs are found, you may handle this case accordingly
                    return View("ExerciseProgramsNotFound");
                }
            }
        }

        public ActionResult ViewNutritionPlan()
        {
            // Ensure the user is logged in
            if (Session["CurrentUser"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            // Retrieve the current user from the session
            User currentUser = (User)Session["CurrentUser"];

            using (Model1 dbModel = new Model1())
            {
                // Retrieve the exercise program for the current user
                NutritionPlan nutritionPlan = dbModel.NutritionPlans.FirstOrDefault(ep => ep.UserId == currentUser.UserId);

                if (nutritionPlan != null)
                {
                    // Pass the exercise program to the view
                    return View(nutritionPlan);
                }
                else
                {
                    // If no exercise program is found, you may handle this case accordingly
                    return View("nutritionPlanNotFound");
                }
            }
        }



    }
}