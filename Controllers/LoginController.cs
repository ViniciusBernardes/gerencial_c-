using Gerencial.Models;
using System.Web.Mvc;


namespace Gerencial.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login

        public ActionResult Acesso()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logar(Login dados)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Acesso", "Login");

            Login logs = new Login();
            var Lista = logs.Logar(dados);
            if (Lista.Count != 0)
            {
                Session["nome"] = Lista[0].Usuario;
                Session["id"] = Lista[0].Id;
                Session["permissao"] = Lista[0].IsAdmin;
                Session["empresa"] = Lista[0].Empresa;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Usuário ou Senha informado Inválido!!!");
                return View("Acesso");
            }


        }

        public ActionResult Logout()
        {
            Session["nome"] = null;
            Session["id"] = null;
            Session["permissao"] = null;
            Session["empresa"] = null;

            return View("Acesso");
        }

    }
}