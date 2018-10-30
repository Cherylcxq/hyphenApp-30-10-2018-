using System;
//using SQLite.Net.Attributes;

namespace hyphenApp
{
	public class dDoctor 
	{
		public dDoctor ()
		{
		}

		public int ID{ get; set; }
		public int UserID { get; set; }
		public string Name {get;set;}
		public string Clinic{ get; set; }
		public string Email {get;set;}
		public string Address {get;set;}
		public string Information {get;set;}


		//public void Encrypt()
		//{
		//	this.Name = App.Device.Encrypt(this.Name, "Doctor");
		//	this.Email = App.Device.Encrypt(this.Email, "Doctor");
		//	this.Address = App.Device.Encrypt(this.Address, "Doctor");
		//	this.Clinic = App.Device.Encrypt(this.Clinic, "Doctor");		
		//}


		//public void Decrypt()
		//{
		//	this.Name = App.Device.Decrypt(this.Name, "Doctor");
		//	this.Email = App.Device.Decrypt(this.Email, "Doctor");
		//	this.Address = App.Device.Decrypt(this.Address, "Doctor");
		//	this.Clinic = App.Device.Decrypt(this.Clinic, "Doctor");		
		//}

	}
}

