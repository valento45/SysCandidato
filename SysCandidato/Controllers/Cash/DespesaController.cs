using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SysCandidato.Models.MtCash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysCandidato.Controllers.Cash
{
    public class DespesaController : Controller
    {
        // GET: DespesaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: DespesaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DespesaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DespesaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InserirDespesa(Despesa model)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DespesaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DespesaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Despesa model)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DespesaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DespesaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Despesa model)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
