using Authentication.Helpers;
using Common;
using Common.Dto;
using Context.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core
{
    public  class AuthenticationService: IAuthenticationService
    {

        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        public AuthenticationService(IUserService userService, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userService = userService;
        }
        public AuthenticateResponse Authenticate(AuthenticateRequest req)
        {
            var signinresult = _signInManager.PasswordSignInAsync(req.Username, req.Password, false, lockoutOnFailure: false);
            signinresult.Wait();
            if (signinresult.Result.Succeeded)              
            {
                var user = _userService.GetByUserName(req.Username);
                var token = generateJwtToken(user);

                return new AuthenticateResponse(user, token);
            }
            else
            {
                return null;
            }
 
  
        }
        private string generateJwtToken(UserDto user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(HttpHelper.GetConfig<string>("AppSettings:Secret"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
