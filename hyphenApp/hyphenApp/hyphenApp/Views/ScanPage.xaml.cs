using Plugin.Media;
using System;
using System.Collections.Generic;
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
    public partial class ScanPage : ContentPage
    //public partial class ScanReceiptPage : ContentPage, INotifyPropertyChanged
    {
        List<string> oldPhotoFilePaths = new List<string>();

        static string filename = "";
        static string email = "";

        static string statusStr = "";

        bool firstLoad = true;

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

        //public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propName)
        {
            //if (PropertyChanged != null)
            //{
            //    PropertyChanged(this, new PropertyChangedEventArgs(propName));
            //}
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        
        public ScanPage()
        {
            InitializeComponent();
            IsLoading = false;
            //BindingContext = this;
            //imgScan.IsVisible = false;
            //bottomSendButtonBar.IsVisible = false;
            HeaderFrame1.IsVisible = false;
            HeaderFrame2.IsVisible = false;
            HeaderFrame4.IsVisible = false;


            //string email = await Task.Run(() => GetUserEmail());

            btnScanPic.Clicked += async (sender, args) =>
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "test.jpg"
                });

                if (file == null)
                    return;

                filename = file.Path;


                //await DisplayAlert("File Location", file.Path, "OK");

                image.Source = ImageSource.FromStream(() =>
                {
                    HeaderFrame1.IsVisible = true;
                    HeaderFrame2.IsVisible = true;
                    HeaderFrame3.IsVisible = false;
                    tbStatus.TextColor = Color.Navy;
                    statusStr = "Ready";
                    tbStatus.Text = statusStr;
                    var stream = file.GetStream();
                    return stream;
                });
            };

            btnPickPic.Clicked += async (sender, args) =>
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

                filename = file.Path;
                if (file == null)
                    return;

                image.Source = ImageSource.FromStream(() =>
                {
                    HeaderFrame1.IsVisible = true;
                    HeaderFrame2.IsVisible = true;
                    HeaderFrame3.IsVisible = false;
                    tbStatus.TextColor = Color.Navy;
                    statusStr = "Ready";
                    tbStatus.Text = statusStr;
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            };
            //btnScanPic.Clicked += async (object sender, EventArgs e) => {
            //    string filePath = await Helpers.ScanPhoto("Scan", this);
            //    if (filePath != "")
            //    {
            //        oldPhotoFilePaths.Add(filePath);
            //        filename = filePath;
            //        imgScan.Source = Helpers.GetPlatformSpecificPath(filePath, "photos/");

            //        //await imgScan.RotateTo(90, 1000);

            //        //using (Stream streamPic = imgScan.GetStream())
            //        //{
            //        //    var picInfo = ExifReader.ReadJpeg(streamPic);
            //        //    ExifOrientation orientation = picInfo.Orientation;
            //        //}

            //        HeaderFrame1.IsVisible = true;
            //        HeaderFrame2.IsVisible = true;
            //        HeaderFrame3.IsVisible = false;
            //        tbStatus.TextColor = Color.Navy;
            //        statusStr = "Ready";
            //        tbStatus.Text = statusStr;
            //    }
            //};


            btnRotate.Clicked += async (object sender, EventArgs e) =>
            {
                await image.RotateTo(90, 1000);
                //await imgScan.RelRotateTo(90, 2000);
                //await imgScan.RelScaleTo(0.5, 2000);
            };

            bottomSendButtonBar.Clicked += async (object sender, EventArgs e) =>
            {

                //bottomSendButtonBar.IsVisible = false;

                tbStatus.TextColor = Color.Blue;
                tbStatus.Text = "Processing!";

                //bottomSendButtonBar.IsVisible = false;
                //btnScanPic.IsVisible = false;

                HeaderFrame1.IsVisible = false;
                HeaderFrame2.IsVisible = false;
                HeaderFrame4.IsVisible = true;

                IsLoading = true;
                statusStr = await Task.Run(() => SendReceiptToServer());
                //bottomSendButtonBar.IsVisible = true;
                //btnScanPic.IsVisible = true;

                HeaderFrame4.IsVisible = false;


                IsLoading = false;
                //SendReceiptToServer();
                if (statusStr.Contains("Error"))
                {
                    statusStr = "Your loyalty points will be updated after verification";
                    tbStatus.TextColor = Color.Red;
                }
                else
                {
                    tbStatus.TextColor = Color.Green;
                }

                HeaderFrame3.IsVisible = true;


                tbStatus.Text = statusStr;

            };

            initLocalizedText();
        }

        //public async Task<async> GetUserEmailAsync()
        //{
        //    string email = await BLL.GetUserEmailID();
        //    //return email;
        //}

        void LoadData()
        {
            bottomSendButtonBar.IsVisible = false;

            tbStatus.TextColor = Color.Blue;
            tbStatus.Text = "Processing!";
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

        public string SendReceiptToServer()
        {
            string strData = App.Device.ReadFileIntoBase64String(filename);
            byte[] fileContents = System.Convert.FromBase64String(strData);

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

        async void PopOutAlert(String e)
        {
            await App.Current.MainPage.DisplayAlert("Warning", e, "OK");
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

        public void UpdatePoints()
        {
            dUser user = BLL.GetUserEmail();
            string email = user.Login;
            string statusStr = Task.Run(() => GetPointsFromServer(email)).Result;
            App.Current.Properties["UserPoints"] = statusStr.Trim();
        }

        private void initLocalizedText()
        {
            //tbStatus.Text = "";
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    if (firstLoad)
        //    {
        //        firstLoad = false;
        //    }

        //}

    }
}