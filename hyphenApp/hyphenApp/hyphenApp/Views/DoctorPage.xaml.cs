using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace hyphenApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DoctorPage : ContentPage
	{
        int doctorID = 0;

        public DoctorPage(int doctorID)
        {
            InitializeComponent();

            this.doctorID = doctorID;
            if (doctorID != 0)
            {
                //topBar.Title = AppResources.DoctorPage_ModifyDoctor;
                //topBar.RightButtonSource = "iconblackdelete";
                //DependencyService.Get<IAnalytics>().GAScreen("Update Doctor");
            }
            else
            {
                ToolbarItems.RemoveAt(1);
                //topBar.Title = AppResources.DoctorPage_AddDoctor;
                //topBar.RightButtonSource = "";
                //DependencyService.Get<IAnalytics>().GAScreen("Add Doctor");
            }

            txtInfo.Focused += (object sender, FocusEventArgs e) => {
                if (Device.OS == TargetPlatform.Android)
                    scvForm.ScrollToAsync(txtInfo, ScrollToPosition.Start, true);
            };

            txtClinic.Focused += (object sender, FocusEventArgs e) => {
                if (Device.OS == TargetPlatform.Android)
                    scvForm.ScrollToAsync(txtClinic, ScrollToPosition.Start, true);
            };

            txtAddress.Focused += (object sender, FocusEventArgs e) => {
                if (Device.OS == TargetPlatform.Android)
                    scvForm.ScrollToAsync(txtAddress, ScrollToPosition.Start, true);
            };


            //topBar.RightButtonClicked += async (object sender, EventArgs e) =>
            //{
            //    string result = await this.DisplayActionSheet(
            //        AppResources.DoctorPage_DeleteDoctor,
            //        AppResources.Common_OptionNo,
            //        AppResources.Common_OptionYes);
            //    if (result == null || result.Equals(AppResources.Common_OptionNo))
            //        return;

            //    BLL.DeleteRecord("dDoctor", doctorID);
            //    App.doctorPage.LoadDoctors();
            //    Navigation.PopModalAsync();
            //};

            initlocalizedText();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            string missingFields = "";
            if (txtName.Text == null || txtName.Text == "")
                missingFields += "\n- " + AppResources.DoctorPage_Name;
            if (txtEmail.Text == null || txtEmail.Text == "")
                missingFields += "\n- " + AppResources.DoctorPage_Email;
            if (missingFields != "")
            {
                await DisplayAlert(AppResources.Common_ErrorTitle,
                    AppResources.DoctorPage_ErrorAlertMissingFields + missingFields,
                    AppResources.Common_OK);
                return;
            }

            if (!Regex.IsMatch(txtEmail.Text, @"\A(?:[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?)\Z"))
            {
                await DisplayAlert(AppResources.Common_ErrorTitle,
                    AppResources.DoctorPage_ErrorAlertInvalidEmail,
                    AppResources.Common_OK);
                return;
            }

            if(doctorID == 0)
            {
                doctorID = Task.Run(() => BLL.GetDoctorsNextID()).Result;
            }

            dDoctor doc = new dDoctor();
            doc.ID = doctorID;
            doc.Clinic = txtClinic.Text;
            doc.Name = txtName.Text;
            doc.Address = txtAddress.Text;
            doc.Information = txtInfo.Text;
            doc.UserID = App.CurrentUserID;
            doc.Email = txtEmail.Text;

            await Task.Run(() => BLL.InsertDoctorRecord(doc));

            //if (doctorID != 0)
            //    BLL.UpdateRecord(doc);
            //else
            //    BLL.InsertRecord(doc);

            App.doctorPage.LoadDoctors();
            await Navigation.PopModalAsync();
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            string result = await this.DisplayActionSheet(
                AppResources.DoctorPage_DeleteDoctor,
                AppResources.Common_OptionNo,
                AppResources.Common_OptionYes);
            if (result == null || result.Equals(AppResources.Common_OptionNo))
                return;

            await Task.Run(() => BLL.DeleteDoctorRecord(doctorID));
            //BLL.DeleteRecord("dDoctor", doctorID);
            App.doctorPage.LoadDoctors();
            await Navigation.PopModalAsync();
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void initlocalizedText()
        {
            txtName.Placeholder = AppResources.DoctorPage_NamePlaceholder;
            txtEmail.Placeholder = AppResources.DoctorPage_EmailPlaceholder;
            txtClinic.Placeholder = AppResources.DoctorPage_ClinicPlaceholder;
            lblContactInfo.Text = AppResources.DoctorPage_ContactInformation;
            lblName.Text = AppResources.DoctorPage_Name + "*";
            lblEmail.Text = AppResources.DoctorPage_Email + "*";
            lblOtherInfo.Text = AppResources.DoctorPage_OtherInfo;
            lblClinic.Text = AppResources.DoctorPage_Clinic;
            lblAddress.Text = AppResources.DoctorPage_Address;
            lblNotes.Text = AppResources.DoctorPage_Notes;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadDoctor(this.doctorID);
        }

        public void LoadDoctor(int doctorID)
        {
            //var doctor = BLL.GetDoctor(doctorID);
            try
            {
                dDoctor doctor = Task.Run(() => BLL.GetDoctor(doctorID)).Result;

                if (doctor == null)
                    return;

                txtEmail.Text = doctor.Email;
                txtAddress.Text = doctor.Address;
                txtInfo.Text = doctor.Information;
                txtClinic.Text = doctor.Clinic;
                txtName.Text = doctor.Name;
            }
            catch
            { }
        }
    }
}




