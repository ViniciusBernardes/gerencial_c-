using Gerencial.biblioteca;
using Gerencial.Repositorio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Gerencial.Models
{
    public class Boletos
    {
        public List<Boletos> MBoletos { get; set; }

        public Boletos MBoletoSimples { get; set; }
        public List<Categoria> MCategoria { get; set; }
        public List<Rota> MRota { get; set; }
        public string Id { get; set; }

        public string Rota { get; set; }

        public string Tipo { get; set; }

        [Required(ErrorMessage = "Favor preencher a Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Favor informar a data de vencimento")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Favor informar o valor do boleto")]
        public string Valor { get; set; }

        [Required(ErrorMessage = "Favor informar a linha digitável do boleto")]
        public string Linha { get; set; }
        public string Status { get; set; }
        public string ConsultaTipo { get; set; }
        public string ConsultaRota { get; set; }
        public string ConsultaStatus { get; set; }
        public string ConsultaDataInicio { get; set; }
        public string ConsultaDataFim { get; set; }

        private readonly Conexao contexto;
        public Util utl;

        public Boletos()
        {
            contexto = new Conexao();
            utl = new Util();
        }

        public List<Boletos> Listar(Boletos boleto)
        {
            var dados = new List<Boletos>();
            string strQuery = "Select b.id, e.nome as rota, c.nome as tipo, b.descricao, b.data, b.valor, b.linha ";
            strQuery += "from boletos b inner join empresa e on e.id = b.rota";
            strQuery += " left join categoria_gasto c on c.id = b.tipo WHERE 1=1";
            if (boleto.ConsultaStatus != "0")
                strQuery += " AND b.status = '" + boleto.ConsultaStatus + "'";
            if (boleto.ConsultaTipo != "0")
                strQuery += " AND c.id = " + boleto.ConsultaTipo + "";
            if (boleto.ConsultaRota != "0")
                strQuery += " AND e.id = " + boleto.ConsultaRota + "";
            if (boleto.ConsultaDataInicio != null)
                strQuery += " AND b.data >= '" + boleto.ConsultaDataInicio + "'";
            if (boleto.ConsultaDataFim != null)
                strQuery += " AND b.data <= '" + boleto.ConsultaDataFim + "'";
            strQuery += " order by b.data ASC";

            var rows = contexto.ExecutaComandoComRetorno(strQuery, null);
            foreach (var row in rows)
            {
                var tempBoleto = new Boletos
                {
                    Id = utl.CodificaId(int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0")),
                    Rota = row["rota"],
                    Tipo = row["tipo"],
                    Descricao = row["descricao"],
                    Data = DateTime.Parse(row["data"]),
                    Valor = row["valor"],
                    Linha = row["linha"]
                };
                dados.Add(tempBoleto);
            }
            return dados;
        }

        public int Cadastrar(Boletos boleto)
        {
            const string commandText = " INSERT INTO boletos (rota, tipo, descricao, valor, data, linha) VALUES (@Rota, @Tipo, @Descricao, @Valor, @Data, @Linha) ";

            var parameters = new Dictionary<string, object>
            {
                {"Rota", boleto.Rota},
                {"Tipo", boleto.Tipo},
                {"Descricao", boleto.Descricao},
                {"Valor", Decimal.Parse(boleto.Valor)},
                {"Data", boleto.Data.ToString("yyyy-MM-dd") },
                {"Linha", boleto.Linha}
            };

            return contexto.ExecutaComando(commandText, parameters);
        }

        public Boletos ListaId(string id)
        {
            var dados = new List<Boletos>();
            const string strQuery = "SELECT b.id, b.rota, b.tipo, b.descricao, b.data, b.valor, b.linha FROM boletos b WHERE b.id = @Id";
            var parametros = new Dictionary<string, object>
            {
                {"Id", utl.DecodificaId(id)}
            };
            var rows = contexto.ExecutaComandoComRetorno(strQuery, parametros);
            foreach (var row in rows)
            {
                var tempBoleto = new Boletos
                {
                    Id = utl.CodificaId(int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0")),
                    Rota = row["rota"],
                    Tipo = row["tipo"],
                    Descricao = row["descricao"],
                    Data = DateTime.Parse(row["data"]),
                    Valor = row["valor"],
                    Linha = row["linha"]
                };
                dados.Add(tempBoleto);
            }
            return dados.FirstOrDefault();
        }

        public int Alterar(Boletos boleto)
        {
            var commandText = " UPDATE boletos SET ";
            commandText += " rota = @Rota, ";
            commandText += " tipo = @Tipo, ";
            commandText += " descricao = @Descricao, ";
            commandText += " valor = @Valor, ";
            commandText += " data = @Data, ";
            commandText += " linha = @Linha ";

            commandText += " WHERE id = @Id ";

            var parameters = new Dictionary<string, object>
            {
                {"Id", utl.DecodificaId(boleto.Id)},
                {"Rota", boleto.Rota},
                {"Tipo", boleto.Tipo},
                {"Descricao", boleto.Descricao},
                {"Valor", Decimal.Parse(boleto.Valor)},
                {"Data", boleto.Data.ToString("yyyy-MM-dd")},
                {"Linha", boleto.Linha},
            };

            return contexto.ExecutaComando(commandText, parameters);
        }

        public int Excluir(string id)
        {
            const string strQuery = "DELETE FROM boletos WHERE id = @Id";
            var parametros = new Dictionary<string, object>
            {
                {"Id", utl.DecodificaId(id)}
            };

            return contexto.ExecutaComando(strQuery, parametros);
        }
    }
}