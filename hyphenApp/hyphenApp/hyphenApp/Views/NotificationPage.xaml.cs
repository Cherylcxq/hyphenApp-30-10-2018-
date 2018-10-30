using Plugin.LocalNotifications;
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
	public partial class NotificationPage : ContentPage
	{
		public NotificationPage ()
		{
			InitializeComponent ();
		}

        private void Notification_Clicked(object sender, EventArgs e)
        {
            CrossLocalNotifications.Current.Show("Reminder", "Please Remember that you have an appointment with Dr Angela at 1pm today.", 101, DateTime.Now.AddSeconds(3));
        }

    }
}