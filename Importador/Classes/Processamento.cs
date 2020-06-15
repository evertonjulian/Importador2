using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Importador.Classes
{
    public class Processamento
    {
        public Processamento(List<Cliente> clientes, List<Venda> vendas, List<Vendedor> vendedores)
        {
            Clientes = clientes;
            Vendas = vendas;
            Vendedores = vendedores;
        }

        List<Cliente> Clientes;
        List<Venda> Vendas;
        List<Vendedor> Vendedores;

        int IdMaiorVenda 
        {
            get
            {
                if (Vendas.Any())
                {
                    return Vendas.OrderByDescending(x => x.Itens.Sum(i => i.Preco * i.Quantidade))
                                 .FirstOrDefault().Id;
                }

                return -1;
            } 
        }

        string PiorVendedor 
        {
            get {

                if (Vendas.Any())
                {
                    return Vendas.GroupBy(x => x.NomeVendedor)
                                 .Select(x => new
                                  {
                                     x.Key,
                                     valorVenda = x.SelectMany(y => y.Itens).Sum(i => i.Preco * i.Quantidade)
                                  }).ToList()
                                 .OrderBy(x => x.valorVenda)
                                 .FirstOrDefault().Key;
                }

                return string.Empty;
            }             
        }

        public void EscreverResultado(string caminhoSaida)
        {
            var writer = new StreamWriter(caminhoSaida);
            writer.WriteLine("Quatidade de clientes no arquivo: " + Clientes.Count);
            writer.WriteLine("Quatidade de vendedores no arquivo: " + Vendedores.Count);
            writer.WriteLine("A venda mais cara é a de Id número " + IdMaiorVenda);
            writer.WriteLine("O pior vendedor é " + PiorVendedor);

            writer.Flush();
            writer.Close();
        }
    }
}
