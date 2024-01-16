using GameWeb.Data;
using GameWeb.Entities;
using GameWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameWeb.Controllers;

public class AccountsController : Controller
{
    private readonly SignInManager<ApplicationUsers> _signInManager;
    private readonly UserManager<ApplicationUsers> _userManager;
    private readonly IConfiguration _configuration;

    public AccountsController(UserManager<ApplicationUsers> userManager, SignInManager<ApplicationUsers> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    //GET: Accounts/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: Accounts/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginModel, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var identityResult = await _signInManager
                .PasswordSignInAsync(loginModel.UserName, loginModel.Password, loginModel.RememberMe, false);

            if (identityResult.Succeeded)
            {
                if (returnUrl == null || returnUrl == "/")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }

            ModelState.AddModelError("", "Username or Password incorrect");
        }

        return View(); 
    }

    //GET: Accounts/Register
    public IActionResult Register()
    {
        return View();
    }

    // POST: Accounts/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel registerModel)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUsers()
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email,
            };

            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {
                string roleName = GetUserRoleName();

                if (!string.IsNullOrEmpty(roleName))
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Can't get role name");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(); 
    }

    private string GetUserRoleName()
    {
        if (_configuration != null)
        {
            var getUserName = _configuration.GetSection("ApplicationRoles");

            if (getUserName.Exists())
            {
                return getUserName.GetValue<string>("User");
            }
        }

        ModelState.AddModelError(string.Empty, "Couldn't find the configuration for 'User' !!!");
        return string.Empty;
    }

    //GET: Accounts/Logout
    public IActionResult Logout()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogoutConfirmed()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("IndexGuest", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DontLogout()
    {
        return RedirectToAction("Index", "Home");
    }
}
