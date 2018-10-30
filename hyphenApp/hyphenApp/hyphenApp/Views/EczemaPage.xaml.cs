using Acr.UserDialogs;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace hyphenApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EczemaPage : ContentPage, INotifyPropertyChanged
    {
        public static DateTime currentlyShownDate;
        List<dTips> pList = new List<dTips>();
        DateTime previouslyShownDate;
        dPest currentPest;
        bool loading = false;

        string filename = null;

        Image[] discomfortImages;
        Label[] discomfortLabels;
        StackLayout[] discomfortStackLayouts;
        int displayWidth;
        int index = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="pestApp.RecordEczema"/> class.
        /// </summary>
        public EczemaPage()
        {
            InitializeComponent();
            DeterminedLogin();

            try
            {
                pList = Task.Run(() => GetTipsFromServer()).Result;
                //lblTips.Text = pList[index].Tips;
                Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                {
                    App.CurrentTips = pList[index].Tips;

                    if (index < pList.Count - 1)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }
                    return true; // True = Repeat again, False = Stop the timer
            });
            }
            catch
            { }

            //btnView.IsVisible = false;

            //App.currentlyShownDate = DateTime.Today;

            //currentlyShownDate = DateTime.Today;
            //previouslyShownDate = currentlyShownDate;

            discomfortStackLayouts = new StackLayout[] { stackD1, stackD2, stackD3, stackD4, stackD5 };
            discomfortImages = new Image[] { btnD1, btnD2, btnD3, btnD4, btnD5 };
            discomfortLabels = new Label[] { mood1Label, mood2Label, mood3Label, mood4Label, mood5Label };

            ////DependencyService.Get<IAnalytics>().GAScreen("Rate Eczema");

            displayWidth = (int)App.ScreenWidth;
            if (displayWidth > 400)
            {
                gridDiscomfort.RowDefinitions[0].Height = Helpers.ScaleSizeByWidth(75, 0.8);
                gridDiscomfort.RowDefinitions[1].Height = Helpers.ScaleSizeByWidth(35, 0.8);

                for (int i = 1; i <= 5; i++)
                {
                    discomfortStackLayouts[i - 1].Padding = new Thickness(0, 0, 0, Helpers.ScaleSizeByWidth(10, 0.8));
                    discomfortImages[i - 1].WidthRequest = Helpers.ScaleSizeByWidth(50, 0.8);
                    discomfortImages[i - 1].HeightRequest = Helpers.ScaleSizeByWidth(50, 0.8);
                    discomfortLabels[i - 1].FontSize = Helpers.ScaleSizeByWidth(12, 0.8);
                }
            }

            for (int i = 1; i <= 5; i++)
            {
                int level = i;

                // Add the tap gesture to the emoticon image.
                //
                TapGestureRecognizer tr = new TapGestureRecognizer();
                discomfortStackLayouts[i - 1].GestureRecognizers.Add(tr);
                tr.Tapped += (object sender, EventArgs e) =>
                {
                    if (ImagesStillAnimating())
                        return;
                    SetDiscomfortLevel(level);
                    SavePestRecord();
                };

                // Add the tap gesture to the mood label .
                //
                tr = new TapGestureRecognizer();
                discomfortLabels[i - 1].GestureRecognizers.Add(tr);
                tr.Tapped += (object sender, EventArgs e) =>
                {
                    if (ImagesStillAnimating())
                        return;

                    SetDiscomfortLevel(level);
                    SavePestRecord();
                };

            }

            currentlyShownDate = DateTime.Today;
            previouslyShownDate = currentlyShownDate;

            btnDate.Clicked += (object sender, EventArgs e) =>
            {
                if (btnDate.Text == "Click")
                {
                    if (LoadCurrentPatientAndDisplay(App.CurrentPatientID))
                    {
                        LoadAndUpdatePage("");
                    }
                }
                else
                {
                    var masterPage = App.mainPage as TabbedPage;
                    masterPage.CurrentPage = masterPage.Children[1]; //Go to Page2
                    masterPage.CurrentPage.Focus();
                }
                //App.PatientTabPage.LoadTabContent(PatientTabPage.CalendarTabView, "");
            };

            tbNotes.Focused += (object sender, FocusEventArgs e) =>
            {
                if (Device.OS == TargetPlatform.Android)
                {
                    stackBottom.HeightRequest = 300;
                    //scvForm.ScrollToAsync(lblNotes, ScrollToPosition.Start, true);
                }
            };

            tbNotes.Unfocused += (object sender, FocusEventArgs e) =>
            {
                stackBottom.HeightRequest = 50;
                SavePestRecord();
            };

            initLocalizedText();
        }

        void ActionSteroidSwitchToggled(object sender, ToggledEventArgs e)
        {
            SavePestRecord();

        }

        void ActionMoisturizerSwitchToggled(object sender, ToggledEventArgs e)
        {
            SavePestRecord();

        }

        void ActionOthersSwitchToggled(object sender, ToggledEventArgs e)
        {
            SavePestRecord();

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
                int imageWidth = 70;
                Image img = new Image()
                { HeightRequest = imageWidth, WidthRequest = imageWidth };
                img.Aspect = Aspect.AspectFill;
                CreatePhotoImageTouchGesture(img, filename);

                img.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });

                stackPhotos.Children.Add(img);
                //btnView.IsVisible = true;

                dPestphoto p = new dPestphoto();
                p.PestID = this.currentPest.ID;
                p.PestDate = App.currentlyShownDate.ToString("yyyy/MM/dd");
                p.FilePath = Helpers.GetFileName(filename);
                await Task.Run(() => BLL.InsertPestPhotoRecord(p));
                //BLL.InsertPestRecord(p);
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
                int imageWidth = 70;
                Image img = new Image()
                { HeightRequest = imageWidth, WidthRequest = imageWidth };
                img.Aspect = Aspect.AspectFill;
                CreatePhotoImageTouchGesture(img, filename);

                img.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });

                stackPhotos.Children.Add(img);
                //btnView.IsVisible = true;
                dPestphoto p = new dPestphoto();
                p.PestID = this.currentPest.ID;
                p.PestDate = App.currentlyShownDate.ToString("yyyy/MM/dd");
                //p.FilePath = Helpers.GetFileName(filename);
                p.FilePath = filename;
                await Task.Run(() => BLL.InsertPestPhotoRecord(p));
                //BLL.InsertRecord(p);

            }
        }

        private void CreatePhotoImageTouchGesture(Image img, string imagePath)
        {
            var imgClick = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            imgClick.Tapped += async (object sender, EventArgs e) =>
            {
                try
                {
                    DateTime date = DateTime.ParseExact(currentPest.Date, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);

                    await Navigation.PushModalAsync(new NavigationPage(new EczemaPhotoPage(App.CurrentPatientID, imagePath, true, date)));
                }
                catch
                {
                }
            };
            img.GestureRecognizers.Add(imgClick);
        }

        async void Edit_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            lock (_tappedLockObject)
            {
                if (_tapped) return;
                _tapped = true;
            }
            await Navigation.PushModalAsync(new NavigationPage(new PatientPage(App.CurrentUserID)));
            LoadCurrentPatientAndDisplay(App.CurrentPatientID);
            reenableTap();
        }

        async void Delete_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            lock (_tappedLockObject)
            {
                if (_tapped) return;
                _tapped = true;
            }
            DateTime pestDate = currentlyShownDate;
            int patientID = App.CurrentPatientID;
            string date = pestDate.ToString("yyyy/MM/dd");
            await Task.Run(() => BLL.DeletePestRecord(patientID, date));
            LoadAndUpdatePage("");
            reenableTap();
        }

     

        private void PrevDay_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            if (!loading && !App.PatientScreen)
            {
                App.PatientScreen = true;
                using (UserDialogs.Instance.Loading("Processing...\n Please Wait", null, null, true, MaskType.Black))
                {
                    try
                    {
                        //SavePestRecord();

                        currentlyShownDate = currentlyShownDate.AddDays(-1);
                        LoadAndUpdatePage("");

                    }
                    catch
                    { }
                    App.PatientScreen = false;

                }
            }
        }
        void NextDay_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            if (!loading && !App.PatientScreen)
            {
                App.PatientScreen = true;

                using (UserDialogs.Instance.Loading("Processing...\n Please Wait", null, null, true, MaskType.Black))
                {
                    try
                    {
                        //SavePestRecord();

                        currentlyShownDate = currentlyShownDate.AddDays(1);
                        LoadAndUpdatePage("");
                    }
                    catch
                    { }
                    App.PatientScreen = false;

                }
            }
        }

        override protected void OnAppearing()
        {
            //string email = Task.Run(() => BLL.GetUserEmailID()).Result;
            //if (email == null)
            //{
            //}
            //else if (!Task.Run(() => BLL.IsPatient()).Result)
            //{
            //    if (!App.PatientScreen)
            //    {
            //        App.PatientScreen = true;
            //        await Navigation.PushModalAsync(new NavigationPage(new PatientPage(App.CurrentUserID)));
            //    }
            //}
            //else if (Task.Run(() => BLL.IsPest()).Result)
            //{
            //    try
            //    {
            //        LoadAndUpdatePage("");
            //    }
            //    catch
            //    { }
            //}
            if (LoadCurrentPatientAndDisplay(App.CurrentPatientID))
            {
                try
                {
                    if(App.UserCreated)
                    {
                        LoadAndUpdatePage("");
                    }
                    initLocalizedText();
                }
                catch
                { }
            }
        }

        private static object _tappedLockObject = new object();
        private static bool _tapped = false;

        private void tapHandler()
        {
            // one-at-a-time access to this block prevents duplicate concurrent requests:
            lock (_tappedLockObject)
            {
                if (_tapped) return;
                _tapped = true;
            }
            //handleTap();
        }

        private void reenableTap()
        {
            _tapped = false;
        }

        async void Location_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            lock (_tappedLockObject)
            {
                if (_tapped) return;
                _tapped = true;
            }
            if (Navigation.NavigationStack.Count == 0 || Navigation.NavigationStack.Last().GetType() != typeof(StoreLocationPage))
            {
                await Navigation.PushModalAsync(new NavigationPage(new StoreLocationPage()));
            }
            reenableTap();
        }

        async void Reward_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            lock (_tappedLockObject)
            {
                if (_tapped) return;
                _tapped = true;
            }
            if (Navigation.NavigationStack.Count == 0 || Navigation.NavigationStack.Last().GetType() != typeof(RewardMenuPage))
            {
                await Navigation.PushModalAsync(new NavigationPage(new RewardMenuPage()));
            }
            reenableTap();
        }

        async void Setting_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            lock (_tappedLockObject)
            {
                if (_tapped) return;
                _tapped = true;
            }
            if (Navigation.NavigationStack.Count == 0 || Navigation.NavigationStack.Last().GetType() != typeof(SettingPage))
            {
                await Navigation.PushModalAsync(new NavigationPage(new SettingPage()));
            }
            reenableTap();
        }

        private async void DeterminedLogin()
        {
            string email = Task.Run(() => BLL.GetUserEmailID()).Result;
            if (email == null)
            {
                App.UserCreated = false;
                //await Task.Run(() => BLL.InitializeDatabaseAsync());
                await Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
            }
            else if (!LoadCurrentPatientAndDisplay(App.CurrentPatientID))
            {
                App.UserCreated = true;
                //await Navigation.PushModalAsync(new NavigationPage(new PatientPage(App.CurrentUserID)));
            }
        }

        private void initLocalizedText()
        {
            lblRateFeeling.Text = AppResources.PatientEczemaTabView_RateFeeling;
            mood1Label.Text = AppResources.Common_Mood1;
            mood2Label.Text = AppResources.Common_Mood2;
            mood3Label.Text = AppResources.Common_Mood3;
            mood4Label.Text = AppResources.Common_Mood4;
            mood5Label.Text = AppResources.Common_Mood5;
            //hintHelpText.Text = AppResources.PatientEczemaTabView_DoctorHint;
            lblPhotos.Text = AppResources.PatientEczemaTabView_Photos;
            //btnPhoto.Text = AppResources.PatientEczemaTabView_AddPhoto;
            lblaction.Text = AppResources.PatientEczemaTabView_WhatDidYouDo;
            lblAppliedSteroids.Text = AppResources.PatientEczemaTabView_ApplySteroids;
            lblappliedMoisturiser.Text = AppResources.PatientEczemaTabView_ApplyMoisturiser;
            lblOthers.Text = AppResources.PatientEczemaTabView_Others;
            lblNotes.Text = AppResources.PatientEczemaTabView_Notes;

            if (App.LanguageCode == null ||
                App.LanguageCode == "" ||
                App.LanguageCode == "en" ||
                App.LanguageCode == "tl" ||
                App.LanguageCode == "fil")
            {
                gridDiscomfort.RowDefinitions[1].Height = 45;
            }
            else if (
                App.LanguageCode == "zh-CN" ||
                App.LanguageCode == "zh-TW")
            {
                gridDiscomfort.RowDefinitions[1].Height = 35;
            }
            else
            {
                gridDiscomfort.RowDefinitions[1].Height = 65;
            }

            if (App.LanguageCode == "fil")
            {
                mood1Label.FontSize = 11;
                mood2Label.FontSize = 11;
                mood3Label.FontSize = 11;
                mood4Label.FontSize = 11;
                mood5Label.FontSize = 11;
            }
            else
            {
                mood1Label.FontSize = 12;
                mood2Label.FontSize = 12;
                mood3Label.FontSize = 12;
                mood4Label.FontSize = 12;
                mood5Label.FontSize = 12;
            }

        }


        /// <summary>
        /// Checks if the images are still animating, if so,
        /// return true for the caller to prevent user from
        /// taking any action until animation is complete.
        /// 
        /// NOTE: This is required in Xamarin.Forms 2.3 otherwise
        /// cancelling / triggering another animation before the
        /// previous is complete causes a crash!
        /// </summary>
        /// <returns>The still animating.</returns>
        private bool ImagesStillAnimating()
        {
            for (int i = 0; i < 5; i++)
                if (discomfortImages[i].AnimationIsRunning("ScaleTo") ||
                    discomfortImages[i].AnimationIsRunning("RotateTo"))
                {
                    return true;
                }
            return false;
        }

        private async void ScaleSelectedImage(Image image, Label label)
        {
            await image.ScaleTo(1.4, 250);
            await image.ScaleTo(1.3, 100);
            /*
			await image.RotateTo(-20, 100);		
			await image.RotateTo(10, 100);
			await image.RotateTo(-10, 100);
			await image.RotateTo(00, 100);	*/
        }

        private async void RotateSelectedImageBegin(Image image, double angle)
        {
            await image.RotateTo(-angle, 100);
        }

        private async void RotateSelectedImageEnd(Image image, double angle)
        {
            await image.RotateTo(angle / 2, 50);
            await image.RotateTo(-angle / 2, 50);
            await image.RotateTo(angle / 4, 30);
            await image.RotateTo(-angle / 4, 30);
            await image.RotateTo(00, 20);
        }

        public void SwitchToTab()
        {
            //App.PatientTabPage.TopBar.Title = AppResources.PatientEczemaTabView_RateEczema;
            //App.PatientTabPage.TopBar.RightButtonSource = "iconblackdelete";
            //App.PatientTabPage.TopBar.RemoveRightButtonEvents();
            //App.PatientTabPage.TopBar.RightButtonClicked += async (object sender, EventArgs e) =>
            //{
            //    string result = await App.PatientTabPage.DisplayActionSheet(
            //        AppResources.PatientEczemaTabView_DeleteRecord,
            //        AppResources.Common_OptionNo,
            //        AppResources.Common_OptionYes);
            //    if (result == null || result.StartsWith("No"))
            //        return;

            //    DeletePestRecord(App.CurrentPatientID, currentlyShownDate);
            //};
        }

        /// <summary>
        /// Sets the discomfort level.
        /// </summary>
        /// <returns>The discomfort level.</returns>
        /// <param name="discomfortLevel">Discomfort level.</param>
        private int SetDiscomfortLevel(int discomfortLevel)
        {
            for (int i = 1; i <= 5; i++)
            {
                if (discomfortLevel == i)
                {
                    if (discomfortLabels[i - 1].TextColor != Color.Black)
                    {
                        //discomfortImages [i - 1].ScaleTo (1.3, 100);
                        ViewExtensions.CancelAnimations(discomfortImages[i - 1]);
                        ScaleSelectedImage(discomfortImages[i - 1], null);
                    }

                    //discomfortImages [i - 1].Opacity = 1;
                    discomfortLabels[i - 1].TextColor = Color.Black;
                }
                else
                {
                    if (discomfortLabels[i - 1].TextColor == Color.Black)
                    {
                        //if (discomfortImages [i - 1].AnimationIsRunning)
                        ViewExtensions.CancelAnimations(discomfortImages[i - 1]);
                        discomfortImages[i - 1].ScaleTo(1, 50);
                    }

                    //discomfortImages [i - 1].Opacity = 0.3;
                    discomfortLabels[i - 1].TextColor = Color.FromHex("999999");
                }
            }
            return discomfortLevel;
        }

        /// <summary>
        /// Find out the discomfort level by looking at the opacity of the image.
        /// </summary>
        /// <returns>The discomfort level.</returns>
        /// <param name="discomfortLevel">Discomfort level.</param>
        private int GetDiscomfortLevel()
        {
            for (int i = 1; i <= 5; i++)
            {
                if (discomfortLabels[i - 1].TextColor == Color.Black)
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// Deletes the PEST record and refresh the screen.
        /// </summary>
        /// <param name="patientID">Patient I.</param>
        /// <param name="pestDate">Pest date.</param>
        public async Task DeletePestRecord(int patientID, DateTime pestDate)
        {
            string date = pestDate.ToString("yyyy/MM/dd");
            dPest pest = Task.Run(() => BLL.GetPest(patientID, date)).Result;

            if (pest != null)
            {
                //var pestPhotos = BLL.GetPestPhotos(pest.ID);
                await Task.Run(() => BLL.DeletePestRecord(pest.PatientID, pest.Date));
                //BLL.DeleteRecord("dPest", pest.ID);
                //BLL.DeleteRecordPhotos(pest.ID);

                //foreach (var pestPhoto in pestPhotos)
                //    App.Device.DeleteFile(Helpers.GetPlatformSpecificPath(pestPhoto.FilePath, "photos/"));
            }

            LoadAndUpdatePage("");
        }
        public bool LoadCurrentPatientAndDisplay(int pID)
        {
            try
            {
                dPatient currentPatient = Task.Run(() => BLL.GetPatient(pID)).Result;
                if (currentPatient == null || currentPatient.Name == "")
                {
                    return false;
                }

                tbName.Text = currentPatient.Name;
                byte[] fileContents = File.ReadAllBytes(currentPatient.ProfilePicturePath);
                imgProfile.Source = ImageSource.FromStream(() => new MemoryStream(fileContents));
                App.PatientProfile = true;
            }
            catch
            { return false; }
            return true;
        }

        /// <summary>
        /// Load a PEST record for the given patient and date.
        /// </summary>
        /// <param name="date">Date.</param>
        public void LoadAndUpdatePage(string data)
        {
            imgPrevDay.IsVisible = false;
            imgNextDay.IsVisible = false;

            loading = true;


            if (data != "")
            {
                try
                {
                    DateTime dateArg = DateTime.ParseExact(data, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                    currentlyShownDate = dateArg;
                    previouslyShownDate = currentlyShownDate;
                }
                catch
                {
                }
            }

            DateTime pestDate = currentlyShownDate;
            int patientID = App.CurrentPatientID;
            string date = pestDate.ToString("yyyy/MM/dd");
            dPest pest = Task.Run(() => BLL.GetPest(patientID, date)).Result;

            // Check and see whether the new date has moved forward / backward
            // so that we can decide on how to animate the calendar.
            //
            int translateToX = 0;
            if (previouslyShownDate < currentlyShownDate)
                translateToX = -20;
            else if (previouslyShownDate > currentlyShownDate)
                translateToX = +20;
            else if (previouslyShownDate == currentlyShownDate)
                translateToX = 00;

            if (translateToX != 0)
            {
                for (int i = 0; i < 5; i++)
                    RotateSelectedImageBegin(discomfortImages[i], translateToX);
                //hwDetails.TranslateTo(0, 10, 0, 100);
                //await hwDiscomfort.TranslateTo(translateToX, 0, 0, 100);
            }

            currentPest = pest;
            if (pest == null)
            {
                int year = pestDate.Year;
                int month = pestDate.Month;
                int day = pestDate.Day;
                //string idStr = year.ToString() + month.ToString() + day.ToString();
                string idStr = month.ToString() + day.ToString();
                //int id = Convert.ToInt32(idStr.Trim());

                //PopOutAlert(idStr);
                pest = new dPest();
                //pest.ID = day;
                pest.ID = App.PestID;
                //pest.ID = -1;
                pest.PatientID = patientID;
                pest.Date = date;
            }
            else
            {
                imgPrevDay.IsVisible = true;
                imgDelete.IsVisible = true;
                lblDelete.IsVisible = true;

                lblDelete.IsVisible = true;
            }

            // Bind the data to the screen
            //
            //btnDate.Text = App.currentlyShownDate.ToString(Helpers.GetShortDateFormat("en"));
            btnDate.Text = currentlyShownDate.ToString(Helpers.GetShortDateFormat(App.LanguageCode));
            tbNotes.Text = pest.Notes;
            SetDiscomfortLevel(pest.Severity);

            chkActionSteroid.IsToggled = pest.ActionTaken != null && pest.ActionTaken.Contains("S");
            chkActionMoisturizer.IsToggled = pest.ActionTaken != null && pest.ActionTaken.Contains("M");
            chkActionOthers.IsToggled = pest.ActionTaken != null && pest.ActionTaken.Contains("O");

            stackPhotos.Children.Clear();

            //if (pest.ID == -1)
            if (pest.Severity == 0)
            {
                hwDetails.IsVisible = false;
                stackDetails.IsVisible = false;
                stackHelpText.IsVisible = true;
                imgDelete.IsVisible = false;

                lblDelete.IsVisible = false;
            }
            else
            {
                hwDetails.IsVisible = true;
                stackDetails.IsVisible = true;
                stackHelpText.IsVisible = false;
                imgDelete.IsVisible = true;
                lblDelete.IsVisible = true;

            }

            // Load photos
            //
            try
            {
                if (stackDetails.IsVisible)
                {
                    List<dPestphoto> photoList = Task.Run(() => BLL.GetPestPhotos(patientID, date)).Result;

                    foreach (var f in photoList)
                    {
                        int imageWidth = 70;
                        Image img = new Image()
                        { HeightRequest = imageWidth, WidthRequest = imageWidth };
                        img.Aspect = Aspect.AspectFill;
                        CreatePhotoImageTouchGesture(img, f.FilePath);

                        byte[] fileContents = File.ReadAllBytes(f.FilePath);
                        img.Source = ImageSource.FromStream(() => new MemoryStream(fileContents));

                        stackPhotos.Children.Add(img);
                    }
                    //btnView.IsVisible = true;
                }
            }
            catch
            { }

            previouslyShownDate = currentlyShownDate;


            // Complete the animation
            //
            if (translateToX != 0)
            {
                for (int i = 0; i < 5; i++)
                    RotateSelectedImageEnd(discomfortImages[i], translateToX);

                //hwDetails.TranslateTo(0, 0, 1, 100);
                //hwDiscomfort.TranslationX = -translateToX;
                //await hwDiscomfort.TranslateTo(0, 0, 1, 200);
            }

            loading = false;

            imgPrevDay.IsVisible = true;
            imgNextDay.IsVisible = currentlyShownDate != DateTime.Today;

        }


        async void PopOutAlert(String e)
        {
            await App.Current.MainPage.DisplayAlert("", e, "OK");
        }

        /// <summary>
        /// Saves the patient record, using all the fields from the
        /// screen.
        /// </summary>
        /// <param name="patientID">Patient I.</param>
        public async void SavePestRecord(DateTime pestDate)
        {
            int patientID = App.CurrentPatientID;
            string date = pestDate.ToString("yyyy/MM/dd");
            dPest pest = Task.Run(() => BLL.GetPest(patientID, date)).Result;
            //dPest pest = BLL.GetPestAsync(patientID, date);
            if (pest == null)
            {
                int year = pestDate.Year;
                int month = pestDate.Month;
                int day = pestDate.Day;
                //string idStr = year.ToString() + month.ToString() + day.ToString();
                string idStr = month.ToString() + day.ToString();
                //int id = Convert.ToInt32(idStr.Trim());
                pest = new dPest();
                pest.ID = App.PestID;
                //pest.ID = -1;
                pest.PatientID = patientID;
                pest.Date = date;
            }

            pest.Notes = tbNotes.Text;
            pest.ActionTaken =
                (chkActionSteroid.IsToggled ? "S" : "-") +
                (chkActionMoisturizer.IsToggled ? "M" : "-") +
                (chkActionOthers.IsToggled ? "O" : "-")
                ;
            pest.Severity = GetDiscomfortLevel();

            if (pest.Severity == 0)
            {
                hwDetails.IsVisible = false;
                stackDetails.IsVisible = false;
                stackHelpText.IsVisible = true;
                imgDelete.IsVisible = false;
                lblDelete.IsVisible = false;


            }
            else
            {
                hwDetails.IsVisible = true;
                stackDetails.IsVisible = true;
                stackHelpText.IsVisible = false;
                imgDelete.IsVisible = true;
                lblDelete.IsVisible = true;

            }

            if (pest.Severity != 0)
            {
                if (pest.ID == -1)
                    await Task.Run(() => BLL.InsertPestRecord(pest));
                else
                    await Task.Run(() => BLL.UpdatePestRecord(pest));
            }

            this.currentPest = pest;

            if (!stackDetails.IsVisible)
            {
                hwDetails.IsVisible = true;
                //hwDetails.Opacity = 0;
                hwDetails.TranslationY = 10;
                stackDetails.IsVisible = true;
                stackHelpText.IsVisible = false;
                //hintHelpText.IsVisible = false;
                //await hwDetails.TranslateTo(0, 0, 1, 100);
            }

        }


        /// <summary>
        /// Saves the patient record, using all the fields from the
        /// screen.
        /// </summary>
        /// <param name="patientID">Patient I.</param>
        public void SavePestRecord()
        {
            SavePestRecord(currentlyShownDate);
        }
    }
}

