using IdentityDemo.Models;
using IdentityDemo.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityDemo.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public IdentityController(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Signup()
        {
            var signupViewModel = new SignupViewModel();
            return View(signupViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel signupViewModel)
        {
            if(ModelState.IsValid)
            {
                if(await _userManager.FindByEmailAsync(signupViewModel.Email) == null)
                {
                    var user = new IdentityUser
                    {
                        Email = signupViewModel.Email,
                        UserName = signupViewModel.Email
                    };
                    var result = await _userManager.CreateAsync(user, signupViewModel.Password);
                    user = await _userManager.FindByEmailAsync(signupViewModel.Email);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    if(result.Succeeded)
                    {
                        var confirmationLink = Url.ActionLink("ConfirmEmail", "Identity", new { userId = user.Id, token = token });
                        await _emailSender.SendEmailAsync("info@mydomain.com", user.Email, "Confirm your email address", confirmationLink);
                        
                            return RedirectToAction("Signin");
                    }

                    ModelState.AddModelError("Signup", string.Join("", result.Errors.Select(x => x.Description)));
                }
            }
            return View(signupViewModel);
        }

        public async Task<IActionResult> Signin()
        {
            return View();
        }

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return new OkResult();
            }
            return new NotFoundResult();
        }
    }
}
