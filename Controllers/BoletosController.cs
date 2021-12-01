using Gerencial.Models;
using System.Web.Mvc;

namespace Gerencial.Controllers
{
    public class BoletosController : Controller
    {
        // GET: Boletos

        public ActionResult Index(int id = 0, string msg = null)
        {
            ViewBag.tipo = id;
            ViewBag.Msg = msg;

            Boletos Boleto = new Boletos();
            Boleto.ConsultaTipo = "0";
            Boleto.ConsultaRota = "0";
            Boleto.ConsultaStatus = "aberto";
            Boleto.ConsultaDataInicio = null;
            Boleto.ConsultaDataFim = null;
            Boleto.MBoletos = Boleto.Listar(Boleto);

            ViewBag.ConsultaTipo = Boleto.ConsultaTipo;
            ViewBag.ConsultaRota = Boleto.ConsultaRota;
            ViewBag.ConsultaStatus = Boleto.ConsultaStatus;
            ViewBag.ConsultaDataInicio = Boleto.ConsultaDataInicio;
            ViewBag.ConsultaDataFim = Boleto.ConsultaDataFim;

            Categoria Cat = new Categoria();
            Boleto.MCategoria = Cat.Listar();

            Rota rot = new Rota();
            Boleto.MRota = rot.Listar();

            return View(Boleto);
        }

        [HttpPost]
        public ActionResult Busca(Boletos dados)
        {
            Boletos Boleto = new Boletos();
            Boleto.MBoletos = Boleto.Listar(dados);

            ViewBag.ConsultaTipo = dados.ConsultaTipo;
            ViewBag.ConsultaRota = dados.ConsultaRota;
            ViewBag.ConsultaStatus = dados.ConsultaStatus;
            ViewBag.ConsultaDataInicio = dados.ConsultaDataInicio;
            ViewBag.ConsultaDataFim = dados.ConsultaDataFim;

            Categoria Cat = new Categoria();
            Boleto.MCategoria = Cat.Listar();

            Rota rot = new Rota();
            Boleto.MRota = rot.Listar();

            return View("Index", Boleto);
        }

        public ActionResult Cadastro()
        {
            Boletos Boleto = new Boletos();

            Categoria Cat = new Categoria();
            Boleto.MCategoria = Cat.Listar();

            Rota rot = new Rota();
            Boleto.MRota = rot.Listar();

            return View(Boleto);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Cadastrar(Boletos dados)
        {
            Boletos boleto = new Boletos();
            var valor = boleto.Cadastrar(dados);

            if (valor == 1)
            {
                return RedirectToAction("Index", "Boletos", new
                {
                    id = 1,
                    msg = "Cadastrado com sucesso!"
                });
            }
            else
            {
                return RedirectToAction("Index", "Boletos", new
                {
                    id = 2,
                    msg = "Erro ao tentar cadastrar!"
                });
            }
        }

        [HttpGet]
        public ActionResult Alteracao(string id)
        {
            Boletos Boleto = new Boletos();
            Boleto.MBoletoSimples = Boleto.ListaId(id);

            Categoria Cat = new Categoria();
            Boleto.MCategoria = Cat.Listar();

            Rota rot = new Rota();
            Boleto.MRota = rot.Listar();

            return View(Boleto);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Alterar(Boletos dados)
        {
            Boletos boleto = new Boletos();
            var valor = boleto.Alterar(dados);

            if (valor == 1)
            {
                return RedirectToAction("Index", "Boletos", new
                {
                    id = 1,
                    msg = "Boleto Alterado com sucesso!"
                });
            }
            else
            {
                return RedirectToAction("Index", "Boletos", new
                {
                    id = 2,
                    msg = "Erro ao tentar alterar Boleto!"
                });
            }
        }

        [HttpGet]
        public ActionResult Excluir(string id)
        {
            Boletos Boleto = new Boletos();
            var valor = Boleto.Excluir(id);

            if (valor == 1)
            {
                return RedirectToAction("Index", "Boletos", new
                {
                    id = 1,
                    msg = "Boleto Excluído com sucesso!"
                });
            }
            else
            {
                return RedirectToAction("Index", "Boletos", new
                {
                    id = 2,
                    msg = "Erro ao tentar excluir Boleto!"
                });
            }
        }

    }
}