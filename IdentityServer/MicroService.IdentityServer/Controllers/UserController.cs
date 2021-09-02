using MicroService.IdentityServer.Dtos;
using MicroService.IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using static IdentityServer4.IdentityServerConstants;
using System.IdentityModel.Tokens.Jwt;

namespace MicroService.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupDto signupDto)
        {
            var user = new ApplicationUser
            {
                Email = signupDto.Email,
                City = signupDto.City,
                UserName = signupDto.UserName,
            };
            var result = await _userManager.CreateAsync(user, signupDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(Response<NoContent>.Fail(result.Errors.Select(x => x.Description).ToList(), 400));
            }
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
            if (userClaim == null) return BadRequest();

            var user = await _userManager.FindByIdAsync(userClaim.Value);
            if (user == null) return BadRequest();

            return Ok(new { Id = user.Id, Email = user.Email, UserName = user.UserName, City = user.City });
        }
    }
}
