using Gerencial.biblioteca;
using Gerencial.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gerencial.Models
{
    public class Chamados
    {
        private readonly Conexao contexto;
        public Util utl;

        public List<Chamados> MChamados { get; set; }
        public Chamados MChamadoSimples { get; set; }
        public List<Rota> MRota { get; set; }
        public List<Agencia> MAgencia { get; set; }

        public List<Categoria> MCategoria { get; set; }
        public List<Categoria> MSubCategoria { get; set; }

        public string Id { get; set; }
        public string Numero { get; set; }
        public string Ocorrencia { get; set; }
        public string Local { get; set; }
        public string Eng { get; set; }
        public string Rota { get; set; }
        public string Execucao { get; set; }
        public string Relatado { get; set; }
        public string IdLocal { get; set; }
        public string Descricao { get; set; }
        public string Detalhe { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime SlaAtendimento { get; set; }
        public DateTime SlaConclusao { get; set; }
        public DateTime? DataAtendimento { get; set; }
        public DateTime? DataConclusao { get; set; }
        public string SubCategoria { get; set; }
        public string Categoria { get; set; }
        public string Status { get; set; }
        public string DiasAberto { get; set; }
        public string DocumentoChamado { get; set; }
        public string ConsultaEstado { get; set; }
        public string ConsultaChamado { get; set; }
        public string ConsultaStatus { get; set; }
        public string ConsultaAgencia { get; set; }
        public string ConsultaRelatado { get; set; }
        public string ConsultaRota { get; set; }
        public string ConsultaDataInicio { get; set; }
        public string ConsultaDataFim { get; set; }
        public HttpPostedFileBase Arquivo { get; set; }


        public Chamados()
        {
            contexto = new Conexao();
            utl = new Util();
        }

        public List<Chamados> Listar(Chamados ch)
        {
            var dados = new List<Chamados>();
            string strQuery = "Select c.id,";
            strQuery += "c.numero,";
            strQuery += "c.ocorrencia,";
            strQuery += "(select concat(l.agencia, ' - ', l.cidade) from local l where c.local = l.id ) as local,";
            strQuery += "l.rota,";
            strQuery += "(select e.nome from empresa e where c.relatado = e.id ) as execucao,";
            strQuery += "local as id_local,";
            strQuery += "c.eng_responsavel as eng,";
            strQuery += "c.relatado,";
            strQuery += "c.descricao,";
            strQuery += "c.detalhes,";
            strQuery += "c.data_inicio,";
            strQuery += "c.data_fim,";
            strQuery += "c.status,";
            strQuery += "c.sla_atendimento as data_atendimento,";
            strQuery += "c.sla_conclusao as data_conclusao,";
            strQuery += "sb.nome as subcategoria,";
            strQuery += "(select DATEDIFF(CURDATE(), ch.data_inicio) from chamado ch where c.id = ch.id) dias_aberto,";
            strQuery += "(SELECT DATE_ADD(cd.data_inicio, INTERVAL sb.sla_atendimento DAY) from chamado cd where c.id = cd.id) as slaatendimento,";
            strQuery += "(SELECT DATE_ADD(cd.data_inicio, INTERVAL sb.sla_conclusao DAY) from chamado cd where c.id = cd.id) as slaconclusao,";
            strQuery += "(select d.nome from documento d where d.tabela = 'chamado' and d.id_origem = c.id) as documento_chamado";
            strQuery += " from chamado c inner join local l on c.local = l.id inner join subcategoria sb ON sb.id = c.descricao";
            strQuery += " where 1 = 1";

            if (ch.ConsultaAgencia != "0")
                strQuery += " and c.local = '" + utl.DecodificaId(ch.ConsultaAgencia) + "'";
            if (ch.ConsultaChamado != null)
                strQuery += " and c.numero = " + ch.ConsultaChamado + "";
            if (ch.ConsultaRelatado != "0")
                strQuery += " and c.relatado = '" + ch.ConsultaRelatado + "'";
            if (ch.ConsultaRota != "0")
                strQuery += " and upper(l.rota) = '" + ch.ConsultaRota.ToUpper() + "'";
            if (ch.ConsultaStatus != "0")
                strQuery += " and upper(c.status) = '" + ch.ConsultaStatus.ToUpper() + "'";
            if (ch.ConsultaEstado != "0")
                strQuery += " and l.estado = '" + ch.ConsultaEstado + "'";
            if (ch.ConsultaDataInicio != null)
                strQuery += " AND c.data_inicio >= '" + ch.ConsultaDataInicio + "'";
            if (ch.ConsultaDataFim != null)
                strQuery += " AND c.data_fim <= '" + ch.ConsultaDataFim + "'";

            strQuery += " order by c.numero DESC";

            var rows = contexto.ExecutaComandoComRetorno(strQuery, null);
            foreach (var row in rows)
            {
                string dataAtendTemp;
                DateTime? dataAtend = null;
                dataAtendTemp = (!string.IsNullOrEmpty(row["data_atendimento"]) ? row["data_atendimento"] : null);
                if (dataAtendTemp != null)
                    dataAtend = DateTime.Parse(row["data_atendimento"]);

                string dataConclTemp;
                DateTime? dataConcl = null;
                dataConclTemp = (!string.IsNullOrEmpty(row["data_conclusao"]) ? row["data_conclusao"] : null);
                if (dataConclTemp != null)
                    dataConcl = DateTime.Parse(row["data_conclusao"]);

                var tempChamado = new Chamados
                {
                    Id = utl.CodificaId(int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0")),
                    Numero = row["numero"],
                    Ocorrencia = row["ocorrencia"],
                    Local = row["local"],
                    Eng = row["eng"],
                    Rota = row["rota"],
                    Execucao = row["execucao"],
                    Relatado = row["relatado"],
                    IdLocal = row["id_local"],
                    Descricao = row["descricao"],
                    Detalhe = row["detalhes"],
                    DataInicio = DateTime.Parse(row["data_inicio"]),
                    DataFim = DateTime.Parse(row["data_fim"]),
                    SlaAtendimento = DateTime.Parse(row["slaatendimento"]),
                    SlaConclusao = DateTime.Parse(row["slaconclusao"]),
                    DataAtendimento = dataAtend,
                    DataConclusao = dataConcl,
                    SubCategoria = row["subcategoria"],
                    Status = row["status"],
                    DiasAberto = row["dias_aberto"],
                    DocumentoChamado = row["documento_chamado"]

                };
                dados.Add(tempChamado);
            }
            return dados;
        }

        public Chamados ListaId(string id)
        {
            var dados = new List<Chamados>();
            string strQuery = "Select c.id,";
            strQuery += "c.numero,";
            strQuery += "c.ocorrencia,";
            strQuery += "(select concat(l.agencia, ' - ', l.cidade) from local l where c.local = l.id ) as local,";
            strQuery += "l.rota,";
            strQuery += "(select e.nome from empresa e where c.relatado = e.id ) as execucao,";
            strQuery += "local as id_local,";
            strQuery += "c.eng_responsavel as eng,";
            strQuery += "c.relatado,";
            strQuery += "c.descricao,";
            strQuery += "c.detalhes,";
            strQuery += "c.data_inicio,";
            strQuery += "c.data_fim,";
            strQuery += "c.status,";
            strQuery += "c.sla_atendimento as data_atendimento,";
            strQuery += "c.sla_conclusao as data_conclusao,";
            strQuery += "sb.categoria as categoria,";
            strQuery += "sb.id as subcategoria,";
            strQuery += "(select DATEDIFF(CURDATE(), ch.data_inicio) from chamado ch where c.id = ch.id) dias_aberto,";
            strQuery += "(SELECT DATE_ADD(cd.data_inicio, INTERVAL sb.sla_atendimento DAY) from chamado cd where c.id = cd.id) as slaatendimento,";
            strQuery += "(SELECT DATE_ADD(cd.data_inicio, INTERVAL sb.sla_conclusao DAY) from chamado cd where c.id = cd.id) as slaconclusao,";
            strQuery += "(select d.nome from documento d where d.tabela = 'chamado' and d.id_origem = c.id) as documento_chamado";
            strQuery += " from chamado c inner join local l on c.local = l.id inner join subcategoria sb ON sb.id = c.descricao";
            strQuery += " where c.id = " + utl.DecodificaId(id);



            var rows = contexto.ExecutaComandoComRetorno(strQuery, null);
            foreach (var row in rows)
            {
                string dataAtendTemp;
                DateTime? dataAtend = null;
                dataAtendTemp = (!string.IsNullOrEmpty(row["data_atendimento"]) ? row["data_atendimento"] : null);
                if (dataAtendTemp != null)
                    dataAtend = DateTime.Parse(row["data_atendimento"]);

                string dataConclTemp;
                DateTime? dataConcl = null;
                dataConclTemp = (!string.IsNullOrEmpty(row["data_conclusao"]) ? row["data_conclusao"] : null);
                if (dataConclTemp != null)
                    dataConcl = DateTime.Parse(row["data_conclusao"]);

                var tempChamado = new Chamados
                {
                    Id = utl.CodificaId(int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0")),
                    Numero = row["numero"],
                    Ocorrencia = row["ocorrencia"],
                    Local = row["local"],
                    Eng = row["eng"],
                    Rota = row["rota"],
                    Execucao = row["execucao"],
                    Relatado = row["relatado"],
                    IdLocal = row["id_local"],
                    Descricao = row["descricao"],
                    Detalhe = row["detalhes"],
                    DataInicio = DateTime.Parse(row["data_inicio"]),
                    DataFim = DateTime.Parse(row["data_fim"]),
                    SlaAtendimento = DateTime.Parse(row["slaatendimento"]),
                    SlaConclusao = DateTime.Parse(row["slaconclusao"]),
                    DataAtendimento = dataAtend,
                    DataConclusao = dataConcl,
                    SubCategoria = row["subcategoria"],
                    Categoria = row["categoria"],
                    Status = row["status"],
                    DiasAberto = row["dias_aberto"],
                    DocumentoChamado = row["documento_chamado"]

                };
                dados.Add(tempChamado);
            }
            return dados.FirstOrDefault();
        }

        public Chamados ListaPorChamado(string Chamado)
        {
            var dados = new List<Chamados>();
            string strQuery = "Select c.id,";
            strQuery += "c.numero,";
            strQuery += "c.ocorrencia,";
            strQuery += "(select concat(l.agencia, ' - ', l.cidade) from local l where c.local = l.id ) as local,";
            strQuery += "l.rota,";
            strQuery += "(select e.nome from empresa e where c.relatado = e.id ) as execucao,";
            strQuery += "local as id_local,";
            strQuery += "c.eng_responsavel as eng,";
            strQuery += "c.relatado,";
            strQuery += "c.descricao,";
            strQuery += "c.detalhes,";
            strQuery += "c.data_inicio,";
            strQuery += "c.data_fim,";
            strQuery += "c.status,";
            strQuery += "c.sla_atendimento as data_atendimento,";
            strQuery += "c.sla_conclusao as data_conclusao,";
            strQuery += "sb.nome as categoria,";
            strQuery += "sb.id as subcategoria,";
            strQuery += "(select DATEDIFF(CURDATE(), ch.data_inicio) from chamado ch where c.id = ch.id) dias_aberto,";
            strQuery += "(SELECT DATE_ADD(cd.data_inicio, INTERVAL sb.sla_atendimento DAY) from chamado cd where c.id = cd.id) as slaatendimento,";
            strQuery += "(SELECT DATE_ADD(cd.data_inicio, INTERVAL sb.sla_conclusao DAY) from chamado cd where c.id = cd.id) as slaconclusao,";
            strQuery += "(select d.nome from documento d where d.tabela = 'chamado' and d.id_origem = c.id) as documento_chamado";
            strQuery += " from chamado c inner join local l on c.local = l.id inner join subcategoria sb ON sb.id = c.descricao";
            strQuery += " where c.numero = " + Chamado;

            var rows = contexto.ExecutaComandoComRetorno(strQuery, null);
            foreach (var row in rows)
            {
                string dataAtendTemp;
                DateTime? dataAtend = null;
                dataAtendTemp = (!string.IsNullOrEmpty(row["data_atendimento"]) ? row["data_atendimento"] : null);
                if (dataAtendTemp != null)
                    dataAtend = DateTime.Parse(row["data_atendimento"]);

                string dataConclTemp;
                DateTime? dataConcl = null;
                dataConclTemp = (!string.IsNullOrEmpty(row["data_conclusao"]) ? row["data_conclusao"] : null);
                if (dataConclTemp != null)
                    dataConcl = DateTime.Parse(row["data_conclusao"]);

                var tempChamado = new Chamados
                {
                    Id = utl.CodificaId(int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0")),
                    Numero = row["numero"],
                    Ocorrencia = row["ocorrencia"],
                    Local = row["local"],
                    Eng = row["eng"],
                    Rota = row["rota"],
                    Execucao = row["execucao"],
                    Relatado = row["relatado"],
                    IdLocal = row["id_local"],
                    Descricao = row["descricao"],
                    Detalhe = row["detalhes"],
                    DataInicio = DateTime.Parse(row["data_inicio"]),
                    DataFim = DateTime.Parse(row["data_fim"]),
                    SlaAtendimento = DateTime.Parse(row["slaatendimento"]),
                    SlaConclusao = DateTime.Parse(row["slaconclusao"]),
                    DataAtendimento = dataAtend,
                    DataConclusao = dataConcl,
                    SubCategoria = row["subcategoria"],
                    Categoria = row["categoria"],
                    Status = row["status"],
                    DiasAberto = row["dias_aberto"],
                    DocumentoChamado = row["documento_chamado"]
                };
                dados.Add(tempChamado);
            }
            return dados.FirstOrDefault();
        }

        public int Cadastrar(Chamados chamado)
        {
            string commandText = " INSERT INTO chamado (numero, ocorrencia, local, eng_responsavel, descricao, detalhes, relatado, data_inicio, data_fim, id_documento, status)";
            commandText += " VALUES (@Numero, @Ocorrencia, @Local, @Eng, @Descricao, @Detalhes, @Relatado, @Data_inicio, @Data_fim, @Id_documento, @Status) ";

            var parameters = new Dictionary<string, object>
            {
                {"Numero", chamado.Numero},
                {"Ocorrencia", chamado.Numero},
                {"Local", utl.DecodificaId(chamado.Local)},
                {"Eng", chamado.Eng},
                {"Descricao", chamado.SubCategoria},
                {"Detalhes", chamado.Detalhe},
                {"Relatado", chamado.Rota},
                {"Data_inicio", chamado.DataInicio},
                {"Data_fim", chamado.DataInicio},
                {"Id_documento", chamado.DocumentoChamado},
                {"Status", chamado.Status}
            };

            return contexto.ExecutaComando(commandText, parameters);
        }

        public int VerificaChamado(string Chamado)
        {
            var dados = new List<Chamados>();
            const string strQuery = "SELECT id FROM chamado WHERE numero = @Chamado";
            var parametros = new Dictionary<string, object>
            {
                {"Chamado", Chamado}
            };
            var rows = contexto.ExecutaComandoComRetorno(strQuery, parametros);
            foreach (var row in rows)
            {
                var tempChamado = new Chamados
                {
                    Id = utl.CodificaId(int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0"))
                };
                dados.Add(tempChamado);
            }
            return dados.Count;
        }

        public int Sinaliza(Chamados chamado)
        {
            var commandText = " UPDATE chamado SET ";
            commandText += " sla_atendimento = @SlaAtendimento, ";
            commandText += " sla_conclusao = @SlaConclusao";
            if (chamado.DataConclusao != null)
                commandText += ", status = 'Fechado' ";
            else
                commandText += ", status = 'Aberto' ";
            commandText += " WHERE id = @Id";


            var parameters = new Dictionary<string, object>
            {
                {"Id", utl.DecodificaId(chamado.Id)},
                {"SlaAtendimento", chamado.DataAtendimento},
                {"SlaConclusao", chamado.DataConclusao},
            };

            return contexto.ExecutaComando(commandText, parameters);
        }

        public int Alterar(Chamados chamado)
        {
            var commandText = " UPDATE chamado SET ";
            commandText += " local = @Local, ";
            commandText += " eng_responsavel = @Eng, ";
            commandText += " descricao = @SubCategoria, ";
            commandText += " detalhes = @Detalhe, ";
            commandText += " relatado = @Rota, ";
            commandText += " data_inicio = @DataInicio, ";
            commandText += " data_fim = @DataInicio, ";
            commandText += " status = @Status";
            commandText += " WHERE id = @Id";


            var parameters = new Dictionary<string, object>
            {
                {"Id", utl.DecodificaId(chamado.Id)},
                {"Local", utl.DecodificaId(chamado.Local)},
                {"Eng", chamado.Eng},
                {"SubCategoria", chamado.SubCategoria},
                {"Detalhe", chamado.Detalhe},
                {"Rota", chamado.Rota},
                {"DataInicio", chamado.DataInicio},
                {"Status", chamado.Status},
            };

            return contexto.ExecutaComando(commandText, parameters);
        }

        public int Excluir(string id)
        {
            const string strQuery = "DELETE FROM chamado WHERE id = @Id";
            var parametros = new Dictionary<string, object>
            {
                {"Id", utl.DecodificaId(id)}
            };

            return contexto.ExecutaComando(strQuery, parametros);
        }
    }
}