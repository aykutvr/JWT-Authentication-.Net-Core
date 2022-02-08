using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.Controllers
{
    public class AccountController : Controller
    {
        private Services.IUserService _userService { get; set; }
        public AccountController(Services.IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Login()
        {
            return View(new Models.LoginViewModel());
        }

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
    }
}
