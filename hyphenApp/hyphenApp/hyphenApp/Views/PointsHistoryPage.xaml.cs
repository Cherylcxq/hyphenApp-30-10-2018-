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
	public partial class PointsHistoryPage : ContentPage
	{
        List<UserPoints> pList = new List<UserPoints>();

        public PointsHistoryPage()
        {
            InitializeComponent();
            string email = Task.Run(() => BLL.GetUserEmailID()).Result;

            pList = Task.Run(() => DownloadString(email)).Result;

            pointsHistory.ItemsSource = pList;
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        public static async Task<List<UserPoints>> DownloadString(string email)
        {
            List<UserPoints> testlist2 = new List<UserPoints>();
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://hdx.azurewebsites.net/GetUser" + "?email=" + email + "&type=gethistorypoints");
            var data = await response.Content.ReadAsStringAsync();

            string[] splitphase1 = data.ToString().Split('^');

            for (int i = 0; i < splitphase1.Length - 1; i++)
            {
                string testreader = splitphase1[0];
                string[] splitphase2 = splitphase1[i].Split('~');
                testlist2.Add(new UserPoints { Points = splitphase2[0] + " points", Amount = splitphase2[1], Date = splitphase2[2], Company = splitphase2[3] });
            }

            return testlist2;
            //var url = @"hdx.azurewebsites.net/GetProducts";
        }
    }
}