using Acr.UserDialogs;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace hyphenApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage, INotifyPropertyChanged
    {
        static string filename = "";

        static string statusStr = "";

        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }

            set
            {
                this.isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public CameraPage()
        {
            InitializeComponent();
            imgUpload.IsVisible = false;
        }
        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Scan_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                SaveToAlbum = true,
                Name = "test.jpg"
            });

            if (file == null)
                return;

            filename = file.Path;
            imgUpload.IsVisible = true;
            tbStatus.Text = "";

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }

        async void Pick_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });


            if (file == null)
                return;

            filename = file.Path;
            imgUpload.IsVisible = true;
            tbStatus.Text = "";

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }


        async void Upload_OnTapGestureRecognizerTappedAsync(object sender, EventArgs args)
        {
            using (UserDialogs.Instance.Loading("Processing...\n Please Wait", null, null, true, MaskType.Black))
            {
                statusStr = await Task.Run(() => SendReceiptToServer());

                imgUpload.IsVisible = true;

                IsLoading = false;
                if (statusStr.Contains("Error"))
                {
                    statusStr = "Your loyalty points will be updated after verification";
                    UserDialogs.Instance.Alert(statusStr, "Error", "Ok");
                    tbStatus.TextColor = Color.Red;
                    imgUpload.IsVisible = false;
                    image.Source = null;
                }
                else
                {
                    tbStatus.TextColor = Color.Green;
                    imgUpload.IsVisible = false;
                    image.Source = null;
                    UserDialogs.Instance.Alert(statusStr, "Success!", "Ok");
                }
                tbStatus.Text = statusStr;
            }

        }

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static byte[] ToByteArray(string value)
        {
            char[] charArr = value.ToCharArray();
            byte[] bytes = new byte[charArr.Length];
            for (int i = 0; i < charArr.Length; i++)
            {
                byte current = Convert.ToByte(charArr[i]);
                bytes[i] = current;
            }

            return bytes;
        }

        public static string XSendReceiptToServer()
        {
            string email = Task.Run(() => BLL.GetUserEmailID()).Result;

            return null;
        }

        public string SendReceiptToServer()
        {
            string email = Task.Run(() => BLL.GetUserEmailID()).Result;
            byte[] fileContents = File.ReadAllBytes(filename);

            Uri webService = new Uri(@"http://hdx.azurewebsites.net/UploadReceipt");
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
            multiPartContent.Headers.Add("FileName", "test.jpg");
            multiPartContent.Headers.Add("email", email);

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

        async void PopOutAlert(String e)
        {
            await App.Current.MainPage.DisplayAlert("", e, "OK");
        }

        public static async Task<string> GetPointsFromServer()
        {
            string email = Task.Run(() => BLL.GetUserEmailID()).Result;
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