using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;

using External_Login.Models.MyDBContext;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartup(typeof(External_Login.Startup))]

namespace External_Login
{
    public class Startup
    {
        public static Func<UserManager<MyAppUser>> UserManagerFactory { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                //未登入會導到的頁面
                LoginPath  = new PathString("/Auth/Login")
            });

            UserManagerFactory = () =>
            {
                var usermanager = new UserManager<MyAppUser>(new UserStore<MyAppUser>(new MyDBContext()));

                usermanager.UserValidator = new UserValidator<MyAppUser>(usermanager) {
                    AllowOnlyAlphanumericUserNames = false
                };


                return usermanager;
            };
        }
    }
}
