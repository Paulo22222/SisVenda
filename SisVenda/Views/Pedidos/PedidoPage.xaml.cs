using Microsoft.Maui.Controls;
using SisVenda.Services;

namespace SisVenda
{
    public partial class PedidoPage : ContentPage
    {
        public PedidoPage()
        {
            InitializeComponent();
            pedidosCollectionView.ItemsSource = PedidoRepository.Pedidos;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            pedidosCollectionView.ItemsSource = null;
            pedidosCollectionView.ItemsSource = PedidoRepository.Pedidos;
        }

        private async void ToolbarItem_Novo_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(PedidoFormPage));
        }

        private async void Editar_Clicked(object sender, EventArgs e)
        {
            if (sender is Button b && b.CommandParameter is int id)
            {
                await Shell.Current.GoToAsync($"{nameof(PedidoFormPage)}?id={id}");
            }
        }

        private async void Excluir_Clicked(object sender, EventArgs e)
        {
            if (sender is Button b && b.CommandParameter is int id)
            {
                var pedido = PedidoRepository.GetById(id);
                if (pedido is null) return;
                var ok = await DisplayAlert("Confirmar", $"Excluir pedido {pedido.Id}?", "Sim", "Não");
                if (!ok) return;
                PedidoRepository.Remove(pedido);
            }
        }
    }
}
