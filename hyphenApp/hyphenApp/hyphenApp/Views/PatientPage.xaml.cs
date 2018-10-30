using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace hyphenApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PatientPage : ContentPage
	{
        int patientID = 0;
        bool hasNoPatients = false;
        List<string> oldPhotoFilePaths = new List<string>();

        bool firstLoad = true;
        string filename = null;

        public PatientPage(int patientID)
        {
            InitializeComponent();


            //DependencyService.Get<IAnalytics>().GAScreen("Add Patient");

            lblName.WidthRequest = Helpers.ScaleSizeByWidth(120, 0.8);
            lblPhoto.WidthRequest = Helpers.ScaleSizeByWidth(120, 0.8);
            lblDateOfBirth.WidthRequest = Helpers.ScaleSizeByWidth(120, 0.8);
            lblGender.WidthRequest = Helpers.ScaleSizeByWidth(120, 0.8);


            var firstPatient = BLL.GetFirstPatientByUser(App.CurrentUserID);
            if (firstPatient == null)
            {
                // This user has no patients added, so do not allow him/her
                // to cancel this screen.
                //topBar.LeftButtonSource = "";
                stackHelpText.IsVisible = true;
                hasNoPatients = true;
            }

            if(patientID == -1)
            {
                stackHelpText.IsVisible = true;
                ToolbarItems.RemoveAt(1);
                patientID = 0;
            }
            else
            {
                stackHelpText.IsVisible = false;
            }

            this.patientID = patientID;

            if (stackHelpText.IsVisible)
            {
                //if (firstPatient == null)
                //    hintHelpText.Text = AppResources.PatientPage_HintHelpFirstPatient;
                //else
                //    hintHelpText.Text = AppResources.PatientPage_HintHelpPatient;
            }

            //btnProfilePic.Clicked += async (object sender, EventArgs e) => {
            //    string filePath = await Helpers.PickPhoto(AppResources.PatientPage_UpdateProfilePic, this);
            //    if (filePath != "")
            //    {
            //        // If this is the first time we are adding to the list
            //        // make sure we add the current patient's photo file path.
            //        ////
            //        //if (oldPhotoFilePaths.Count == 0)
            //        //    oldPhotoFilePaths.Add(imgProfile.Source ?? "");

            //        //// Add the newly taken photo file path
            //        ////
            //        //oldPhotoFilePaths.Add(filePath);

            //        //imgProfile.ImageSource = Helpers.GetPlatformSpecificPath(filePath, "photos/");
            //    }
            //};

            TapGestureRecognizer tr = new TapGestureRecognizer();
            tr.Tapped += (object sender, EventArgs e) =>
            {
                chkMale.Checked = false;
                chkFemale.Checked = true;
            };
            stackFemale.GestureRecognizers.Add(tr);

            tr = new TapGestureRecognizer();
            tr.Tapped += (object sender, EventArgs e) =>
            {
                chkMale.Checked = true;
                chkFemale.Checked = false;
            };
            stackMale.GestureRecognizers.Add(tr);

            //topBar.RightButtonClicked += async (object sender, EventArgs e) =>
            //{
            //    string result = await DisplayActionSheet(
            //        AppResources.PatientPage_DeleteFamilyMember,
            //        AppResources.Common_OptionNo,
            //        AppResources.Common_OptionYes);
            //    if (result == null || result.Equals(AppResources.Common_OptionNo))
            //        return;

            //    DeletePatient(patientID, App.CurrentUserID);

            //    // If the user happened the delete the current patient,
            //    // make sure we load up another existing patient.
            //    //
            //    if (App.CurrentPatientID == patientID)
            //    {
            //        App.CurrentPatientID = BLL.GetFirstPatientByUser(App.CurrentUserID).ID;
            //    }

            //    App.PatientTabPage.LoadAndUpdateTopSideBars();
            //    App.PatientTabPage.CurrentView.LoadAndUpdatePage("");
            //    Navigation.PopModalAsync();
            //};


            // Cancel without saving. Make sure we delete all photos in the
            // oldPhotosFilePath except the FIRST one.
            //
            //topBar.LeftButtonClicked += (object sender, EventArgs e) =>
            //{
            //    for (int i = 1; i < oldPhotoFilePaths.Count; i++)
            //        if (oldPhotoFilePaths[i] != "")
            //        {
            //            try
            //            {
            //                App.Device.DeleteFile(oldPhotoFilePaths[i]);
            //            }
            //            catch
            //            {
            //            }
            //        }

            //    Navigation.PopModalAsync();
            //};

            tbName.Focused += (object sender, FocusEventArgs e) => {
                //if (Device.OS == TargetPlatform.Android)
                //    scvForm.ScrollToAsync(tbName, ScrollToPosition.Start, true);
            };


            //    bottomSaveButtonBar.SaveButtonClicked += (object sender, EventArgs e) =>
            //    {
            //        // Validate the record
            //        //
            //        string errorMessage = "";
            //        if (tbName.Text == null || tbName.Text == "")
            //            errorMessage += "\n- " + AppResources.PatientPage_Name;
            //        if (!chkFemale.IsChecked && !chkMale.IsChecked)
            //            errorMessage += "\n- " + AppResources.PatientPage_Gender;
            //        if (errorMessage != "")
            //        {
            //            DisplayAlert(
            //                AppResources.Common_ErrorTitle,
            //                AppResources.PatientPage_ErrorMissingFields + errorMessage,
            //                AppResources.Common_OK);
            //            return;
            //        }

            //        if (dpBirthday.Date > DateTime.Now)
            //        {
            //            DisplayAlert(
            //                AppResources.Common_ErrorTitle,
            //                AppResources.PatientPage_ErrorInvalidDateOfBirth,
            //                AppResources.Common_OK);
            //            return;
            //        }

            //        SavePatient(patientID);

            //        App.ReloadPatient();
            //        App.PatientTabPage.LoadAndUpdateTopSideBars();

            //        if (App.PatientTabPage.CurrentView is IPatientTabView)
            //        {
            //            ((IPatientTabView)App.PatientTabPage.CurrentView).LoadAndUpdatePage("");
            //        }

            //        Navigation.PopModalAsync();

            //    };

            //    initLocalizedText();
        }

        private void initLocalizedText()
        {
            lblFamilyMembersParticulars.Text = AppResources.PatientPage_FamilyMembersParticulars;
            lblPhoto.Text = AppResources.PatientPage_Photo;
            //btnProfilePic.Text = AppResources.PatientPage_Choose;
            lblName.Text = AppResources.PatientPage_Name + "*";
            tbName.Placeholder = AppResources.PatientPage_Name;
            lblDateOfBirth.Text = AppResources.PatientPage_DateOfBirth + "*";
            lblGender.Text = AppResources.PatientPage_Gender + "*";
            lblMale.Text = AppResources.PatientPage_GenderMale;
            lblFemale.Text = AppResources.PatientPage_GenderFemale;

            dpBirthday.Format = Helpers.GetShortDateFormat(App.LanguageCode);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (firstLoad)
            {
                LoadCurrentPatientAndDisplay(this.patientID);
                firstLoad = false;
            }
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            string errorMessage = "";
            if (tbName.Text == null || tbName.Text == "")
                errorMessage += "\n- " + "Enter Patient Name";
                //errorMessage += "\n- " + AppResources.PatientPage_Name;
            if (!chkFemale.Checked && !chkMale.Checked)
                errorMessage += "\n- " + "Select Patient Gender";
                //errorMessage += "\n- " + AppResources.PatientPage_Gender;
            if (errorMessage != "")
            {
                await DisplayAlert(
                    "Error",
                    "" + errorMessage,
                    "OK");
                return;
            }

            if (dpBirthday.Date > DateTime.Now)
            {
                await DisplayAlert(
                    "Error",
                    "Invalid Date Of Birth",
                    "OK");
                return;
            }

            SavePatient(patientID);
            App.PatientScreen = false;
            var masterPage = App.mainPage as TabbedPage;
            masterPage.CurrentPage = masterPage.Children[0];
            masterPage.CurrentPage.Focus();
            await Navigation.PopModalAsync();
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Camera_OnTapGestureRecognizerTapped(object sender, EventArgs args)
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

            if (filename != "")
            {
                //int imageWidth = 70;
                //Image img = new Image()
                //{ HeightRequest = imageWidth, WidthRequest = imageWidth };
                //img.Aspect = Aspect.AspectFill;
                imgProfile.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            }
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

            if (filename != "")
            {
                //int imageWidth = 70;
                //Image img = new Image()
                //{ HeightRequest = imageWidth, WidthRequest = imageWidth };
                //img.Aspect = Aspect.AspectFill;

                imgProfile.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            }
        }


        /// <summary>
        /// Loads the current patient and display onto the screen.
        /// </summary>
        /// <param name="pID">P I.</param>
        public void LoadCurrentPatientAndDisplay(int pID)
        {
            try
            {
                dPatient currentPatient = Task.Run(() => BLL.GetPatient(pID)).Result;
                if (currentPatient == null)
                    return;

                //lvPatients.ItemsSource = patient;
                tbName.Text = currentPatient.Name;
                chkMale.Checked = currentPatient.Gender == "M";
                chkFemale.Checked = currentPatient.Gender == "F";
                DateTime birthday = Convert.ToDateTime(currentPatient.Birthday);
                dpBirthday.Date = birthday;
                filename = currentPatient.ProfilePicturePath;
                byte[] fileContents = File.ReadAllBytes(currentPatient.ProfilePicturePath);
                imgProfile.Source = ImageSource.FromStream(() => new MemoryStream(fileContents));
            }
            catch
            { }
        }


        /// <summary>
        /// Saves the patient record.
        /// </summary>
        /// <param name="patientID">Patient ID.</param>
        public async void SavePatient(int patientID)
        {
            string name = tbName.Text;
            string birthday = dpBirthday.Date.ToString("yyyy/MM/dd");

            dPatient record = new dPatient();
            record.UserID = App.CurrentUserID;
            record.Name = name;
            record.Birthday = birthday;
            record.Gender = chkMale.Checked ? "M" : chkFemale.Checked ? "F" : "";
            record.ProfilePicturePath = filename;

            if (patientID == 0)
            {
                await Task.Run(() => BLL.InsertPatientRecord(record));
                //BLL.InsertRecord(record);
            }
            else
            {
                record.ID = patientID;
                await Task.Run(() => BLL.InsertPatientRecord(record));
                //BLL.UpdateRecord(record);
            }
            App.CurrentPatientID = record.ID;

            // Make sure we delete all oldPhotosFilePath
            // except the LAST one.
            //
            //for (int i = 0; i < oldPhotoFilePaths.Count - 1; i++)
            //    if (oldPhotoFilePaths[i] != "")
            //    {
            //        try
            //        {
            //            App.Device.DeleteFile(oldPhotoFilePaths[i]);
            //        }
            //        catch
            //        {
            //        }
            //    }

        }


        /// <summary>
        /// Deletes the patient.
        /// </summary>
        /// <param name="patientID">Patient I.</param>
        /// <param name="userID">User I.</param>
        public void DeletePatient(int patientID, int userID)
        {
            // Remove the patient record first, so it can
            // no longer be accessed.
            //
            //App.BLL.DeleteRecord("dPatient", patientID);


            // Here we delete all the photos
            //
            DateTime currentDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            string earliestDateString = BLL.GetPestPhotosEarliestDate(patientID);
            DateTime earliestDate = currentDate;

            try
            {
                earliestDate = DateTime.ParseExact(earliestDateString, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                earliestDate = new DateTime(earliestDate.Year, earliestDate.Month, 1);
            }
            catch
            {
            }

            while (currentDate >= earliestDate)
            {
                //List<dPestphoto> photos = BLL.GetPestPhotos(patientID,
                //    currentDate.ToString("yyyy/MM/dd"),
                //    currentDate.AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd"));

                List<dPestphoto> photos = Task.Run(() => BLL.GetPestPhotos(patientID,
               currentDate.ToString("yyyy/MM/dd"),
                    currentDate.AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd"))).Result;

                foreach (var photo in photos)
                {
                    App.Device.DeleteFile(Helpers.GetPlatformSpecificPath(photo.FilePath, "photos/"));
                }
                currentDate = currentDate.AddMonths(-1);

            }


            // Then finally, we delete the PEST / Photo records
            //
            BLL.DeletePestRecordAndPhotos(patientID);
        }



        /// <summary>
        /// Disallow user from pressing the back button to go
        /// back to the previous screen if there are no other
        /// patients created.
        /// </summary>
        /// <returns>To be added.</returns>
        /// <remarks>To be added.</remarks>
        protected override bool OnBackButtonPressed()
        {
            if (hasNoPatients)
                return true;
            else
                return base.OnBackButtonPressed();
        }



    }
}

