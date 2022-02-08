
var builder = WebApplication.CreateBuilder(args);


//--------------------------------------------------------------------------------------------------------------
//User Service Implementation
builder.Services.AddSingleton<JWTAuthentication.Services.IUserService, JWTAuthentication.Services.UserService>();
//--------------------------------------------------------------------------------------------------------------

//--------------------------------------------------------------------------------------------------------------
//JWT Authentication Service Implementation With Extension Method
builder.Services.UseCustomJWTAuthentication<JWTAuthentication.Dto.UserDto>(config =>
{
    config.SetSecretKey("B?E(H+MbQeThWmZq4t7w9z$C&F)J@NcRfUjXn2r5u8x/A%D*G-KaPdSgVkYp3s6v9y$B&E(H+MbQeThWmZq4t7w!z%C*F-JaNcRfUjXn2r5u8x/A?D(G+KbPeSgVkYp3s6v9y$B&E)H@McQfTjWmZq4t7w!z%C*F-JaNdRgUkXp2r5u8x/A?D(G+KbPeShVmYq3t6v9y$B&E)H@McQfTjWnZr4u7x!z%C*F-JaNdRgUkXp2s5v8y/B?D(G+KbPeS");
    config.SetJWTIssuer("https://www.github.com/");
});
//--------------------------------------------------------------------------------------------------------------

builder.Services.AddControllersWithViews();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

//--------------------------------------------------------------------------------------------------------------
//JWT Authentication Get Token From Session on Each Request
app.UseCustomJWTAuthentication();
//--------------------------------------------------------------------------------------------------------------


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
