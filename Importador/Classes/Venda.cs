using System;
using System.Collections.Generic;

namespace Importador.Classes
{
    public class Venda
    {
        public Venda(int id, List<Item> listaItens, string nomeVendedor)
        {
            Id = id;
            Itens = listaItens;
            NomeVendedor = nomeVendedor;
        }

        public int Id { get; set; }
        public List<Item> Itens { get; set; }
        public string NomeVendedor { get; set; }
    }
}
