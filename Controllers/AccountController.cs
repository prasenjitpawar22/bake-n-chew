using BakeNChew.Data;
using BakeNChew.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BakeNChew.Controllers;


public class AccountController : Controller
{
    private SignInManager<ApplicationUser> _signInManager;
    private ILogger<AccountController> _logger;
    private IUserStore<ApplicationUser> _userStore;
    private UserManager<ApplicationUser> _userManager;
    private ApplicationDbContext _db;

    public AccountController(UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext db,
        ILogger<AccountController> loginLogger)
    {
        _signInManager = signInManager;
        _logger = loginLogger;
        _userStore = userStore;
        _userManager = userManager;
        _db = db;
    }

    //-------------
    // login
    //-------------
    [HttpGet]
    public async Task<IActionResult> Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            //Redirect to Home...

            return RedirectToAction("Index", "Home");
        }

        _logger.LogInformation("okkk in login page from controller");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        // returnUrl ??= Url.Content("~/");
        // _logger.LogInformation("in Post login method");
        // ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            _logger.LogInformation("valid.");

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                _logger.LogInformation("Invalid details");
                return View();
            }
        }

        // _logger.LogInformation("INvalid.", model.Email, model.Password);

        // If we got this far, something failed, redisplay form
        return View();
    }

    //-------------
    // Register
    //-------------

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        // returnUrl ??= Url.Content("~/");
        // ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        if (ModelState.IsValid)
        {
            var user = CreateUser();

            //congif lastname and firstname
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.CustomerAddress = model.CustomerAddress;
            user.PhoneNumber = model.PhoneNumber;

            await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
            // await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, model.Password);


            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                await _signInManager.SignInAsync(user, isPersistent: false);

                //add cart to user
                int tempIdCart = new Random().Next(1000, 9999);
                CartModel cart = new CartModel
                {
                    CartId = tempIdCart,
                    CartPrice = 0,
                    TotalProducts = 0,
                    CustomerEmail = model.Email,
                    // CustomerId = _userManager.GetUserId(HttpContext.User).ToString(),
                    DateCreated = DateTime.Now.ToUniversalTime()
                };

                _db.Carts.Add(cart);
                _db.SaveChanges();

                return RedirectToAction("Index", "Home");

            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

        }
        return View();
    }

    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    //-------------
    // logout
    //-------------


    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");

        // This needs to be a redirect so that the browser performs a new
        // request and the identity for the user gets updated.
        return RedirectToAction("index", "Home");

    }
}
