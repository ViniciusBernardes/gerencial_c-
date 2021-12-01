using Gerencial.biblioteca;
using Gerencial.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Services;

namespace Gerencial.Controllers
{
    public class ChamadosController : Controller
    {
        public Chamados ch;
        public ChamadosController()
        {
            ch = new Chamados();
        }
        // GET: Chamados
        public ActionResult Index(int id = 0, string msg = null)
        {
            ViewBag.tipo = id;
            ViewBag.Msg = msg;

            ch.ConsultaAgencia = "0";
            ch.ConsultaChamado = null;
            ch.ConsultaStatus = "Aberto";
            ch.ConsultaEstado = "MINAS GERAIS";
            ch.ConsultaDataInicio = null;
            ch.ConsultaDataFim = null;
            ch.ConsultaRelatado = "0";
            ch.ConsultaRota = "0";

            ch.MChamados = ch.Listar(ch);

            Rota rot = new Rota();
            ch.MRota = rot.Listar();

            Agencia ag = new Agencia();
            ch.MAgencia = ag.Listar();

            return View(ch);
        }

        [HttpPost]
        public ActionResult Busca(Chamados chamado)
        {
            ch.ConsultaAgencia = chamado.ConsultaAgencia;
            ch.ConsultaChamado = chamado.ConsultaChamado;
            ch.ConsultaStatus = chamado.ConsultaStatus;
            ch.ConsultaEstado = chamado.ConsultaEstado;
            ch.ConsultaDataInicio = chamado.ConsultaDataInicio;
            ch.ConsultaDataFim = chamado.ConsultaDataFim;
            ch.ConsultaRelatado = chamado.ConsultaRelatado;
            ch.ConsultaRota = chamado.ConsultaRota;

            ch.MChamados = ch.Listar(chamado);

            Rota rot = new Rota();
            ch.MRota = rot.Listar();

            Agencia ag = new Agencia();
            ch.MAgencia = ag.Listar();

            return View("Index", ch);
        }

        public ActionResult GerenciarES(int id = 0, string msg = null)
        {
            ViewBag.tipo = id;
            ViewBag.Msg = msg;

            ch.ConsultaAgencia = "0";
            ch.ConsultaChamado = null;
            ch.ConsultaStatus = "Aberto";
            ch.ConsultaEstado = "ESPIRITO SANTO";
            ch.ConsultaDataInicio = null;
            ch.ConsultaDataFim = null;
            ch.ConsultaRelatado = "0";
            ch.ConsultaRota = "0";

            ch.MChamados = ch.Listar(ch);

            Rota rot = new Rota();
            ch.MRota = rot.Listar();

            Agencia ag = new Agencia();
            ch.MAgencia = ag.Listar();

            return View("Index", ch);
        }

        public ActionResult Cadastro()
        {
            Rota rot = new Rota();
            ch.MRota = rot.Listar();

            Agencia ag = new Agencia();
            ch.MAgencia = ag.Listar();

            Categoria cat = new Categoria();
            ch.MCategoria = cat.ListarChamado();

            return View(ch);
        }

        [WebMethod]
        public string BuscaSubCategoria(string Categoria)
        {
            Categoria cat = new Categoria();
            var retorno = cat.ListarSubCategoriaChamado(Categoria);
            var textoSelect = "<option value=\"\" selected=\"selected\">Selecione...</option>";
            foreach (var item in retorno)
            {
                textoSelect += "<option value=" + item.Id + ">" + item.Nome + "</option>";
            }
            return textoSelect;
        }

