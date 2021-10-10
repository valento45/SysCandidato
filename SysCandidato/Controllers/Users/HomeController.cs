using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SysCandidato.Models;
using SysCandidato.Models.AccessBE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
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
        [ValidateAntiForgeryToken]
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

                    if (!result.Succeeded)
                    {
                        string erros = TrataExcecao(result.Errors);
                        ModelState.AddModelError("", erros);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Error(IQueryable<IdentityError> _erros)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var _user = await _userManager.FindByNameAsync(loginModel.UserName);
                if (_user != null && await _userManager.CheckPasswordAsync(_user, loginModel.Password))
                {
                    loginModel.Password = _userManager.PasswordHasher.HashPassword(_user, loginModel.Password);
                    LoginModel.SetHashCode(loginModel.UserName);
                    HttpContext.Session.SetString("SessionUser", Access.Encrypt(LoginModel.GetHashCode().ToString(), JsonConvert.SerializeObject(loginModel)));

                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Usuário ou senha inválida!");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }



        private string TrataExcecao(IEnumerable<IdentityError> erros)
        {
            string result = "";

            foreach (IdentityError erro in erros)
            {
                if (result != string.Empty)
                {
                    result += "\n" + erro.Description;
                }
                else
                    result = erro.Description;
            }
            return result;
        }
    }
}
