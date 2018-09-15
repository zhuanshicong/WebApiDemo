using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityMiddleware.IdentityProvider.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMiddleware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;

        public AccountController(UserManager<UserModel> userManager)
        {
            _userManager = userManager;
        }
        [HttpPost("Register")]
        [Authorize(Roles = "Root")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {

            if (ModelState.IsValid)
            {
                var user = new UserModel
                {
                    UserName = model.UserName,
                    NickName = model.NickName,
                    JoinDate = DateTime.Now,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Ok();
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    //_logger.LogInformation(3, "User created a new account with password.");
                    //return RedirectToLocal(returnUrl);
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code,item.Description);
                }
                return BadRequest(ModelState);
            }

            return BadRequest(ModelState);
        }



        [HttpGet("Test")]
        [Authorize(Roles = "Root")]
        public IActionResult Test()
        {
            return Ok();
        }
        [HttpPost("AddUserRole")]
        [Authorize(Roles = "Root")]
        public async Task<IActionResult> AddUserRole(AddUserRoleModel model)
        {

            if (ModelState.IsValid)
            {
                var userModel = await _userManager.FindByNameAsync(model.UserName);
                var result = await _userManager.AddToRolesAsync(userModel, model.RoleNames);
                if (result.Succeeded)
                {
                    return Ok();
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
                return BadRequest(ModelState);
            }

            return BadRequest(ModelState);
        }


    }
}