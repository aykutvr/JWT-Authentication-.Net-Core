using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public static partial class Extensions
{
    public static void UseCustomJWTAuthentication(this IServiceCollection @this)
    {
        @this.AddSession();
        @this.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = JWTAuthentication.Settings.JWTIssuer,
               ValidAudience = JWTAuthentication.Settings.JWTIssuer,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTAuthentication.Settings.SecretKey)),
               ClockSkew = TimeSpan.Zero
           };
       });
    }

    public static void UseCustomJWTAuthentication(this WebApplication @this)
    {
        @this.UseSession();
        @this.Use(async (context, next) =>
        {
            var JWToken = context.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(JWToken))
            {
                if (context.Request.Headers.ContainsKey("Authorization"))
                    context.Request.Headers["Authorization"] = "Bearer " + JWToken;
                else
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
            }
            else
            {
            }
            await next();
        });
        @this.UseMiddleware<JWTAuthentication.Middlewares.JWTMiddleware>();
        @this.UseAuthentication();
    }
}
