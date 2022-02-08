
var builder = WebApplication.CreateBuilder(args);


//--------------------------------------------------------------------------------------------------------------
//User Service Implementation
builder.Services.AddSingleton<JWTAuthentication.Services.IUserService, JWTAuthentication.Services.UserService>();
//--------------------------------------------------------------------------------------------------------------

//--------------------------------------------------------------------------------------------------------------
//JWT Authentication Service Implementation With Extension Method
builder.Services.UseCustomJWTAuthentication();
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
