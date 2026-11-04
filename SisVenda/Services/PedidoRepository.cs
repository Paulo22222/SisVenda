using System.Collections.ObjectModel;
using System.Linq;
using SisVenda.Models;

namespace SisVenda.Services
{
    public static class PedidoRepository
    {
        public static ObservableCollection<Pedido> Pedidos { get; } = new ObservableCollection<Pedido>();

        public static Pedido? GetById(int id) => Pedidos.FirstOrDefault(p => p.Id == id);

        public static void Add(Pedido p)
        {
            Pedidos.Add(p);
        }

        public static void Remove(Pedido p)
        {
            Pedidos.Remove(p);
        }
    }
}
