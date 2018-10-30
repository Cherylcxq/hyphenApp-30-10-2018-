using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hyphenApp
{
    public class dReminder
    {
        public static string[] ReminderTypeOptions = {
            AppResources.dReminder_RateEczema,
            AppResources.dReminder_ApplySteroids,
            AppResources.dReminder_ApplyMoisturisers,
            AppResources.dReminder_OtherReminder };
        public static string[] ReminderTypeCodes = { "RA", "AT", "AM", "OT" };

        public static string[] FrequencyTypeOptions = { AppResources.dReminder_Day, AppResources.dReminder_Week };
        public static string[] FrequencyTypeCodes = { "D", "W" };

        public static string[] DaysOfWeekShort = {
            AppResources.Common_Sunday,
            AppResources.Common_Monday,
            AppResources.Common_Tuesday,
            AppResources.Common_Wednesday,
            AppResources.Common_Thursday,
            AppResources.Common_Friday,
            AppResources.Common_Saturday
        };


        public dReminder()
        {
        }

        public int ID { get; set; }

        public int Enabled { get; set; }
        public int PatientID { get; set; }

        public string Notes { get; set; }
        public int Frequency { get; set; }
        public string FrequencyType { get; set; }
        public string TimesOfDay { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DaysOfWeek { get; set; }
        public string ReminderType { get; set; }

        public string EnabledString
        {
            get
            {
                if (this.Enabled == 1)
                    return "";
                else
                    return "\uf1f7";
            }
        }

        public string ReminderTypeString
        {
            get
            {
                int index = Helpers.FindStringInArray(dReminder.ReminderTypeCodes, this.ReminderType);
                return dReminder.ReminderTypeOptions[index];
            }
        }

        public string FrequencyTypeLabel
        {
            get
            {
                return AppResources.PatientReminderListTabView_FrequencyTypeLabel;
            }
        }

        public string TimesOfDayLabel
        {
            get
            {
                return AppResources.PatientReminderListTabView_TimesOfDayLabel;
            }
        }

        public string DaysOfWeekLabel
        {
            get
            {
                return AppResources.PatientReminderListTabView_DaysOfWeekLabel;
            }
        }

        public string FrequencyTypeString
        {
            get
            {
                int index = Helpers.FindStringInArray(dReminder.FrequencyTypeCodes, this.FrequencyType);
                return dReminder.FrequencyTypeOptions[index];
            }
        }

        public string TimesOfDayString
        {
            get
            {
                string str = "";

                if (App.LanguageCode == "vi")
                {
                    foreach (var time in TimesOfDayAsDateTimeList)
                        str += (str == "" ? "" : ", ") + time.ToString("HH:mm");
                }
                else
                {
                    foreach (var time in TimesOfDayAsDateTimeList)
                        str += (str == "" ? "" : ", ") + time.ToString("hh:mm tt");
                }

                return str;
            }
        }

        public string DaysOfWeekString
        {
            get
            {
                string str = "";

                for (int i = 0; i < this.DaysOfWeek.Length && i < 7; i++)
                {
                    if (this.DaysOfWeek[i] == '1')
                        str += (str == "" ? "" : ", ") + dReminder.DaysOfWeekShort[i];
                }

                return str;
            }

        }

        /// <summary>
        /// Returns the list of the times of day as a List of DateTime
        /// objects.
        /// </summary>
        /// <value>The times of day as date time list.</value>
        public List<DateTime> TimesOfDayAsDateTimeList
        {
            get
            {
                List<DateTime> list = new List<DateTime>();
                foreach (var time in TimesOfDay.Split(','))
                {
                    try
                    {
                        DateTime t = DateTime.ParseExact(time, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                        list.Add(t);
                    }
                    catch
                    {
                    }
                }
                return list;

            }
        }

        public bool IsWeekly
        {
            get
            {
                return this.FrequencyType == "W";
            }
        }


        /// <summary>
        /// Generates a list of x local notifications for this
        /// current dReminder object. 
        /// </summary>
        /// <returns>The local notifications.</returns>
        public List<PestNotification> GenerateLocalNotifications()
        {
            dPatient patient = Task.Run(() => BLL.GetPatient(this.PatientID)).Result;
            //var patient = BLL.GetPatient (this.PatientID);
            List<PestNotification> notifications = new List<PestNotification>();
            List<DateTime> dateTimeList = TimesOfDayAsDateTimeList;
            DateTime endDate = DateTime.ParseExact(this.EndDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            string title = AppResources.dReminder_Reminder;
            string body = AppResources.dReminder_ReminderFor + " " + patient.Name + ": ";
            bool rateEczema = (this.ReminderType == "RA");

            if (this.ReminderType != "OT")
            {
                body += dReminder.ReminderTypeOptions[
                    Helpers.FindStringInArray(dReminder.ReminderTypeCodes, this.ReminderType)];
            }
            else
            {
                body += this.Notes;
            }

            DateTime currentDateTime = DateTime.ParseExact(this.StartDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            while (currentDateTime <= endDate && notifications.Count <= GetMaxNumberOfNotifications())
            {
                if (this.FrequencyType == "D")
                {
                    // Handles the daily notifications
                    //
                    foreach (var time in dateTimeList)
                    {
                        DateTime fireDateTime =
                            new DateTime(
                                currentDateTime.Year, currentDateTime.Month, currentDateTime.Day,
                                time.Hour, time.Minute, 0, DateTimeKind.Local);

                        if (fireDateTime > DateTime.Now && fireDateTime < endDate)
                        {

                            //CrossLocalNotifications.Current.Show(title, body, this.PatientID, fireDateTime);
                            notifications.Add(new PestNotification
                            {
                                RateEczema = rateEczema,
                                PatientID = this.PatientID,
                                Title = title,
                                Body = body,
                                FireDateTime = fireDateTime
                            });
                        }

                        if (notifications.Count > GetMaxNumberOfNotifications())
                            break;
                    }

                    currentDateTime = currentDateTime.AddDays(this.Frequency);
                }
                else if (this.FrequencyType == "W")
                {
                    // Handles the weekly notifications
                    //
                    for (int i = 0; i < 7; i++)
                    {
                        DateTime currentDateTime2 = currentDateTime.AddDays(i);

                        if (this.DaysOfWeek[(int)currentDateTime2.DayOfWeek] == '1')
                        {
                            foreach (var time in dateTimeList)
                            {
                                DateTime fireDateTime =
                                    new DateTime(
                                        currentDateTime2.Year, currentDateTime2.Month, currentDateTime2.Day,
                                        time.Hour, time.Minute, 0, DateTimeKind.Local);

                                if (fireDateTime > DateTime.Now && fireDateTime < endDate)
                                {
                                    //CrossLocalNotifications.Current.Show(title, body, i+1, fireDateTime);
                                    notifications.Add(new PestNotification
                                    {
                                        RateEczema = rateEczema,
                                        PatientID = this.PatientID,
                                        Title = title,
                                        Body = body,
                                        FireDateTime = fireDateTime
                                    });
                                }

                                if (notifications.Count > GetMaxNumberOfNotifications())
                                    break;
                            }
                        }
                    }

                    currentDateTime = currentDateTime.AddDays(this.Frequency * 7);
                }

            }

            return notifications;
        }



        /// <summary>
        /// Cancels all local notifications.
        /// 
        /// This should be called when the application resumes and just
        /// before the new notifications are re-generated.
        /// </summary>
        /// <returns><c>true</c> if cancell all notifications; otherwise, <c>false</c>.</returns>
        public static void CancellAllNotifications()
        {
            try
            {
                List<dReminder> reminders = Task.Run(() => BLL.GetAllReminders()).Result;
                //var reminders = BLL.GetAllReminders();

                foreach (var reminder in reminders)
                    for (int i = 0; i < GetMaxNumberOfNotifications(); i++)
                        CrossLocalNotifications.Current.Cancel(i + 1);
            }
            catch
            {

            }
            //App.LocalNotifications.Cancel(i + 1);
        }


        /// <summary>
        /// Cancels and reschedules all local notifications again.
        /// 
        /// This method should be called only when the application
        /// goes to sleep.
        /// </summary>
        /// <param name="reminders">Reminders.</param>
        public static void CancelAndRescheduleNotifications()
        {
            CancellAllNotifications();
            try
            {
                //var reminders = BLL.GetAllReminders();
                List<dReminder> reminders = Task.Run(() => BLL.GetAllReminders()).Result;

                List<PestNotification> notifications = new List<PestNotification>();

                System.Diagnostics.Debug.WriteLine("Original list of notifications");
                foreach (var reminder in reminders)
                {
                    if (reminder.Enabled == 1)
                    {
                        var pestNotifications = reminder.GenerateLocalNotifications();

                        //foreach (var pn in pestNotifications)
                        //{
                        //	System.Diagnostics.Debug.WriteLine (pn.FireDateTime.ToString ("yyyy/MM/dd HH:mm") + " - " + pn.Title);
                        //}

                        notifications.AddRange(pestNotifications);
                    }

                }

           
        
    
            notifications.Sort((x, y) =>
            {
                return string.Compare(
                    x.FireDateTime.ToString("yyyyMMdd HHmmss") + x.Title,
                    y.FireDateTime.ToString("yyyyMMdd HHmmss") + y.Title);
            });


            // After sorting, just pick out the first x notifications for
            // notification.
            //
            //System.Diagnostics.Debug.WriteLine ("Final list of notifications");
            DateTime lastFireDateTime = DateTime.Now;
            int lastPatientID = 1;

            for (int i = 0; i < notifications.Count && i < GetMaxNumberOfNotifications(); i++)
            {
                var n = notifications[i];
                System.Diagnostics.Debug.WriteLine(n.FireDateTime.ToString("yyyy/MM/dd HH:mm") + " - " + n.Body);
                CrossLocalNotifications.Current.Show(n.Title, n.Body, i + 1, n.FireDateTime);

                //App.LocalNotifications.Show(n.Title, n.Body, n.PatientID.ToString(),
                //    i + 1, n.FireDateTime);

                lastPatientID = n.PatientID;
                lastFireDateTime = n.FireDateTime;
            }


            // For iOS devices, we can only have up to 64 notifications.'
            // So after the 63rd notification has been scheduled,
            // we scheduled a last one to reminder the user to open
            // the app to receive more notifications
            //
            if (Xamarin.Forms.Device.OS == Xamarin.Forms.TargetPlatform.iOS &&
                notifications.Count == GetMaxNumberOfNotifications())
            {
                CrossLocalNotifications.Current.Show(
                AppResources.dReminder_Reminder,
                    AppResources.dReminder_ReminderNotification,
                    GetMaxNumberOfNotifications() + 1,
                    lastFireDateTime.AddMinutes(1));

                //App.LocalNotifications.Show(
                //    AppResources.dReminder_Reminder,
                //    AppResources.dReminder_ReminderNotification,
                //    lastPatientID.ToString(),
                //    GetMaxNumberOfNotifications() + 1,
                //    lastFireDateTime.AddMinutes(1));
            }
            }
            catch
            {

            }
        }


        private static int GetMaxNumberOfNotifications()
        {
            if (Xamarin.Forms.Device.OS == Xamarin.Forms.TargetPlatform.Android)
                return 50;
            else
                return 63;
        }
    }



    public class PestNotification
    {
        public bool RateEczema;
        public int PatientID;
        public int NotificationID;
        public string Title;
        public string Body;
        public DateTime FireDateTime;
    }
}

