using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            ClaimsPrincipal principal = HttpContext.User;
            var testtt=HttpContext.User.IsInRole("Root");
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}