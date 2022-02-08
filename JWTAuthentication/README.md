
# Asp.Net Core JWT Authentication Example

This example shows how to using JWT authentication on Asp.Net Core Projects

### Required Dependencies
 - System.IdentityModel.Tokens.JWT
 - Microsoft.AspNetCore.Authentication.JwtBearer




### File Descriptions
#### JWTAuthorizeAttribute.cs
This attribute using for JWT authentication as determinate of authenticated request
```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class JWTAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Items["User"] == null)
            context.Result = new UnauthorizedResult();
    }
}
```
#### Settings.cs
```csharp
public class Settings
{
    internal static string SecretKey { get; set;} = "";
    internal static string JWTIssuer { get; set; } = "";
}
```
#### UseCustomJWTAuthentication.cs
This extension methods using for implement to project and configuration to jwt authentication
```csharp
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
```
#### JWTokenBuilder.cs
Token generator class from user information.
```csharp
public class JWTokenBuilder
{
    public static string Build(Dto.UserDto userData)
    {
        Dictionary<string, object> claims = new Dictionary<string, object>();
        claims.Add(ClaimTypes.Name, userData.UserName);
        claims.Add("id", userData.Id);
        claims.Add(ClaimTypes.Email, userData.EmailAddress);
        var key = Encoding.ASCII.GetBytes(Settings.SecretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = claims,
                Audience = Settings.JWTIssuer,
                Issuer = Settings.JWTIssuer,
                NotBefore = new DateTimeOffset(DateTime.Now).DateTime.AddMinutes(-1),
                Subject = new ClaimsIdentity(claims.Select(s=> new Claim(s.Key.ToString(),s.Value.ToString()))),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
    }
}
```
#### AccountController.cs
The code block which making login process.
```csharp
...
[HttpPost]
public IActionResult Login(Models.LoginViewModel loginData)
{
    var user = _userService.Get(loginData.EmailAddress, loginData.Password);
    if(user != null)
    {
        string jwToken = _userService.Login(user);
        if (!string.IsNullOrEmpty(jwToken))
        {
            HttpContext.Session.SetString("JWToken", jwToken);
            return Redirect("/home/privacy");
        }
    }
    return View(loginData);
}
...
```
#### HomeController.cs
Authenticated action
```csharp
...
[JWTAuthorize]
public IActionResult Privacy()
{
    return View();
}
...
```