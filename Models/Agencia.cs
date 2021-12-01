using Gerencial.biblioteca;
using Gerencial.Repositorio;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Gerencial.Models
{

    public class Agencia
    {
        private readonly Conexao contexto;
        public Util utl;
        public string Id { get; set; }

        [Required(ErrorMessage = "Favor Selecionar a rota de atendimento")]
        public string Rota { get; set; }

        [Required(ErrorMessage = "Favor preencher o número da agencia")]
        public string NAgencia { get; set; }

        [Required(ErrorMessage = "Favor informar a Cidade")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Favor informar o Estado")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "Favor informar a distancia da agencia até a sede")]
        public string Km { get; set; }

        public List<Agencia> MAgencia { get; set; }

        public Agencia()
        {
            contexto = new Conexao();
            utl = new Util();
        }

        public List<Agencia> Listar()
        {
            var dados = new List<Agencia>();
            string strQuery = "Select id, rota, agencia, cidade, estado, km from local";
            strQuery += " order by cidade, agencia";

            var rows = contexto.ExecutaComandoComRetorno(strQuery, null);
            foreach (var row in rows)
            {
                var tempAgencia = new Agencia
                {
                    Id = utl.CodificaId(int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0")),
                    Rota = utl.Rota(row["rota"]),
                    NAgencia = row["agencia"],
                    Cidade = row["cidade"],
                    Estado = row["estado"],
                    Km = row["km"]
                };
                dados.Add(tempAgencia);
            }
            return dados;
        }

        public List<Agencia> ListarES()
        {
            var dados = new List<Agencia>();
            string strQuery = "Select id, rota, agencia, cidade, estado, km from local WHERE upper(estado) = 'ESPIRITO SANTO'";
            strQuery += " order by cidade, agencia";

            var rows = contexto.ExecutaComandoComRetorno(strQuery, null);
            foreach (var row in rows)
            {
                var tempAgencia = new Agencia
                {
                    Id = utl.CodificaId(int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0")),
                    Rota = utl.Rota(row["rota"]),
                    NAgencia = row["agencia"],
                    Cidade = row["cidade"],
                    Estado = row["estado"],
                    Km = row["km"]
                };
                dados.Add(tempAgencia);
            }
            return dados;
        }

        public int Cadastrar(Agencia agencia)
        {
            var estado = "";
            const string commandText = " INSERT INTO local (rota, agencia, cidade, estado, km) VALUES (@Rota, @Agencia, @Cidade, @Estado, @Km) ";
            if (agencia.Cidade.ToUpper() == "ESPIRITO SANTO")
                estado = "ESPIRITO SANTO";
            else
                estado = "MINAS GERAIS";
            var parameters = new Dictionary<string, object>
            {
                {"Rota", agencia.Rota},
                {"Agencia", agencia.NAgencia},
                {"Cidade", agencia.Cidade},
                {"Estado", estado},
                {"Km", agencia.Km}
            };

            return contexto.ExecutaComando(commandText, parameters);
        }

        public Agencia ListaId(string id)
        {
            var dados = new List<Agencia>();
            const string strQuery = "SELECT id, rota, agencia, cidade, estado, km FROM local WHERE id = @Id";
            var parametros = new Dictionary<string, object>
            {
                {"Id", utl.DecodificaId(id)}
            };
            var rows = contexto.ExecutaComandoComRetorno(strQuery, parametros);
            foreach (var row in rows)
            {
                var tempAgencia = new Agencia
                {
                    Id = utl.CodificaId(int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0")),
                    Rota = row["rota"],
                    NAgencia = row["agencia"],
                    Cidade = row["cidade"],
                    Estado = row["estado"],
                    Km = row["km"]
                };
                dados.Add(tempAgencia);
            }
            return dados.FirstOrDefault();
        }

        public int Alterar(Agencia ag)
        {
            var estado = "";
            var commandText = " UPDATE local SET ";
            commandText += " rota = @Rota, ";
            commandText += " cidade = @Cidade, ";
            commandText += " estado = @Estado, ";
            commandText += " km = @Km ";

            commandText += " WHERE id = @Id and agencia = @Agencia";

            if (ag.Cidade.ToUpper() == "ESPIRITO SANTO")
                estado = "ESPIRITO SANTO";
            else
                estado = "MINAS GERAIS";

            var parameters = new Dictionary<string, object>
            {
                {"Id", utl.DecodificaId(ag.Id)},
                {"Rota", ag.Rota},
                {"Agencia", ag.NAgencia},
                {"Cidade", ag.Cidade},
                {"Estado", estado},
                {"Km", ag.Km},
            };

            return contexto.ExecutaComando(commandText, parameters);
        }

        public int Excluir(string id)
        {
            const string strQuery = "DELETE FROM local WHERE id = @Id";
            var parametros = new Dictionary<string, object>
            {
                {"Id", utl.DecodificaId(id)}
            };

            return contexto.ExecutaComando(strQuery, parametros);
        }

        public int VerificarCadastrado(string agencia)
        {
            var dados = new List<Agencia>();
            const string strQuery = "SELECT id, rota, agencia, cidade, estado, km FROM local WHERE agencia = @Agencia";
            var parametros = new Dictionary<string, object>
            {
                {"Agencia", agencia}
            };
            var rows = contexto.ExecutaComandoComRetorno(strQuery, parametros);
            foreach (var row in rows)
            {
                var tempAgencia = new Agencia
                {
                    Id = utl.CodificaId(int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0")),
                    Rota = row["rota"],
                    NAgencia = row["agencia"],
                    Cidade = row["cidade"],
                    Estado = row["estado"],
                    Km = row["km"]
                };
                dados.Add(tempAgencia);
            }
            return dados.Count;
        }
    }
}