using Microsoft.AspNetCore.Mvc;
using PetMe.Business.Services;

namespace PetMe.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }


    }

    
}
