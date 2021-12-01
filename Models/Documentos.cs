using Gerencial.biblioteca;
using Gerencial.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace Gerencial.Models
{
    public class Documentos
    {
        private readonly Conexao contexto;
        public Util utl;

        public int Id { get; set; }
        public string IdOrigem { get; set; }
        public string Pasta { get; set; }
        public string Tabela { get; set; }
        public string Nome { get; set; }


        public Documentos()
        {
            contexto = new Conexao();
            utl = new Util();
        }

        public int Cadastrar(Documentos doc)
        {
            const string commandText = " INSERT INTO documento (id_origem, pasta, tabela, nome) VALUES (@Id, @Pasta, @Tabela, @Nome) ";
            var parameters = new Dictionary<string, object>
            {
                {"Id", doc.IdOrigem},
                {"Pasta", doc.Pasta},
                {"Tabela", doc.Tabela},
                {"Nome", doc.Nome}
            };

            return contexto.ExecutaComando(commandText, parameters);
        }

        public Documentos UltimoId()
        {
            var dados = new List<Documentos>();
            const string strQuery = "SELECT id FROM documento WHERE pasta = 'chamado' order by id DESC limit 1";

            var rows = contexto.ExecutaComandoComRetorno(strQuery, null);
            foreach (var row in rows)
            {
                var tempDocumento = new Documentos
                {
                    Id = int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0")
                };
                dados.Add(tempDocumento);
            }
            return dados.FirstOrDefault();
        }
    }
}