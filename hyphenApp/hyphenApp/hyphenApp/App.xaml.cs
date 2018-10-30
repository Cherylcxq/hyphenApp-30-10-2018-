using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using hyphenApp.Views;
using System.Threading.Tasks;
using System.Globalization;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace hyphenApp
{
    public partial class App : Application
    {
        public static Application myApp;
        public static Page loginPage = null;
        public static Page mainPage = null;

        public static double ScreenHeight { get; set; }
        public static double ScreenWidth { get; set; }

        public App()
        {
            InitializeComponent();

            if (LanguageCode == null)
            {
                if (System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("en"))
                    App.Current.Properties["Language"] = "en";
                else if (System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("fil"))
                    App.Current.Properties["Language"] = "fil";
                else if (System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("vi"))
                    App.Current.Properties["Language"] = "vi";
                else if (System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("id"))
                    App.Current.Properties["Language"] = "id";
                else if (System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("ms"))
                    App.Current.Properties["Language"] = "ms";
                else if (System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("zh-CN"))
                    App.Current.Properties["Language"] = "zh-CN";
                else if (System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("zh-TW"))
                    App.Current.Properties["Language"] = "zh-TW";
                else
                    App.Current.Properties["Language"] = "en";
            }
            if (LanguageCode != null)
            {
                //AppResources.Culture = new System.Globalization.CultureInfo(LanguageCode);
                //App.Device.SetCulture(App.LanguageCode);

                CultureInfo myCulture = new CultureInfo(App.LanguageCode);
                CultureInfo.DefaultThreadCurrentCulture = myCulture;
            }
            MainPage = mainPage = new MainPage();
        }

#if DEBUG
        public const string GoogleAnalyticsTrackingID = "UA-74313937-2";
#else
		public const string GoogleAnalyticsTrackingID = "UA-74313937-1";
#endif

        public const string GoogleAnalyticsAppName = "PEST";
        public const string GoogleAnalyticsAppVersion = "1.0";

        public static DoctorListPage doctorPage = null;



        /// <summary>
        /// Device-dependence interface.
        /// </summary>
        public static IDevice device = null;
        public static IDevice Device
        {
            get
            {
                if (device == null)
                    device = DependencyService.Get<IDevice>();
                return device;
            }
        }


        /// <summary>
        /// Media Picker
        /// </summary>
        private static IMediaPicker mediaPicker = null;
        public static IMediaPicker MediaPicker
        {
            get
            {
                if (mediaPicker == null)
                    mediaPicker = DependencyService.Get<IMediaPicker>();
                return mediaPicker;
            }
        }


        /// <summary>
        /// Media Picker
        /// </summary>
        private static IEmailService emailService = null;
        public static IEmailService EmailService
        {
            get
            {
                if (emailService == null)
                    emailService = DependencyService.Get<IEmailService>();
                return emailService;
            }
        }

        ///// <summary>
        ///// Local notifications.
        ///// </summary>
        ///// 

        //public static ILocalNotifications localNotifications;
        //public static ILocalNotifications LocalNotifications
        //{
        //    get
        //    {
        //        if (localNotifications == null)
        //            localNotifications = DependencyService.Get<ILocalNotifications>();
        //        return localNotifications;
        //    }
        //}
        private static int pestID = -1;
        public static int PestID
        {
            get { return pestID; }
            set
            {
                pestID = value++;
                App.Current.Properties["PestID"] = value;
            }
        }

        private static bool userCreated = false;
        public static bool UserCreated
        {
            get { return userCreated; }
            set
            {
                userCreated = value;
                App.Current.Properties["UserCreated"] = value;
            }
        }


        private static bool patientScreen = false;
        public static bool PatientScreen
        {
            get { return patientScreen; }
            set
            {
                patientScreen = value;
                App.Current.Properties["PatientScreen"] = value;
            }
        }

        private static bool patientProfile = false;
        public static bool PatientProfile
        {
            get { return patientProfile; }
            set
            {
                patientProfile = value;
                App.Current.Properties["PatientProfile"] = value;
            }
        }

        private static int currentUserID = 0;
        public static int CurrentUserID
        {
            get { return currentUserID; }
            set
            {
                currentUserID = value;
                App.Current.Properties["UserID"] = value;
            }
        }

        private static string currentUserLogin = "";
        public static string CurrentUserLogin
        {
            get { return currentUserLogin; }
            set
            {
                currentUserLogin = value;
                App.Current.Properties["UserLogin"] = value;
            }
        }

        private static string savedFileName = "";
        public static string SavedFileName
        {
            get { return savedFileName; }
            set
            {
                savedFileName = value;
                App.Current.Properties["SavedFileName"] = value;
            }
        }

        private static string currentTips = "";
        public static string CurrentTips
        {
            get { return currentTips; }
            set
            {
                currentTips = value;
                App.Current.Properties["Tips"] = value;
            }
        }

        /// <summary>
        /// The current patient ID.
        /// </summary>
        private static int currentPatientID = 0;
        public static int CurrentPatientID
        {
            get { return currentPatientID; }
            set
            {
                currentPatientID = value;
                App.Current.Properties["PatientID"] = value;
            }
        }

        /// <summary>
        /// The current patient object.
        /// </summary>
        private static dPatient currentPatient = null;
        public static dPatient CurrentPatient
        {
            get
            {
                if (currentPatient == null || currentPatient.ID != currentPatientID)
                {
                    currentPatient = Task.Run(() => BLL.GetPatient(currentPatientID)).Result;
                    //currentPatient = BLL.GetPatient(currentPatientID);
                }
                return currentPatient;
            }
        }

        /// <summary>
        /// Language Settings
        /// </summary>
        /// 
        public static string languageCode;
        public static string LanguageCode
        {
            get
            {
                if (languageCode == null && App.Current.Properties.ContainsKey("Language"))
                    languageCode = (string)App.Current.Properties["Language"];
                return languageCode;
            }
            set
            {
                languageCode = value;
                App.Current.Properties["Language"] = languageCode;
            }
        }

        /*
		/ <summary>
		/ The current user object.
		/ </summary>
		
		*/

        private static dUser currentUser = null;
        public static dUser CurrentUser
        {
            get
            {
                if (currentUser == null || currentUser.ID != currentUserID)
                {
                    //currentUser = BLL.GetUser(currentUserID);
                }
                return currentUser;
            }
        }

        public static DateTime currentlyShownDate;
        public static DateTime CurrentShownDate
        {
            get { return currentlyShownDate; }
            set
            {
                currentlyShownDate = value;
                App.Current.Properties["CurrentShownDate"] = value;
            }
        }

        /// <summary>
        /// Local notifications.
        /// </summary>
        /// 

        public static ILocalNotifications localNotifications;
        public static ILocalNotifications LocalNotifications
        {
            get
            {
                if (localNotifications == null)
                    localNotifications = DependencyService.Get<ILocalNotifications>();
                return localNotifications;
            }
        }

        //public void LocalNotificationTapped(string userData)
        //{
        //    if (userData != "")
        //    {
        //        int patientID = Convert.ToInt32(userData);
        //        var patient = BLL.GetPatient(patientID);

        //        App.CurrentUserID = patient.UserID;
        //        App.CurrentPatientID = patient.ID;

        //        // Pop off all pages until we reach the patient tab page.
        //        //
        //        //if (Helpers.TopmostModalPage() != null)
        //        //{
        //        //    while (!(Helpers.TopmostModalPage() is PatientTabPage))
        //        //    {
        //        //        await this.MainPage.Navigation.PopModalAsync(false);
        //        //    }
        //        //}

        //        //App.PatientTabPage.LoadAndUpdateTopSideBars();
        //        //App.PatientTabPage.LoadTabContent(PatientTabPage.PestTabView, DateTime.Today.ToString("yyyy/MM/dd"));

        //        //App.MainTabPage.LoadAndUpdateTopSideBars();
        //        //App.MainTabPage.LoadTabContent(MainTabPage.LoyaltyTabView, DateTime.Today.ToString("yyyy/MM/dd"));
        //    }

        //}

        async void PopOutAlert(String e)
        {
            await App.Current.MainPage.DisplayAlert("", e, "OK");
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            dReminder.CancelAndRescheduleNotifications();
        }

        protected override void OnResume()
        {
            PopOutAlert(App.CurrentTips);
            dReminder.CancellAllNotifications();
        }
    }
}
