using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MinhasCompras.Models;

namespace MinhasCompras.Views;


public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
	public ListaProduto()
	{
		InitializeComponent();
		lst_produtos.ItemsSource = lista;
	}

    protected async override void OnAppearing()
    {
		try
		{
			List<Produto> tmp = await App.Db.GetAll();
			tmp.ForEach(i => lista.Add(i));
		}
		catch (Exception ex) 
		{
			await DisplayAlert("Ops", ex.Message, "OK!");
		}
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try 
		{
			Navigation.PushAsync(new Views.NovoProduto());
		}
		catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "Ok");
		}
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
		try
		{
			string q = e.NewTextValue;
			lista.Clear();
			List<Produto> tmp = await App.Db.Search(q);
			tmp.ForEach(i => lista.Add(i));
		}
		catch (Exception ex) 
		{
			await DisplayAlert("Ops", ex.Message, "OK!");
		}
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
		try
		{
			double soma = lista.Sum(i => i.Total);
			string msg = $"O total é {soma:C}";
			DisplayAlert("Total dos produtos", msg, "Perfeito!");
		}
		catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "OK!");
		}
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    { // Remover produto com menu.
		try
		{
			MenuItem selecionado = sender as MenuItem;
			Produto p = selecionado.BindingContext as Produto;
			bool confirm = await DisplayAlert("Quer mesmo fazer isso bro?", $"Remover {p.Descricao}?", "Claro mano!", "Vish... Melhor n");

			if (confirm)
			{
				await App.Db.Delete(p.Id);
				lista.Remove(p);
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK!");
		}
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    { // Seleciona item para poder Editar na pagina Editar produto.
		try
		{
			Produto p = e.SelectedItem as Produto;
			Navigation.PushAsync(new Views.EditarProduto
			{
				BindingContext = p,
			});
		}
		catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "OK!");
		}
    }
}