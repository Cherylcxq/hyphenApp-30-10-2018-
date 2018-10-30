using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using hyphenApp.Models;
using hyphenApp.Views;
using hyphenApp.ViewModels;

namespace hyphenApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RedeemPage : ContentPage
    {
        RedeemViewModel viewModel;

        public RedeemPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new RedeemViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Redemption;
            if (item == null)
                return;

            await Navigation.PushAsync(new RedeemDetailPage(new RedeemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}