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
	public partial class RewardMenuPage : ContentPage
	{
        List<dTips> pList = new List<dTips>();
        List<dPoints> pointList = new List<dPoints>();
        static string statusStr = "";

        int index = 0;
        public RewardMenuPage ()
		{
			InitializeComponent ();
            //try
            //{
            //    pList = Task.Run(() => GetTipsFromServer()).Result;
            //    LoadAndUpdatePage(true);
            //    lblTips.Text = pList[index].Tips;
            //    Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            //    {
            //        lblTips.Text = pList[index].Tips;
            //        LoadAndUpdatePage(false);

            //        if (index < pList.Count - 1)
            //        {
            //            index++;
            //        }
            //        else
            //        {
            //            index = 0;
            //        }
            //        return true; // True = Repeat again, False = Stop the timer
            //    });
            //}
            //catch
            //{ }
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        public void LoadAndUpdatePage(bool bGetPoints)
        {
            if (bGetPoints)
            {
                try
                {
                    string email = Task.Run(() => BLL.GetUserEmailID()).Result;
                    statusStr = Task.Run(() => GetPointsFromServer(email)).Result;
                    App.Current.Properties["UserPoints"] = statusStr.Trim();
                }
                catch { }
            }
            //try
            //{
            //    if (App.Current.Properties["UserPoints"] != null)
            //    {
            //        lblPoints.Text = "Available points: " + App.Current.Properties["UserPoints"];
            //    }
            //}
            //catch
            //{ }
        }

        public static async Task<string> GetPointsFromServer(string email)
        {
            string strData = "";
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://hdx.azurewebsites.net/GetUser" + "?email=" + email + "&type=getpoints");
            //var data = await response.Content.ReadAsStringAsync();

            Task<String> stringContentsTask = response.Content.ReadAsStringAsync();
            String stringContents = stringContentsTask.Result;
            int index = stringContents.IndexOf('<');
            statusStr = strData = stringContents.Substring(0, index - 1);

            return strData;
        }

        async void ScanReceipt_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            await Navigation.PushModalAsync(new NavigationPage(new CameraPage()));
        }

        async Task Redemption_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            await Navigation.PushModalAsync(new NavigationPage(new RedeemPage()));
        }

        async Task RedeemHistory_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            await Navigation.PushModalAsync(new NavigationPage(new RedeemHistoryPage()));
        }

        async Task PointsHistory_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            await Navigation.PushModalAsync(new NavigationPage(new PointsHistoryPage()));
        }

        public static async Task<List<dTips>> GetTipsFromServer()
        {
            List<dTips> testlist2 = new List<dTips>();
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://hdx.azurewebsites.net/GetTips");
            var data = await response.Content.ReadAsStringAsync();

            string[] splitphase1 = data.ToString().Split('~');

            for (int i = 0; i < splitphase1.Length - 1; i++)
            {
                string testreader = splitphase1[i];
                testlist2.Add(new dTips { Tips = testreader });
            }

            return testlist2;
        }
    }
}