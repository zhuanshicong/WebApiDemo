using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        [HttpPost("RegisterMany")]
        [Authorize(Roles = "Root")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<List<RegisterManyResultModel>>> RegisterMany(RegisterManyModel[] model)
        {
            var finalResult= new List<RegisterManyResultModel>();
            if (ModelState.IsValid)
            {
                foreach (var item in model)
                {
                    var user = new UserModel
                    {
                        UserName = item.UserName,
                        NickName = item.NickName,
                        JoinDate = DateTime.Now,
                    };
                    var result = await _userManager.CreateAsync(user, "QWE!@#123qwe");
                    var finalResultTemp=new RegisterManyResultModel()
                    {
                        UserName = item.UserName
                    };
                    if (result.Succeeded)
                    {
                        finalResultTemp.Reustl = true;
                        //return Ok();
                    }
                    else
                    {
                        finalResultTemp.Reustl = false;
                        foreach (var errItem in result.Errors)
                        {
                            var errTemp=new RegisterErrNoteModel()
                            {
                                Code = errItem.Code,
                                Description = errItem.Description
                            };
                            finalResultTemp.ResultNotes.Add(errTemp);
                        }
                    }
                    finalResult.Add(finalResultTemp);
                }

                return finalResult;
            }

            return BadRequest(ModelState);
        }
        [HttpPost("EditUserRole")]
        [Authorize(Roles = "Root")]
        public async Task<IActionResult> EditUserRole(EditUserRoleModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            switch (model.EditRoleType)
            {
                case EditRoleType.Add:
                {
                    var userModel = await _userManager.FindByNameAsync(model.UserName);
                    var result=await _userManager.AddToRolesAsync(userModel, model.RoleNames);
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
                case EditRoleType.Delete:
                {
                    var userModel = await _userManager.FindByNameAsync(model.UserName);
                    var result = await _userManager.RemoveFromRolesAsync(userModel, model.RoleNames);
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
                default:
                    ModelState.AddModelError("err", "请输入正确的用户角色修改模式!");
                    return BadRequest(ModelState);
            }
            //var result = await _userManager.AddToRolesAsync(userModel, model.RoleNames);
            //if (result.Succeeded)
            //{
            //    return Ok();
            //}

            //foreach (var item in result.Errors)
            //{
            //    ModelState.AddModelError(item.Code, item.Description);
            //}
            //return BadRequest(ModelState);
        }
        [HttpPost("EditUserClaims")]
        [Authorize(Roles = "Root")]
        public async Task<IActionResult> EditUserClaims(EditUserClaimModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            switch (model.EditUserClaimType)
            {
                case EditUserClaimType.Add:
                {
                    var userModel = await _userManager.FindByNameAsync(model.UserName);;
                    var claimSources = model.ClaimModel;
                    var claimsTemp = (from claimSource in claimSources
                        select new Claim(claimSource.ClaimType, claimSource.ClaimType));
                    var result = await _userManager.AddClaimsAsync(userModel, claimsTemp);
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
                case EditUserClaimType.Delete:
                {
                    var userModel = await _userManager.FindByNameAsync(model.UserName); ;
                    var claimSources = model.ClaimModel;
                    var claimsTemp = (from claimSource in claimSources
                        select new Claim(claimSource.ClaimType, claimSource.ClaimType));
                    var result = await _userManager.RemoveClaimsAsync(userModel, claimsTemp);
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
                default:
                    ModelState.AddModelError("err", "请输入正确的用户声明修改模式!");
                    return BadRequest(ModelState);
            }
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var userModel = await _userManager.FindByNameAsync(model.UserName);;
            var result =await _userManager.ChangePasswordAsync(userModel, model.OldPassword, model.NewPassword);
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

        [HttpPost("ResetPassword")]
        [Authorize(Roles = "Root")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            var userModel = await _userManager.FindByNameAsync(model.UserName);
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(userModel);
            var result = await _userManager.ResetPasswordAsync(userModel, resetToken, model.NewPassword);
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
        [HttpGet("Test")]
        [Authorize(Roles = "Root")]
        public IActionResult Test()
        {
            return Ok();
        }


    }
}