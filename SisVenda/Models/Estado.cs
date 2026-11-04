namespace SisVenda.Models
{
    public class Estado
    {
        public string? Sigla { get; set; }
        public string? Nome { get; set; }
        public List<string> Cidades { get; set; } = new List<string>();
    }

    public class EstadosCidades
    {
        public List<Estado> Estados { get; set; } = new List<Estado>();
    }
}
