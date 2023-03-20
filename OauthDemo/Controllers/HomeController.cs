using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPIApplication.Security;

namespace WebAPIApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ITokenProvider _tokenProvider;
        private readonly IAuthorizationService _authorizationService;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, ITokenProvider tokenProvider, IAuthorizationService authorizationService)
        {
            _logger = logger;
            _configuration = configuration;
            _tokenProvider = tokenProvider;
        }

        [HttpGet(Name = "GetData")]
        [Authorize]
        public IActionResult Get()
        {
         
            return Ok("Token Authorizations Successfull");
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return Ok(_tokenProvider.CreateToken(HttpContext.User, false,HttpContext.Request.Host.Value));
        }

        [HttpGet("signinwithgoogle")]
        public IActionResult SignInwithGoogle()
        {
            try
            {
                var redirectUrl = Url.Action("CallBack", "Auth");
                var properties = new AuthenticationProperties
                {
                    RedirectUri = redirectUrl
                };
                var response = Challenge(properties, GoogleDefaults.AuthenticationScheme);
                return response;
            }
            catch (Exception ex)
            {
                var result = ex;
                throw;
            }

        }

        [HttpGet("CallBack")]
        public async Task<IActionResult> Callback()
        {
            var authResult = await HttpContext.AuthenticateAsync("Google");
            if (authResult.Succeeded)
            {
                var emailClaim = authResult.Principal.FindFirst(ClaimTypes.Email);
                var email = emailClaim.Value; // Retrieve the user's email address from the authentication result

            
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

       
    }
}