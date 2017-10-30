using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace External_Login.Models.MyDBContext
{
    public class MyDBContext :IdentityDbContext
    {
        public MyDBContext():base("DefaultConnection")
        {

        }

    }
}