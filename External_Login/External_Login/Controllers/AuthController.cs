using External_Login.Models.MyDBContext;
using External_Login.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace External_Login.Controllers
{
   


    public class AuthController : Controller
    {
        private readonly UserManager<MyAppUser> userManager;

        public AuthController():this(Startup.UserManagerFactory.Invoke())
        {

        }

        public AuthController(UserManager<MyAppUser> userManager)
        {
            this.userManager = userManager;
        }


        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var viewModel = new LoginViewModel() {
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel viewModel)
        {
       
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = userManager.Find(viewModel.Email, viewModel.Password);


            if (user !=null)
            {
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                SignIn(user);
                return Redirect(GetRedirectUrl(viewModel.ReturnUrl));
            }
            else
            {
                ModelState.AddModelError("", "信箱或密碼錯誤!");
                return View();
            }
           
           
        }

        [HttpGet]
        public ActionResult Regist(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            RegistViewModel model = new RegistViewModel() { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public ActionResult Regist(RegistViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = new MyAppUser() {
                UserName = viewModel.Email,
                Country = viewModel.Country,
                Age = viewModel.Age
            };

            var result = userManager.Create(user,viewModel.Password);
            if (result.Succeeded)
            {
                SignIn(user);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error);

                }
                return View();
            }

        
        }
    

        [HttpGet]
        public ActionResult LogOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                Session.Clear();
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
               
            }
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && userManager !=null)
            {
                userManager.Dispose();
            }

            base.Dispose(disposing);
        }

        private void SignIn(MyAppUser user)
        {
            var identity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            GetAuthenticationManager().SignIn(identity);

        }

        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("Index", "Home");
            }
            else
            {
                return returnUrl;
            }
        }

    }
}