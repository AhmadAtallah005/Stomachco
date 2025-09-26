using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stomachco.Data;
using Stomachco.Models;
using Stomachco.Models.ViewModel;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Stomachco.Controllers
{
    public class AccountController : Controller
    {
        #region configration

        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
        private RoleManager<IdentityRole> roleManager;
        private StomDbContext stomDbContext;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, StomDbContext stomDbContext)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.stomDbContext = stomDbContext;
        }


        #endregion

        #region user

        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync("User"))
                    {
                        await roleManager.CreateAsync(new IdentityRole("User"));
                    }
                    await userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("Login");
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);

                }
            }

            return View(model);
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email!, model.Password!, false, false);

                if (result.Succeeded)
                { return RedirectToAction("Index", "Home"); }
                ModelState.AddModelError("", "Invalid UserName OR Password");
                return View(model);

            }

            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        #endregion

        #region Role

        public IActionResult RolesList()
        {
            return View(roleManager.Roles);
        }
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = model.RoleName
                };

                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {

                    return RedirectToAction("RolesList");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }

                return View(model);
            }
            return View(model);

        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string? id)
        {
            if (id == null)
            {
                return RedirectToAction("RolesList");
            }
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return RedirectToAction("RolesList");
            }

            EditRoleViewModel model = new EditRoleViewModel
            { RoleID = role.Id, RoleName = role.Name };




            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name!))
                {
                    model.Users.Add(user.UserName!);
                }

            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(model.RoleID!);
                if (role == null)
                {
                    return RedirectToAction(nameof(Error));
                }
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("RolesList");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return View(model);
            }
            return View(model);
        }


        public async Task<IActionResult> DeleteRole(string? id)
        {
            if (id == null)
            {
                return RedirectToAction("RolesList");
            }
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return RedirectToAction("RolesList");
            }

            await roleManager!.DeleteAsync(role);

            return RedirectToAction("RolesList");

        }


        public async Task<IActionResult> ModifyUsersInRole(string? id)
        {
            if (id == null)
            {
                return RedirectToAction("RolesList");
            }
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return RedirectToAction("RolesList");
            }

            List<UserRoleViewModel> models = new List<UserRoleViewModel>();


            foreach (var user in userManager.Users)
            {
                UserRoleViewModel userRole = new UserRoleViewModel
                {
                    UserName = user.UserName,
                    UserId = user.Id,

                };

                if (await userManager.IsInRoleAsync(user, role.Name!))
                {
                    userRole.IsSelected = true;
                }
                else
                {
                    userRole.IsSelected = false;
                }


                models.Add(userRole);
            }

            return View(models);
        }
        [HttpPost]
        public async Task<IActionResult> ModifyUsersInRole(string? id, List<UserRoleViewModel> models)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error));
            }
            var role = await roleManager.FindByIdAsync(id);
            if (role == null) { return RedirectToAction(nameof(Error)); }

            IdentityResult result = new IdentityResult();
            for (int i = 0; i < models.Count(); i++)
            {
                var user = await userManager.FindByIdAsync(models[i].UserId!);
                if (models[i].IsSelected && !(await userManager.IsInRoleAsync(user!, role.Name!)))
                {
                    result = await userManager.AddToRoleAsync(user!, role.Name!);
                }
                else if (!models[i].IsSelected && (await userManager.IsInRoleAsync(user!, role.Name!)))
                {
                    result = await userManager.RemoveFromRoleAsync(user!, role.Name!);
                }

            }
            if (result.Succeeded)
            {
                return RedirectToAction("RolesList");
            }
            return View(models);
        }

        public IActionResult Error()
        {
            return View();
        }
        #endregion


        public IActionResult FeedBack()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FeedBack(FeedbackViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await userManager.GetUserAsync(User);

                var Feed = new FeedBack
                {
                    Email = user!.Email,
                    UserId = user.Id,
                    Message = model.Message

                };
                stomDbContext.feedBacks.Add(Feed);
                await stomDbContext.SaveChangesAsync();

                TempData["Success"] = "تم إرسال ملاحظتك بنجاح";
                return RedirectToAction("Index","Home");



            }


            return View(model);

        }





    }
}
