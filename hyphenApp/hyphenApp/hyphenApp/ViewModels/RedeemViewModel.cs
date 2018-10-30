using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using hyphenApp.Models;
using hyphenApp.Views;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace hyphenApp.ViewModels
{
    public class RedeemViewModel : BaseViewModel
    {
        public ObservableCollection<Redemption> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        List<Redemption> redemption = new List<Redemption>();

        List<dProduct> pList = new List<dProduct>();

        public RedeemViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Redemption>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            pList = Task.Run(() => DownloadString()).Result;

            for (int i = 0; i < pList.Count(); i++)
            {
                //productArrayList.Add(pList[i]);
                redemption.Add(new Redemption
                {
                    Name = pList[i].Name,
                    ProductID = pList[i].ProductID,
                    Selected = false,
                    Description = pList[i].Description,
                    Points = pList[i].Points + " points",
                    Source = pList[i].Source
                });

            }

            //MessagingCenter.Subscribe<NewItemPage, Item>(this, "Redemption", async (obj, item) =>
            //{
            //    var newItem = item as Redemption;
            //    Items.Add(newItem);
            //    await DataStore.AddItemAsync(newItem);
            //});
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in redemption)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public static async Task<List<dProduct>> DownloadString()
        {
            List<dProduct> testlist2 = new List<dProduct>();
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://hdx.azurewebsites.net/GetProducts");
            var data = await response.Content.ReadAsStringAsync();

            string[] splitphase1 = data.ToString().Split('^');

            for (int i = 0; i < splitphase1.Length - 1; i++)
            {
                string testreader = splitphase1[0];
                string[] splitphase2 = splitphase1[i].Split('~');
                testlist2.Add(new dProduct { ProductID = splitphase2[0], Name = splitphase2[1], Description = splitphase2[2], Points = splitphase2[3], Source = "http://hdx.azurewebsites.net/GetProducts?productid=" + splitphase2[0] });
            }

            return testlist2;
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
            strData = stringContents.Substring(0, index - 1);

            return strData;
        }
    }
}