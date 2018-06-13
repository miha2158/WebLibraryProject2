using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebLibraryProject2.Models;


namespace WebLibraryProject2.Controllers
{
    public static class Ext
    {
        public static bool isAdmin(this IIdentity userIdentity)
        {
            bool auth = userIdentity.IsAuthenticated;
            bool admin = false;
            string userName = userIdentity.GetUserName();
            using (var db = new ApplicationDbContext())
                admin = db.Users.First(e => e.UserName == userName).Roles.Any(e => e.RoleId == db.Roles.First(d => d.Name == "admin" || d.Name == "Admin").Id);
            return admin && auth;
        }
    }
}