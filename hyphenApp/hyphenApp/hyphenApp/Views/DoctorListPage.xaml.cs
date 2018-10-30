using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace hyphenApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DoctorListPage : ContentPage
	{
        public DoctorListPage(int patientID, int userId, string emailSubject, string emailMessage)
        {
            App.doctorPage = this;

            InitializeComponent();

            //DependencyService.Get<IAnalytics>().GAScreen("View Doctor List");

            dPatient currentPatient = App.CurrentPatient;
            if (currentPatient != null)
            {
                //topBar.Subtitle = currentPatient.Name;
                //topBar.ProfileImageSource = Helpers.GetPlatformSpecificPath(currentPatient.ProfilePicturePath, "photos/");
            }

            LoadDoctors();

            // Initialize the top bar
            //
            //topBar.LeftButtonSource = "iconblackclose";
            //topBar.LeftButtonClicked += (object sender, EventArgs e) =>
            //{
            //    Navigation.PopModalAsync();
            //};
            //topBar.RightButtonClicked += (object sender, EventArgs e) =>
            //{
            //    if (Helpers.AlreadyHasModalPageAbove(this))
            //        return;

            //    Navigation.PushModalAsync(new DoctorPage(0));
            //};

            lvDoctor.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
            {
                //    string jsonStr =  "{
                //                "type":"ecfg", 
                //  "version":"1", 
                //  "email_to" : "someone@yoursite.com", 
                //  "email_cc" : ["john@yoursite.com", "jane@yoursite.com"], 
                //  "email_bcc" : ["bob@yoursite.com"], 
                //  "email_subject" : "Requested documents", 
                //  "email_body" : "<html><body>Text</body></html>", 
                //  "email_attachments" : [{ 
                //    "path" : "email/ClinicalStudy.pdf",
                //    "mime_type" : "application/pdf" }]
                //}";

        //public class ExternalAccount
        //{
        //    public string Name { get; set; }
        //}

        //ExternalAccount account = new ExternalAccount() { Name = "Someone" };
        //        string json = JsonConvert.SerializeObject(account);
        //        string base64EncodedExternalAccount = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

                var doctor = e.Item as dDoctor;

                //if (!App.EmailService.CanSend)
                //{
                //    await DisplayAlert(
                //        AppResources.DoctorListPage_SendEmailFailTitle,
                //        AppResources.DoctorListPage_SendEmailFailMessage,
                //        AppResources.Common_OK);
                //    return;
                //}


                //List<string> attachments = new List<string>();
                ////attachments.Add(App.SavedFileName);
                //attachments.Add("");

                // For iOS, we need to close the doctor list screen
                // first, since we are going to use the topmost view controller
                // (and this cannot be the doctor list screen, since it's going to be closed)
                // to display the email view controller.
                //
                if (Device.OS == TargetPlatform.iOS)
                    await Navigation.PopModalAsync();

                string emailStr = "mailto:" + doctor.Email + "?subject=" + emailSubject + "&body=" + String.Format(AppResources.DoctorListPage_EmailDear, doctor.Name) + ",\n\n" + emailMessage;

                Device.OpenUri(new Uri(emailStr));

                //App.EmailService.ShowDraft(
                //    emailSubject,
                //    String.Format(AppResources.DoctorListPage_EmailDear, doctor.Name) + ",\n\n" + emailMessage,
                //    false,
                //    doctor.Email,
                //    attachments);

                // For Android, we can close the doctor list screen
                // later.
                //
                if (Device.OS == TargetPlatform.Android)
                    await Navigation.PopModalAsync();

                lvDoctor.SelectedItem = null;
            };

            initLocalizedText();
        }

        //public async Task SendEmail(string subject, string body, List<string> recipients)
        //{
        //    try
        //    {
        //        var message = new EmailMessage
        //        {
        //            Subject = subject,
        //            Body = body,
        //            To = recipients,
        //            //Cc = ccRecipients,
        //            //Bcc = bccRecipients
        //        };
        //        await Email.ComposeAsync(message);
        //    }
        //    catch (FeatureNotSupportedException fbsEx)
        //    {
        //        // Email is not supported on this device
        //    }
        //    catch (Exception ex)
        //    {
        //        // Some other exception occurred
        //    }
        //}

        private void initLocalizedText()
        {
            //topBar.Title = AppResources.DoctorListPage_EmailDoctor;
        }

        /// <summary>
        /// Loads a list of doctors under the current User ID.
        /// If there are no doctors, then we open the DoctorPage
        /// to add one.
        /// </summary>
        public void LoadDoctors()
        {
            List<dDoctor> doctors = Task.Run(() => BLL.GetDoctorsByUserID(App.CurrentUserID)).Result;
            //var doctors = BLL.GetDoctorsByUserID(App.CurrentUserID);
            if (doctors != null)
            {
                lvDoctor.ItemsSource = doctors;
                if (doctors.Count == 0)
                {
                    lblHelpText.Text = AppResources.DoctorListPage_HelpTextNoDoctors;
                    stackHelpText.IsVisible = true;
                    lvDoctor.IsVisible = false;
                }
                else
                {
                    lblHelpText.Text = AppResources.DoctorListPage_HelpTextSelectDoctor;
                    lvDoctor.IsVisible = true;

                }
            }
            else
            {
                lblHelpText.Text = AppResources.DoctorListPage_HelpTextNoDoctors;
                stackHelpText.IsVisible = true;
                lvDoctor.IsVisible = false;
            }
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            try
            {
                var item = args.SelectedItem as dDoctor;
                if (item == null)
                    return;

                await Navigation.PushModalAsync(new NavigationPage(new DoctorPage(item.ID)));

                // Manually deselect item.
                lvDoctor.SelectedItem = null;
            }
            catch
            { }
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new DoctorPage(0)));

        }


        public async Task btnEditDoctorClicked(object sender, EventArgs e)
        {
            var view = sender as View;

            int doctorID = (int)((TapGestureRecognizer)view.GestureRecognizers[0]).CommandParameter;
            await Navigation.PushModalAsync(new NavigationPage(new DoctorPage(doctorID)));
            //Navigation.PushModalAsync(new DoctorPage(doctorID));

        }
    }
}

