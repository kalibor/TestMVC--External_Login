using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace External_Login.Models.MyDBContext
{
    public class MyAppUser: IdentityUser
    {
        public string Country { get; set; }

        public int Age { get; set; }

    }
}