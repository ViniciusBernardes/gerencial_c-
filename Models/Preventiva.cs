using Gerencial.biblioteca;
using Gerencial.Repositorio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gerencial.Models
{
    public class Preventiva
    {
        private readonly Conexao contexto;
        public Util utl;

        public string Id { get; set; }
        public string Local { get; set; }
        [Required(ErrorMessage = "Favor informar o mês da preventiva")]
        public string Mes { get; set; }
        [Required(ErrorMessage = "Favor informar a data da preventiva elétrica")]
        public DateTime? DataPreventivaEletrica { get; set; }
        [Required(ErrorMessage = "Favor informar a data da preventiva de ar condicionado")]
        public DateTime? DataPreventivaAr { get; set; }
        public int PreventivaEletrica { get; set; }
        public int PreventivaAr { get; set; }
        public string IdLocal { get; set; }
        public List<Preventiva> MPreventiva { set; get; }
        public Preventiva MPreventivaSimples { get; set; }
        public int ConsultaMes { get; set; }
        public List<Agencia> MAgencia { get; set; }
        [Required(ErrorMessage = "Favor anexar a preventiva elétrica")]
        public HttpPostedFileBase AnexoEletrica { get; set; }
        [Required(ErrorMessage = "Favor anexar a preventiva de ar condicionado")]
        public HttpPostedFileBase AnexoAr { get; set; }

        public Preventiva()
        {
            contexto = new Conexao();
            utl = new Util();
        }

        public List<Preventiva> Listar(int Mes)
        {
            var dados = new List<Preventiva>();
            string strQuery = "Select concat(l.agencia, ' - ', l.cidade) as local,";
            strQuery += "p.id,";
            strQuery += "p.mes,";
            strQuery += "p.data_preventiva_eletrica,";
            strQuery += "p.data_preventiva_ar,";
            strQuery += "p.prev_eletrica,";
            strQuery += "p.prev_ar,";
            strQuery += "p.local as id_local ";
            strQuery += "from local l left ";
            strQuery += "join preventiva p ";
            strQuery += "on l.id = p.local and p.mes = " + Mes + " ";
            strQuery += "WHERE UPPER(l.estado) = 'ESPIRITO SANTO' ";
            strQuery += "order by p.id DESC";

            var rows = contexto.ExecutaComandoComRetorno(strQuery, null);
            foreach (var row in rows)
            {
                string dataPrevEletricaTemp;
                DateTime? dataPrevEletrica = null;
                dataPrevEletricaTemp = (!string.IsNullOrEmpty(row["data_preventiva_eletrica"]) ? row["data_preventiva_eletrica"] : null);
                if (dataPrevEletricaTemp != null)
                    dataPrevEletrica = DateTime.Parse(row["data_preventiva_eletrica"]);

                string dataPrevArTemp;
                DateTime? dataPrevAr = null;
                dataPrevArTemp = (!string.IsNullOrEmpty(row["data_preventiva_ar"]) ? row["data_preventiva_ar"] : null);
                if (dataPrevArTemp != null)
                    dataPrevAr = DateTime.Parse(row["data_preventiva_ar"]);

                if (row["id"] != null)
                {
                    var tempPreventiva = new Preventiva
                    {
                        Id = utl.CodificaId(int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0")),
                        Local = row["local"],
                        Mes = row["mes"],
                        DataPreventivaEletrica = dataPrevEletrica,
                        DataPreventivaAr = dataPrevEletrica,
                        PreventivaEletrica = int.Parse(row["prev_eletrica"]),
                        PreventivaAr = int.Parse(row["prev_ar"]),
                        IdLocal = row["id_local"]
                    };
                    dados.Add(tempPreventiva);
                }
                else
                {
                    var tempPreventiva = new Preventiva
                    {
                        Id = null,
                        Local = row["local"],
                        Mes = null,
                        DataPreventivaEletrica = dataPrevEletrica,
                        DataPreventivaAr = dataPrevEletrica,
                        PreventivaEletrica = 0,
                        PreventivaAr = 0,
                        IdLocal = null
                    };
                    dados.Add(tempPreventiva);
                }
            }
            return dados;
        }

        public int Cadastrar(Preventiva prev)
        {
            const string commandText = " INSERT INTO preventiva (mes, data_preventiva_eletrica, data_preventiva_ar, local, funcionario, detalhe, prev_eletrica, prev_ar) VALUES (@Mes, @DataPrevEletrica, @DataPrevAr, @Local, @Funcionario, @Detalhe, @PrevEletrica, @PrevAr) ";

            var parameters = new Dictionary<string, object>
            {
                {"Mes", prev.Mes},
                {"DataPrevEletrica", prev.DataPreventivaEletrica},
                {"DataPrevAr", prev.DataPreventivaAr},
                {"Local", utl.DecodificaId(prev.Local)},
                {"Funcionario", ""},
                {"Detalhe", "" },
                {"PrevEletrica", 1},
                {"PrevAr", 1}
            };

            return contexto.ExecutaComando(commandText, parameters);
        }

        public Preventiva ListaId(string id)
        {
            var dados = new List<Preventiva>();
            string strQuery = "Select concat(l.agencia, ' - ', l.cidade) as local,";
            strQuery += "p.id,";
            strQuery += "p.mes,";
            strQuery += "p.data_preventiva_eletrica,";
            strQuery += "p.data_preventiva_ar,";
            strQuery += "p.prev_eletrica,";
            strQuery += "p.prev_ar,";
            strQuery += "p.local as id_local ";
            strQuery += "from local l left ";
            strQuery += "join preventiva p ";
            strQuery += "on l.id = p.local ";
            strQuery += "WHERE UPPER(l.estado) = 'ESPIRITO SANTO' ";
            strQuery += "AND p.id = " + utl.DecodificaId(id);

            var rows = contexto.ExecutaComandoComRetorno(strQuery, null);
            foreach (var row in rows)
            {
                string dataPrevEletricaTemp;
                DateTime? dataPrevEletrica = null;
                dataPrevEletricaTemp = (!string.IsNullOrEmpty(row["data_preventiva_eletrica"]) ? row["data_preventiva_eletrica"] : null);
                if (dataPrevEletricaTemp != null)
                    dataPrevEletrica = DateTime.Parse(row["data_preventiva_eletrica"]);

                string dataPrevArTemp;
                DateTime? dataPrevAr = null;
                dataPrevArTemp = (!string.IsNullOrEmpty(row["data_preventiva_ar"]) ? row["data_preventiva_ar"] : null);
                if (dataPrevArTemp != null)
                    dataPrevAr = DateTime.Parse(row["data_preventiva_ar"]);

                var tempPreventiva = new Preventiva
                {
                    Id = utl.CodificaId(int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0")),
                    Local = row["local"],
                    Mes = row["mes"],
                    DataPreventivaEletrica = dataPrevEletrica,
                    DataPreventivaAr = dataPrevEletrica,
                    PreventivaEletrica = int.Parse(row["prev_eletrica"]),
                    PreventivaAr = int.Parse(row["prev_ar"]),
                    IdLocal = row["id_local"]
                };
                dados.Add(tempPreventiva);
            }

            return dados.FirstOrDefault();
        }

        public int Alterar(Preventiva preventiva)
        {
            var commandText = " UPDATE preventiva SET ";
            commandText += " mes = @Mes, ";
            commandText += " data_preventiva_eletrica = @DataPrevEletrica, ";
            commandText += " data_preventiva_ar = @DataPrevAr, ";
            commandText += " local = @Local ";
            commandText += " WHERE id = @Id";

            var parameters = new Dictionary<string, object>
            {
                {"Id", int.Parse(utl.DecodificaId(preventiva.Id))},
                {"Mes", preventiva.Mes},
                {"DataPrevEletrica", preventiva.DataPreventivaEletrica},
                {"DataPrevAr", preventiva.DataPreventivaAr},
                {"Local", utl.DecodificaId(preventiva.Local)},
            };

            return contexto.ExecutaComando(commandText, parameters);
        }

    }

}