        [WebMethod]
        public int VerificaChamado(string Chamado)
        {
            var retorno = ch.VerificaChamado(Chamado);

            return retorno;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Cadastrar(Chamados chamado)
        {
            Documentos doc = new Documentos();
            int idDocumento = 0;
            try
            {
                if (chamado.Arquivo.ContentLength > 0)
                {
                    var documento = doc.UltimoId();
                    idDocumento = documento.Id + 1;
                    ch.DocumentoChamado = idDocumento.ToString();
                    string _FileName = Path.GetFileName(ch.Arquivo.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Arquivos/Chamado/"), idDocumento.ToString() + ".pdf");
                    ch.Arquivo.SaveAs(_path);
                }
                else
                {
                    return RedirectToAction("Index", "Chamados", new
                    {
                        id = 2,
                        msg = "Erro ao anexar chamado!"
                    });
                }

                var valor = ch.Cadastrar(chamado);

                if (valor == 1)
                {
                    Util utl = new Util();
                    var chamadoObj = ch.ListaPorChamado(chamado.Numero);
                    if (chamadoObj != null)
                    {
                        doc.IdOrigem = utl.DecodificaId(chamadoObj.Id);
                        doc.Pasta = "chamado";
                        doc.Tabela = "chamado";
                        doc.Nome = idDocumento.ToString() + ".pdf";
                        var retornoDocumento = doc.Cadastrar(doc);
                        if (retornoDocumento == 1)
                        {
                            return RedirectToAction("Index", "Chamados", new
                            {
                                id = 1,
                                msg = "Chamado Cadastrado com sucesso!"
                            });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Chamados", new
                            {
                                id = 2,
                                msg = "Erro ao tentar cadastrar Chamado!"
                            });
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Chamados", new
                        {
                            id = 2,
                            msg = "Erro ao tentar cadastrar Chamado!"
                        });
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Chamados", new
                    {
                        id = 2,
                        msg = "Erro ao tentar cadastrar Chamado!"
                    });
                }
            }
            catch (Exception ex)
            {
                var error = ex;
                return RedirectToAction("Index", "Chamados", new
                {
                    id = 2,
                    msg = "Erro ao tentar cadastrar Chamado: " + error
                });
            }
        }

        [HttpGet]
        public ActionResult Sinaliza(string Id)
        {
            try
            {
                ch.MChamadoSimples = ch.ListaId(Id);
                if (ch.MChamadoSimples.Id != null)
                    return View(ch);
                else
                {
                    return RedirectToAction("Index", "Chamados", new
                    {
                        id = 2,
                        msg = "Chamado inexistente!"
                    });
                }
            }
            catch (Exception error)
            {
                return RedirectToAction("Index", "Chamados", new
                {
                    id = 2,
                    msg = "Erro ao mostrar chamado: " + error
                });
            }
        }

        [HttpPost]
        public ActionResult Sinalizar(Chamados chamado)
        {
            try
            {
                var retorno = ch.Sinaliza(chamado);
                if (retorno == 1)
                {
                    return RedirectToAction("Index", "Chamados", new
                    {
                        id = 1,
                        msg = "Chamado Sinalizado com sucesso!"
                    });
                }
                else
                {
                    return RedirectToAction("Index", "Chamados", new
                    {
                        id = 2,
                        msg = "Erro ao sinalizar chamado "
                    });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Chamados", new
                {
                    id = 2,
                    msg = "Erro ao sinalizar chamado: " + ex
                });
            }
        }

        [HttpGet]
        public ActionResult Alteracao(string id)
        {
            Rota rot = new Rota();
            ch.MRota = rot.Listar();

            Agencia ag = new Agencia();
            ch.MAgencia = ag.Listar();

            Categoria cat = new Categoria();
            ch.MCategoria = cat.ListarChamado();

            ch.MChamadoSimples = ch.ListaId(id);

            ch.MSubCategoria = cat.ListarSubCategoriaChamado(ch.MChamadoSimples.Categoria);

            return View(ch);
        }

        [HttpPost]
        public ActionResult Alterar(Chamados chamado)
        {
            try
            {
                if (chamado.Arquivo != null)
                {
                    var caminhoDocumento = Server.MapPath("~/Arquivos/Chamado/" + chamado.DocumentoChamado);
                    if (System.IO.File.Exists(caminhoDocumento))
                    {
                        string _FileName = Path.GetFileName(chamado.Arquivo.FileName);
                        var _path = Path.Combine(Server.MapPath("~/Arquivos/Chamado/"), chamado.DocumentoChamado);
                        System.IO.File.Delete(_path);

                        string _pathNovo = Path.Combine(Server.MapPath("~/Arquivos/Chamado/"), chamado.DocumentoChamado);
                        chamado.Arquivo.SaveAs(_pathNovo);
                    }
                    else
                    {
                        Documentos doc = new Documentos();
                        var documento = doc.UltimoId();
                        var idDocumento = documento.Id + 1;
                        string _pathNovo = Path.Combine(Server.MapPath("~/Arquivos/Chamado/"), idDocumento.ToString() + ".pdf");
                        chamado.Arquivo.SaveAs(_pathNovo);

                        Util utl = new Util();
                        doc.IdOrigem = utl.DecodificaId(chamado.Id);
                        doc.Pasta = "chamado";
                        doc.Tabela = "chamado";
                        doc.Nome = idDocumento.ToString() + ".pdf";
                        var retornoDocumento = doc.Cadastrar(doc);
                    }

                }

                var retorno = ch.Alterar(chamado);
                if (retorno == 1)
                {
                    return RedirectToAction("Index", "Chamados", new
                    {
                        id = 1,
                        msg = "Chamado Alterado com sucesso!"
                    });
                }
                else
                {
                    return RedirectToAction("Index", "Chamados", new
                    {
                        id = 2,
                        msg = "Erro ao Alterar chamado "
                    });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Chamados", new
                {
                    id = 2,
                    msg = "Erro ao Alterar chamado: " + ex
                });
            }
        }

        [HttpGet]
        public ActionResult Excluir(string id)
        {
            try
            {
                var valor = ch.Excluir(id);

                if (valor == 1)
                {
                    return RedirectToAction("Index", "Chamados", new
                    {
                        id = 1,
                        msg = "Chamado Excluído com sucesso!"
                    });
                }
                else
                {
                    return RedirectToAction("Index", "Chamados", new
                    {
                        id = 2,
                        msg = "Erro ao tentar excluir Chamado!"
                    });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Chamados", new
                {
                    id = 2,
                    msg = "Erro ao tentar excluir Chamado: " + ex
                });
            }
        }

        public ActionResult VerificaChamadoAnexo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VerificarPlanilha(Chamados Chamado)
        {
            try
            {
                object[,] obj = null;
                int noOfCol = 0;
                int noOfRow = 0;
                var dados = new List<Chamados>();
                var tempChamado = new Chamados();
                var tempChamado2 = new Chamados();

                //HttpFileCollectionBase file = Request.Files;

                if ((Chamado.Arquivo != null) && (Chamado.Arquivo.ContentLength > 0))
                {
                    //string fileName = file.FileName; 
                    //string fileContentType = file.ContentType; 
                    byte[] fileBytes = new byte[Chamado.Arquivo.ContentLength];
                    var data = Request.InputStream.Read(fileBytes, 0, Convert.ToInt32(Chamado.Arquivo.ContentLength));
                    // var usersList = new List<Users>(); 
                    //using (var package = new ExcelPackage()) 
                    using (var package = new ExcelPackage(Chamado.Arquivo.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        noOfCol = workSheet.Dimension.End.Column;
                        noOfRow = workSheet.Dimension.End.Row;
                        obj = new object[noOfRow, noOfCol];
                        obj = (object[,])workSheet.Cells.Value;

                        var caminhoDocumento = Server.MapPath("~/Arquivos/Chamado/Planilha/Chamados.xlsx");
                        if (System.IO.File.Exists(caminhoDocumento))
                            System.IO.File.Delete(caminhoDocumento);


                        var excelPackage = new ExcelPackage();
                        excelPackage.Workbook.Properties.Author = "Construtora Eletrica Luz";
                        excelPackage.Workbook.Properties.Title = "Planilha de Chamados";

                        // Aqui simplesmente adiciono a planilha inicial
                        var sheet = excelPackage.Workbook.Worksheets.Add("Planilha");
                        sheet.Name = "Planilha";

                        sheet.Cells[1, 1].Value = "Chamado";
                        sheet.Cells[1, 2].Value = "Agencia";
                        sheet.Cells[1, 3].Value = "Categoria";
                        sheet.Cells[1, 4].Value = "Descrição";
                        sheet.Cells[1, 5].Value = "Rota";
                        sheet.Cells[1, 6].Value = "Data";

                        int j = 2;
                        for (int i = 0; i < noOfRow; i++)
                        {
                            var temp = ch.ListaPorChamado(obj[i, 0].ToString());
                            if (temp != null)
                            {
                                sheet.Cells[j, 1].Value = temp.Numero;
                                sheet.Cells[j, 2].Value = temp.Local.ToUpper();
                                sheet.Cells[j, 3].Value = temp.Categoria;
                                sheet.Cells[j, 4].Value = temp.Detalhe;
                                sheet.Cells[j, 5].Value = temp.Rota;
                                sheet.Cells[j, 6].Value = temp.DataInicio.ToString("dd/MM/yyyy");
                                dados.Add(temp);
                            }
                            else
                            {
                                ch.Numero = obj[i, 0].ToString();

                                sheet.Cells[j, 1].Value = ch.Numero;
                                sheet.Cells[j, 2].Value = "";
                                sheet.Cells[j, 3].Value = "";
                                sheet.Cells[j, 4].Value = "";
                                sheet.Cells[j, 5].Value = "";
                                sheet.Cells[j, 6].Value = "";

                                tempChamado2 = ch;
                                dados.Add(tempChamado2);
                            }
                            j++;
                        }
                        FileInfo excelFile = new FileInfo(Server.MapPath("~/Arquivos/Chamado/Planilha/Chamados.xlsx"));
                        excelPackage.SaveAs(excelFile);

                        ch.MChamados = dados;
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Chamados", new
                {
                    id = 2,
                    msg = "Erro ao tentar verificar anexo: " + ex
                });
            }

            return View(ch);
        }

    }
}