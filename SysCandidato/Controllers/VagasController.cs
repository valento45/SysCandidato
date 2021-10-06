using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SysCandidato.Models;
using Microsoft.AspNetCore.Identity;
using SysCandidato.Models.AccessBE;

namespace SysCandidato.Controllers
{
    public class VagasController : Controller
    {

        [Route("")]
        [Route("Vagas")]
        public IActionResult Index()
        {
            return View(VagasModel.GetAllVagas());
        }

        [HttpGet]
        public IActionResult AdicionarVagas()
        {
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
                return View(vaga);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdicionarCandidatos(VagasModel vaga)
        {
            if (ModelState.IsValid)
            {
                vaga.InsertCandidatos();
                return RedirectToAction(nameof(Index));

            }
            else
                return View();
        }




    }
}
