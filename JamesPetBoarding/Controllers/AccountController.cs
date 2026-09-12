using JamesPetBoarding.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string normalizedEmail = model.Email.Trim().ToLower();

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel employee = dbContext.Employees
                .FirstOrDefault(x => x.Email.ToLower() == normalizedEmail);

            if (employee == null || !employee.IsActive)
            {
                ModelState.AddModelError(
                    "",
                    "Invalid login attempt.");

                return View(model);
            }

            ApplicationUser identityUser = await UserManager.FindByEmailAsync(model.Email);

            if (identityUser == null)
            {
                ModelState.AddModelError(
                    "",
                    "Invalid login attempt.");

                return View(model);
            }

            if (!identityUser.EmailConfirmed)
            {
                ModelState.AddModelError(
                    "",
                    "You must confirm your email address before logging in.");

                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(
                model.Email, 
                model.Password, 
               isPersistent: false,
                shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Dashboard");

                case SignInStatus.LockedOut:
                    return View("Lockout");

                case SignInStatus.Failure:
                default:

                    int failedAttempts = await UserManager.GetAccessFailedCountAsync(identityUser.Id);

                    int maxAttempts = UserManager.MaxFailedAccessAttemptsBeforeLockout;

                    int attemptsRemaining = maxAttempts - failedAttempts;

                    if (attemptsRemaining == 1)
                    {
                        ModelState.AddModelError(
                            "",
                            "Invalid email or password. You have 1 attempt remaining. " +
                            "One more failed attempt will temporarily lock your account.");
                    }
                    else
                    {
                        ModelState.AddModelError(
                            "",
                            "Invalid email or password. You have " +
                            attemptsRemaining +
                            " attempts remaining.");
                    }
                   
                    return View(model);
            }
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string normalizedEmail = model.Email.Trim().ToLower();

                ApplicationDbContext dbContext = new ApplicationDbContext();

                EmployeeModel employee = dbContext.Employees
                    .FirstOrDefault(x => x.Email.ToLower() == normalizedEmail);

                if (employee == null || !employee.IsActive)
                {
                    ModelState.AddModelError(
                        "",
                        "Registration is not available for this email address. Please contact an administrator.");

                    model.Email = "";

                    return View(model);

                }

                ApplicationUser existingIdentityUser = UserManager.FindByEmail(model.Email.Trim());

                if (existingIdentityUser != null)
                {
                    ModelState.AddModelError(
                        "",
                        "Registration is not available for this email address. Please contact an administrator.");

                    model.Email = "";

                    return View(model);

                }

                var user = new ApplicationUser 
                { 
                    UserName = model.Email, 
                    Email = model.Email 
                };

                var result = await UserManager.CreateAsync(user, model.Password);
               
                if (result.Succeeded)
                {
                    
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    
                    var callbackUrl = Url.Action(
                        "ConfirmEmail", 
                        "Account", 
                        new { 
                            userId = user.Id, 
                            code = code 
                        }, 
                        protocol: Request.Url.Scheme);

                    await UserManager.SendEmailAsync(
                        user.Id, 
                        "Confirm your Paws & Reservations Account", 
                        "<p>Welcome to Paws & Reservations!</p>" +
                        "<p>Your account has been successfully created.</p>" +
                        "<p>Please confirm your email address to complete your account setup.</p>" +
                        "<p><a href=\"" + callbackUrl + "\">Confirm Email Address</a></p>" +
                        "<p>If you did not create this account, you can ignore this email.</p>" +
                        "<p>Thank you,<br />Paws & Reservations, LLC</p>");

                    return RedirectToAction("RegistrationConfirmation");
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/RegistrationConfirmation
        [AllowAnonymous]
        public ActionResult RegistrationConfirmation()
        {
            return View();
        }

        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);

                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    return View("ForgotPasswordConfirmation");
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                var callbackUrl = Url.Action(
                    "ResetPassword", 
                    "Account", 
                    new { userId = user.Id, code = code }, 
                    protocol: Request.Url.Scheme);	
                
                await UserManager.SendEmailAsync(
                    user.Id, 
                    "Reset Your Paws & Reservations Password", 
                    "<p>We received a request to reset your Paws & Reservations password.</p>" +
                    "<p>Click the link below to choose a new password.</p>" +
                    "<p><a href=\"" + callbackUrl + "\">Reset Password</a></p>" +
                    "<p>If you did not request a password reset, you can ignore this email.</p>" +
                    "<p>Thank you,<br />Paws & Reservations, LLC</p>");

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        #endregion
    }
}
