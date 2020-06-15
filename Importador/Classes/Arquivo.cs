using Importador.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Importador.Classes
{
    public static class Arquivo
    {
        static readonly char separador = Convert.ToChar(ConfigurationManager.AppSettings["Separador"]);

        public static List<string> LerArquivo(string arquivo)
        {
            List<string> conteudoArquivo = new List<string>();

            if (File.Exists(arquivo))
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

            return conteudoArquivo;
        }

        public static Processamento ProcessarArquivo(List<string> conteudoArquivo)
        {
            List<Cliente> clientes = new List<Cliente>();
            List<Venda> vendas = new List<Venda>();
            List<Vendedor> vendedores = new List<Vendedor>();

            foreach (var linha in conteudoArquivo)
            {
                var tipoDado = (Enums.TipoDados)Convert.ToInt32(linha.Substring(0, linha.IndexOf(separador)));
                
                switch (tipoDado)
                {
                    case Enums.TipoDados.Vendedor:
                        
                        vendedores.Add(ExtrairDadosVendedor(linha));
                        break;

                    case Enums.TipoDados.Cliente:
                            
                        clientes.Add(ExtrairDadosCliente(linha));
                        break;

                    case Enums.TipoDados.Venda:
                        
                        vendas.Add(ExtrairDadosVenda(linha));
                        break;

                    default:
                        break;
                }
                
            }

            return new Processamento(clientes, vendas, vendedores);
        }
        
        private static Vendedor ExtrairDadosVendedor(string dados)
        {
            var cpf = Regex.Match(dados, $"{separador}(\\d*){separador}").Groups[1].Value;
            var nome = Regex.Match(dados, $"\\d{separador}(\\D*){separador}\\d").Groups[1].Value;
            var salario = decimal.Parse(dados.Substring(dados.LastIndexOf(separador) + 1).Replace('.', ','));

            return new Vendedor(cpf, nome, salario);
        }

        private static Cliente ExtrairDadosCliente(string dados)
        {
            var cnpj = Regex.Match(dados, $"{separador}(\\d*){separador}").Groups[1].Value;
            var nome = Regex.Match(dados, $"\\d{separador}(\\D*){separador}\\d").Groups[1].Value;
            var areaNegocios = dados.Substring(dados.LastIndexOf(separador) + 1);

            return new Cliente(cnpj, nome, areaNegocios);
        }

        private static Venda ExtrairDadosVenda(string dados)
        {
            var idVenda = Convert.ToInt32(Regex.Match(dados, $"\\d{separador}(\\d+){separador}\\[").Groups[1].Value);
            var nomeVendedor = dados.Substring(dados.LastIndexOf($"]{separador}") + 1);

            var dadosArrayItens = Regex.Match(dados, $"\\d{separador}\\[(...+)\\]{separador}").Groups[1].Value;
            var listaItens = new List<Item>();

            foreach (var item in dadosArrayItens.Split(','))
            {
                var dadosItem = item.Split('-');

                var idItem = Convert.ToInt32(dadosItem[0]);
                var quantidade = Convert.ToInt32(dadosItem[1]);
                var preco = decimal.Parse(dadosItem[2].Replace('.', ','));

                listaItens.Add(new Item(idItem, quantidade, preco));
            }

            return new Venda(idVenda, listaItens, nomeVendedor);
        }

    }
}
