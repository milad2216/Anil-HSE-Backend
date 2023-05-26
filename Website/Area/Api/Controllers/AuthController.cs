using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Collections.Generic;
using Website.Area.Api.RequestModel.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;
using System;
using Anil.Services.Seo;
using System.Threading.Tasks;
using Anil.Core.Domain.Customers;
using Anil.Core.Common;
using Anil.Web.Areas.Admin.Models.Users;
using Anil.Core.Infrastructure.Helpers;
using System.Linq;
using Anil.Web.Areas.Api.Infrastructure.Mapper.Extensions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Website.Area.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ApiBaseController<User,UserViewModel, UserViewModel, IUserService>
    {
        public AuthController(IConfiguration config, IUserService userService): base(config, userService)
        {
        }

        // POST: api/Auth/Login
        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> Login(LoginRequestModel model)
        {
            var user = await _service.GetUserByUsernameAndPassword(model.Username, PasswordHelper.HashPassword(model.Password));
            if (user == null)
            {
                return new JsonResult(new CFResult
                {
                    Status = Anil.Core.Infrastructure.Enums.Types.OperationResultType.Faild,
                    Message = "نام کاربری یا رمز عبور اشتباه است."
                });
            }
            return new JsonResult(new CFResult
            {
                Status = Anil.Core.Infrastructure.Enums.Types.OperationResultType.Success,
                Message = GenerateToken(user),
                Extra = user.ToModel<UserViewModel>()
            });
        }

        public override JsonResult Create(UserViewModel model)
        {
            model.Password = PasswordHelper.HashPassword(model.Password);
            return new JsonResult(_service.Create(model.ToEntity<User>()));
        }

        // To generate token
        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Username));
            foreach (var role in user.Roles?.Split(",")?.ToList() ?? new List<string>())
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims.ToArray(),
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
