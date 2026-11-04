using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.Maui.Controls;
using SisVenda.Models;
using SisVenda.Services;

namespace SisVenda
{
    [QueryProperty(nameof(OrderId), "id")]
    public partial class PedidoFormPage : ContentPage
    {
        int orderId = 0;
        readonly ObservableCollection<ItemPedido> itens = new ObservableCollection<ItemPedido>();

        public string OrderId
        {
            get => orderId.ToString();
            set
            {
                if (int.TryParse(value, out var id))
                {
                    orderId = id;
                }
            }
        }

        public PedidoFormPage()
        {
            InitializeComponent();
            clientePicker.ItemsSource = ClienteRepository.Clientes;
            produtoPicker.ItemsSource = ProdutoRepository.Produtos;
            itensCollectionView.ItemsSource = itens;
            itens.CollectionChanged += (s, e) => AtualizarTotal();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (orderId > 0)
            {
                var p = PedidoRepository.GetById(orderId);
                if (p != null)
                {
                    PopulateForm(p);
                }
            }
            else
            {
                ClearForm();
            }
        }

        private void ProdutoPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (produtoPicker.SelectedItem is Produto produto)
            {
                valorEntry.Text = produto.Preco.ToString("F2", new CultureInfo("pt-BR"));
            }
        }

        private async void AdicionarItem_Clicked(object sender, EventArgs e)
        {
            if (produtoPicker.SelectedItem is not Produto produto)
            {
                await DisplayAlert("Validação", "Selecione um produto.", "OK");
                return;
            }

            if (!int.TryParse(quantidadeEntry.Text, out var quantidade) || quantidade <= 0)
            {
                await DisplayAlert("Validação", "Quantidade inválida.", "OK");
                return;
            }

            if (!TryParseValor(valorEntry.Text, out var valor) || valor <= 0)
            {
                await DisplayAlert("Validação", "Valor unitário inválido.", "OK");
                return;
            }

            itens.Add(new ItemPedido
            {
                ProdutoId = produto.Id,
                ProdutoNome = produto.Nome,
                Quantidade = quantidade,
                ValorUnitario = valor
            });

            produtoPicker.SelectedIndex = -1;
            quantidadeEntry.Text = string.Empty;
            valorEntry.Text = string.Empty;
        }

        private void RemoverItem_Clicked(object sender, EventArgs e)
        {
            if (sender is Button b && b.CommandParameter is ItemPedido item)
            {
                itens.Remove(item);
            }
        }

        private async void Salvar_Clicked(object sender, EventArgs e)
        {
            if (clientePicker.SelectedItem is not Cliente cliente)
            {
                await DisplayAlert("Validação", "Cliente é obrigatório.", "OK");
                return;
            }

            if (itens.Count == 0)
            {
                await DisplayAlert("Validação", "Inclua pelo menos um item.", "OK");
                return;
            }

            var observacao = observacaoEditor.Text?.Trim();

            if (orderId > 0)
            {
                var p = PedidoRepository.GetById(orderId);
                if (p != null)
                {
                    p.Cliente = cliente;
                    p.Data = dataPicker.Date;
                    p.Observacao = observacao;
                    p.Itens = itens.ToList();
                }
            }
            else
            {
                var novo = new Pedido
                {
                    Id = PedidoRepository.Pedidos.Any() ? PedidoRepository.Pedidos.Max(x => x.Id) + 1 : 1,
                    Cliente = cliente,
                    Data = dataPicker.Date,
                    Observacao = observacao,
                    Itens = itens.ToList()
                };
                PedidoRepository.Add(novo);
            }

            await Shell.Current.GoToAsync("..");
        }

        private async void Cancelar_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        static bool TryParseValor(string? texto, out decimal valor)
        {
            if (decimal.TryParse(texto, NumberStyles.Number, new CultureInfo("pt-BR"), out valor))
            {
                return true;
            }

            return decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out valor);
        }

        void AtualizarTotal()
        {
            totalLabel.Text = $"Total: {itens.Sum(i => i.Subtotal).ToString("C2")}";
        }

        void PopulateForm(Pedido p)
        {
            clientePicker.SelectedItem = p.Cliente;
            dataPicker.Date = p.Data;
            observacaoEditor.Text = p.Observacao;
            itens.Clear();
            foreach (var i in p.Itens)
            {
                itens.Add(new ItemPedido
                {
                    ProdutoId = i.ProdutoId,
                    ProdutoNome = i.ProdutoNome,
                    Quantidade = i.Quantidade,
                    ValorUnitario = i.ValorUnitario
                });
            }
        }

        void ClearForm()
        {
            clientePicker.SelectedIndex = -1;
            dataPicker.Date = DateTime.Today;
            produtoPicker.SelectedIndex = -1;
            quantidadeEntry.Text = string.Empty;
            valorEntry.Text = string.Empty;
            observacaoEditor.Text = string.Empty;
            itens.Clear();
        }
    }
}
