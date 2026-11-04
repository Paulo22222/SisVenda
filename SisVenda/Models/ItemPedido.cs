namespace SisVenda.Models
{
    public class ItemPedido
    {
        public int ProdutoId { get; set; }
        public string? ProdutoNome { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }

        public decimal Subtotal => Quantidade * ValorUnitario;

        public string ValorUnitarioFormatado => ValorUnitario.ToString("C2");
        public string SubtotalFormatado => Subtotal.ToString("C2");
    }
}
