using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
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
                    userModel.UserRole = 1;
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
                }
                ModelState.Clear();
            }

            return View("AddOrEdit", new User());
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
            LoginViewModel loginModel = new LoginViewModel();
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
        public ActionResult Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                using (Model1 dbModel = new Model1())
                {
                    var user = dbModel.Users.FirstOrDefault(u => u.UserMail == loginModel.UserMail);

                    if (user != null && user.UserPass == loginModel.UserPass)
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
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(int userId)
        {
            using (Model1 dbModel = new Model1())
            {
                // Retrieve the user from the database based on the userId
                User user = dbModel.Users.Find(userId);

                if (user != null)
                {
                    return View(user);
                }
                else
                {
                    ViewBag.ErrorMessage = "User not found";
                    return RedirectToAction("AdminIndex");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User userModel, HttpPostedFileBase UserPhotoFile)
        {
            if (ModelState.IsValid)
            {
                using (Model1 dbModel = new Model1())
                {
                    // Retrieve the current user from the database
                    User currentUser = dbModel.Users.Find(userModel.UserId);

                    if (currentUser != null)
                    {
                        // Update user information
                        currentUser.UserName = userModel.UserName;
                        currentUser.UserSurname = userModel.UserSurname;
                        // Update other properties as needed

                        // Check if there is a UserPhotoFile in the request (i.e., the user wants to update the profile picture)
                        if (UserPhotoFile != null && UserPhotoFile.ContentLength > 0)
                        {
                            // Same photo update logic as in your SaveC action
                            string uploadFolderPath = Server.MapPath("~/Uploads");

                            if (!Directory.Exists(uploadFolderPath))
                            {
                                Directory.CreateDirectory(uploadFolderPath);
                            }

                            string fileName = Path.GetFileName(UserPhotoFile.FileName);
                            string filePath = Path.Combine(uploadFolderPath, fileName);

                            UserPhotoFile.SaveAs(filePath);

                            currentUser.UserPhoto = "~/Uploads/" + fileName;
                        }

                        // Save changes to the database
                        dbModel.Entry(currentUser).State = EntityState.Modified;
                        dbModel.SaveChanges();

                        ViewBag.SuccessMessage = "User information updated successfully";

                        // Redirect to the AdminIndex action after successful update
                        return RedirectToAction("AdminIndex");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "User not found";
                    }
                }
            }

            // If the ModelState is not valid, return to the EditUser view with the user model
            return View(userModel);
        }
       


    }
}