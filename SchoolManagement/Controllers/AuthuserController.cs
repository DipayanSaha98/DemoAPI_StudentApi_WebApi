using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Model;
using SchoolManagement.Services;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolManagement.Controllers
{
    public class AuthuserController : Controller
    {
        private readonly ApiUserService _apiService;
        public AuthuserController()
        {
            _apiService = new ApiUserService();
        }
        public IActionResult Login()
        {
            LoginResponseDTO obj = new LoginResponseDTO();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO obj)
        {
            LoginResponseDTO objResponse = new LoginResponseDTO();
            objResponse = await _apiService.AuthenticateUser(obj);

            //Capture the token , set the token in session that is used across the session period of that particular user
            if (objResponse != null && objResponse.Token.ToString() !="")
            {
                
                //allows to user to sign in with these claims using coookie based authentication establishing a valid session .
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, objResponse.UserDetails.Username));      //creating an identity claim
                identity.AddClaim(new Claim(ClaimTypes.Role, objResponse.UserDetails.Role));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);  // now the application knows someone is logged in with username and role.

                // Set the token in session
                HttpContext.Session.SetString("APIToken", objResponse.Token);                           // "APIToken" is the key name here.
           
                // Redirect to the home page or any other page after successful login
                return RedirectToAction("Index", "Home");
            }
            else
            {
                HttpContext.Session.SetString("APIToken", "");
            }

            return View(objResponse);
        }

        public async Task<IActionResult> Logout()    // used the http.context class of DotNet to manage login and logout .
        {
            await HttpContext.SignOutAsync();  // Sign out the user
            HttpContext.Session.SetString("APIToken", "");
            return RedirectToAction("Login", "AuthUser");
        }

        public IActionResult AccessDenied()   
        {

            return View();
        }
    }
}
