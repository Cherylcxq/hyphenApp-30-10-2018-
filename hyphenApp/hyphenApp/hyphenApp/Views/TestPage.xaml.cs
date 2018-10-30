using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace hyphenApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TestPage : ContentPage
	{
        public TestPage ()
		{
			InitializeComponent ();
            DeterminedLogin();

        }

        private async void DeterminedLogin()
        {
            string email = Task.Run(() => BLL.GetUserEmailID()).Result;
            if (email == null)
            {
                await Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
            }
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Write_Clicked(object sender, EventArgs e)
        {
           
            await Task.Run(() => SaveDataAsync(txtWrite.Text, "dDoctor.txt"));
        }

        async void Read_Clicked(object sender, EventArgs e)
        {
            lbName.Text = await Task.Run(() => ReadDataAsync("dDoctor.txt"));
        }

        void Delete_Clicked(object sender, EventArgs e)
        {
            DeleteFile("dDoctor.txt");
        }

        public void DeleteFile(string filename)
        {
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
            File.Delete(backingFile);

        }

        public async Task SaveDataAsync(string str, string filename)
        {
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
            using (var writer = File.CreateText(backingFile))
            {
                await writer.WriteLineAsync(str);
            }
        }

        public async Task<string> ReadDataAsync(string filename)
        {
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);

            if (backingFile == null || !File.Exists(backingFile))
            {
                return null;
            }

            var str = "";
            using (var reader = new StreamReader(backingFile, true))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    //if (int.TryParse(line, out var newcount))
                    //{
                    //    count = newcount;
                    //}
                    str += line;
                }
            }
            return str;
        }

        public string FormatPestString(dPest pest)
        {
            string str = pest.ID.ToString();
            str += "~";
            str += pest.PatientID.ToString();
            str += "~";
            str += pest.Severity.ToString();
            str += "~";
            str += pest.ActionTaken;
            str += "~";
            str += pest.Notes;
            str += "~";
            str += pest.Date;
            str += "~";
            str += pest.Day;
            str += "~";
            str += pest.Month;
            str += "^";
            return str;
        }

        private async void ReadPestRecord(string ID, string PatientID, string Date)
        {
            List<string> listStr = new List<string>();
            string str = await Task.Run(() => ReadDataAsync("dDoctor.txt"));

            string[] words = str.Split('^');
            foreach (string s in words)
            {
                listStr.Add(s);
            }
            foreach (string s in listStr)
            {
                string[] arrays = str.Split('~');
                if(arrays[0] == ID && arrays[1] == PatientID && arrays[5] == Date)
                {

                }
            }

            //return str;
        }
    }
}