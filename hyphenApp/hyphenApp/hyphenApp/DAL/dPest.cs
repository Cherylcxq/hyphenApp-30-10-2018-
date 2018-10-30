using System;
using System.Threading;
//using SQLite.Net.Attributes;

namespace hyphenApp
{
	public class dPest
	{
		public dPest ()
		{
		}

        //[PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int PatientID { get; set; }
		public int Severity { get; set; }
		public string ActionTaken { get; set; }
		public string Notes { get; set; }
		public string Date { get; set; }

		public string Day 
		{
			get 
			{
				try 
				{ 
					return DateTime.ParseExact(this.Date, "yyyy/MM/dd", 
						System.Globalization.CultureInfo.InvariantCulture).ToString("dd");
				}
				catch
				{
					return "";
				}
			}
		}

		public string Month
		{
			get 
			{
				try { 
					return DateTime.ParseExact(this.Date, "yyyy/MM/dd", 
						System.Globalization.CultureInfo.InvariantCulture).ToString("MMM");
				}
				catch
				{
					return "";
				}
			}
		}

		//public string SyncFlag{ get; set; }
	}
}

