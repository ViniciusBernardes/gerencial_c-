using System;

namespace Gerencial.biblioteca
{
    public class Util
    {
        public string Rota(string Regiao)
        {
            var Retorno = "";
            switch (Regiao.ToUpper())
            {
                case "NORTE":
                    Retorno = "Norte de Minas";
                    break;
                case "GRANDE BH":
                    Retorno = "Grande BH";
                    break;
                case "TRIANGULO MINEIRO":
                    Retorno = "Triângulo Mineiro";
                    break;
                case "ESPIRITO SANTO":
                    Retorno = "Espírito Santo";
                    break;
                default:
                    Retorno = "Agencia sem Rota";
                    break;
            }
            return Retorno;
        }

        public string Engenheiro(string eng)
        {
            var retorno = string.Empty;
            switch (eng)
            {
                case "1":
                    retorno = "Anselmo Franklin";
                    break;
                case "2":
                    retorno = "Vanessa Mara";
                    break;
                case "3":
                    retorno = "Alexandre Machado";
                    break;
                case "4":
                    retorno = "Alessandro Bulhoes";
                    break;
                default:
                    retorno = " - ";
                    break;
            }
            return retorno;
        }

        public string CodificaId(int Id)
        {
            string retorno = string.Empty;
            string n1 = string.Empty;
            string n2 = string.Empty;
            Random rnd = new Random();
            n1 = rnd.Next(100, 200).ToString();
            n2 = rnd.Next(1000, 2000).ToString();
            retorno = n1 + Id.ToString() + n2;
            return retorno;
        }

        public string DecodificaId(string id)
        {
            if (id.Length > 7)
            {
                var ret1 = id.Substring(3, id.Length - 3);
                //int n = id.Length - 4;
                var ret2 = ret1.Substring(0, ret1.Length - 4);
                return ret2;
            }
            else
                return "0";
        }
    }
}