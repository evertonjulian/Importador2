namespace Importador.Classes
{
    public class Item
    {
        public Item(int id, int quantidade, decimal preco)
        {
            Id = id;
            Quantidade = quantidade;
            Preco = preco;
        }

        public int Id { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
    }
}
