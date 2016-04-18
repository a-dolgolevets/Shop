using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Shop.ServicesFacade.Concrete.Identity;
using Shop.ViewModel.Identity;

namespace Shop.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly IUserService userService;

        public ManageController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId<int>();
            var model = new ManageAccountViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await userService.UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await userService.UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await userService.UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await HttpContext.GetOwinContext().Authentication.TwoFactorBrowserRememberedAsync(userId.ToString())
            };
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await userService.UserManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await userService.UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await userService.SignInManager.SignInAsync(user, false, false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        public ActionResult SetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.UserManager.AddPasswordAsync(User.Identity.GetUserId<int>(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await userService.UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                    if (user != null)
                    {
                        await userService.SignInManager.SignInAsync(user, false, false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            return View(model);
        }

        #region Helpers
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = userService.UserManager.FindById(User.Identity.GetUserId<int>());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}