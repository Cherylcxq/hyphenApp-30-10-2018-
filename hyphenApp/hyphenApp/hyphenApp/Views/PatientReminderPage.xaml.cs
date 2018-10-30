using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;

namespace hyphenApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientReminderPage : ContentPage
    {
        bool updateTime = false;
        CheckBox[] dayOfWeekCheckBoxes;
        StackLayout[] dayOfWeekStackLayouts;
        TimeOfDay currentlyEditingTimeOfDay = null;

        int reminderID = 0;

        public static int id = 0;

        ObservableCollection<TimeOfDay> timeOfDayList = new ObservableCollection<TimeOfDay>();

        public PatientReminderPage(int reminderID)
        {
            InitializeComponent();

            this.reminderID = reminderID;

            dReminder.ReminderTypeOptions = new string[] {
                    AppResources.dReminder_RateEczema,
                    AppResources.dReminder_ApplySteroids,
                    AppResources.dReminder_ApplyMoisturisers,
                    AppResources.dReminder_OtherReminder };
            dReminder.FrequencyTypeOptions = new string[] { AppResources.dReminder_Day, AppResources.dReminder_Week };

            dayOfWeekCheckBoxes = new CheckBox[] { chkSun, chkMon, chkTue, chkWed, chkThur, chkFri, chkSat };
            dayOfWeekStackLayouts = new StackLayout[] { btnSun, btnMon, btnTue, btnWed, btnThur, btnFri, btnSat };

            dPatient currentPatient = App.CurrentPatient;
            if (currentPatient != null)
            {
                //topBar.Subtitle = currentPatient.Name;
                //topBar.ProfileImageSource = Helpers.GetPlatformSpecificPath(currentPatient.ProfilePicturePath, "photos/");
            }

            //Current Date
            DateTime now = DateTime.Now;

            if (reminderID != 0)
            {
                //topBar.RightButtonSource = "iconblackdelete";
                //topBar.Title = AppResources.PatientReminderPage_TitleUpdateReminder;
                //DependencyService.Get<IAnalytics>().GAScreen("Update Reminder");

            }
            else
            {

                ToolbarItems.RemoveAt(1);
                //topBar.RightButtonSource = "";
                //topBar.Title = AppResources.PatientReminderPage_TitleAddReminder;
                //DependencyService.Get<IAnalytics>().GAScreen("Add Reminder");
            }

            // Show the time picker for editing.
            //
            lvTimesOfDay.ItemTapped += (object sender, ItemTappedEventArgs e) =>
            {
                var tap = sender as ListView;

                TimeOfDay timeOfDay = tap.SelectedItem as TimeOfDay;
                tap.SelectedItem = null;

                if (timeOfDay != null)
                {
                    currentlyEditingTimeOfDay = timeOfDay;
                    updateTime = false;
                    timeTimeOfDay.Time = timeOfDay.Time.Subtract(timeOfDay.Time.Date);
                    updateTime = true;
                    timeTimeOfDay.Unfocus();
                    timeTimeOfDay.Focus();
                }
            };

            // Populate the pickers
            //
            foreach (var opt in dReminder.ReminderTypeOptions)
                pickReminderType.Items.Add(opt);
            foreach (var opt in dReminder.FrequencyTypeOptions)
                pickFrequencyType.Items.Add(opt);
            for (int i = 1; i < 30; i++)
                pickFrequency.Items.Add(i.ToString());


            // Focus the timepicker, so that we can show the Time Picker.
            //
            btnAdd.Clicked += (object sender, EventArgs e) =>
            {
                currentlyEditingTimeOfDay = null;
                updateTime = true;
                timeTimeOfDay.Unfocus();
                timeTimeOfDay.Focus();
            };


            // When time is changed
            //
            DateTime defaultTime = new DateTime(1, 1, 1, timeTimeOfDay.Time.Hours, timeTimeOfDay.Time.Minutes, 1);
            updateTime = false;
            timeTimeOfDay.Time = defaultTime.TimeOfDay;

            if (Device.OS == TargetPlatform.Android)
            {
                timeTimeOfDay.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == TimePicker.TimeProperty.PropertyName && updateTime)
                        AddOrUpdateTime();
                };
            }
            else
            {
                timeTimeOfDay.Unfocused += (object sender, FocusEventArgs e) =>
                {
                    AddOrUpdateTime();
                };
            }


            pickReminderType.SelectedIndexChanged += (sender, e) =>
            {
                if (Convert.ToString(dReminder.ReminderTypeCodes[pickReminderType.SelectedIndex]) == "OT")
                {
                    OthersStack.IsVisible = true;
                }
                else
                {
                    txtOthers.Text = null;
                    OthersStack.IsVisible = false;
                }
            };


            pickFrequencyType.SelectedIndexChanged += (sender, e) =>
            {

                if (dReminder.FrequencyTypeCodes[pickFrequencyType.SelectedIndex] == "D")
                {
                    stackWeek.IsVisible = false;
                    //tvMonth.IsVisible = false;
                }
                else if (dReminder.FrequencyTypeCodes[pickFrequencyType.SelectedIndex] == "W")
                {
                    stackWeek.IsVisible = true;
                    //tvMonth.IsVisible = false;
                }
            };



            //topBar.RightButtonClicked += async (sender, e) =>
            //{
            //    string result = await DisplayActionSheet(
            //        AppResources.PatientReminderPage_DeleteReminder,
            //        AppResources.Common_OptionNo,
            //        AppResources.Common_OptionYes);
            //    if (result == null || result.Equals(AppResources.Common_OptionNo))
            //        return;


            //    DeleteReminder(reminderID);

            //    if (App.PatientTabPage.CurrentView is PatientReminderListTabView)
            //    {
            //        ((PatientReminderListTabView)App.PatientTabPage.CurrentView).LoadAndUpdatePage("");
            //    }

            //    Navigation.PopModalAsync();

            //};

            // Set event to toggle the checkbox.
            //
            for (int i = 0; i < this.dayOfWeekCheckBoxes.Length; i++)
            {
                var tr = new TapGestureRecognizer();
                CheckBox chkDayOfWeek = this.dayOfWeekCheckBoxes[i];
                tr.Tapped += (object sender, EventArgs e) =>
                {
                    chkDayOfWeek.Checked = !chkDayOfWeek.Checked;
                };
                this.dayOfWeekStackLayouts[i].GestureRecognizers.Add(tr);
            }


            LoadReminder(reminderID);

            initLocalizedText();
        }
        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayActionSheet(
                AppResources.PatientReminderPage_DeleteReminder,
                AppResources.Common_OptionNo,
                AppResources.Common_OptionYes);
            if (result == null || result.Equals(AppResources.Common_OptionNo))
                return;

            DeleteReminder(reminderID);

            await Navigation.PopModalAsync();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if (dateStartDate.Date > dateEndDate.Date)
            {
                await DisplayAlert(
                    AppResources.Common_ErrorTitle,
                    AppResources.PatientReminderPage_ErrorInvalideStartEndDates,
                    AppResources.Common_OK);
                return;
            }
            if (timeOfDayList.Count == 0)
            {
                await DisplayAlert(
                    AppResources.Common_ErrorTitle,
                    AppResources.PatientReminderPage_ErrorMissingTimeOfDay,
                    AppResources.Common_OK);
                return;
            }
            if (pickFrequencyType.SelectedIndex == 1 &&
                !chkMon.Checked && !chkTue.Checked && !chkWed.Checked &&
                !chkThur.Checked && !chkFri.Checked && !chkSat.Checked && !chkSun.Checked)
            {
                await DisplayAlert(
                    AppResources.Common_ErrorTitle,
                    AppResources.PatientReminderPage_ErrorMissingDayOfWeek,
                    AppResources.Common_OK);
                return;
            }
            if (dReminder.ReminderTypeCodes[pickReminderType.SelectedIndex] == "OT" &&
                (txtOthers.Text == null || txtOthers.Text == ""))
            {
                await DisplayAlert(
                    AppResources.Common_ErrorTitle,
                    AppResources.PatientReminderPage_ErrorMissingReminderDescription,
                    AppResources.Common_OK);
                return;
            }

            if (reminderID == 0)
            {
                reminderID = Task.Run(() => BLL.GetRemindersNextID()).Result;
            }

            SaveReminder(reminderID);
            await Navigation.PopModalAsync();
        }

        private void initLocalizedText()
        {
            //topBar.Title = AppResources.PatientReminderPage_Title;


            txtOthers.Placeholder = AppResources.PatientReminderPage_OthersPlaceHolder;
            lblRemindMeTo.Text = AppResources.PatientReminderPage_RemindMeTo;
            lblFrequency.Text = AppResources.PatientReminderPage_Frequency;
            lblStarts.Text = AppResources.PatientReminderPage_Starts;
            lblEnds.Text = AppResources.PatientReminderPage_Ends;
            lblRemindEvery.Text = AppResources.PatientReminderPage_RemindEvery;
            lblTimesOfTheDay.Text = AppResources.PatientReminderPage_TimesOfTheDay + "*";
            btnAdd.Text = AppResources.PatientReminderPage_AddTime;
            lblDaysOfTheWeek.Text = AppResources.PatientReminderPage_DaysOfTheWeek + "*";
            lblSunday.Text = AppResources.Common_Sunday;
            lblMonday.Text = AppResources.Common_Monday;
            lblTuesday.Text = AppResources.Common_Tuesday;
            lblWednesday.Text = AppResources.Common_Wednesday;
            lblThursday.Text = AppResources.Common_Thursday;
            lblFriday.Text = AppResources.Common_Friday;
            lblSaturday.Text = AppResources.Common_Saturday;
            dateStartDate.Format = Helpers.GetShortDateFormat(App.LanguageCode);
            dateEndDate.Format = Helpers.GetShortDateFormat(App.LanguageCode);

            pickReminderType.FontSize = 16;

        }


        /// <summary>
        /// Adds the time to the list.
        /// </summary>
        private void AddOrUpdateTime()
        {
            if (timeOfDayList.Count >= 5 && currentlyEditingTimeOfDay == null)
            {
                // Don't allow the user to add more than 5 timings.
                //
                return;
            }
            else
            {
                if (currentlyEditingTimeOfDay != null)
                {
                    int currentlyEditingTimeOfDayIndex = timeOfDayList.IndexOf(currentlyEditingTimeOfDay);
                    timeOfDayList.RemoveAt(currentlyEditingTimeOfDayIndex);
                    currentlyEditingTimeOfDay = null;
                }

                TimeSpan t = timeTimeOfDay.Time;
                DateTime timeToAdd = new DateTime(1, 1, 1, t.Hours, t.Minutes, 0);

                DateTime prevTimeInList = new DateTime(1, 1, 1, 0, 0, 0);
                int indexToAdd = -1;
                for (int i = 0; i < timeOfDayList.Count; i++)
                {
                    if (timeOfDayList[i].Time == timeToAdd)
                    {
                        indexToAdd = -2;
                        break;
                    }

                    if (prevTimeInList <= timeToAdd && timeToAdd < timeOfDayList[i].Time)
                    {
                        indexToAdd = i;
                        break;
                    }
                    prevTimeInList = timeOfDayList[i].Time;
                }
                if (indexToAdd >= 0)
                    timeOfDayList.Insert(indexToAdd, new TimeOfDay(timeToAdd));
                else if (indexToAdd == -1)
                    timeOfDayList.Add(new TimeOfDay(timeToAdd));

                RefreshTimeOfDayList();

                lvTimesOfDay.HeightRequest = (lvTimesOfDay.RowHeight + 2) * timeOfDayList.Count;
                lvTimesOfDay.ItemsSource = timeOfDayList;

                btnAdd.IsVisible = (timeOfDayList.Count < 5);
            }
        }


        /// <summary>
        /// Deletes the reminder.
        /// </summary>
        /// <param name="reminderID">Reminder I.</param>
        public async void DeleteReminder(int reminderID)
        {
            await Task.Run(() => BLL.DeleteReminderRecord(reminderID));
            //BLL.DeleteRecord("dReminder", reminderID);
        }




        /// <summary>
        /// Loads the reminder and display it to the screen.
        /// </summary>
        /// <param name="selectedId">Selected identifier.</param>
        public void LoadReminder(int reminderID)
        {
            dReminder r = new dReminder();
            if (reminderID != 0)
                r = Task.Run(() => BLL.GetReminder(reminderID)).Result;
            //r = BLL.GetReminder(reminderID);
            else
            {
                r.Enabled = 1;
                r.StartDate = DateTime.Today.ToString("yyyy/MM/dd");
                r.EndDate = DateTime.Today.AddMonths(12).ToString("yyyy/MM/dd");
                r.Frequency = 1;
                r.FrequencyType = "D";
                r.ReminderType = "RA";
            }

            switchEnabled.IsToggled = (r.Enabled == 1);
            pickReminderType.SelectedIndex = Helpers.FindStringInArray(dReminder.ReminderTypeCodes, r.ReminderType);
            pickFrequency.SelectedIndex = r.Frequency - 1;
            pickFrequencyType.SelectedIndex = Helpers.FindStringInArray(dReminder.FrequencyTypeCodes, r.FrequencyType);

            if (r.ReminderType == "OT")
            {
                OthersStack.IsVisible = true;
                txtOthers.Text = r.Notes;
            }
            else
                OthersStack.IsVisible = false;

            try
            {
                dateStartDate.Date = DateTime.ParseExact(r.StartDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
            }
            try
            {
                dateEndDate.Date = DateTime.ParseExact(r.EndDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
            }

            // Show the list of times.
            //
            string timesOfDayStr = r.TimesOfDay ?? "";
            timeOfDayList.Clear();
            foreach (string timeStr in timesOfDayStr.Split(','))
            {
                try
                {
                    DateTime timeFromDatabase = DateTime.ParseExact(timeStr, "HH:mm",
                        System.Globalization.CultureInfo.InvariantCulture);

                    DateTime timeToAdd = new DateTime(1, 1, 1, timeFromDatabase.Hour, timeFromDatabase.Minute, 0);
                    timeOfDayList.Add(new TimeOfDay(timeToAdd));
                }
                catch
                {
                }
            }
            lvTimesOfDay.HeightRequest = (lvTimesOfDay.RowHeight + 2) * timeOfDayList.Count;
            lvTimesOfDay.ItemsSource = timeOfDayList;

            // Show the day of weeks
            //
            stackWeek.IsVisible = (r.FrequencyType == "W");
            if (stackWeek.IsVisible)
            {
                for (int i = 0; i < dayOfWeekCheckBoxes.Length; i++)
                {
                    dayOfWeekCheckBoxes[i].Checked = (r.DaysOfWeek[i] == '1');
                }
            }

        }


        /// <summary>
        /// Saves the reminder into the database.
        /// </summary>
        /// <param name="reminderID">Reminder I.</param>
        public async void SaveReminder(int reminderID)
        {
            // Create the object and save into the database
            //
            var r = new dReminder();
            r.ID = reminderID;
            r.Enabled = switchEnabled.IsToggled ? 1 : 0;
            r.Notes = txtOthers.Text;
            r.PatientID = App.CurrentPatientID;
            r.ReminderType = dReminder.ReminderTypeCodes[pickReminderType.SelectedIndex];
            r.StartDate = dateStartDate.Date.ToString("yyyy/MM/dd");
            r.EndDate = dateEndDate.Date.ToString("yyyy/MM/dd");
            r.Frequency = pickFrequency.SelectedIndex + 1;
            r.FrequencyType = dReminder.FrequencyTypeCodes[pickFrequencyType.SelectedIndex];

            string timesOfDayStr = "";
            foreach (var timeOfDay in timeOfDayList)
                timesOfDayStr += (timesOfDayStr == "" ? "" : ",") + timeOfDay.Time.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            r.TimesOfDay = timesOfDayStr;

            string daysOfWeekStr = "";
            if (r.FrequencyType == "W")
            {
                for (int i = 0; i < dayOfWeekCheckBoxes.Length; i++)
                {
                    if (dayOfWeekCheckBoxes[i].Checked)
                        daysOfWeekStr += "1";
                    else
                        daysOfWeekStr += "0";
                }
            }
            r.DaysOfWeek = daysOfWeekStr;

            //if (reminderID == 0)
            await Task.Run(() => BLL.InsertReminderRecord(r));
            //else
            //    await Task.Run(() => BLL.UpdateReminderRecord(r));
        }



        /// <summary>
        /// Deletes the selected time of day.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        /// 


        public void DeleteTimeOfDay(object sender, EventArgs e)
        {
            var view = sender as View;
            string timeOfDayString = (string)((TapGestureRecognizer)view.GestureRecognizers[0]).CommandParameter;


            for (int i = 0; i < timeOfDayList.Count; i++)
            {
                if (timeOfDayList[i].TimeIn12HourFormat == timeOfDayString)
                {
                    timeOfDayList.RemoveAt(i);
                    break;
                }
            }
            RefreshTimeOfDayList();

            lvTimesOfDay.ItemsSource = timeOfDayList;
            lvTimesOfDay.HeightRequest = (lvTimesOfDay.RowHeight + 2) * timeOfDayList.Count;

            btnAdd.IsVisible = true;
        }


        /// <summary>
        /// Refreshes the entire time of day list.
        /// THis is required for Android, otherwise it causes some 
        /// update issues.
        /// </summary>
        private void RefreshTimeOfDayList()
        {
            if (Device.OS == TargetPlatform.Android)
            {
                List<TimeOfDay> timesOfDay = new List<TimeOfDay>();
                foreach (var timeOfDay in timeOfDayList)
                    timesOfDay.Add(timeOfDay);
                timeOfDayList.Clear();
                foreach (var timeOfDay in timesOfDay)
                    timeOfDayList.Add(timeOfDay);
            }


        }

    }

}


