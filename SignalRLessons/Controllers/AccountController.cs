using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalRLessons.Models.DBO;
using SignalRLessons.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRLessons.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ChatDbContext context;
        readonly IWebHostEnvironment environment;

        public AccountController(
            UserManager<User> _userManager,
            SignInManager<User> _signInManager,
            ChatDbContext _context, 
            IWebHostEnvironment _environment
            )
        {
            userManager = _userManager;
            signInManager = _signInManager;
            context = _context;
            environment = _environment;
        }

        public async Task<IActionResult> AllUsers()
        {

            var allUsers = await context.Users.Where(e => e.UserName != User.Identity.Name).ToListAsync();
            return View(allUsers);
        }

        public async Task<IActionResult> MyPage()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model )
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("", "Пользователь не найден или неверный пароль");
                    return View(model);
                }
                var loginResult = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (loginResult.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Chat", "Chat");
                    }
                }
                return View(model);
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Name = model.Name,
                    Status = 1
                };
                // Сохранение фотографии.
                string photoPath = "/images/Profileimages/defult.webp";
                if (model.Photo != null)
                {
                    photoPath = string.Concat("/images/Profileimages/", DateTime.Now.ToString("dd_MM_yy_mm_ss"), model.Photo.FileName).Replace(" ", "");
                    using (var FS = new FileStream(environment.WebRootPath + photoPath, FileMode.Create))
                    {
                        await model.Photo.CopyToAsync(FS);
                    }
                }

                user.Photo = photoPath;

                var createResult = await userManager.CreateAsync(user, model.Password);

                if (createResult.Succeeded)
                {
                    var newUser = await userManager.FindByNameAsync(model.Email);
                    var loginResult = await signInManager.PasswordSignInAsync(newUser, model.Password, false, false);

                    if (loginResult.Succeeded)
                    {
                        return RedirectToAction("Chat", "Chat");
                    }

                }
                else//иначе
                {
                    foreach (var identityError in createResult.Errors)
                    {
                        ModelState.AddModelError("", identityError.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Chat", "Chat");
        }
    }
}
