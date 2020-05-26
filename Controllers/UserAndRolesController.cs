using EverestAlbumStore.Models;
using EverestAlbumStore.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EverestAlbumStore.Controllers
{
    public class UserAndRolesController : Controller
    {
        private ApplicationUserManager _userManager;
        public UserAndRolesController()
        {
        }
        public UserAndRolesController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: UserAndRoles
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> AddUserToRole()
        {
            ApplicationUserAndRoleViewModel vm = new ApplicationUserAndRoleViewModel();
            var Users = await db.Users.ToListAsync();
            var roles = await db.Roles.ToListAsync();
            ViewBag.UserId = new SelectList(Users, "Id", "UserName");
            ViewBag.RoleId = new SelectList(roles, "Id", "Name");
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> AddUserToRole(ApplicationUserAndRoleViewModel model)
        {
            var role = db.Roles.Find(model.RoleId);
            if (role != null)
            {
                await UserManager.AddToRoleAsync(model.UserId, role.Name);
            }
            return RedirectToAction("AddUserToRole");

        }
    }
}