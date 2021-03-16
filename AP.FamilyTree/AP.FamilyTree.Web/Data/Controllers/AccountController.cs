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

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Logout()
        //{
        //    await mSignInManager.SignOutAsync();
        //    return RedirectToPage("login");
        //}
    }
}
