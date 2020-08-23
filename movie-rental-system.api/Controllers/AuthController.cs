using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using movie_rental_system.core.DTOs;
using movie_rental_system.core.Services;

namespace movie_rental_system.api.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api")]
    public class AuthController : APIBaseController
    {
        protected readonly AuthService service;
        public AuthController(AuthService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Registers a user into the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /register
        ///     {
        ///        "FirstName": "Jake",
        ///        "LastName": "Damian",
        ///        "Email": "jakedamian@abc.com",
        ///        "Password": "Password321"
        ///     }
        ///
        /// </remarks>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO_In userDTO)
        {
            await this.service.AddUserAsync(userDTO);
            return Ok();
        }

        
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthDTO_In authDTO)
        {   

            var auth = await this.service.AuthenticateAsync(authDTO, ipAddress());
            if (auth == null)
                return Unauthorized();
            this.setTokenCookie(auth.RefreshToken);
            return Ok(auth);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var auth = await this.service.RefreshTokenAsync(refreshToken, ipAddress());
            if (auth == null)
                return Unauthorized();
            this.setTokenCookie(auth.RefreshToken);
            return Ok(auth);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken(RevokeTokenDTO_In revokeTokenDTO)
        {
            // accept token from request body or cookie
            var token = revokeTokenDTO.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = await this.service.RevokeTokenAsync(token, ipAddress());

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }

        [HttpGet("auth-user")]
        public async Task<ActionResult<AuthUserDTO_Out>> GetAuthenticatedUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userId = identity.Name;
                var user = await this.service.GetUserAsync(new Guid(userId));
                return new AuthUserDTO_Out
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    MailingAddress = user.MailingAddress,
                    Role = user.Role,
                    Loyalty = user.Loyalty,
                    Active = user.Active
                };

            }

            return BadRequest();
        }


        //private methods
        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}