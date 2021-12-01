using Gerencial.Models;
using System.Web.Mvc;

namespace Gerencial.Controllers
{
    public class AgenciaController : Controller
    {
        // GET: Agencia
        public ActionResult Index(int id = 0, string msg = null)
        {
            ViewBag.tipo = id;
            ViewBag.Msg = msg;

            Agencia ag = new Agencia();
            ag.MAgencia = ag.Listar();
            return View(ag);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Cadastrar(Agencia dados)
        {
            Agencia ag = new Agencia();
            var verifica = ag.VerificarCadastrado(dados.NAgencia);
            if (verifica != 0)
            {
                return RedirectToAction("Index", "Agencia", new
                {
                    id = 2,
                    msg = "Erro ao tentar cadastrar, número da Agencia já cadastrada"
                });
            }
            else
            {
                var valor = ag.Cadastrar(dados);

                if (valor == 1)
                {
                    return RedirectToAction("Index", "Agencia", new
                    {
                        id = 1,
                        msg = "Agencia Cadastrada com sucesso!"
                    });
                }
                else
                {
                    return RedirectToAction("Index", "Agencias", new
                    {
                        id = 2,
                        msg = "Erro ao tentar cadastrar Agencia!"
                    });
                }
            }
        }

        [HttpGet]
        public ActionResult Alteracao(string id)
        {
            Agencia ag = new Agencia();
            var agencia = ag.ListaId(id);

            return View(agencia);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Alterar(Agencia dados)
        {
            Agencia ag = new Agencia();
            var valor = ag.Alterar(dados);

            if (valor == 1)
            {
                return RedirectToAction("Index", "Agencia", new
                {
                    id = 1,
                    msg = "Agencia Alterada com sucesso!"
                });
            }
            else
            {
                return RedirectToAction("Index", "Agencia", new
                {
                    id = 2,
                    msg = "Erro ao tentar alterar Agencia!"
                });
            }
        }

        [HttpGet]
        public ActionResult Excluir(string id)
        {
            Agencia ag = new Agencia();
            var valor = ag.Excluir(id);

            if (valor == 1)
            {
                return RedirectToAction("Index", "Agencia", new
                {
                    id = 1,
                    msg = "Agencia Excluída com sucesso!"
                });
            }
            else
            {
                return RedirectToAction("Index", "Agencia", new
                {
                    id = 2,
                    msg = "Erro ao tentar excluir Agencia!"
                });
            }
        }
    }
}