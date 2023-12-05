using System;
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
                            // Resim dosyasını kaydetmek için kullanılacak klasörü belirleyin
                            string uploadFolderPath = Server.MapPath("~/Uploads");

                            // Eğer klasör yoksa, oluşturun
                            if (!Directory.Exists(uploadFolderPath))
                            {
                                Directory.CreateDirectory(uploadFolderPath);
                            }

                            // Resim dosyasını kaydetmek için dosya adını belirleyin
                            string fileName = Path.GetFileName(UserPhotoFile.FileName);

                            // Dosya yolunu belirleyin
                            string filePath = Path.Combine(uploadFolderPath, fileName);

                            // Resim dosyasını kaydedin
                            UserPhotoFile.SaveAs(filePath);

                            // UserModel içindeki UserPhoto özelliğini güncelleyin
                            userModel.UserPhoto = "~/Uploads/" + fileName; // Dosya yolunu UserModel'e ekleyin
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

      
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
               
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    stringBuilder.Append(hashedBytes[i].ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
        public ActionResult EditP()
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditP(User userModel, HttpPostedFileBase UserPhotoFile)
        {
            if (ModelState.IsValid)
            {
                using (Model1 dbModel = new Model1())
                {
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

                    dbModel.Entry(userModel).State = EntityState.Modified;
                    dbModel.SaveChanges();

                    Session["CurrentUser"] = userModel;

                    ViewBag.SuccessMessage = "User information updated successfully";
                }
            }

            return View(userModel);
        }
        public ActionResult Login()
        {
            LoginDto loginModel = new LoginDto();
            return View(loginModel);
        }
      
        public ActionResult Edit(int userId)
        {
            using (Model1 dbModel = new Model1())
            {
                // Retrieve the user from the database based on the userId
                var user = dbModel.Users.Find(userId);

                if (user == null)
                {
                    // Handle the case where the user is not found
                    return HttpNotFound();
                }

                // Pass the user object to the view for editing
                return View(user);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(User updatedUser, HttpPostedFileBase UserPhotoFile)
        {
            if (ModelState.IsValid)
            {
                using (Model1 dbModel = new Model1())
                {
                    // Retrieve the existing user from the database
                    var existingUser = dbModel.Users.Find(updatedUser.UserId);

                    if (existingUser != null)
                    {
                        // Update user details
                        existingUser.UserName = updatedUser.UserName;
                        // Update other fields as needed

                        // Update user photo if a new file is provided
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

                            existingUser.UserPhoto = "~/Uploads/" + fileName;
                        }

                        // Save changes to the database
                        dbModel.SaveChanges();

                        // Update the user in the session
                        Session["CurrentUser"] = existingUser;

                        ViewBag.SuccessMessage = "User information updated successfully";
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "User not found";
                    }
                }
            }

            return View("Edit", updatedUser);
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

                    if (user != null && user.RecordStatus == 1 && user.UserPass == HashPassword(loginModel.UserPass ))
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

                    var user = dbModel.Users.FirstOrDefault(u => u.UserId == currentUser.UserId);

                    user.TargetId = goal;

                    int affectedRows = dbModel.SaveChanges();

                    if (affectedRows > 0)
                    {


                        var coaches = dbModel.Coaches.FirstOrDefault(x => x.TargetId == goal && x.CoachQuota > 0);

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
                        user.UserPass = HashPassword(userModel.UserPass);

                        
                        if (dbModel.Entry(user).State == EntityState.Detached)
                        {
                            dbModel.Users.Attach(user);
                            dbModel.Entry(user).State = EntityState.Modified;
                        }

                       
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
        public ActionResult SendMessageToCoach()
        {
            // Kullanıcı giriş yapmış mı kontrol et
            if (Session["CurrentUser"] != null)
            {
                // Aktif kullanıcıyı al
                User currentUser = (User)Session["CurrentUser"];

                // Eğer kullanıcının antrenörü yoksa, hata mesajı göster veya yönlendirme yap
                if (currentUser.CoachId == null)
                {
                    ViewBag.ErrorMessage = "You don't have a coach assigned. Please contact the admin.";
                    return RedirectToAction("Index");
                }

                // Antrenör ID'sini al
                int coachID = currentUser.CoachId.Value;

                // Antrenör bilgilerini veritabanından al
                using (Model1 dbModel = new Model1())
                {
                    Coach coach = dbModel.Coaches.Find(coachID);

                    // Antrenör bilgilerini ViewBag'e ekle
                    ViewBag.CoachName = coach.CoachName;
                    ViewBag.CoachID = coach.CoachId;

                    // Görüntülenecek bir view döndür
                    return View();
                }
            }
            else
            {
                // Kullanıcı giriş yapmamışsa, login sayfasına yönlendir
                return RedirectToAction("Login");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessageToCoachAction(int receiverID, string content)
        {
            if (Session["CurrentUser"] != null)
            {
                User currentUser = (User)Session["CurrentUser"];

                using (MessageModel dbModel = new MessageModel())
                {
                    // Oluşturulan MessageModel sınıfını kullanarak yeni bir mesaj oluşturun
                    Message newMessage = new Message
                    {
                        SenderID = currentUser.UserId,
                        ReceiverID = receiverID,
                        Content = content,
                        SentAt = DateTime.Now
                    };

                    // Yeni mesajı veritabanına ekleyin
                    dbModel.Messages.Add(newMessage);
                    dbModel.SaveChanges();

                    ViewBag.SuccessMessage = "Message sent successfully";
                }
            }
            else
            {
                return RedirectToAction("Login");
            }

            return RedirectToAction("Index");
        }
        public ActionResult SaveProgress()
        {
            // Check if the user is logged in
            if (Session["CurrentUser"] != null)
            {
                // Get the current user from the session
                User currentUser = (User)Session["CurrentUser"];

                // You can create a model to represent progress or use ViewBag/ViewData
                // For simplicity, let's use ViewBag here
                ViewBag.CurrentUserId = currentUser.UserId;

                // Add any other necessary data to ViewBag or ViewData

                return View();
            }
            else
            {
                // Redirect to login if the user is not logged in
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProgressAction(User userModel)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the current user from the database
                using (Model1 dbModel = new Model1())
                {
                    User currentUser = dbModel.Users.Find(userModel.UserId);

                    if (currentUser != null)
                    {
                        // Save progress data to the database
                        // Example: currentUser.Progress = userModel.Progress;

                        // Save changes to the database
                        dbModel.Entry(currentUser).State = EntityState.Modified;
                        dbModel.SaveChanges();

                        ViewBag.SuccessMessage = "Progress saved successfully";
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "User not found";
                    }
                }
            }

            // Redirect to the SaveProgress view to display any messages
            return View("SaveProgress");
        }

    }
}