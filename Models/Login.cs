using Gerencial.Repositorio;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Gerencial.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Favor preencher o Nome de Usuário")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Favor preencher a senha")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        public int Id { get; set; }
        public string IsAdmin { get; set; }
        public string Empresa { get; set; }

        private readonly Conexao contexto;

        public Login()
        {
            contexto = new Conexao();
        }

        public List<Login> Logar(Login log)
        {
            var dados = new List<Login>();
            string strQuery = "Select id, nome, isAdmin, empresa FROM usuarios";
            strQuery += " WHERE upper(login) = '" + log.Usuario.ToUpper() + "' AND senha = '" + RetornaMD5(log.Senha) + "'";
            var rows = contexto.ExecutaComandoComRetorno(strQuery, null);
            foreach (var row in rows)
            {
                var tempPessoa = new Login
                {
                    Id = int.Parse(!string.IsNullOrEmpty(row["id"]) ? row["id"] : "0"),
                    Usuario = row["nome"],
                    IsAdmin = row["isAdmin"],
                    Empresa = row["empresa"]
                };
                dados.Add(tempPessoa);
            }
            return dados;
        }

        private string RetornaMD5(string input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}