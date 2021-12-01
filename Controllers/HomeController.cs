using System.Web.Mvc;

namespace Gerencial.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["nome"] == null)
            {
                return RedirectToAction("Acesso", "Login");
            }
            else
            {
                if (Session["empresa"].ToString() == "0")
                    return View();
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}