using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace hyphenApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        static string statusStr = "";
        Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        public UserPage()
        {
            InitializeComponent();

            btnSave.Clicked += (object sender, EventArgs e) =>
            {
                Save();
            };

            //DependencyService.Get<IAnalytics>().GAScreen("Create New User Account");

            TapGestureRecognizer tr = new TapGestureRecognizer();
            tr.Tapped += async (object sender, EventArgs e) =>
            {
                await Navigation.PushModalAsync(new NavigationPage(new PolicyPage()));
            };
            lblTermsOfService.GestureRecognizers.Add(tr);


            //touchView.TouchDown += (object sender, EventArgs e) =>
            //{
            //    imgSave.FadeTo(0.5, 1);
            //};
            //touchView.TouchUp += async (object sender, EventArgs e) =>
            //{
            //    await imgSave.FadeTo(1, 100, Easing.Linear);

            //    if (txtLogin.Text == null || txtPassword.Text == null || txtConfirmPassword.Text == null ||
            //        txtLogin.Text == "" || txtPassword.Text == "" || txtConfirmPassword.Text == "")
            //    {
            //        //System.Diagnostics.Debug.WriteLine("Displaying alert!");
            //        await DisplayAlert(
            //            AppResources.Common_ErrorTitle,
            //            AppResources.UserPage_MissingFields,
            //            AppResources.Common_OK);
            //        return;
            //    }

            //    string email = txtLogin.Text.Trim();
            //    string password = txtPassword.Text.Trim();

            //    if (!ValidateEmail(email))
            //    {
            //        await DisplayAlert(
            //        AppResources.Common_ErrorTitle,
            //        "Invalid email. Please key in valid email address!",
            //        AppResources.Common_OK);
            //        return;
            //    }

            //    if (BLL.GetUserByLogin(email) != null)
            //    {

            //        await DisplayAlert(
            //            AppResources.Common_ErrorTitle,
            //            AppResources.UserPage_LoginAlreadyExists,
            //            AppResources.Common_OK);
            //        return;
            //    }

            //    if (txtPassword.Text != txtConfirmPassword.Text)
            //    {
            //        await DisplayAlert(
            //            AppResources.Common_ErrorTitle,
            //            AppResources.UserPage_PasswordsMismatch,
            //            AppResources.Common_OK);
            //        return;
            //    }

            //    statusStr = await Task.Run(() => RegisterUserInServer(email, password));

            //    if (statusStr.Trim() == "true")
            //    {
            //        AddNewUser(email, password);
            //        await Navigation.PopModalAsync(false);
            //        await App.myApp.MainPage.Navigation.PushModalAsync(new PatientPage(0));
            //    }

            //    //await DisplayAlert(
            //    //       "Register",
            //    //       statusStr,
            //    //       AppResources.Common_OK);

            //    //            dUser user = new dUser();
            //    //user.Login = txtLogin.Text;
            //    //user.Password = App.Device.Hash(txtLogin.Text + txtPassword.Text);

            //    //BLL.InsertRecord(user);

            //    //App.CurrentUserID = user.ID;
            //    //await Navigation.PopModalAsync(false);
            //    //await App.myApp.MainPage.Navigation.PushModalAsync (new PatientTabPage());	

            //};

            //btnCancel.Clicked += async (object sender, EventArgs e) => {
            //    await Navigation.PopModalAsync();
            //    await App.myApp.MainPage.Navigation.PushModalAsync(new LoginPage());
            //};
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Save()
        //async void Save_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            //await Navigation.PushModalAsync(new NavigationPage(new CameraPage()));
            if (txtLogin.Text == null || txtPassword.Text == null || txtConfirmPassword.Text == null ||
                   txtLogin.Text == "" || txtPassword.Text == "" || txtConfirmPassword.Text == "")
            {
                //System.Diagnostics.Debug.WriteLine("Displaying alert!");
                await DisplayAlert(
                    AppResources.Common_ErrorTitle,
                    AppResources.UserPage_MissingFields,
                    AppResources.Common_OK);
                return;
            }

            string email = txtLogin.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (!ValidateEmail(email))
            {
                await DisplayAlert(
                "Warning",
                "Invalid email. Please key in valid email address!",
                "Ok");
                return;
            }

            //if (BLL.GetUserByLogin(email) != null)
            //{

            //    await DisplayAlert(
            //        "Warning",
            //        AppResources.UserPage_LoginAlreadyExists,
            //        AppResources.Common_OK);
            //    return;
            //}

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                await DisplayAlert(
                    "Warning",
                    "Password and  Confirm Password Mismatch",
                    AppResources.Common_OK);
                return;
            }

            statusStr = await Task.Run(() => RegisterUserInServer(email, password));

            if (statusStr.Trim() == "true")
            {
                await Task.Run(() => BLL.InsertUserEmailID(email));
                await Navigation.PopModalAsync();

                //AddNewUser(email, password);
                //await Navigation.PopModalAsync(false);
                //await App.myApp.MainPage.Navigation.PushModalAsync(new PatientPage(0));
            }
        }

        private void initLocalizedText()
        {
            //lblHead.Text = AppResources.UserPage_CreateAccount;
            //lblLoginName.Text = AppResources.UserPage_LoginName;
            //lblPassword.Text = AppResources.UserPage_Password;
            //btnCancel.Text = AppResources.UserPage_Cancel;
            //lblTapSave.Text = AppResources.UserPage_TapSave;
            //lblTermsOfService.Text = AppResources.UserPage_TermsAndConditions;
        }

        public string RegisterUserInServer(string email, string password)
        {
            string strData = "";
            byte[] fileContents = System.Convert.FromBase64String(strData);

            Uri webService = new Uri(@"http://hdx.azurewebsites.net/VerifyUser");
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
            multiPartContent.Headers.Add("password", password);
            multiPartContent.Headers.Add("type", "register");

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

        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
        }

        /// <summary>
        /// Adds the new user to the database.
        /// </summary>
        //      public void AddNewUser () 
        //{
        //	dUser newUser = new dUser ();
        //	newUser.Login = txtLogin.Text;
        //	newUser.Password = App.Device.Hash(txtLogin.Text + txtPassword.Text);

        //	BLL.InsertRecord (newUser);

        //	App.CurrentUserID = newUser.ID;

        //}
        public void AddNewUser(string email, string password)
        {
            dUser newUser = new dUser();
            newUser.Login = email;
            newUser.Password = App.Device.Hash(email + password);

            BLL.InsertRecord(newUser);

            App.CurrentUserLogin = newUser.Login;
            //Navigation.PopModalAsync();

        }

    }
}

