using System;

namespace hyphenApp
{
	/// <summary>
	/// This class is used for building the Time of Day 
	/// UI in the Reminders page.
	/// </summary>
	public class TimeOfDay
	{

		DateTime timeOfDay;

		public DateTime Time { get { return timeOfDay; } }

		public string TimeIn12HourFormat
		{
			get 
			{
				if (App.LanguageCode == "vi")
				{
					return timeOfDay.ToString ("HH:mm");;
				}
				else
				{
					return timeOfDay.ToString ("hh:mm tt");
				}
			}
		}

		public string TimeIn24HourFormat
		{
			get { return timeOfDay.ToString ("HH:mm"); }
		}

		public TimeOfDay (DateTime timeOfDay)
		{
			this.timeOfDay = new DateTime(1, 1, 1, timeOfDay.Hour, timeOfDay.Minute, 0);
		}
	}
}

