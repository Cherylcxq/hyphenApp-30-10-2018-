using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using hyphenApp.Models;
using hyphenApp.ViewModels;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace hyphenApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RedeemDetailPage : ContentPage
    {
        static string statusStr = "";
        RedeemDetailViewModel viewModel;

        public RedeemDetailPage(RedeemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public RedeemDetailPage()
        {
            InitializeComponent();

            var item = new Redemption
            {
                Name = "Item 1",
                Description = "This is an item description."
            };

            viewModel = new RedeemDetailViewModel(item);
            BindingContext = viewModel;
            UpdatePoints();
            int redeempoints = Convert.ToInt32(lbPoints.Text);
            int balancepoint = Convert.ToInt32(App.Current.Properties["UserPoints"]);

            if (balancepoint >= redeempoints)
            {
                btnRedeem.IsVisible = true;
            }
            else
            {
                btnRedeem.IsVisible = false;
            }

            btnRedeem.Clicked += async (object sender, EventArgs e) =>
            {
                string email = Task.Run(() => BLL.GetUserEmailID()).Result;
                statusStr = await Task.Run(() => UserRedemptionInServer(email, lbProductID.Text));
                if (statusStr.Trim() == "true")
                {
                    UpdatePoints();
                    await Navigation.PopModalAsync(false);
                }
            };
        }

        public void UpdatePoints()
        {
            string email = Task.Run(() => BLL.GetUserEmailID()).Result;
            string statusStr = Task.Run(() => GetPointsFromServer(email)).Result;
            App.Current.Properties["UserPoints"] = statusStr.Trim();
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

        public string UserRedemptionInServer(string email, string productid)
        {
            string strData = "";
            byte[] fileContents = System.Convert.FromBase64String(strData);

            Uri webService = new Uri(@"http://hdx.azurewebsites.net/doTransaction");
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, webService);
            requestMessage.Headers.ExpectContinue = false;

            string boundary = "---HyphenBoundary";

            MultipartFormDataContent multiPartContent = new MultipartFormDataContent(boundary);
            HttpContent byteArrayContent = new ByteArrayContent(fileContents);

            byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("binary/octet-stream");
            byteArrayContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                //   Name = "\"media[file]\"",
                FileName = "image.jpg"
            };

            multiPartContent.Add(byteArrayContent);
            multiPartContent.Headers.Add("email", email);
            multiPartContent.Headers.Add("productid", productid);

            requestMessage.Content = multiPartContent;

            HttpClient httpClient = new HttpClient();
            try
            {
                //   httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("binary/octet-stream"));
                Task<HttpResponseMessage> httpRequest = httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
                HttpResponseMessage httpResponse = httpRequest.Result;
                HttpStatusCode statusCode = httpResponse.StatusCode;
                HttpContent responseContent = httpResponse.Content;

                if (responseContent != null)
                {
                    Task<String> stringContentsTask = responseContent.ReadAsStringAsync();
                    String stringContents = stringContentsTask.Result;
                    int index = stringContents.IndexOf('<');
                    statusStr = stringContents.Substring(0, index - 1);

                    //PopOutAlert(statusStr);

                    //statusStr = stringContents;
                    Debug.WriteLine(stringContents);

                    return statusStr;
                }
            }
            catch (Exception ex)
            {
                //   Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}