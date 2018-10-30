using System;
//using SQLite.Net.Attributes;

namespace hyphenApp
{
    public class dTips : IEncryptedObject
    {
        public dTips()
        {
        }

        public dTips (string _Tips)
        {
            Tips = _Tips;
        }

        //[PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Tips { get; set; }


        public void Encrypt()
        {
            //this.Name = App.Device.Encrypt(this.Name, "Doctor");
            //this.Email = App.Device.Encrypt(this.Email, "Doctor");
            //this.Address = App.Device.Encrypt(this.Address, "Doctor");
            //this.Clinic = App.Device.Encrypt(this.Clinic, "Doctor");
        }


        public void Decrypt()
        {
            //this.Name = App.Device.Decrypt(this.Name, "Doctor");
            //this.Email = App.Device.Decrypt(this.Email, "Doctor");
            //this.Address = App.Device.Decrypt(this.Address, "Doctor");
            //this.Clinic = App.Device.Decrypt(this.Clinic, "Doctor");
        }

    }
}

