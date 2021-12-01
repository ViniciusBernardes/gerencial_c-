using Gerencial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gerencial.Controllers
{
    public class CategoriaController : Controller
    {
        // GET: Categoria
        public ActionResult Index(int id = 0, string msg = null)
        {
            ViewBag.tipo = id;
            ViewBag.Msg = msg;

            Categoria cat = new Categoria();
            cat.MCategoria = cat.Listar();
            return View(cat);
        }

        [HttpPost]
        public ActionResult Cadastrar(Categoria dados)
        {
            Categoria cat = new Categoria();
            var valor = cat.Cadastrar(dados);

            if (valor == 1)
            {
                return RedirectToAction("Index", "Categoria", new
                {
                    id = 1,
                    msg = "Categoria Cadastrada com sucesso!"
                });
            }
            else
            {
                return RedirectToAction("Index", "Categoria", new
                {
                    id = 2,
                    msg = "Erro ao tentar cadastrar Categoria!"
                });
            }
        }

        [HttpGet]
        public ActionResult Alteracao(string id)
        {
            Categoria cat = new Categoria();
            var dados = cat.ListaId(id);

            return View(dados);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Alterar(Categoria dados)
        {
            Categoria cat = new Categoria();
            var valor = cat.Alterar(dados);

            if (valor == 1)
            {
                return RedirectToAction("Index", "Categoria", new
                {
                    id = 1,
                    msg = "Categoria Alterada com sucesso!"
                });
            }
            else
            {
                return RedirectToAction("Index", "Categoria", new
                {
                    id = 2,
                    msg = "Erro ao tentar alterar Categoria!"
                });
            }
        }
    }
}