using LocalNotifications.Plugin;
using LocalNotifications.Plugin.Abstractions;
using System;
using Xamarin.Forms;

namespace hyphenApp.Views
{
    public partial class LocalPage : ContentPage
    {
        const int _SAMPLE_ID = 1;
        int _secondsToDelivery;

        public LocalPage()
        {
            InitializeComponent();
        }

        void ScheduledSwitchToggled(object sender, ToggledEventArgs e)
        {
            ScheduleSecondsPicker.IsVisible = ScheduleSwitch.IsToggled;

            if (!ScheduleSwitch.IsToggled)
            {
                _secondsToDelivery = 0;
            }
        }

        void Handle_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            _secondsToDelivery = int.Parse(ScheduleSecondsPicker.Items[ScheduleSecondsPicker.SelectedIndex]);
        }

        void SendButtonClicked(object sender, EventArgs e)
        {

            var notification = new LocalNotification
            {
                Text = "Hello Plugin",
                Title = "Nbation Plugin",
                Id = 2,
                NotifyTime = DateTime.Now.AddSeconds(2)
            };

            var notifier = CrossLocalNotifications.CreateLocalNotifier();
            notifier.Notify(notification);
            if (_secondsToDelivery > 0)
            {
                //CrossLocalNotifications.Current.Show(TitleEntry.Text, BodyEntry.Text, _SAMPLE_ID, DateTime.Now.AddSeconds(_secondsToDelivery));
            }
            else
            {
                //CrossLocalNotifications.Current.Show(TitleEntry.Text, BodyEntry.Text, _SAMPLE_ID);
            }
        }

        void CancelButtonClicked(object sender, EventArgs e)
        {
            //CrossLocalNotifications.Current.Cancel(_SAMPLE_ID);
        }
    }
}
