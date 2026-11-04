namespace SisVenda.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public Cliente? Cliente { get; set; }
        public DateTime Data { get; set; }
        public string? Observacao { get; set; }
        public List<ItemPedido> Itens { get; set; } = new List<ItemPedido>();

        public decimal Total => Itens.Sum(i => i.Subtotal);

        public string ClienteNome => Cliente?.Nome ?? string.Empty;
        public string DataFormatada => Data.ToString("dd/MM/yyyy");
        public string TotalFormatado => Total.ToString("C2");
    }
}
