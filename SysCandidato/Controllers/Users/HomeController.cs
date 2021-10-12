using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SysCandidato.Models;
using SysCandidato.Models.AccessBE;
using SysCandidato.Models.EmailGateway;
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
                        return View("Success", "Usuário cadastrado com sucesso!");
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
                if (loginModel.UserName == null)
                {
                    ModelState.AddModelError("", "Usuário ou senha inválida!");
                    return View();
                }
                User _user = await _userManager.FindByNameAsync(loginModel.UserName);
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
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            return View(new ResetPasswordModel { Token = token, Email = email });
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", TrataExcecao(result.Errors));
                        return View();
                    }
                    return View("Success", "Senha redefinida com sucesso!");
                }
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetURL = Url.Action(nameof(ResetPassword), "Home", new { token = token, email = model.Email }, Request.Scheme);
                    string message = await EmailModel.EnviaMensagemEmail(model.Email, "errojoiasmrx@gmail.com", "Resetar senha", $"Clique no link para redefinir sua senha \r\n {resetURL}", "errojoiasmrx@gmail.com", "erro1234");
                    if (message.ToLower() != "success")
                    {
                        return View("success", "Tivemos um probleminha ao enviar email. :/ \r\nMensagem de erro:\r\n" + message);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nenhum usuário cadastrado com este e-mail.");
                }
            }
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
