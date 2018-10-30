using System;
//using SQLite.Net.Attributes;

namespace hyphenApp
{
	public class dUser
	{
		public dUser ()
		{
		}
		//[PrimaryKey,AutoIncrement]
		public int ID { get; set; }

		public string Login{ get; set; }
		public string Password { get; set; }

	}
}
