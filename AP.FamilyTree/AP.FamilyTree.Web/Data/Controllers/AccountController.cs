using System.Threading.Tasks;
using AP.FamilyTree.Web.Data.Models;
using AP.FamilyTree.Web.Data.Services;
using AP.FamilyTree.Web.Globals;
using AP.FamilyTree.Web.PageModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AP.FamilyTree.Web.Data.Controllers
{
    public class AccountController : Microsoft.AspNetCore.Mvc.Controller
    {
        private MailingService MailingService;
        private SignInManager<IdentityUser> mSignInManager;
        private readonly UserManager<IdentityUser> mUserManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, MailingService service)
        {
            mUserManager = userManager;
            mSignInManager = signInManager;
            MailingService = service;
        }

        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("login")]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var emailErrorMessage = EmailVerification.GetNotCorrectStatusEmail(model.Email);
                if (!string.IsNullOrEmpty(emailErrorMessage))
                {
                    ModelState.AddModelError(string.Empty, emailErrorMessage);
                }
                else
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
                        ModelState.AddModelError(string.Empty, "Неправильный логин и (или) пароль");
                        // model.Error = "Неправильный логин и (или) пароль";
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var emailErrorMessage = EmailVerification.GetNotCorrectStatusEmail(model.Email);
                if (!string.IsNullOrEmpty(emailErrorMessage))
                {
                    ModelState.AddModelError(string.Empty, emailErrorMessage);
                }
                else
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
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.Route("forgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.Route("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await mUserManager.FindByEmailAsync(model.Email);
                if (user == null /*|| !(await mUserManager.IsEmailConfirmedAsync(user))*/)
                {
                    // пользователь с данным email может отсутствовать в бд
                    // тем не менее мы выводим стандартное сообщение, чтобы скрыть 
                    // наличие или отсутствие пользователя в бд
                    return View("ForgotPasswordConfirmation");
                }

                var code = await mUserManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                //EmailService emailService = new EmailService();
                var message = new MailItem()
                {
                    ToEmail = model.Email,
                    Subject = "Reset Password",
                    Text = $"Для сброса пароля пройдите по <a href='{callbackUrl}'>ссылке</a>"
                };
                MailingService.SendMessage(message);
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.Route("resetPassword")]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Mvc.Route("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await mUserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View("ResetPasswordConfirmation");
            }
            var result = await mUserManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return View("ResetPasswordConfirmation");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
    }
}
