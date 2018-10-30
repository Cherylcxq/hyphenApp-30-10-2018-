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
    public partial class ReminderPage : ContentPage
    {
        int reminderCount = 0;
        public ReminderPage()
        {
            InitializeComponent();

            //Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            //{
            //    lblTips.Text = App.CurrentTips;
            //    return true; // True = Repeat again, False = Stop the timer
            //});

            //DependencyService.Get<IAnalytics>().GAScreen("View Reminder List");


            //lvReminder.ItemTapped += (sender, e) =>
            //{
            //    if (Helpers.AlreadyHasModalPageAbove(App.PatientTabPage))
            //        return;

            //    var tap = sender as ListView;
            //    var reminder = tap.SelectedItem as dReminder;
            //    lvReminder.SelectedItem = null;
            //    Navigation.PushModalAsync(new PatientReminderPage(reminder.ID));
            //};

            LoadCurrentPatientAndDisplay(App.CurrentPatientID);
            if (App.UserCreated)
            {
                LoadAndUpdatePage("");
            }
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

        override protected void OnAppearing()
        {
            if (LoadCurrentPatientAndDisplay(App.CurrentPatientID))
            {

                if (App.UserCreated)
                {
                    LoadAndUpdatePage("");
                }
            }
        }

        async void PopOutAlert(String e)
        {
            await App.Current.MainPage.DisplayAlert("", e, "OK");
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as dReminder;
            if (item == null)
                return;

            await Navigation.PushModalAsync(new NavigationPage(new PatientReminderPage(item.ID)));

            // Manually deselect item.
            lvReminder.SelectedItem = null;
        }

        async void Add_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            lock (_tappedLockObject)
            {
                if (_tapped) return;
                _tapped = true;
            }
            if (Navigation.NavigationStack.Count == 0 || Navigation.NavigationStack.Last().GetType() != typeof(PatientReminderPage))
            {
                DateTime dt = DateTime.Now;
                //string date = dt.ToString("yyyy/MM/dd");
                int year = dt.Year;
                int month = dt.Month;
                int day = dt.Day;
                int hour = dt.Hour;
                int minute = dt.Minute;
                int second = dt.Second;
                string idStr = month.ToString() + day.ToString() + hour.ToString() + minute.ToString() + second.ToString();
                int id = Convert.ToInt32(idStr.Trim());

                int index = Task.Run(() => BLL.GetRemindersNextID()).Result;
                await Navigation.PushModalAsync(new NavigationPage(new PatientReminderPage(0)));
            }
            reenableTap();

            //DateTime dt = DateTime.Now;
            ////string date = dt.ToString("yyyy/MM/dd");
            //int year = dt.Year;
            //int month = dt.Month;
            //int day = dt.Day;
            //int hour = dt.Hour;
            //int minute = dt.Minute;
            //int second = dt.Second;
            //string idStr = month.ToString() + day.ToString() + hour.ToString() + minute.ToString() + second.ToString();
            //int id = Convert.ToInt32(idStr.Trim());

            //int index = Task.Run(() => BLL.GetRemindersNextID()).Result;

            //await Navigation.PushModalAsync(new NavigationPage(new PatientReminderPage(0)));
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

        //override protected void OnAppearing()
        //{
        //    if (LoadCurrentPatientAndDisplay(App.CurrentPatientID))
        //    {
        //        LoadAndUpdatePage("");
        //    }
        //}

        //public void SwitchToTab()
        //{
        //    App.PatientTabPage.TopBar.Title = AppResources.PatientReminderListTabView_Reminders;
        //    App.PatientTabPage.TopBar.RightButtonSource = "iconwhiteadd";
        //    App.PatientTabPage.TopBar.RemoveRightButtonEvents();
        //    App.PatientTabPage.TopBar.RightButtonClicked += (object sender, EventArgs e) =>
        //    {
        //        if (Helpers.AlreadyHasModalPageAbove(App.PatientTabPage))
        //            return;

        //        Navigation.PushModalAsync(new PatientReminderPage(0));
        //    };

        //}

        /// <summary>
        /// Loads the list of reminders given the Patient ID and
        /// binds it to the listview.
        /// </summary>
        /// <param name = "patientID" > Patient I.</param>
        public void LoadAndUpdatePage(string data)
        {
            //Force cancelling of all notifications while in the app.
            // This tries to fix the problem of notifications still firing
            // (even if the app was force closed, and for some reason
            // the cancellation was never called when the app first resumed
            // from background)
            try
            {
                dReminder.CancellAllNotifications();

                List<dReminder> reminders = Task.Run(() => BLL.GetRemindersByPatientID(App.CurrentPatientID)).Result;
                //List<dReminder> reminders = BLL.GetRemindersByPatientID(App.CurrentPatientID);

                //PopOutAlert(reminders.Count.ToString());

                if (reminders != null)
                {
                    foreach (var reminder in reminders)
                    {
                        if (reminder.ReminderType != "OT")
                            reminder.Notes = reminder.ReminderTypeString;

                    }

                    reminderCount = reminders.Count;

                    if (reminders.Count == 0)
                    {
                        lblHelpText.Text = AppResources.PatientReminderListTabView_HintHelp;
                        stackHelpText.IsVisible = true;
                        lvReminder.IsVisible = false;
                    }
                    else
                    {
                        stackHelpText.IsVisible = false;
                        lvReminder.IsVisible = true;
                    }

                    lvReminder.ItemsSource = reminders;
                }
                else
                {
                    lblHelpText.Text = AppResources.PatientReminderListTabView_HintHelp;
                    stackHelpText.IsVisible = true;
                    lvReminder.IsVisible = false;
                }
            }
            catch
            { }
        }
    }
}
