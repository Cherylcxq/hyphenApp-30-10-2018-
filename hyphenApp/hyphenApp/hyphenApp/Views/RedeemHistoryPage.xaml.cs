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
    public partial class RedeemHistoryPage : ContentPage
    {
        List<UserRedemption> pList = new List<UserRedemption>();

        public RedeemHistoryPage()
        {
            InitializeComponent();
            string email = Task.Run(() => BLL.GetUserEmailID()).Result;

            pList = Task.Run(() => DownloadString(email)).Result;

            redeemHistory.ItemsSource = pList;
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        public static async Task<List<UserRedemption>> DownloadString(string email)
        {
            List<UserRedemption> testlist2 = new List<UserRedemption>();
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://hdx.azurewebsites.net/GetUser" + "?email=" + email + "&type=gethistoryredemption");
            var data = await response.Content.ReadAsStringAsync();

            string[] splitphase1 = data.ToString().Split('^');

            for (int i = 0; i < splitphase1.Length - 1; i++)
            {
                string testreader = splitphase1[0];
                string[] splitphase2 = splitphase1[i].Split('~');
                testlist2.Add(new UserRedemption { points = splitphase2[0] + " points", status = splitphase2[1], submitdate = splitphase2[2], deliverydate = splitphase2[3], productname = splitphase2[4].ToUpper() });
            }

            return testlist2;
        }
    }
}

