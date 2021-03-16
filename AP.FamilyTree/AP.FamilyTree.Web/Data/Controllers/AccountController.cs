using System.Threading.Tasks;
using AP.FamilyTree.Db;
using AP.FamilyTree.Web.PageModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AP.FamilyTree.Web.Data.Controllers
{
    public class AccountController : Microsoft.AspNetCore.Mvc.Controller
    {
        private SignInManager<IdentityUser> mSignInManager;
        private readonly UserManager<IdentityUser> mUserManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            mUserManager = userManager;
            mSignInManager = signInManager;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [Route("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await mSignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToPage("/_Host");
                    }
                }
                else
                {
                    model.Error = "Неправильный логин и (или) пароль";
                }
            }
            return View(model);
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { Email = model.Email, UserName = model.Email };
                // добавляем пользователя
                var result = await mUserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await mSignInManager.SignInAsync(user, false);
                    return RedirectToPage("/_Host");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [Route("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await mSignInManager.SignOutAsync();
            return Redirect("~/");
            //return RedirectToPage("/login");
        }
    }
}
