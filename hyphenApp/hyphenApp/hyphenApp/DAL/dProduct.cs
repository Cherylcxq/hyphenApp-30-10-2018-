using System;
//using SQLite.Net.Attributes;

namespace hyphenApp
{
    public class dProduct : IEncryptedObject
    {
        public dProduct()
        {
        }

        public dProduct(string _ProductID, string _Name, string _Points, string _Source, string _Description)
        {
            ProductID = _ProductID;
            Name = _Name;
            Points = _Points;
            Source = _Source;
            Description = _Description;
        }

        //[PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string ProductID { get; set; }
        public string Name { get; set; }
        public string Points { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }


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

