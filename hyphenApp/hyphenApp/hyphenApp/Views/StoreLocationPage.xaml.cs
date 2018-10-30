using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace hyphenApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StoreLocationPage : ContentPage
    {
        static string latStr = "";
        static string longStr = "";

        List<StoreLocation> location = new List<StoreLocation>();

        public StoreLocationPage()
        {
            InitializeComponent();
            refresh();
        }

        void Refresh_Clicked(object sender, EventArgs e)
        {
            refresh();
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void refresh()
        {
            try
            {
                string email = Task.Run(() => BLL.GetUserEmailID()).Result;

                if (App.Current.Properties["Latitude"].ToString() != null && App.Current.Properties["Longitude"].ToString() != null)
                {
                    string latStr = App.Current.Properties["Latitude"].ToString();
                    string longStr = App.Current.Properties["Longitude"].ToString();

                    location = Task.Run(() => DownloadString(email, latStr, longStr)).Result;

                    initLocation();
                }
            }
            catch { }
        }

            private void initLocation()
        {
            storeLocation.ItemsSource = location;

            storeLocation.ItemSelected += (sender, args) =>
            {
                string gps = ((StoreLocation)storeLocation.SelectedItem).GPS;

                string urlStr = "http://maps.google.com/?saddr=" + latStr + "," + longStr + "&daddr=" + gps;

                Device.OpenUri(new Uri(urlStr));
            };
        }

        public static async Task<List<StoreLocation>> DownloadString(string email, string latStr, string longStr)
        {
            List<StoreLocation> testlist2 = new List<StoreLocation>();
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://hdx.azurewebsites.net/GetStoreLocation?email=" + email + "&lattitude=" + latStr + "&longtitude=" + longStr);
            var data = await response.Content.ReadAsStringAsync();

            string[] splitphase1 = data.ToString().Split('^');

            for (int i = 0; i < splitphase1.Length - 1; i++)
            {
                string testreader = splitphase1[0];
                string[] splitphase2 = splitphase1[i].Split('~');
                testlist2.Add(new StoreLocation { GPS = splitphase2[0], Address = splitphase2[1], Name = splitphase2[2], Distance = "Approximately " + splitphase2[3] + " KM", Source = "http://hdx.azurewebsites.net/GetProducts?productid=" + splitphase2[0] });
            }

            return testlist2;
        }
    }
}