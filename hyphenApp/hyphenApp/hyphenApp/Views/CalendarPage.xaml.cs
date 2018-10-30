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
	public partial class CalendarPage : ContentPage
	{
        bool loading = false;
        DateTime previouslySelectedMonthYear = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        List<Label> lblDates = new List<Label>();
        List<Image> imgPestDates = new List<Image>();
        List<StackLayout> stackDates = new List<StackLayout>();
        //public static DateTime currentlyShownDate;

        int displayWidth;

        public CalendarPage()
        {
            InitializeComponent();

            //Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            //{
            //    lblTips.Text = App.CurrentTips;
            //    return true; // True = Repeat again, False = Stop the timer
            //});
            //DependencyService.Get<IAnalytics>().GAScreen("View Eczema Calendar");

            //displayWidth = App.Device.GetDisplayResolution().Width;
            displayWidth = (int)App.ScreenWidth;

            try
            {
                lblDay01.Text = AppResources.PatientCalendarTabView_ShortSunday;
                lblDay02.Text = AppResources.PatientCalendarTabView_ShortMonday;
                lblDay03.Text = AppResources.PatientCalendarTabView_ShortTuesday;
                lblDay04.Text = AppResources.PatientCalendarTabView_ShortWednesday;
                lblDay05.Text = AppResources.PatientCalendarTabView_ShortThursday;
                lblDay06.Text = AppResources.PatientCalendarTabView_ShortFriday;
                lblDay07.Text = AppResources.PatientCalendarTabView_ShortSaturday;
            }
            catch
            { }

            // Create the dates in the calendar
            //
            for (int r = 0; r < 6; r++)
            {
                for (int d = 0; d < 7; d++)
                {
                    StackLayout stackDate = new StackLayout();
                    stackDate.Padding = new Thickness(5, 5, 5, 5);
                    stackDate.Spacing = 2;
                    stackDate.Orientation = StackOrientation.Vertical;

                    Label lblDate = new Label();
                    lblDate.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    lblDate.HorizontalTextAlignment = TextAlignment.Center;
                    lblDate.VerticalOptions = LayoutOptions.Start;
                    lblDate.FontSize = 16;
                    lblDate.HeightRequest = 18;
                    stackDate.Children.Add(lblDate);

                    Image imgPestDate = new Image();
                    imgPestDate.Source = "";
                    lblDate.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    imgPestDate.HeightRequest = 16;
                    imgPestDate.Aspect = Aspect.AspectFit;
                    stackDate.Children.Add(imgPestDate);

                    gridCalendar.Children.Add(stackDate, d, r * 2 + 3);

                    imgPestDates.Add(imgPestDate);
                    lblDates.Add(lblDate);
                    stackDates.Add(stackDate);

                    // Set the event so that when clicked, redirect to the 
                    // pest record page.
                    // 
                    TapGestureRecognizer tr = new TapGestureRecognizer();
                    tr.Tapped += (object sender, EventArgs e) =>
                    {
                        if (lblDate.Text == "")
                            return;

                        try
                        {
                            int month = pickMonth.SelectedIndex + 1;
                            int year = Convert.ToInt32(pickYear.Items[pickYear.SelectedIndex]);


                            int day = Convert.ToInt32(lblDate.Text);
                            DateTime date = new DateTime(year, month, day);

                            // Don't allow user to select a future date.
                            if (date <= DateTime.Today)
                            {
                                EczemaPage.currentlyShownDate = date;

                                var masterPage = App.mainPage as TabbedPage;
                                masterPage.CurrentPage = masterPage.Children[0]; //Go to Page2
                                masterPage.CurrentPage.Focus();
                                //PatientEczemaTabView.currentlyShownDate = date;
                                //App.PatientTabPage.LoadTabContent(PatientTabPage.PestTabView, "");
                            }
                        }
                        catch
                        {
                            EczemaPage.currentlyShownDate = DateTime.Today;
                            LoadAndUpdatePage("");
                            LoadCurrentPatientAndDisplay(App.CurrentPatientID);
                        }
                    };

                    //stack.Spacing = 2;
                    stackDate.GestureRecognizers.Add(tr);
                }
            }


            //  Populate the month picker.
            //
            DateTime currentMonthYear = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            string[] monthsString = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            //monthsString[0] = AppResources.Common_Month01;
            //monthsString[1] = AppResources.Common_Month02;
            //monthsString[2] = AppResources.Common_Month03;
            //monthsString[3] = AppResources.Common_Month04;
            //monthsString[4] = AppResources.Common_Month05;
            //monthsString[5] = AppResources.Common_Month06;
            //monthsString[6] = AppResources.Common_Month07;
            //monthsString[7] = AppResources.Common_Month08;
            //monthsString[8] = AppResources.Common_Month09;
            //monthsString[9] = AppResources.Common_Month10;
            //monthsString[10] = AppResources.Common_Month11;
            //monthsString[11] = AppResources.Common_Month12;

            //monthsString[0] = "01";
            //monthsString[1] = "02";
            //monthsString[2] = "03";
            //monthsString[3] = "04";
            //monthsString[4] = "05";
            //monthsString[5] = "06";
            //monthsString[6] = "07";
            //monthsString[7] = "08";
            //monthsString[8] = "09";
            //monthsString[9] = "10";
            //monthsString[10] = "11";
            //monthsString[11] = "12";

            for (int i = 0; i < 12; i++)
            {
                pickMonth.Items.Add(monthsString[i]);
                if (i == currentMonthYear.Month - 1)
                    pickMonth.SelectedIndex = i;
            }

            //  Populate the year picker.
            //
            DateTime currentYear = DateTime.Today.AddYears(-14);
            int index = currentMonthYear.Year - currentYear.Year;
            if (index > 14)
                index = 14;
            if (index < 0)
                index = 0;
            for (int i = 0; i < 15; i++)
            {
                pickYear.Items.Add(currentYear.ToString("yyyy"));
                currentYear = currentYear.AddYears(1);
            }
            pickYear.SelectedIndex = index;

            bool alwaysUpdate = true;

            pickMonth.SelectedIndexChanged += (object sender, EventArgs e) =>
            {
                if (!loading)
                {
                    if (alwaysUpdate)
                    {
                        AddMonthYear(0);
                        LoadAndUpdatePage("");
                    }
                }
            };

            pickYear.SelectedIndexChanged += (object sender, EventArgs e) =>
            {
                if (!loading)
                {
                    if (alwaysUpdate)
                    {
                        AddMonthYear(0);
                        LoadAndUpdatePage("");
                    }
                }
            };

            btnPrevMonth.Clicked += (object sender, EventArgs e) =>
            {
                if (!loading)
                {
                    alwaysUpdate = false;
                    AddMonthYear(-1);
                    LoadAndUpdatePage("");
                    alwaysUpdate = true;
                }
            };

            btnNextMonth.Clicked += (object sender, EventArgs e) =>
            {
                if (!loading)
                {
                    alwaysUpdate = false;
                    AddMonthYear(+1);
                    LoadAndUpdatePage("");
                    alwaysUpdate = true;
                }
            };

            //initLocalizedText();

            string email = Task.Run(() => BLL.GetUserEmailID()).Result;
            if (email == null)
            {
            }
            //else if (tbName.Text == "")
            //{
            //}
            else
            {
                if (App.UserCreated)
                {
                    LoadCurrentPatientAndDisplay(App.CurrentPatientID);
                }
            }
        }
        //bool alwaysUpdate = true;

        //private void PrevMonth_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        //{
        //    if (!loading)
        //    {
        //        alwaysUpdate = false;
        //        AddMonthYear(-1);
        //        LoadAndUpdatePage("");
        //        alwaysUpdate = true;
        //    }
        //}
        //void NextMonth_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        //{
        //    if (!loading)
        //    {
        //        alwaysUpdate = false;
        //        AddMonthYear(+1);
        //        LoadAndUpdatePage("");
        //        alwaysUpdate = true;
        //    }
        //}

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

        private void initLocalizedText()
        {
            lblTapDate.Text = AppResources.PatientCalendarTabView_TapDate;
            mood1Label.Text = AppResources.Common_Mood1;
            mood2Label.Text = AppResources.Common_Mood2;
            mood3Label.Text = AppResources.Common_Mood3;
            mood4Label.Text = AppResources.Common_Mood4;
            mood5Label.Text = AppResources.Common_Mood5;

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



        private void AddMonthYear(int monthsToAdd)
        {
            int month = pickMonth.SelectedIndex + 1;
            int year = Convert.ToInt32(pickYear.Items[pickYear.SelectedIndex]);
            DateTime currentMonth = new DateTime(year, month, 1).AddMonths(monthsToAdd);

            EczemaPage.currentlyShownDate = currentMonth;

            if (monthsToAdd != 0)
            {
                pickMonth.SelectedIndex = currentMonth.Month - 1;

                for (int i = 0; i < pickYear.Items.Count; i++)
                {
                    if (pickYear.Items[i] == currentMonth.Year.ToString())
                    {
                        pickYear.SelectedIndex = i;
                        break;
                    }
                }
            }
        }


        public void LoadAndUpdatePage(string data)
        {
            loading = true;

            try
            {
                if (EczemaPage.currentlyShownDate == null)
                {
                    EczemaPage.currentlyShownDate = DateTime.Now;
                }

                int month = EczemaPage.currentlyShownDate.Month;
                int year = EczemaPage.currentlyShownDate.Year;

                pickMonth.SelectedIndex = month - 1;

                for (int i = 0; i < pickYear.Items.Count; i++)
                {
                    if (pickYear.Items[i] == year.ToString())
                    {
                        pickYear.SelectedIndex = i;
                        break;
                    }
                }

                int patientID = App.CurrentPatientID;
                List<dPest> pestRecords = Task.Run(() => BLL.GetPestHistoryByPatientIDMonthAndYear(patientID, month, year)).Result;

                DateTime currentlySelectedMonth = new DateTime(year, month, 1);
                DateTime currentlySelectedMonthEnd = currentlySelectedMonth.AddMonths(1).AddDays(-1);

                List<dPestphoto> pestPhotos = Task.Run(() => BLL.GetPestPhotos(patientID,
                    currentlySelectedMonth.ToString("yyyy/MM/dd"), currentlySelectedMonthEnd.ToString("yyyy/MM/dd"))).Result;

                btnPrevMonth.Text = "< " + currentlySelectedMonth.AddMonths(-1).ToString("MMM yyyy");
                btnNextMonth.Text = currentlySelectedMonth.AddMonths(+1).ToString("MMM yyyy") + " >";

                AnimateCalendar(currentlySelectedMonth, pestRecords, pestPhotos);
            }
            catch
            { }

            loading = false;
        }

        private async void AnimateCalendar(DateTime currentlySelectedMonth, List<dPest> pestRecords, List<dPestphoto> pestPhotos)
        {
            btnPrevMonth.IsVisible = false;
            btnNextMonth.IsVisible = false;

            int[] photoCount = new int[40];

            if (pestPhotos != null)
            {
                foreach (var pestPhoto in pestPhotos)
                {
                    int day = Convert.ToInt32(pestPhoto.Day);
                    photoCount[day - 1]++;
                }
            }

           
            int[] pestLevel = new int[40];
            if (pestRecords != null)
            {
                foreach (var pest in pestRecords)
                {
                    int day = Convert.ToInt32(pest.Day);
                    pestLevel[day - 1] = pest.Severity;
                }
            }

            //Disallow the user from moving to a future month

            DateTime todayMonthYear = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            //Check and see whether the new date has moved forward / backward
            //so that we can decide on how to animate the calendar.

            int translateToX = 0;
            if (previouslySelectedMonthYear < currentlySelectedMonth)
                translateToX = -20;
            else if (previouslySelectedMonthYear > currentlySelectedMonth)
                translateToX = +20;
            else if (previouslySelectedMonthYear == currentlySelectedMonth)
                translateToX = 00;

            if (translateToX != 0)
            {
                //await hwCalendar.TranslateTo(translateToX, 0, 0, 100, Easing.Linear);
                await hwCalendar.FadeTo(0, 200);
            }

            //Update the dates in the calendar

            int startDay = (int)currentlySelectedMonth.DayOfWeek;
            int daysInMonth = currentlySelectedMonth.AddMonths(1).AddDays(-1).Day;
            for (int i = 0; i < lblDates.Count; i++)
            {
                if (i - startDay >= 0 && i - startDay < daysInMonth)
                {
                    lblDates[i].Text = (i - startDay + 1).ToString();

                    DateTime calendarDate = new DateTime(currentlySelectedMonth.Year, currentlySelectedMonth.Month, i - startDay + 1);
                    if (calendarDate == DateTime.Today)
                        lblDates[i].TextColor = Xamarin.Forms.Color.Blue;
                    else if (calendarDate <= DateTime.Today)
                        lblDates[i].TextColor = Xamarin.Forms.Color.Black;
                    else
                        lblDates[i].TextColor = Xamarin.Forms.Color.FromHex("dddddd");

                    string imageName = "pest" + pestLevel[i - startDay] + "small";

                    if (photoCount[i - startDay] > 0)
                        imageName += "ph";

                    imgPestDates[i].Source = imageName;
                }
                else
                {
                    lblDates[i].TextColor = Xamarin.Forms.Color.Black;
                    lblDates[i].Text = "";
                    imgPestDates[i].Source = "";
                }
            }
            gridCalendar.ForceLayout();

            previouslySelectedMonthYear = currentlySelectedMonth;


            //Complete the animation

            if (translateToX != 0)
            {
                hwCalendar.TranslationX = -translateToX;
                hwCalendar.TranslationY = 0;
                //await hwCalendar.TranslateTo(0, 0, 1, 100, Easing.Linear);
                await hwCalendar.FadeTo(1, 200);
            }


            btnPrevMonth.IsVisible = true;
            btnNextMonth.IsVisible = currentlySelectedMonth < todayMonthYear;
        }

        override protected void OnAppearing()
        {
            string email = Task.Run(() => BLL.GetUserEmailID()).Result;
            if (email == null)
            {
            }
            else if (!Task.Run(() => BLL.IsPatient()).Result)
            {
            }
            else if (Task.Run(() => BLL.IsPest()).Result)
            {
                try
                {
                    if (App.UserCreated)
                    {
                        //LoadAndUpdatePage("");
                        LoadCurrentPatientAndDisplay(App.CurrentPatientID);
                        //initLocalizedText();
                    }
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Use this to react to orientation change.
        /// </summary>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (gridMoods == null)
                return;

            if (width > height)
            {
                 //landscape mode
                gridMoods.HorizontalOptions = LayoutOptions.FillAndExpand;
                gridMoods.RowDefinitions[0].Height = 30;
                gridCalendar.RowDefinitions[3].Height = 30;
                gridCalendar.RowDefinitions[5].Height = 30;
                gridCalendar.RowDefinitions[7].Height = 30;
                gridCalendar.RowDefinitions[9].Height = 30;
                gridCalendar.RowDefinitions[11].Height = 30;
                gridCalendar.RowDefinitions[13].Height = 30;
                stackMood1.Orientation = StackOrientation.Horizontal;
                stackMood2.Orientation = StackOrientation.Horizontal;
                stackMood3.Orientation = StackOrientation.Horizontal;
                stackMood4.Orientation = StackOrientation.Horizontal;
                stackMood5.Orientation = StackOrientation.Horizontal;
                for (int i = 0; i < stackDates.Count; i++)
                {
                    stackDates[i].Orientation = StackOrientation.Horizontal;
                    lblDates[i].VerticalOptions = LayoutOptions.CenterAndExpand;
                    lblDates[i].HorizontalOptions = LayoutOptions.Start;
                    stackDates[i].Padding = new Thickness(gridCalendar.Width / 14 - 5, 5, 5, 5);
                }
            }
            else
            {
                 //portrait mode
                gridMoods.HorizontalOptions = LayoutOptions.CenterAndExpand;
                gridMoods.RowDefinitions [0].Height = 55;

                if (App.LanguageCode == null ||
                    App.LanguageCode == "" ||
                    App.LanguageCode == "en" ||
                    App.LanguageCode == "tl" ||
                    App.LanguageCode == "fil")
                {
                    gridMoods.RowDefinitions[0].Height = 55;
                }
                else if (App.LanguageCode == "zh-CN" ||
                         App.LanguageCode == "zh-TW")
                {
                    gridMoods.RowDefinitions[0].Height = 45;
                }
                else
                {
                    gridMoods.RowDefinitions[0].Height = 80;
                }

                 //Adjust height of calendar cells
                
                int cellHeight = Helpers.ScaleSizeByWidth(48, 0.8);

                gridCalendar.RowDefinitions[3].Height = cellHeight;
                gridCalendar.RowDefinitions[5].Height = cellHeight;
                gridCalendar.RowDefinitions[7].Height = cellHeight;
                gridCalendar.RowDefinitions[9].Height = cellHeight;
                gridCalendar.RowDefinitions[11].Height = cellHeight;
                gridCalendar.RowDefinitions[13].Height = cellHeight;

                int imageWidth = Helpers.ScaleSizeByWidth(16, 0.8);
                int fontSize = Helpers.ScaleSizeByWidth(16, 0.8);
                int labelHeight = Helpers.ScaleSizeByWidth(18, 0.8);

                for (int i = 0; i < imgPestDates.Count; i++)
                {
                    lblDates[i].FontSize = fontSize;
                    lblDates[i].HeightRequest = labelHeight;
                    imgPestDates[i].WidthRequest = imageWidth;
                    imgPestDates[i].HeightRequest = imageWidth;
                }

                stackMood1.Orientation = StackOrientation.Vertical;
                stackMood2.Orientation = StackOrientation.Vertical;
                stackMood3.Orientation = StackOrientation.Vertical;
                stackMood4.Orientation = StackOrientation.Vertical;
                stackMood5.Orientation = StackOrientation.Vertical;
                for (int i = 0; i < stackDates.Count; i++)
                {
                    stackDates[i].Orientation = StackOrientation.Vertical;
                    lblDates[i].VerticalOptions = LayoutOptions.Start;
                    lblDates[i].HorizontalOptions = LayoutOptions.CenterAndExpand;
                    stackDates[i].Padding = new Thickness(5, 5, 5, 5);
                }
            }
        }
    }
}

