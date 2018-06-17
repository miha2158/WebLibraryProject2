using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
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

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
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

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
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
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
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

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public ActionResult TestFill()
        {
            if(User.IsInRole("Admin"))
                using (var db = new LibraryDatabase())
                {
                    if(!db.Authors.Any())
                    {
                        for (int i = 0; i < 6; i++)
                            db.Authors.Add(Author.FillBlanks());
                        db.SaveChanges();
                    }
                    var authors = db.Authors.ToArray();

                    if(!db.Readers.Any())
                    {
                        for (int i = 0; i < 10; i++)
                            db.Readers.Add(Reader.FillBlanks());
                        db.SaveChanges();
                    }
                    var readers = db.Readers.ToArray();

                    Courses[] courses;
                    if (!db.Courses.Any())
                    {
                        courses = new[]
                        {
                            new Courses {Id = 1, Course = 1},
                            new Courses {Id = 2, Course = 2},
                            new Courses {Id = 3, Course = 3},
                            new Courses {Id = 4, Course = 4},
                        };
                        foreach (var t in courses)
                            db.Courses.Add(t);
                        db.SaveChanges();
                    }
                    courses = db.Courses.ToArray();

                    Discipline[] disciplines;
                    if (!db.Disciplines.Any())
                    {
                        disciplines = new[]
                        {
                            new Discipline
                            {
                                Id = 1,
                                Name = "Программирование",
                            },
                            new Discipline
                            {
                                Id = 2,
                                Name = "Конструирование ПО",
                            },
                            new Discipline
                            {
                                Id = 3,
                                Name = "НИС",
                            }
                        };
                        foreach (var t in disciplines)
                            db.Disciplines.Add(t);
                        db.SaveChanges();
                    }
                    disciplines = db.Disciplines.ToArray();

                    Publication[] publications;
                    if(!db.Publications.Any())
                    {
                        publications = new[]
                        {
                            new Publication("Принципы программирования",
                                            authors[2],
                                            ePublicationType.None,
                                            eBookPublication.Book,
                                            new DateTime(1985, 4, 1),
                                            "Росмэн")
                            {
                                Id = 1,
                                Courses = new[]
                                {
                                    courses[0],
                                    courses[1]
                                },
                                Disciplines = new[]
                                {
                                    disciplines[0]
                                }
                            },
                            new Publication("Справочник по C#",
                                            new[]
                                            {
                                                authors[1],
                                                authors[2]
                                            },
                                            ePublicationType.None,
                                            eBookPublication.Book,
                                            new DateTime(2011, 6, 1),
                                            "Справочники")
                            {
                                Id = 3,
                                Courses = new[]
                                {
                                    courses[0],
                                },
                                Disciplines = new[]
                                {
                                    disciplines[0],
                                }
                            },
                            new Publication("Pascal.NET programming guide",
                                            new[]
                                            {
                                                authors[2],
                                                authors[3]
                                            },
                                            ePublicationType.Educational,
                                            eBookPublication.Publication,
                                            new DateTime(2001, 8, 1),
                                            "Питер")
                            {
                                Id = 2,
                                Courses = new[]
                                {
                                    courses[1],
                                    courses[3]
                                },
                                Disciplines = new[]
                                {
                                    disciplines[0],
                                    disciplines[1]
                                }
                            },
                            new Publication("Как писать божественный код",
                                            authors[5],
                                            ePublicationType.Scientific,
                                            eBookPublication.Publication,
                                            new DateTime(2018, 3, 1),
                                            null)
                            {
                                Id = 6,
                                InternetLocation = "https://youtube.com/",
                                Courses = new[]
                                {
                                    courses[2],
                                    courses[3]
                                },
                                Disciplines = new[]
                                {
                                    disciplines[0],
                                }
                            },
                            new Publication("Почему Perl 6 - лучший язык программирования",
                                            new[]
                                            {
                                                authors[3],
                                                authors[4],
                                            },
                                            ePublicationType.Educational,
                                            eBookPublication.Publication,
                                            new DateTime(2015, 1, 1), null)
                            {
                                Id = 4,
                                InternetLocation = "https://google.com/",
                                Courses = new[]
                                {
                                    courses[1],
                                    courses[2]
                                },
                                Disciplines = new[]
                                {
                                    disciplines[1],
                                }
                            },
                            new Publication("Где учиться на программиста",
                                            authors[5],
                                            ePublicationType.Educational,
                                            eBookPublication.Publication,
                                            new DateTime(2016, 11, 1),
                                            null)
                            {
                                Id = 5,
                                InternetLocation = "https://wikipedia.org/",
                                Courses = new[]
                                {
                                    courses[1],
                                    courses[3],
                                },
                                Disciplines = new[]
                                {
                                    disciplines[2],
                                }
                            },
                        };
                        foreach (var t in publications)
                            db.Publications.Add(t);
                        db.SaveChanges();
                    }
                    publications = db.Publications.ToArray();

                    Stats[] stats;
                    if(!db.Stats.Any())
                    {
                        stats = new[]
                        {
                            new Stats
                            {
                                Id = 1,
                                DateTaken = new DateTime(2016, 01, 09),
                                Publication = publications[2]
                            },
                            new Stats
                            {
                                Id = 2,
                                DateTaken = new DateTime(2017, 06, 10),
                                Publication = publications[2]
                            },
                            new Stats
                            {
                                Id = 3,
                                DateTaken = new DateTime(2017, 11, 15),
                                Publication = publications[1]
                            },
                            new Stats
                            {
                                Id = 4,
                                DateTaken = new DateTime(2018, 08, 20),
                                Publication = publications[1]
                            },
                            new Stats
                            {
                                Id = 5,
                                DateTaken = new DateTime(2018, 03, 01),
                                Publication = publications[2]
                            },
                        };
                        foreach (var t in stats)
                            db.Stats.Add(t);
                        db.SaveChanges();
                    }

                    BookLocation[] locations;
                    if(!db.BookLocations.Any())
                    {
                        locations = new[]
                        {
                            new BookLocation
                            {
                                Id = 1,
                                Room = 307,
                                Place = "здесь",
                                IsTaken = true,
                                Reader = readers[2],
                                Publication = publications[2]
                            },
                            new BookLocation
                            {
                                Id = 2,
                                Room = 321,
                                Place = "где-то была",
                                IsTaken = false,
                                Publication = publications[2]
                            },
                            new BookLocation
                            {
                                Id = 3,
                                Room = 501,
                                Place = "в столе",
                                IsTaken = true,
                                Reader = readers[1],
                                Publication = publications[1]
                            },

                            new BookLocation
                            {
                                Id = 4,
                                Room = 321,
                                Place = "на верхней полке шкафа",
                                IsTaken = false,
                                Publication = publications[2]
                            },
                            new BookLocation
                            {
                                Id = 5,
                                Room = 318,
                                Place = "в правом шкафу слева",
                                IsTaken = false,
                                Publication = publications[0]
                            },
                            new BookLocation
                            {
                                Id = 6,
                                Room = 302,
                                Place = "на столе",
                                IsTaken = false,
                                Publication = publications[1]
                            },
                            new BookLocation
                            {
                                Id = 7,
                                Room = 323,
                                Place = "под потолком",
                                IsTaken = false,
                                Publication = publications[0]
                            },
                        };
                        foreach (var t in locations)
                        {
                            db.BookLocations.Add(t);
                        }
                        db.SaveChanges();
                    }
                }

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
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

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
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}