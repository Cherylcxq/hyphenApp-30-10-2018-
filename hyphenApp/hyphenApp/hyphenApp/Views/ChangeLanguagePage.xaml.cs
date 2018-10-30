using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace hyphenApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChangeLanguagePage : ContentPage
	{
        List<Language> languages = new List<Language>();

        public ChangeLanguagePage ()
		{
			InitializeComponent ();
            languages.Add(new Language { Name = "English", CultureCode = "en", Selected = false });
            languages.Add(new Language { Name = "Bahasa Indonesia", CultureCode = "id", Selected = false });
            languages.Add(new Language { Name = "中文", CultureCode = "zh-CN", Selected = false });
            languages.Add(new Language { Name = "Malay", CultureCode = "ms", Selected = false });
            languages.Add(new Language { Name = "Tagalog", CultureCode = "fil", Selected = false });
            languages.Add(new Language { Name = "Tiếng Việt", CultureCode = "vi", Selected = false });

            initLocalizedText();
            initLstLanguages();

        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }


        private void initLocalizedText()
        {
            //Title = AppResources.SideBar_ChangeLanguage;
        }


        private void initLstLanguages()
        {


            foreach (var language in languages)
            {
                if (language.CultureCode.Equals(App.LanguageCode))
                {
                    language.Selected = true;
                    break;
                }
            }
            lstLanguage.ItemsSource = languages;

            lstLanguage.ItemSelected += async (sender, args) =>
            {
                foreach (var language in languages)
                    language.Selected = false;
                ((Language)lstLanguage.SelectedItem).Selected = true;

                App.LanguageCode = ((Language)lstLanguage.SelectedItem).CultureCode;

                AppResources.Culture = new System.Globalization.CultureInfo(App.LanguageCode);
                //App.Device.SetCulture(App.LanguageCode);

                CultureInfo myCulture = new CultureInfo(App.LanguageCode);
                CultureInfo.DefaultThreadCurrentCulture = myCulture;

                //dReminder.ReminderTypeOptions = new string[] {
                //    AppResources.dReminder_RateEczema,
                //    AppResources.dReminder_ApplySteroids,
                //    AppResources.dReminder_ApplyMoisturisers,
                //    AppResources.dReminder_OtherReminder };
                //dReminder.FrequencyTypeOptions = new string[] { AppResources.dReminder_Day, AppResources.dReminder_Week };
                //dReminder.DaysOfWeekShort = new string[] {
                //    AppResources.Common_Sunday,
                //    AppResources.Common_Monday,
                //    AppResources.Common_Tuesday,
                //    AppResources.Common_Wednesday,
                //    AppResources.Common_Thursday,
                //    AppResources.Common_Friday,
                //    AppResources.Common_Saturday
                //};

                //App.PatientTabPage.initLocalizedText();
                //App.PatientTabPage.reloadTabs();
                //App.PatientTabPage.IsPresented = false;

                await Navigation.PopModalAsync();

            };
        }
    }
}

