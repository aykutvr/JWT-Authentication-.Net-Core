﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
namespace JWTAuthentication.Middlewares
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;

        //Inject UserServcie to Middleware
        private Services.IUserService _userService { get; set; }
        public JWTMiddleware(RequestDelegate next,Services.IUserService userService)
        {
            _next = next;
            _userService = userService;
        }
        public async Task Invoke(HttpContext context)
        {
            //Get Berarer token from header values
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            //if token is not empty get user information for authentication
            if (token != null)
                attachUserToContext(context, token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Settings.JWTIssuer,
                    ValidAudience = Settings.JWTIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type =="id").Value);
                context.Items["User"] = _userService.Get(userId);

            }
            catch (Exception ex)
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
