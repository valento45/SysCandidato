using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SysCandidato.Models;
using SysCandidato.Models.AccessBE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysCandidato.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<User> _userManager;
        public HomeController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    user = new User
                    {
                        UserName = model.UserName
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if(!result.Succeeded)
                    {
                        IQueryable<IdentityError> _erros = result.Errors.AsQueryable();
                    }
                    else
                        return View("Success");
                }               
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Error(IQueryable<IdentityError> _erros)
        {
            return View();
        }

    }
}
