using System;
//using SQLite.Net.Attributes;
using Xamarin.Forms;

namespace hyphenApp
{
	public class dPatient
	{
		public dPatient ()
		{
		}


		//[PrimaryKey, AutoIncrement]
		public int ID{ get; set; }


		public int UserID{ get; set; }
	
		public string Name{ get; set; }
		public string Gender{ get; set; }
		public string Birthday{ get; set; }
		public string ProfilePicturePath { get; set; }


		//public string ActualProfilePicturePath 
		//{ 
		//	get
		//	{
		//		return Helpers.GetPlatformSpecificPath (ProfilePicturePath, "photos/");
		//	}
		//}
			
		public Xamarin.Forms.Color SideBarBackgroundColor
		{
			get
			{
				if (this.ID == App.CurrentPatientID)
					return Xamarin.Forms.Color.White;
				else
					return Xamarin.Forms.Color.FromHex ("7500b9");
			}
		}

		public Xamarin.Forms.Color SideBarTextColor
		{
			get
			{
				if (this.ID == App.CurrentPatientID)
					return Xamarin.Forms.Color.Black;
				else
					return Xamarin.Forms.Color.White;
			}
		}

		public string CircleFrameImageSource
		{
			get
			{
				if (this.ID == App.CurrentPatientID)
					return "circleframesideselected";
				else
					return "circleframesidenormal";
			}
		}

		//public void Encrypt()
		//{
		//	this.Name = App.Device.Encrypt(this.Name, "Patient");		
		//}


		//public void Decrypt()
		//{
		//	this.Name = App.Device.Decrypt(this.Name, "Patient");		
		//}

	}
}

