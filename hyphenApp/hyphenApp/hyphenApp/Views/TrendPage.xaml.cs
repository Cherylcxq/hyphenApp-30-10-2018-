using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace hyphenApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrendPage : ContentPage
    {
        double scale;
        List<Image> photoImages = new List<Image>();

        DateTime selectedDateFrom;
        DateTime selectedDateTo;
        public static string SavedFileName = "";

        string currentlySelectedDate = "";
        string[] chartModeOptions = { "Days", "Weeks" };
        string[] chartTypeOptions = { "Scatter", "Pie", "Line", "Bar" };

        DateTime lastClickDateTime = DateTime.Today.AddDays(-1);

        public TrendPage()
        {
            InitializeComponent();

            //Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            //{
            //    lblTips.Text = App.CurrentTips;
            //    return true; // True = Repeat again, False = Stop the timer
            //});

            chartModeOptions[0] = AppResources.PatientTrendChartTabView_Days;
            chartModeOptions[1] = AppResources.PatientTrendChartTabView_Weeks;

            //scale = App.Device.GetDisplayResolution().Scale;

            foreach (var opt in chartTypeOptions)
                pickChartType.Items.Add(opt);
            pickChartType.SelectedIndex = 0;

            foreach (var opt in chartModeOptions)
                pickChartMode.Items.Add(opt);
            pickChartMode.SelectedIndex = 0;

            for (int i = 1; i < 5; i++)
                pickPeriod.Items.Add((i * 10).ToString());
            pickPeriod.SelectedIndex = 0;

            btnDoctor.Clicked += async (object sender, EventArgs e) => {
                for (int i = 0; i < 5; i++)
                {
                    //string tryFileName = "chart" + i + ".png";
                    //string fileName = App.Device.GetDocumentsFolder() + "/" + tryFileName;
                    //if (Device.OS == TargetPlatform.Android)
                    //    fileName = App.Device.GetPublicFolder() + "/" + tryFileName;
                    //SavedFileName = fileName;

                    try
                    {
                        //wv2.CaptureImageFileName = fileName;
                        //wv2.CaptureImageFileName = "";

                        string subject = String.Format(AppResources.DoctorListPage_EmailSubject, App.CurrentPatient.Name);
                        //string subject =  "Subject";
                        //string message = "test";

                        string message =
                            String.Format(AppResources.DoctorListPage_EmailBody, (pickChartMode.SelectedIndex == 0 ? AppResources.DoctorListPage_EmailDaily : AppResources.DoctorListPage_EmailWeekly)) + "\n\n" +
                            String.Format(AppResources.DoctorListPage_EmailBody2, ((pickPeriod.SelectedIndex + 1) * 10).ToString() + " " + (pickChartMode.SelectedIndex == 0 ? AppResources.DoctorListPage_EmailDays : AppResources.DoctorListPage_EmailWeeks)) + "\n\n";

                        //await Navigation.PushModalAsync(new NavigationPage(new LocalPage()));
                        await Navigation.PushModalAsync(new NavigationPage(new DoctorListPage(App.CurrentPatientID, App.CurrentUserID, subject, message)));
                        //Navigation.PushModalAsync(new DoctorListPage(App.CurrentPatientID, App.CurrentUserID, subject, message));
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            await App.Current.MainPage.DisplayAlert("Oops!", "Can't prepare the chart for sending. Ensure that you have inserted an SD card and sufficient space on the card.", "OK");
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Oops!", "Can't prepare the chart for sending. Ensure that you have sufficient space in your device.", "OK");
                        }
                        break;
                    }

                }

            };

            pickChartType.SelectedIndexChanged += async (object sender, EventArgs e) =>
            {
                await LoadAndUpdatePage("");
            };

            pickChartMode.SelectedIndexChanged += async (object sender, EventArgs e) =>
            {
                await LoadAndUpdatePage("");
            };

            pickPeriod.SelectedIndexChanged += async (object sender, EventArgs e) => {
                await LoadAndUpdatePage("");
            };

            btnDetails.Clicked += (object sender, EventArgs e) => {
                if (btnDetails.Opacity > 0)
                {
                    //App.PatientTabPage.LoadTabContent(PatientTabPage.PestTabView, currentlySelectedDate);
                }
            };

            initLocalizedText();
            LoadCurrentPatientAndDisplay(App.CurrentPatientID);
        }
        public bool LoadCurrentPatientAndDisplay(int pID)
        {
            try
            {
                dPatient currentPatient = Task.Run(() => BLL.GetPatient(pID)).Result;
                if (currentPatient == null || currentPatient.Name == "")
                {
                    return false;
                }

                tbName.Text = currentPatient.Name;
                byte[] fileContents = File.ReadAllBytes(currentPatient.ProfilePicturePath);
                imgProfile.Source = ImageSource.FromStream(() => new MemoryStream(fileContents));
                App.PatientProfile = true;
            }
            catch
            { return false; }
            return true;
        }

        override protected async void OnAppearing()
        {
            if (LoadCurrentPatientAndDisplay(App.CurrentPatientID))
            {
                try
                {
                    if (App.UserCreated)
                    {
                        //await LoadAndUpdatePage("");
                        LoadPhotos();
                    }
                }
                catch
                { }
            }
        }

        private static object _tappedLockObject = new object();
        private static bool _tapped = false;

        private void tapHandler()
        {
            // one-at-a-time access to this block prevents duplicate concurrent requests:
            lock (_tappedLockObject)
            {
                if (_tapped) return;
                _tapped = true;
            }
            //handleTap();
        }

        private void reenableTap()
        {
            _tapped = false;
        }

        async void Location_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            lock (_tappedLockObject)
            {
                if (_tapped) return;
                _tapped = true;
            }
            if (Navigation.NavigationStack.Count == 0 || Navigation.NavigationStack.Last().GetType() != typeof(StoreLocationPage))
            {
                await Navigation.PushModalAsync(new NavigationPage(new StoreLocationPage()));
            }
            reenableTap();
        }

        async void Reward_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            lock (_tappedLockObject)
            {
                if (_tapped) return;
                _tapped = true;
            }
            if (Navigation.NavigationStack.Count == 0 || Navigation.NavigationStack.Last().GetType() != typeof(RewardMenuPage))
            {
                await Navigation.PushModalAsync(new NavigationPage(new RewardMenuPage()));
            }
            reenableTap();
        }

        async void Setting_OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            lock (_tappedLockObject)
            {
                if (_tapped) return;
                _tapped = true;
            }
            if (Navigation.NavigationStack.Count == 0 || Navigation.NavigationStack.Last().GetType() != typeof(SettingPage))
            {
                await Navigation.PushModalAsync(new NavigationPage(new SettingPage()));
            }
            reenableTap();
        }

        private void initLocalizedText()
        {
            lblShowTheLast.Text = AppResources.PatientTrendChartTabView_ShowTheLast;
            btnDoctor.Text = AppResources.PatientTrendChartTabView_EmailDoctor;
            lblHelpText.Text = AppResources.PatientTrendChartTabView_HintHelp;
            lblDate.Text = AppResources.PatientTrendChartTabView_Photos;
            btnDetails.Text = AppResources.PatientTrendChartTabView_ViewRecord;
            lblNoPhotos.Text = AppResources.PatientTrendChartTabView_NoPhotos;
        }

        async void PopOutAlert(String e)
        {
            await App.Current.MainPage.DisplayAlert("", e, "OK");
        }


        // JS bridging: https://techieabdo.wordpress.com/2015/05/25/xamarin-webview-calling-c-code-from-js/

        /// <summary>
        /// Loads the chart on to the WebView.
        /// </summary>
        public async Task LoadAndUpdatePage(string extraData)
        {
            int period;
            string labels = "";
            string values = "";
            string pointSelectedScript = "";
            string hasPhotosScript = "";
            string hasNoPhotosScript = "";

            lastClickDateTime = DateTime.Today.AddDays(-1);

            hwDetails.Opacity = 0;
            //btnDetails.Opacity = 0;
            stackHelpText.IsVisible = true;
            stackPhotos.Children.Clear();

            //try
            //{
            if (pickChartMode.SelectedIndex == 0)
            {
                // Daily mode
                period = (pickPeriod.SelectedIndex + 1) * 10;
                DateTime today = DateTime.Today;
                DateTime fromDate = today.AddDays(-period);

                List<dPestphoto> pestPhotos = Task.Run(() => BLL.GetPestPhotos(App.CurrentPatientID, fromDate.ToString("yyyy/MM/dd"), today.ToString("yyyy/MM/dd"))).Result;

                List<dPest> pestList = Task.Run(() => BLL.GetPestByPatientIDAndDateRange(App.CurrentPatientID,
        //fromDate.ToString("yyyy/MM/dd"), today.ToString("yyyy/MM/dd"))).Result;
        fromDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture),
                            today.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture))).Result;
                ////List<dPestphoto> pestPhotos = BLL.GetPestPhotos(
                ////    App.CurrentPatientID,
                ////    fromDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture),
                ////    today.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture));

                // Check if there are any photos for a certain day, and
                // put it into a list.
                //
                bool[] hasPhotos = new bool[period + 1];
                foreach (var pestPhoto in pestPhotos)
                {
                    //DateTime dt = DateTime.Parse(pestPhoto.PestDate);
                    DateTime dt = DateTime.ParseExact(pestPhoto.PestDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                    int index = dt.Subtract(fromDate.Date).Days;

                    hasPhotos[index] = true;
                }

                // Put all the readings into a list first.
                //
                int[] readings = new int[period + 1];
                for (int i = 0; i < period + 1; i++)
                    readings[i] = -999;
                foreach (var pest in pestList)
                {
                    //DateTime dt = DateTime.Parse(pest.Date);
                    DateTime dt = DateTime.ParseExact(pest.Date, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                    int index = dt.Subtract(fromDate.Date).Days;
                    readings[index] = pest.Severity;
                }

                // Then generate the javascript array
                //
                for (int i = 0; i < period + 1; i++)
                {
                    labels += ((i == 0) ? "" : ",") + "'" + fromDate.AddDays(i).ToString(
                        Helpers.GetShortDateFormat(App.LanguageCode), new System.Globalization.CultureInfo(App.LanguageCode)) + "'";
                    values += ((i == 0) ? "" : ",") + readings[i].ToString();
                    hasPhotosScript += ((i == 0) ? "" : ",") + (hasPhotos[i] ? "1" : "0");
                    hasNoPhotosScript += ((i == 0) ? "" : ",") + "0";
                }
            }
            else
            {
                // Weekly mode
                period = (pickPeriod.SelectedIndex + 1) * 10;
                DateTime today = DateTime.Today;
                DateTime fromDate = today.AddDays(-period * 7);
                while (fromDate.DayOfWeek != DayOfWeek.Sunday)
                    fromDate = fromDate.AddDays(-1);

                List<dPestphoto> pestPhotos = Task.Run(() => BLL.GetPestPhotos(App.CurrentPatientID, fromDate.ToString("yyyy/MM/dd"), today.ToString("yyyy/MM/dd"))).Result;
                List<dPest> pestList = Task.Run(() => BLL.GetPestByPatientIDAndDateRange(App.CurrentPatientID, fromDate.ToString("yyyy/MM/dd"), today.ToString("yyyy/MM/dd"))).Result;

                //List<dPest> pestList = pestApp.BLL.GetPestByPatientIDAndDateRange(
                //    App.CurrentPatientID,
                //   fromDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture),
                //    today.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture));
                //List<dPestphoto> pestPhotos = BLL.GetPestPhotos(
                //    App.CurrentPatientID,
                //    fromDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture),
                //    today.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture));

                // Check if there are any photos for a certain day, and
                // put it into a list.
                //
                bool[] hasPhotos = new bool[period + 1];
                foreach (var pestPhoto in pestPhotos)
                {
                    //DateTime dt = DateTime.Parse(pestPhoto.PestDate);
                    DateTime dt = DateTime.ParseExact(pestPhoto.PestDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                    int index = dt.Subtract(fromDate).Days / 7;

                    hasPhotos[index] = true;
                }

                // Put all the readings into a list first.
                //
                int[] readings = new int[period + 1];
                int[] count = new int[period + 1];

                foreach (var pest in pestList)
                {
                    //DateTime dt = DateTime.Parse(pest.Date);
                    DateTime dt = DateTime.ParseExact(pest.Date, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                    int index = dt.Subtract(fromDate).Days / 7;

                    readings[index] += pest.Severity;
                    count[index]++;
                }

                // Then generate the javascript array
                //
                for (int i = 0; i < period + 1; i++)
                {
                    labels += ((i == 0) ? "" : ",") + "'" + fromDate.AddDays(i * 7).ToString(
                        Helpers.GetShortDateFormat(App.LanguageCode),
                        new System.Globalization.CultureInfo(App.LanguageCode)) + "'";

                    double result = -999;
                    if (count[i] > 0)
                        result = (double)readings[i] / count[i];
                    values += ((i == 0) ? "" : ",") + result.ToString("0.0");
                    //values += ((i == 0) ? "" : ",") + result.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);

                    hasPhotosScript += ((i == 0) ? "" : ",") + (hasPhotos[i] ? "1" : "0");
                    hasNoPhotosScript += ((i == 0) ? "" : ",") + "0";
                }
            }

            // Prepare the HTML to be displayed
            //
            //var htmlSource = new HtmlWebViewSource();
            //var htmlSource2 = new HtmlWebViewSource();
            //if (Device.OS == TargetPlatform.iOS)
            //{
            //    htmlSource.BaseUrl = "Content/";
            //    htmlSource2.BaseUrl = "Content/";
            //    pointSelectedScript = "function pointSelected(el) {location.href = 'http://myapp/?' + el.label; } ";
            //}
            //else
            //{
            //    htmlSource.BaseUrl = "file:///android_asset/";
            //    htmlSource2.BaseUrl = "file:///android_asset/";
            //    pointSelectedScript = "function pointSelected(el) { JSBridge.triggerevent(el.label);  } ";
            //}

            //App.Device.SetWebViewLoadEventHandler((object sender, string data) =>
            //{

            //    Device.BeginInvokeOnMainThread(() =>
            //{
            DateTime today1 = DateTime.Today;
            DateTime fromDate1 = today1.AddDays(-period);
            string data = fromDate1.ToString("yyyy/MM/dd");
            data = WebUtility.UrlDecode(data);
            DateTime dateFrom = DateTime.Parse(data);
            //DateTime dateFrom = DateTime.ParseExact(
            //    data,
            //    Helpers.GetShortDateFormat(App.LanguageCode), new System.Globalization.CultureInfo(App.LanguageCode));
            DateTime dateTo = dateFrom;

            // Update the rating tab and calendar to show
            // the selected date.
            //
            App.currentlyShownDate = dateFrom;

            selectedDateFrom = dateFrom;
            selectedDateTo = dateTo;

            if (pickChartMode.SelectedIndex == 1)
            {
                currentlySelectedDate = "";
                dateTo = dateTo.AddDays(6);
                btnDetails.IsVisible = false;
                lblDate.Text = dateFrom.ToString() + " to " + dateTo.ToString();
                //lblDate.Text = AppResources.PatientTrendChartTabView_Photos + " (" +
                //    dateFrom.ToString(
                //        Helpers.GetShortDateFormat(App.LanguageCode), new System.Globalization.CultureInfo(App.LanguageCode)) + " to " +
                //    dateTo.ToString(
                //        Helpers.GetShortDateFormat(App.LanguageCode), new System.Globalization.CultureInfo(App.LanguageCode)) + ")";
            }
            else
            {
                currentlySelectedDate = dateFrom.ToString("yyyy/MM/dd");
                btnDetails.IsVisible = true;
                lblDate.Text = dateFrom.ToString();
                //lblDate.Text = AppResources.PatientTrendChartTabView_Photos + " (" +
                //    dateFrom.ToString(
                //        Helpers.GetShortDateFormat(App.LanguageCode), new System.Globalization.CultureInfo(App.LanguageCode)) + ")";
            }


            //PopOutAlert(dateFrom.ToString("yyyy/MM/dd"));
            //var photos = BLL.GetPestPhotos(App.CurrentPatientID, dateFrom.ToString("yyyy/MM/dd"), dateTo.ToString("yyyy/MM/dd"));
            List<dPestphoto> photos = Task.Run(() => BLL.GetPestPhotos(App.CurrentPatientID, dateFrom.ToString("yyyy/MM/dd"), dateTo.ToString("yyyy/MM/dd"))).Result;

            stackHelpText.IsVisible = false;

            bool animatePanel = false;
            if (DateTime.Now.Subtract(lastClickDateTime).TotalSeconds >= 3)
                animatePanel = true;

            if (animatePanel)
            {
                hwDetails.Opacity = 0;
                hwDetails.TranslationY = 50;
            }
            else
            {
                hwDetails.Opacity = 1;
                hwDetails.TranslationY = 0;
            }

            stackPhotos.Children.Clear();
            foreach (var photo in photos)
            {
                CreatePhotoImageAndInsertToScreen(photo.FilePath, photo.PestDate, dateFrom, dateTo);
            }
            scvPhotos.IsVisible = photos.Count > 0;
            lblNoPhotos.IsVisible = photos.Count == 0;

            //if (animatePanel)
            //    hwDetails.TranslateTo(0, 0, 1, 100);

            lastClickDateTime = DateTime.Now;
            //});

            //});

            try
            {
                //PopOutAlert(labels);
                var htmlSource = new HtmlWebViewSource();
                var htmlSource2 = new HtmlWebViewSource();

                htmlSource.BaseUrl = "file:///android_asset/";
                //htmlSource2.BaseUrl = "file:///android_asset/";
                pointSelectedScript = "function pointSelected(el) { JSBridge.triggerevent(el.label);  } ";
                //wv.Source = new HtmlWebViewSource { Html = "" };
                //htmlSource.Html = await GenerateHtmlSourceForCharts(period, 10, "", pointSelectedScript, labels, values, hasPhotosScript, false);
                htmlSource.Html = await LoadAndShowHTMLInBrowser(period, 10, "", pointSelectedScript, labels, values, hasPhotosScript, false);

                //if (photoImages.Count > 0 && photoImages[selectedIndex].Source == img.Source)
                {
                    //wv.Source = new HtmlWebViewSource { Html = html };
                    wv.Source = htmlSource;

                    //PopOutAlert(values);
                }
            }
            catch
            {
            }

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //htmlSource.Html = GenerateHtmlSourceForCharts(period, 10, "", pointSelectedScript, labels, values, hasPhotosScript, false);
            //wv.Source = htmlSource;

            //    // We generate another copy of the chart into a second web view
            //    // for export.
            //    string chartType = (pickChartMode.SelectedIndex == 1) ? "WeeklyAverage" : "Daily";
            //    //string chartType = (pickChartMode.SelectedIndex == 1) ? AppResources.PatientTrendChartTabView_WeeklyAverage : AppResources.PatientTrendChartTabView_Daily;
            //    //htmlSource2.Html = GenerateHtmlSourceForCharts(period, 10, chartType + AppResources.PatientTrendChartTabView_EczemaChartFor + App.CurrentPatient.Name, pointSelectedScript, labels, values, hasNoPhotosScript, true);
            //    htmlSource2.Html = GenerateHtmlSourceForCharts(period, 10, chartType + "EczemaChartFor" + "App.CurrentPatient.Name", pointSelectedScript, labels, values, hasNoPhotosScript, true);

            //    wv2.Source = htmlSource2;
            //});
            //}
            //catch { }
        }



        /// <summary>
        /// Loads the photos and display at the bottom.
        /// </summary>
        /// <param name="date">Date.</param>
        public void LoadPhotos()
        {
            //List<dPestphoto> photos = Task.Run(() => BLL.GetPestPhotos(App.CurrentPatientID, selectedDateFrom.ToString("yyyy/MM/dd"), selectedDateTo.ToString("yyyy/MM/dd"))).Result;

            //var photos = BLL.GetPestPhotos(App.CurrentPatientID, selectedDateFrom.ToString("yyyy/MM/dd"), selectedDateTo.ToString("yyyy/MM/dd"));

            //stackPhotos.Children.Clear();
            //foreach (var photo in photos)
            //{
            //    CreatePhotoImageAndInsertToScreen(photo.FilePath, photo.PestDate, selectedDateFrom, selectedDateTo);


            //}

            int count = LoadPhotosWithDate(App.CurrentPatientID, selectedDateFrom.ToString("yyyy/MM/dd"), selectedDateTo.ToString("yyyy/MM/dd"));

            scvPhotos.IsVisible = count > 0;
            lblNoPhotos.IsVisible = count == 0;

        }


        /// <summary>
        /// Generates the html source for charts.
        /// </summary>
        /// <returns>The html source for charts.</returns>
        /// <param name="pointSelectedScript">Point selected script.</param>
        /// <param name="labels">Labels.</param>
        /// <param name="values">Values.</param>
        /// 
        public string FormatDataString(string labels, string values)
        {
            string str = "";
            string[] dates = labels.Split(',');
            string[] points = values.Split(',');

            for (int i = 0; i < dates.Length; i++)
            {
                try
                {
                    if (Convert.ToInt32(points[i]) > 0)
                    {
                        str += "[" + dates[i] + "," + points[i] + "]";
                        if (i < dates.Length - 1)
                        {
                            str += ",";
                        }
                    }
                }
                catch
                { }
            }

            return str;
        }

        public string chartTypeHTML(string labels, string values)
        {
            string[] dates = labels.Split(',');
            string[] points = values.Split(',');
            int aRating = 0;
            int bRating = 0;
            int cRating = 0;
            int dRating = 0;
            int eRating = 0;

            for (int i = 0; i < dates.Length; i++)
            {
                try
                {
                    switch (Convert.ToInt32(points[i]))
                    {
                        case 1:
                            aRating++;
                            break;
                        case 2:
                            bRating++;
                            break;
                        case 3:
                            cRating++;
                            break;
                        case 4:
                            dRating++;
                            break;
                        case 5:
                            eRating++;
                            break;
                    }
                }
                catch
                { }
            }
            string dataStr = FormatDataString(labels, values);

            string html1 =
                               @"<html>
                                            <head>
                                                <script type=""text/javascript"" src=""https://www.gstatic.com/charts/loader.js""></script>
                                                <script type=""text/javascript""" + "" + @" >
                                                  google.charts.load('current', {'packages':['corechart']});
                                              google.charts.setOnLoadCallback(drawChart);

                                              function drawChart() {
                                              var data = google.visualization.arrayToDataTable([
                                                                                ['Date', 'Rating']," + dataStr + @"]);

                                                var options = {
                                                  title: 'Eczema Trend',
                                                  hAxis: {title: 'Date', minValue: 0, maxValue: 20},
                                                  vAxis: {title: 'Rating', minValue: 1, maxValue: 5},
                                                    tooltip: {trigger: 'selection'},
                                                  legend: 'none',
                                                    annotations: {
                                                    textStyle: {
                                                      fontName: 'Times-Roman',
                                                      fontSize: 18,
                                                      bold: true,
                                                      italic: true,
                                                      // The color of the text.
                                                      color: '#871b47',
                                                      // The color of the text outline.
                                                      auraColor: '#d799ae',
                                                      // The transparency of the text.
                                                      opacity: 0.8
                                                    }
                                                  }
                                                };

                                                var chart = new google.visualization.ScatterChart(document.getElementById('chart_div'));

                                                chart.setAction({
                                                                id: 'previous',                  // An id is mandatory for all actions.
                                                                text: 'View Me!',       // The text displayed in the tooltip.
                                                                action: function () {           // When clicked, the following runs.
                                                                selection = chart.getSelection();

                                                                var member = diffData.getFormattedValue(selection[0].row, 0);

                                                                alert('test');
                                                                //alert(member);
                                                                //alert('<img src='https://upload.wikimedia.org/wikipedia/commons/0/03/Silver_medal.svg' style='width:25px;height:25px'/>');

                                                                }
                                                            });

                                                chart.draw(data, options);
                                              }
                            </script>
                        </head>
                        <body>
                            <div id=""chart_div"" style=""width: 370px; height: 200px""></div>
                        </body>
                    </html>";

            string html2 =
                                @"<html>
                                <head>
                                    <script type=""text/javascript"" src=""https://www.gstatic.com/charts/loader.js""></script>
                                    <script type=""text/javascript""" + "" + @" >
                                        google.charts.load('current', {'packages':['corechart']});
                                        google.charts.setOnLoadCallback(drawChart);
                                                    function drawChart()
                                                    {
                                                        var data = google.visualization.arrayToDataTable([
                                                          ['Rating', 'Percentage'],
                                                          ['Normal'," + aRating.ToString() + @"],
                                                          ['A little unhappy'," + bRating.ToString() + @"],
                                                          ['Quite unhappy'," + cRating.ToString() + @"],
                                                          ['Very unhappy'," + dRating.ToString() + @"],
                                                          ['Extremely unhappy'," + eRating.ToString() + @"]
                                            ]);

                                                    var options = {
                                              title: 'Eczema Daily Percentage',
                                              pieHole: 0.4,
                                            };

                                                var chart = new google.visualization.PieChart(document.getElementById('donutchart'));
                                                chart.draw(data, options);
                                            }
                                            
                                    </script>
                                </head>
                                <body>
                                    <div id=""donutchart"" style=""width: 360px; height: 160px""></div>
                                </body>
                            </html>";

            string html3 =
                                @"<html>
                                            <head>
                                                <script type=""text/javascript"" src=""https://www.gstatic.com/charts/loader.js""></script>
                                                <script type=""text/javascript""" + "" + @" >
                                                    google.charts.load('current', {'packages':['corechart']});
                                                    google.charts.setOnLoadCallback(drawChart);

                                function drawChart() {


                                  var data = google.visualization.arrayToDataTable([
                                    ['Date', 'Rating']," + dataStr + @"]);

                                    var options = {
                                    title: 'Eczema Rating',
                                    curveType: 'function',
                                    legend: { position: 'bottom' }
                                    };

                                    var chart = new google.visualization.LineChart(document.getElementById('curve_chart'));

                                    chart.draw(data, options);
                                }
                            </script>
                        </head>
                        <body>
                            <div id=""curve_chart"" style=""width: 360px; height: 170px""></div>
                        </body>
                    </html>";

            string html4 =
                                @"<html>
                                            <head>
                                                <script type=""text/javascript"" src=""https://www.gstatic.com/charts/loader.js""></script>
                                                <script type=""text/javascript""" + "" + @" >
                                                    google.charts.load('current', {'packages':['corechart']});
                                                    google.charts.setOnLoadCallback(drawChart);

                                function drawChart() {


                                        var data = google.visualization.arrayToDataTable([
                                                ['Rating', 'Percentage', { role: 'style' } ],
                                                ['Normal'," + aRating.ToString() + @", '#b87333'],
                                                ['A little unhappy'," + bRating.ToString() + @", 'silver'],
                                                ['Quite unhappy'," + cRating.ToString() + @", 'gold'],
                                                ['Very unhappy'," + dRating.ToString() + @", 'color: #e5e4e2'],
                                                ['Extremely unhappy'," + eRating.ToString() + @", 'color: #e5e4e2']
                                              ]);

                                              var view = new google.visualization.DataView(data);
                                                view.setColumns([0, 1,
                                                               { calc: 'stringify',
                                                                 sourceColumn: 1,
                                                                 type: 'string',
                                                                 role: 'annotation' },
                                                               2]);

                                              var options = {
                                                title: 'Eczema Daily Percentage',
                                                width: 360,
                                                height: 170,
                                                bar: {groupWidth: '95%'},
                                                legend: { position: 'none' },
                                              };

                                            var chart = new google.visualization.BarChart(document.getElementById('barchart_values'));
                                            chart.draw(view, options);
                                            }
                            </script>
                        </head>
                        <body>
                            <div id=""barchart_values"" style=""width: 360px; height: 170px""></div>
                        </body>
                    </html>";

            

            if (pickChartType.SelectedIndex == 0)
                return html1;
            else if (pickChartType.SelectedIndex == 1)
                return html2;
            else if (pickChartType.SelectedIndex == 2)
                return html3;
            else if (pickChartType.SelectedIndex == 3)
                return html4;
            else
                return html1;

        }

        public async Task<string> LoadAndShowHTMLInBrowser(int numberOfDataPoints, int chartPeriod, string title, string pointSelectedScript,
            string labels, string values, string hasPhotos, bool generateForExport)
        {
            try
            {
                //PopOutAlert(hasPhotos);
                string result = await System.Threading.Tasks.Task.Run(() =>
                {
                    string html = chartTypeHTML(labels, values);
                    return html;
                });

                return result;
            }
            catch
            {
                return "";
            }
        }

        public async Task<string> GenerateHtmlSourceForCharts(int numberOfDataPoints, int chartPeriod, string title, string pointSelectedScript,
            string labels, string values, string hasPhotos, bool generateForExport)
        //private string GenerateHtmlSourceForCharts(int numberOfDataPoints, int chartPeriod, string title, string pointSelectedScript, 
        //    string labels, string values, string hasPhotos, bool generateForExport)
        {
            try
            {
                string result = await System.Threading.Tasks.Task.Run(() =>
                {
                    int labelSkip = 1;
                    //if (!generateForExport)
                    labelSkip = numberOfDataPoints / 10;

                    float styleDivisor = 1;
                    //if (!generateForExport)
                    {
                        if (numberOfDataPoints == 10)
                            styleDivisor = 1;
                        else if (numberOfDataPoints == 20)
                            styleDivisor = 1.5f;
                        else if (numberOfDataPoints == 30)
                            styleDivisor = 2.0f;
                        else if (numberOfDataPoints == 40)
                            styleDivisor = 2.5f;
                    }
                    string html =
                        @"<html>
                                <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0' name='viewport' />
                                <script src='html/chart.js'></script>
                                <style type='text/css'>
                                * {
                                    -webkit-touch-callout: none;
                                    -webkit-user-select: none; 
                                }
                              a, canvas {
                                  -webkit-tap-highlight-color:rgba(0,0,0,0);
                              }       
                                </style>

                                <body style='background-color: white; padding: 10px'>" +
            (generateForExport ? ("<span style='font-family:Arial; font-size: 11pt; font-weight:bold'>" + title + @"</span>") : "") +
                            @"
                                    <canvas id='myChart' ></canvas>
                                    <script type='text/javascript'>" + pointSelectedScript + @"

                            var labelSkip = " + labelSkip.ToString() + @"; 
                            var styleDivisor = " + styleDivisor.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + @"; 

                            var w = window.innerWidth
                            || document.documentElement.clientWidth
                            || document.body.clientWidth;

                            if (w < 0) w = " + this.wv.Width + @";

                            var h = window.innerHeight
                            || document.documentElement.clientHeight
                            || document.body.clientHeight;

                            var zoom = " + (generateForExport ? "1.5;" : "w / 320;") + @"
                            var chartHeight = (h - (20 * zoom));
                            document.getElementById('myChart').width = 280 * " + chartPeriod + @" * zoom / 10;
                            document.getElementById('myChart').height = " +
                    (generateForExport ? "300;" : "(chartHeight < 200 ? 200 : chartHeight);") + @"

                            var options = 
                            {
                                animation: false,
                                animationSteps: 4,

                                // Number - Scale label font size in pixels
                                scaleFontSize: 12 * zoom * 0.8,

                                // Number - Tooltip label font size in pixels
                                tooltipFontSize: 14 * zoom * 0.8,

                                // Number - Tooltip title font size in pixels
                                tooltipTitleFontSize: 14 * zoom * 0.8,


                                // Number - pixel width of padding around tooltip text
                                tooltipYPadding: 6 * zoom * 0.8,

                                // Number - pixel width of padding around tooltip text
                                tooltipXPadding: 6 * zoom * 0.8,

                                // Number - Size of the caret on the tooltip
                                tooltipCaretSize: 4 * zoom * 0.8,

                                // Number - Pixel radius of the tooltip border
                                tooltipCornerRadius: 6 * zoom * 0.8,

                                // Number - Pixel offset from point x to tooltip edge
                                tooltipXOffset: 10 * zoom * 0.8,

                                // Boolean - If we want to override with a hard coded scale
                                scaleOverride: true,

                                // ** Required if scaleOverride is true **
                                // Number - The number of steps in a hard coded scale
                                scaleSteps: 5,
                                // Number - The value jump in the hard coded scale
                                scaleStepWidth: 1,
                                // Number - The scale starting value
                                scaleStartValue: 0,

                                ///Boolean - Whether grid lines are shown across the chart
                                scaleShowGridLines : true,

                                //String - Colour of the grid lines
                                scaleGridLineColor : 'rgba(0,0,0,.05)',

                                //Number - Width of the grid lines
                                scaleGridLineWidth : 1 * zoom,

                                //Boolean - Whether to show horizontal lines (except X axis)
                                scaleShowHorizontalLines: true,

                                //Boolean - Whether to show vertical lines (except Y axis)
                                scaleShowVerticalLines: true,

                                //Boolean - Whether the line is curved between points
                                bezierCurve : false,

                                //Number - Tension of the bezier curve between points
                                bezierCurveTension : 0.4,

                                //Boolean - Whether to show a dot for each point
                                pointDot : true,

                                //Number - Radius of each point dot in pixels
                                pointDotRadius : 6 * zoom / styleDivisor, 

                                //Number - Pixel width of point dot stroke
                                pointDotStrokeWidth : 3 * zoom / styleDivisor,

                                //Number - amount extra to add to the radius to cater for hit detection outside the drawn point
                                pointHitDetectionRadius : 8 * zoom / styleDivisor,

                                //Boolean - Whether to show a stroke for datasets
                                datasetStroke : true,

                                //Number - Pixel width of dataset stroke
                                datasetStrokeWidth : 3 * zoom / styleDivisor,

                                //Boolean - Whether to fill the dataset with a colour
                                datasetFill : true,

                                //String - A legend template
                                legendTemplate : ""<ul class=\""<%=name.toLowerCase()%>-legend\""><% for (var i=0; i<datasets.length; i++){%><li><span style=\""background-color:<%=datasets[i].strokeColor%>\""></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>""

                            };

                            var data = {
                                hasPhotos: [" + hasPhotos + @"], 
                                labels: [" + labels + @"],
                                datasets: [
                                    {
                                        label: 'Eczema Trend',
                                        fillColor: 'rgba(220,220,220,0.2)',
                                        strokeColor: '#f77',
                                        pointColor: '#ff7',
                                        pointStrokeColor: '#f77',
                                        pointHighlightFill: '#0ff',
                                        pointHighlightStroke: '#00f',
                                        data: [" + values + @"]
                                    }
                                ]
                            };

                            var ctx = document.getElementById('myChart').getContext('2d');

                            var myLineChart = new Chart(ctx).Line(data, options);


                                </script>		
                                </body>
                            </html>";
                    return html;
                });

                return result;
            }
            catch
            {
                return "";
            }

            //int labelSkip = 1;
            //        //if (!generateForExport)
            //        labelSkip = numberOfDataPoints / 10;

            //        float styleDivisor = 1;
            //        //if (!generateForExport)
            //        {
            //            if (numberOfDataPoints == 10)
            //                styleDivisor = 1;
            //            else if (numberOfDataPoints == 20)
            //                styleDivisor = 1.5f;
            //            else if (numberOfDataPoints == 30)
            //                styleDivisor = 2.0f;
            //            else if (numberOfDataPoints == 40)
            //                styleDivisor = 2.5f;
            //        }

            //        return @"<html>
            //                    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0' name='viewport' />
            //                    <script src='html/chart.js'></script>
            //                    <style type='text/css'>
            //                    * {
            //                        -webkit-touch-callout: none;
            //                        -webkit-user-select: none; 
            //                    }
            //                  a, canvas {
            //                      -webkit-tap-highlight-color:rgba(0,0,0,0);
            //                  }       
            //                    </style>

            //                    <body style='background-color: white; padding: 10px'>" +
            //        (generateForExport ? ("<span style='font-family:Arial; font-size: 11pt; font-weight:bold'>" + title + @"</span>") : "") +
            //                        @"
            //                        <canvas id='myChart' ></canvas>
            //                        <script type='text/javascript'>" + pointSelectedScript + @"

            //                var labelSkip = " + labelSkip.ToString() + @"; 
            //                var styleDivisor = " + styleDivisor.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + @"; 

            //                var w = window.innerWidth
            //                || document.documentElement.clientWidth
            //                || document.body.clientWidth;

            //                if (w < 0) w = " + this.wv.Width + @";

            //                var h = window.innerHeight
            //                || document.documentElement.clientHeight
            //                || document.body.clientHeight;

            //                var zoom = " + (generateForExport ? "1.5;" : "w / 320;") + @"
            //                var chartHeight = (h - (20 * zoom));
            //                document.getElementById('myChart').width = 280 * " + chartPeriod + @" * zoom / 10;
            //                document.getElementById('myChart').height = " +
            //                (generateForExport ? "300;" : "(chartHeight < 200 ? 200 : chartHeight);") + @"

            //                var options = 
            //                {
            //                    animation: false,
            //                    animationSteps: 4,

            //                    // Number - Scale label font size in pixels
            //                    scaleFontSize: 12 * zoom * 0.8,

            //                    // Number - Tooltip label font size in pixels
            //                    tooltipFontSize: 14 * zoom * 0.8,

            //                    // Number - Tooltip title font size in pixels
            //                    tooltipTitleFontSize: 14 * zoom * 0.8,


            //                    // Number - pixel width of padding around tooltip text
            //                    tooltipYPadding: 6 * zoom * 0.8,

            //                    // Number - pixel width of padding around tooltip text
            //                    tooltipXPadding: 6 * zoom * 0.8,

            //                    // Number - Size of the caret on the tooltip
            //                    tooltipCaretSize: 4 * zoom * 0.8,

            //                    // Number - Pixel radius of the tooltip border
            //                    tooltipCornerRadius: 6 * zoom * 0.8,

            //                    // Number - Pixel offset from point x to tooltip edge
            //                    tooltipXOffset: 10 * zoom * 0.8,

            //                    // Boolean - If we want to override with a hard coded scale
            //                    scaleOverride: true,

            //                    // ** Required if scaleOverride is true **
            //                    // Number - The number of steps in a hard coded scale
            //                    scaleSteps: 5,
            //                    // Number - The value jump in the hard coded scale
            //                    scaleStepWidth: 1,
            //                    // Number - The scale starting value
            //                    scaleStartValue: 0,

            //                    ///Boolean - Whether grid lines are shown across the chart
            //                    scaleShowGridLines : true,

            //                    //String - Colour of the grid lines
            //                    scaleGridLineColor : 'rgba(0,0,0,.05)',

            //                    //Number - Width of the grid lines
            //                    scaleGridLineWidth : 1 * zoom,

            //                    //Boolean - Whether to show horizontal lines (except X axis)
            //                    scaleShowHorizontalLines: true,

            //                    //Boolean - Whether to show vertical lines (except Y axis)
            //                    scaleShowVerticalLines: true,

            //                    //Boolean - Whether the line is curved between points
            //                    bezierCurve : false,

            //                    //Number - Tension of the bezier curve between points
            //                    bezierCurveTension : 0.4,

            //                    //Boolean - Whether to show a dot for each point
            //                    pointDot : true,

            //                    //Number - Radius of each point dot in pixels
            //                    pointDotRadius : 6 * zoom / styleDivisor, 

            //                    //Number - Pixel width of point dot stroke
            //                    pointDotStrokeWidth : 3 * zoom / styleDivisor,

            //                    //Number - amount extra to add to the radius to cater for hit detection outside the drawn point
            //                    pointHitDetectionRadius : 8 * zoom / styleDivisor,

            //                    //Boolean - Whether to show a stroke for datasets
            //                    datasetStroke : true,

            //                    //Number - Pixel width of dataset stroke
            //                    datasetStrokeWidth : 3 * zoom / styleDivisor,

            //                    //Boolean - Whether to fill the dataset with a colour
            //                    datasetFill : true,

            //                    //String - A legend template
            //                    legendTemplate : ""<ul class=\""<%=name.toLowerCase()%>-legend\""><% for (var i=0; i<datasets.length; i++){%><li><span style=\""background-color:<%=datasets[i].strokeColor%>\""></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>""

            //                };

            //                var data = {
            //                    hasPhotos: [" + hasPhotos + @"], 
            //                    labels: [" + labels + @"],
            //                    datasets: [
            //                        {
            //                            label: 'Eczema Trend',
            //                            fillColor: 'rgba(220,220,220,0.2)',
            //                            strokeColor: '#f77',
            //                            pointColor: '#ff7',
            //                            pointStrokeColor: '#f77',
            //                            pointHighlightFill: '#0ff',
            //                            pointHighlightStroke: '#00f',
            //                            data: [" + values + @"]
            //                        }
            //                    ]
            //                };

            //                var ctx = document.getElementById('myChart').getContext('2d');

            //                var myLineChart = new Chart(ctx).Line(data, options);


            //                    </script>		
            //                    </body>
            //                </html>";
        }


        private void CreatePhotoImageTouchGesture(Image img, string imagePath)
        {
            var imgClick = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            imgClick.Tapped += async (object sender, EventArgs e) =>
            {
                try
                {
                    //webView.Source = new HtmlWebViewSource { Html = "" };
                    //string html = Task.Run(() => LoadAndShowImageInBrowser(imagePath)).Result;

                    ////PopOutAlert(html);

                    ////if (photoImages.Count > 0 && photoImages[selectedIndex].Source == image.Source)
                    //{
                    //    webView.Source = new HtmlWebViewSource { Html = html };
                    //}
                }
                catch
                {
                }
            };
            img.GestureRecognizers.Add(imgClick);
        }

        private int LoadPhotosWithDate(int patientID, string dateFrom, string dateTo)
        {
            photoImages.Clear();
            stackPhotos.Children.Clear();

            // Load up and add photos of a given date range into the stack layout below.
            //
            List<dPestphoto> photoList = Task.Run(() => BLL.GetPestPhotos(patientID, dateFrom, dateTo)).Result;

            foreach (var f in photoList)
            {
                int imageWidth = 70;
                Image img = new Image()
                { HeightRequest = imageWidth, WidthRequest = imageWidth };
                img.Aspect = Aspect.AspectFill;
                CreatePhotoImageTouchGesture(img, f.FilePath);

                byte[] fileContents = File.ReadAllBytes(f.FilePath);
                img.Source = ImageSource.FromStream(() => new MemoryStream(fileContents));

                stackPhotos.Children.Add(img);
            }

            return photoList.Count;
        }

        /// <summary>
        /// Creates a image view to show the photo and insert it to the screen.
        /// </summary>
        /// <param name="imagePath">Image path.</param>
        private void CreatePhotoImageAndInsertToScreen(string imagePath, string pestDate, DateTime dateFrom, DateTime dateTo)
        {
            imagePath = Helpers.GetPlatformSpecificPath(imagePath, "photos/");

            Image img = new Image()
            { HeightRequest = 70, WidthRequest = 70 };
            img.Aspect = Aspect.AspectFill;
            stackPhotos.Children.Add(img);

            // Bug fix - set the image path only AFTER the width / height are set.
            img.Source = imagePath;

            var imgClick = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            imgClick.Tapped += (object sender, EventArgs e) =>
            {
                try
                {
                    DateTime date = DateTime.ParseExact(pestDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                    Navigation.PushModalAsync(
                        new PatientEczemaPhotoPage(App.CurrentPatientID, imagePath, true, date, date, date));
                }
                catch
                {
                }
            };
            img.GestureRecognizers.Add(imgClick);
        }
    }
}
