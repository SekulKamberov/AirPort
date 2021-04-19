namespace Airport.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Data.Models;
    using Models.Account;

    using Web.Infrastructure.Extensions;
    using Common.Enums;

    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : BaseController
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger logger;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegistrationType() => View();

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterUser(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(RegisterUserFormModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var user = new RegularUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    RegistrationDate = DateTime.UtcNow.ToLocalTime()
                };

                var result = await this.userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    this.logger.LogInformation("User created a new account with password.");

                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

                    await this.signInManager.SignInAsync(user, isPersistent: false);

                    this.logger.LogInformation("User created a new account with password.");

                    return RedirectToLocal(returnUrl);
                }

                AddErrors(result);
            }

            return View(model);
        }

        public IActionResult RegisterCompany() =>
            View(new RegisterCompanyFormModel()
            { 
                Towns = this.GenerateSelectListTowns() 
            });

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterCompany(RegisterCompanyFormModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var company = new Company
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Address = model.Address,
                    ChiefFirstName = model.ChiefFirstName,
                    ChiefLastName = model.ChiefLastName,
                    Description = model.Description,
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    TownId = model.Town,
                    UniqueReferenceNumber = model.UniqueReferenceNumber,
                    Logo = model.Logo.GetFormFileBytes(),
                    RegistrationDate = DateTime.UtcNow
                };

                var result = await this.userManager.CreateAsync(company, model.Password);

                if (result.Succeeded)
                {
                    await this.userManager.AddToRoleAsync(company, Role.Company.ToString());

                    this.logger.LogInformation("Company created a new account with password.");

                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(company);

                    await this.signInManager.SignInAsync(company, isPersistent: false);

                    this.logger.LogInformation("Company created a new account with password.");

                    return RedirectToLocal(returnUrl);
                }

                AddErrors(result);
            }

            model.Towns = this.GenerateSelectListTowns();
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginFormModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await this.signInManager
                    .PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    this.logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    this.logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            this.logger.LogInformation("User logged out.");

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

    }
}
