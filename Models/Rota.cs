using Gerencial.Repositorio;
using System.Collections.Generic;

namespace Gerencial.Models
{
    public class Rota
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        private readonly Conexao contexto;
        public Rota()
        {
            contexto = new Conexao();
        }

        public List<Rota> Listar()
        {
            var dados = new List<Rota>();
            string strQuery = "Select id, nome from empresa order by nome";

            var rows = contexto.ExecutaComandoComRetorno(strQuery, null);
            foreach (var row in rows)
            {
                var tempRota = new Rota
                {
                    Id = int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0"),
                    Nome = row["nome"]
                };
                dados.Add(tempRota);
            }
            return dados;
        }
    }
}