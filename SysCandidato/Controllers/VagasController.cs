using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SysCandidato.Models;
using Microsoft.AspNetCore.Identity;
using SysCandidato.Models.AccessBE;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace SysCandidato.Controllers
{
    public class VagasController : Controller
    {
        [HttpGet]
        [Route("Vagas")]
        public IActionResult Index()
        {
            byte[] jsonUser;
            if ((bool)HttpContext.Session?.TryGetValue("SessionUser", out jsonUser))
            {
                var user = JsonConvert.DeserializeObject<LoginModel>(Access.Decrypt(LoginModel.GetHashCode().ToString(), HttpContext.Session?.GetString("SessionUser")));
                return View(VagasModel.GetAllVagas());
            }
            else
                return NotFound("Usuário não autenticado !");
        }

        [HttpGet]
        public IActionResult AdicionarVagas()
        {
            var user = JsonConvert.DeserializeObject<LoginModel>(Access.Decrypt(LoginModel.UserLogado.UserName, HttpContext.Session?.GetString("SessionUser")));

            if (user.UserName == string.Empty)
                return NotFound("Usuário não autenticado !");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdicionarVagas(VagasModel vagas)
        {
            if (ModelState.IsValid && vagas != null)
            {
                VagasModel.InsertVaga(vagas);
                return RedirectToAction(nameof(Index));
            }
            else
                return View();
        }

        [HttpGet]
        // [Route("Vagas/{idvaga}")]
        [Route("Vagas/AdicionarCandidatos/{idvaga}")]
        public IActionResult AdicionarCandidatos(int idvaga)
        {
            var vaga = VagasModel.GetVagaById(idvaga);
            if (vaga == null || vaga.IdVaga <= 0)
                return NotFound();
            else
            {
                var candidato = new PessoasModel { IdVaga = idvaga };
                return View(candidato);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdicionarCandidatos(PessoasModel pessoa)
        {
            if (ModelState.IsValid)
            {
                if (pessoa.IdVaga > 0)
                {
                    VagasModel vaga = VagasModel.GetVagaById(pessoa.IdVaga);
                    vaga.Candidatos.Add(pessoa);
                    vaga.InsertCandidatos();
                }
                else
                    return NotFound("Dados da vaga inconsistentes !");

                return RedirectToAction(nameof(Index));
            }
            else
                return View();
        }

        [HttpGet]
        [Route("Vagas/ExibirCandidatos/{idvaga}")]

        public IActionResult ExibirCandidatos(int idvaga)
        {
            byte[] jsonUser;
            if ((bool)HttpContext.Session?.TryGetValue("SessionUser", out jsonUser))
            {
                if (!(idvaga > 0))
                {
                    return NotFound("Nenhuma vaga selecionada!");
                }
                else
                {
                    var candidatos = VagasModel.GetCandidatosByIdVaga(idvaga);
                    candidatos.ForEach(atualizarVaga(idvaga));
                    return View(candidatos);
                }
            }
            else
                return NotFound("Usuário não autenticado !");

        }
        private Action<PessoasModel> atualizarVaga(int idvaga)
        {
            return x => x.IdVaga = idvaga;
        }

    }
}
