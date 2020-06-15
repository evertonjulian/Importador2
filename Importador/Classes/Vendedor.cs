using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importador.Classes
{
    public class Vendedor : Pessoa
    {
        public Vendedor(string cpf, string nome, decimal salario)
        {
            Cpf = cpf;
            Nome = nome;
            Salario = salario;
        }

        public string Cpf { get; set; }

        public decimal Salario { get; set; }

    }
}
