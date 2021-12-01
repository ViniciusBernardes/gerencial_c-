using Gerencial.biblioteca;
using Gerencial.Models;
using Ionic.Zip;
using System;
using System.IO;
using System.Web.Mvc;

namespace Gerencial.Controllers
{
    public class PreventivaController : Controller
    {
        public Preventiva prev;
        public Util utl;
        public PreventivaController()
        {
            prev = new Preventiva();
            utl = new Util();
        }
        // GET: Preventiva
        public ActionResult Index(int id = 0, string msg = null)
        {
            ViewBag.tipo = id;
            ViewBag.Msg = msg;

            prev.ConsultaMes = int.Parse(DateTime.Now.ToString("MM"));
            prev.MPreventiva = prev.Listar(prev.ConsultaMes);
            return View(prev);
        }

        [HttpPost]
        public ActionResult Busca(string ConsultaMes)
        {
            prev.ConsultaMes = int.Parse(ConsultaMes);
            prev.MPreventiva = prev.Listar(prev.ConsultaMes);
            return View("Index", prev);
        }

        public ActionResult Cadastro()
        {
            Agencia ag = new Agencia();
            prev.MAgencia = ag.ListarES();
            return View(prev);
        }

        [HttpPost]
        public ActionResult Cadastrar(Preventiva preventiva)
        {
            try
            {
                var retorno = prev.Cadastrar(preventiva);
                if (retorno == 1)
                {
                    string _pathPrevEletrica = Path.Combine(Server.MapPath("~/Arquivos/Preventiva/Eletrica/"), preventiva.Mes + "_" + utl.DecodificaId(preventiva.Local) + ".pdf");
                    preventiva.AnexoEletrica.SaveAs(_pathPrevEletrica);

                    string _pathPrevAr = Path.Combine(Server.MapPath("~/Arquivos/Preventiva/ArCondicionado/"), preventiva.Mes + "_" + utl.DecodificaId(preventiva.Local) + ".pdf");
                    preventiva.AnexoAr.SaveAs(_pathPrevAr);

                    return RedirectToAction("Index", "Preventiva", new
                    {
                        id = 1,
                        msg = "Preventiva Cadastrada com sucesso!"
                    });
                }
                else
                {
                    return RedirectToAction("Index", "Preventiva", new
                    {
                        id = 2,
                        msg = "Erro ao tentar cadastrar Preventiva!"
                    });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Preventiva", new
                {
                    id = 2,
                    msg = "Erro ao tentar cadastrar Preventiva: " + ex
                });
            }
        }

        [HttpGet]
        public ActionResult Alteracao(string Id)
        {
            try
            {
                Agencia ag = new Agencia();
                prev.MAgencia = ag.ListarES();
                prev.MPreventivaSimples = prev.ListaId(Id);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Preventiva", new
                {
                    id = 2,
                    msg = "Erro ao tentar alterar Preventiva: " + ex
                });
            }
            return View(prev);
        }

        [HttpPost]
        public ActionResult Alterar(Preventiva preventiva)
        {
            try
            {
                var retorno = prev.Alterar(preventiva);
                if (retorno == 1)
                {
                    if (preventiva.AnexoEletrica != null)
                    {
                        string _pathPrevEletrica = Path.Combine(Server.MapPath("~/Arquivos/Preventiva/Eletrica/"), preventiva.Mes + "_" + utl.DecodificaId(preventiva.Local) + ".pdf");
                        if (System.IO.File.Exists(_pathPrevEletrica))
                            System.IO.File.Delete(_pathPrevEletrica);

                        preventiva.AnexoEletrica.SaveAs(_pathPrevEletrica);
                    }

                    if (preventiva.AnexoAr != null)
                    {
                        string _pathPrevAr = Path.Combine(Server.MapPath("~/Arquivos/Preventiva/ArCondicionado/"), preventiva.Mes + "_" + utl.DecodificaId(preventiva.Local) + ".pdf");
                        if (System.IO.File.Exists(_pathPrevAr))
                            System.IO.File.Delete(_pathPrevAr);

                        preventiva.AnexoAr.SaveAs(_pathPrevAr);
                    }
                }

                return RedirectToAction("Index", "Preventiva", new
                {
                    id = 1,
                    msg = "Preventiva Alterada com sucesso!"
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Preventiva", new
                {
                    id = 2,
                    msg = "Erro ao tentar alterar Preventiva: " + ex
                });
            }
        }

        [HttpGet]
        public ActionResult GerarPreventiva(string Id)
        {
            try
            {
                System.IO.File.Delete(Server.MapPath("~/Arquivos/Preventiva/preventiva.zip"));

                prev.ConsultaMes = int.Parse(Id);
                prev.MPreventiva = prev.Listar(prev.ConsultaMes);


                foreach (var item in prev.MPreventiva)
                {
                    //ELÉTRICA 
                    string _pathPrevEletrica = Path.Combine(Server.MapPath("~/Arquivos/Preventiva/Eletrica/"), item.Mes + "_" + item.IdLocal + ".pdf");
                    string _novoPatchPrevEletrica = Path.Combine(Server.MapPath("~/Arquivos/Preventiva/Download/Eletrica/"), item.Local + ".pdf");

                    if (System.IO.File.Exists(_novoPatchPrevEletrica))
                        System.IO.File.Delete(_novoPatchPrevEletrica);

                    if (System.IO.File.Exists(_pathPrevEletrica))
                        System.IO.File.Copy(_pathPrevEletrica, _novoPatchPrevEletrica);

                    //AR CONDICIONADO
                    string _pathPrevAr = Path.Combine(Server.MapPath("~/Arquivos/Preventiva/ArCondicionado/"), item.Mes + "_" + item.IdLocal + ".pdf");
                    string _novoPatchPrevAr = Path.Combine(Server.MapPath("~/Arquivos/Preventiva/Download/ArCondicionado/"), item.Local + ".pdf");

                    if (System.IO.File.Exists(_novoPatchPrevAr))
                        System.IO.File.Delete(_novoPatchPrevAr);

                    if (System.IO.File.Exists(_pathPrevAr))
                        System.IO.File.Copy(_pathPrevAr, _novoPatchPrevAr);

                }

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(Server.MapPath("~/Arquivos/Preventiva/Download/"));
                    zip.Save(Server.MapPath("~/Arquivos/Preventiva/Download/preventiva.zip"));
                }

                return RedirectToAction("Index", "Preventiva", new
                {
                    id = 1,
                    msg = "Preventiva Gerada com sucesso!"
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Preventiva", new
                {
                    id = 2,
                    msg = "Erro ao tentar gerar Preventiva: " + ex
                });
            }
        }
    }
}