using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importador.Classes
{
    public class Cliente : Pessoa
    {
        public Cliente(string cnpj, string nome, string areaNegocios)
        {
            Cnpj = cnpj;
            Nome = nome;
            AreaNegocios = areaNegocios;
        }
        public string Cnpj { get; set; }

        public string AreaNegocios { get; set; }
    }
}
