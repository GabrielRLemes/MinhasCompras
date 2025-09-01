using System.Collections.ObjectModel;
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
        List<Produto> tmp = await App.Db.GetAll();
		tmp.ForEach(i => lista.Add(i));
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
		string q = e.NewTextValue;
		lista.Clear();
        List<Produto> tmp = await App.Db.Search(q);
        tmp.ForEach(i => lista.Add(i));
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
		double soma = lista.Sum(i => i.Total);
		string msg = $"O total é {soma:C}";
		DisplayAlert("Total dos produtos", msg, "Perfeito!");
    }

    private void MenuItem_Clicked(object sender, EventArgs e)
    {

    }
}