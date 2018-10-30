using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace hyphenApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        static string statusStr = "";
        Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        //public LoginPage()
        //{
        //    InitializeComponent();
        //}

        public LoginPage()
        {
            InitializeComponent();

            btnStart.Clicked += async (object sender, EventArgs e) =>
            {
                var masterPage = App.mainPage as TabbedPage;
                masterPage.CurrentPage = masterPage.Children[0];
                masterPage.CurrentPage.Focus();
                await Navigation.PopModalAsync();
            };
            btnLogin.Clicked += async (object sender, EventArgs e) =>
            {
                //AccessGranting();

                string email = txtUser.Text.Trim();
                string password = txtPass.Text.Trim();

                if (!ValidateEmail(email))
                {
                    PopOutAlert("Invalid email. Please key in valid email address!");
                    //await DisplayAlert(
                    //AppResources.Common_ErrorTitle,
                    //"Invalid email. Please key in valid email address!",
                    //AppResources.Common_OK);
                    return;
                }

                if (email != null && password != null)
                {
                    statusStr = await Task.Run(() => VerifyUserInServer(email, password));
                    if (statusStr.Trim() == "true")
                    {
                        AddNewUser(email, password);
                        //await Navigation.PushModalAsync(new NavigationPage(new PatientPage(App.CurrentUserID)));
                        if (!App.PatientScreen)
                        {
                            App.PatientScreen = true;
                            //await Navigation.PopModalAsync();
                            frameCreate.IsVisible = true;
                            frameRegister.IsVisible = false;
                            frameLogin.IsVisible = false;

                            await Navigation.PushModalAsync(new NavigationPage(new PatientPage(-1)));
                        }
                      

                    }
                }
            };

            btnRegister.Clicked += async (object sender, EventArgs e) =>
            {
                await Navigation.PushModalAsync(new NavigationPage(new UserPage()));

            };
            //lblMem.GestureRecognizers.Add(new TapGestureRecognizer((view) => Navigation.PushModalAsync (new UserPage ())));

            //initLocalizedString();
        }

        async void PopOutAlert(String e)
        {
            await App.Current.MainPage.DisplayAlert("Warning", e, "OK");
        }

        public void AddNewUser(string email, string password)
        {
            Task.Run(() => BLL.InsertUserEmailID(email));

            App.CurrentUserLogin = email;
        }

        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
        }

        private void initLocalizedString()
        {
            btnLogin.Text = AppResources.LoginPage_LoginBtn;
            //lblWelcome.Text = AppResources.LoginPage_Welcome;
            btnRegister.Text = AppResources.LoginPage_CreateAccount;
        }

        public void AccessGranting()
        {
            //Navigation.PushModalAsync (new TestPage (0));
            //return;


            dUser user = BLL.CheckAccess(txtUser.Text, txtPass.Text);

            //	lblMessage.Text = Convert.ToString (result);
            if (user != null)
            {

                //           if(!ValidateEmail(txtUser.Text))
                //            {
                //                DisplayAlert(
                //                AppResources.LoginPage_LoginFailTitle,
                //                "Invalid email. Please key in valid email address!",
                //                AppResources.Common_OK);
                //            }

                //            App.CurrentUserID = user.ID;

                //var patient = BLL.GetFirstPatientByUser (user.ID);
                //if (patient != null)
                //{
                //	App.CurrentPatientID = patient.ID;
                //	Navigation.PushModalAsync (new PatientTabPage ());
                //}
                //else
                //{
                //	Navigation.PushModalAsync (new PatientTabPage ());
                //}
            }
            else
            {
                DisplayAlert(
                    AppResources.LoginPage_LoginFailTitle,
                    AppResources.LoginPage_LoginFailMessage,
                    AppResources.Common_OK);
            }

        }

        public string VerifyUserInServer(string email, string password)
        {
            string strData = "";
            byte[] fileContents = System.Convert.FromBase64String(strData);

            Uri webService = new Uri(@"http://hdx.azurewebsites.net/VerifyUser");
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, webService);
            requestMessage.Headers.ExpectContinue = false;

            string boundary = "---HyphenBoundary";

            MultipartFormDataContent multiPartContent = new MultipartFormDataContent(boundary);
            HttpContent byteArrayContent = new ByteArrayContent(fileContents);

            //byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("binary/octet-stream");
            //byteArrayContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            //{
            //    Name = "file",
            //    //   Name = "\"media[file]\"",
            //    FileName = "image.jpg"
            //};

            multiPartContent.Add(byteArrayContent);
            multiPartContent.Headers.Add("email", email);
            multiPartContent.Headers.Add("password", password);
            multiPartContent.Headers.Add("type", "verify");

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
