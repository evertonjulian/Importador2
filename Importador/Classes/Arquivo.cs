using Importador.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importador.Classes
{
    public class Arquivo
    {
        StreamWriter writer;

        public void VerificarDiretorio()
        {
            var arquivos = Directory.GetFiles(@"C:\data\in", "*.dat");

            if (arquivos.Length > 0)
            {
                ExecutarLeituraArquivos(arquivos);
            }
        }

        protected void ExecutarLeituraArquivos(string[] arquivos)
        {
            foreach (var arquivo in arquivos)
            {
                if (File.Exists(arquivo))
                {
                    var nomeArquivo = Path.GetFileNameWithoutExtension(arquivo);
                    writer = new StreamWriter(@"C:\data\out\" + nomeArquivo + ".done.dat");

                    var conteudoArquivo = LerArquivo(arquivo);

                    if (conteudoArquivo.Any())
                    {
                        ProcessarArquivo(conteudoArquivo);
                    }

                    File.Move(arquivo, @"C:\data\read\" + Path.GetFileName(arquivo));

                    writer.Flush();
                    writer.Close();
                }
            }
        }

        protected List<string> LerArquivo(string arquivo)
        {
            List<string> conteudoArquivo = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(arquivo))
                {
                    String linha;

                    while ((linha = sr.ReadLine()) != null)
                    {
                        conteudoArquivo.Add(linha);
                    }
                }
            }
            catch (Exception ex)
            {
                writer.WriteLine("Ocorreu um erro durante a leitura do arquivo " +
                                 arquivo + ": " + ex.Message);
            }

            return conteudoArquivo;
        }

        protected void ProcessarArquivo(List<string> listaDados)
        {
            List<Vendedor> vendedores = new List<Vendedor>();
            List<Cliente> clientes = new List<Cliente>();
            List<Venda> vendas = new List<Venda>();

            foreach (var linha in listaDados)
            {
                var dados = linha.Split('|');

                if (dados.Length == 4)
                {
                    var tipoRegistro = (Enums.TipoRegistro)Convert.ToInt32(dados[0]);

                    switch (tipoRegistro)
                    {
                        case Enums.TipoRegistro.Vendedor:

                            var novoVendedor = new Vendedor
                            {
                                Cpf = dados[1],
                                Nome = dados[2],
                                Salario = Convert.ToDecimal(dados[3])
                            };

                            vendedores.Add(novoVendedor);
                            break;

                        case Enums.TipoRegistro.Cliente:

                            var novoCliente = new Cliente
                            {
                                Cnpj = dados[1],
                                Nome = dados[2],
                                AreaNegocios = dados[3]
                            };

                            clientes.Add(novoCliente);
                            break;

                        case Enums.TipoRegistro.Venda:

                            var novaVenda = new Venda
                            {
                                Id = Convert.ToInt32(dados[1]),
                                Itens = new List<Item>(),
                                NomeVendedor = dados[3]
                            };

                            var listaItens = dados[2].Remove(0, 1).Remove(dados[2].Length - 2, 1).Split(',');

                            foreach (var item in listaItens)
                            {
                                var dadosItem = item.Split('-');

                                var novoItem = new Item
                                {
                                    Id = Convert.ToInt32(dadosItem[0]),
                                    Quantidade = Convert.ToInt32(dadosItem[1]),
                                    Preco = Decimal.Parse(dadosItem[2].Replace('.', ','))
                                };

                                novaVenda.Itens.Add(novoItem);
                            }

                            vendas.Add(novaVenda);
                            break;

                        default:
                            break;
                    }
                }
            }

            var pior = vendas.GroupBy(x => x.NomeVendedor)
                                     .Select(x => new
                                     {
                                         x.Key,
                                         valorVenda = x.SelectMany(y => y.Itens).Sum(i => i.Preco * i.Quantidade)
                                     }).ToList()
                                     .OrderBy(x => x.valorVenda)
                                     .FirstOrDefault();

            var maiorVenda = vendas.OrderByDescending(x => x.Itens.Sum(i => i.Preco * i.Quantidade))
                                   .FirstOrDefault();

            writer.WriteLine("Quatidade total de clientes no arquivo: " + clientes.Count);
            writer.WriteLine("Quatidade total de vendedores no arquivo: " + vendedores.Count);
            writer.WriteLine("A venda mais cara é a de Id número " + maiorVenda.Id);
            writer.WriteLine("O pior vendedor é " + pior.Key);
        }
        
    }
}
