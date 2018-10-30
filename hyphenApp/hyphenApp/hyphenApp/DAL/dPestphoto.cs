using System;
//using SQLite.Net.Attributes;
using Xamarin.Forms;

namespace hyphenApp
{
	public class dPestphoto
	{

		//[PrimaryKey, AutoIncrement]
		public int ID{ get; set; }

		public int PestID { get; set; }

        public int PatientID { get; set; }

        public string PestDate { get; set; }

		public string FilePath { get; set; }

		public string Day 
		{
			get 
			{
				try 
				{ 
					return DateTime.ParseExact(this.PestDate, "yyyy/MM/dd", 
						System.Globalization.CultureInfo.InvariantCulture).ToString("dd");
				}
				catch
				{
					return "";
				}
			}
		}


		public dPestphoto ()
		{
		}


	}
}

