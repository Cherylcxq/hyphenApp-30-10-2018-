using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace hyphenApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingPage : ContentPage
	{
        public SettingPage()
        {
            InitializeComponent();
            try
            {
                LoadAndUpdatePage(true);
            }
            catch
            { }
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        public void LoadAndUpdatePage(bool bGetPoints)
        {
           
        }

        async void AddMember_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            await Navigation.PushModalAsync(new NavigationPage(new PatientPage(App.CurrentUserID)));
        }

        async Task ChangeLanguage_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            await Navigation.PushModalAsync(new NavigationPage(new ChangeLanguagePage()));
        }

        async Task About_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            await Navigation.PushModalAsync(new NavigationPage(new AboutPage()));
        }

        //async Task PointsHistory_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        //{
        //    await Navigation.PushModalAsync(new NavigationPage(new PointsHistoryPage()));
        //}

    }
}