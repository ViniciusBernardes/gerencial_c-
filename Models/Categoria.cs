using Gerencial.biblioteca;
using Gerencial.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace Gerencial.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string SlaAtendimento { get; set; }
        public string SlaConclusao { get; set; }
        private readonly Conexao contexto;
        public List<Categoria> MCategoria { get; set; }
        public Util utl { get; set; }

        public Categoria()
        {
            contexto = new Conexao();
            utl = new Util();
        }

        public List<Categoria> Listar()
        {
            var dados = new List<Categoria>();
            string strQuery = "Select id, nome from categoria_gasto order by nome";

            var rows = contexto.ExecutaComandoComRetorno(strQuery, null);
            foreach (var row in rows)
            {
                var tempCategoria = new Categoria
                {
                    Id = int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0"),
                    Nome = row["nome"]
                };
                dados.Add(tempCategoria);
            }
            return dados;
        }

        public Categoria ListaId(string id)
        {
            var dados = new List<Categoria>();
            const string strQuery = "SELECT id, nome FROM categoria_gasto WHERE id = @Id";
            var parametros = new Dictionary<string, object>
            {
                {"Id", utl.DecodificaId(id)}
            };
            var rows = contexto.ExecutaComandoComRetorno(strQuery, parametros);
            foreach (var row in rows)
            {
                var tempAgencia = new Categoria
                {
                    Id = int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0"),
                    Nome = row["nome"]
                };
                dados.Add(tempAgencia);
            }
            return dados.FirstOrDefault();
        }

        public List<Categoria> ListarChamado()
        {
            var dados = new List<Categoria>();
            string strQuery = "Select id, nome from categoria order by nome";

            var rows = contexto.ExecutaComandoComRetorno(strQuery, null);
            foreach (var row in rows)
            {
                var tempCategoria = new Categoria
                {
                    Id = int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0"),
                    Nome = row["nome"]
                };
                dados.Add(tempCategoria);
            }
            return dados;
        }

        public List<Categoria> ListarSubCategoriaChamado(string cat)
        {
            var dados = new List<Categoria>();
            string strQuery = "Select id, nome, sla_atendimento, sla_conclusao from subcategoria where categoria = '" + cat + "' order by nome";

            var rows = contexto.ExecutaComandoComRetorno(strQuery, null);
            foreach (var row in rows)
            {
                var tempCategoria = new Categoria
                {
                    Id = int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0"),
                    Nome = row["nome"],
                    SlaAtendimento = row["sla_atendimento"],
                    SlaConclusao = row["sla_conclusao"]
                };
                dados.Add(tempCategoria);
            }
            return dados;
        }

        public int Cadastrar(Categoria cat)
        {
            const string commandText = " INSERT INTO categoria_gasto (nome) VALUES (@Nome) ";
           
            var parameters = new Dictionary<string, object>
            {
                {"Nome", cat.Nome},
            };

            return contexto.ExecutaComando(commandText, parameters);
        }

        public int Alterar(Categoria cat)
        {
            var commandText = " UPDATE categoria_gasto SET ";
            commandText += " nome = @Nome ";
            commandText += " WHERE id = @Id";

            var parameters = new Dictionary<string, object>
            {
                {"Id", cat.Id},
                {"Nome", cat.Nome}
            };

            return contexto.ExecutaComando(commandText, parameters);
        }
    }
}