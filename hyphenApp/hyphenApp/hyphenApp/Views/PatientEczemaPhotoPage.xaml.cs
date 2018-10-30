using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace hyphenApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientEczemaPhotoPage : ContentPage
    {
        List<Image> photoImages = new List<Image>();
        int selectedIndex = 0;
        string selectedImageFilePath;
        bool isLoading = false;

        public PatientEczemaPhotoPage(int patientID, string fpath, bool showDeleteButton, DateTime pestDate, DateTime dateFrom, DateTime dateTo)
        {
            InitializeComponent();

            //DependencyService.Get<IAnalytics>().GAScreen("View Photo");

            //dPatient currentPatient = App.CurrentPatient;
            //if (currentPatient != null)
            //{
            //    topBar.Subtitle = currentPatient.Name;
            //    topBar.ProfileImageSource = Helpers.GetPlatformSpecificPath(currentPatient.ProfilePicturePath, "photos/");
            //}


            //this.selectedImageFilePath = fpath;

            // Load up and add photos of a given date range into the stack layout below.
            //
            //LoadPhotosWithinDateRange(patientID, dateFrom, dateTo);


            //var image = new Image { Source = fpath };

            //ShowImage(image);

            //topBar.RightButtonClicked += async (object sender, EventArgs e) =>
            //{
            //    string result = await DisplayActionSheet(
            //        AppResources.PatientEczemaPage_DeletePhoto,
            //        AppResources.Common_OptionNo,
            //        AppResources.Common_OptionYes);
            //    if (result == null || result.Equals(AppResources.Common_OptionNo))
            //        return;

            //    string filePathStoredInDatabase = selectedImageFilePath;
            //    //if (Device.OS == TargetPlatform.iOS)
            //    filePathStoredInDatabase = Helpers.GetFileName(selectedImageFilePath);

            //    int photoID = pestApp.BLL.GetPhotoID(filePathStoredInDatabase);
            //    pestApp.BLL.DeleteRecord("dPestphoto", photoID);
            //    App.Device.DeleteFile(selectedImageFilePath);

            //    stackPhotos.Children.RemoveAt(selectedIndex);
            //    photoImages.RemoveAt(selectedIndex);

            //    if (selectedIndex >= photoImages.Count)
            //        selectedIndex--;

            //    if (selectedIndex >= 0)
            //    {
            //        selectedImageFilePath = photoImages[selectedIndex].ImageSource;
            //        photoImages[selectedIndex].Opacity = 1.0;
            //    }
            //    else
            //        selectedImageFilePath = "";

            //    if (photoImages.Count == 0)
            //        Navigation.PopModalAsync();
            //    else
            //    {
            //        ShowImage(selectedImageFilePath);
            //    }
            //    if (App.PatientTabPage.CurrentView is PatientEczemaTabView)
            //    {
            //        ((PatientEczemaTabView)App.PatientTabPage.CurrentView).LoadAndUpdatePage("");
            //    }
            //    else if (App.PatientTabPage.CurrentView is PatientTrendChartTabView)
            //    {
            //        if (photoImages.Count == 0)
            //            // This forces the page to reload itself, so that the black dot
            //            // on the chart disappears.
            //            ((PatientTrendChartTabView)App.PatientTabPage.CurrentView).LoadAndUpdatePage("");
            //        else
            //            // If there are >= 1 photos left, we just reload the 
            //            // photo stack layout.
            //            ((PatientTrendChartTabView)App.PatientTabPage.CurrentView).LoadPhotos();
            //    }

            //};

            initLocalizedText();

        }

        private void initLocalizedText()
        {
        }


        /// <summary>
        /// Loads up a list of photos from the current patient
        /// within a given date range.
        /// 
        /// Returns the total number of photos loaded.
        /// </summary>
        private int LoadPhotosWithinDateRange(int patientID, DateTime dateFrom, DateTime dateTo)
        {
            photoImages.Clear();
            stackPhotos.Children.Clear();

            // Load up and add photos of a given date range into the stack layout below.
            //
            //var photos = BLL.GetPestPhotos(patientID,
            //    dateFrom.ToString("yyyy/MM/dd"), dateTo.ToString("yyyy/MM/dd"));
            List<dPestphoto> photos = Task.Run(() => BLL.GetPestPhotos(patientID,
              dateFrom.ToString("yyyy/MM/dd"), dateTo.ToString("yyyy/MM/dd"))).Result;

            int index = 0;
            foreach (var photo in photos)
            {
                bool selected = Helpers.GetPlatformSpecificPath(photo.FilePath, "photos/") == selectedImageFilePath;
                var img = CreatePhotoImageAndInsertToScreen(photo.FilePath,
                    selected, index);

                if (selected)
                    selectedIndex = index;

                photoImages.Add(img);
                index++;

            }

            return photos.Count;
        }


        /// <summary>
        /// Selects the image.
        /// </summary>
        /// <param name="image">Image.</param>
        private async void ShowImage(Image image)
        {
            isLoading = true;

            // Find index of photoImages with the same file path.
            //
            int imageIndex = 0;
            for (int i = 0; i < photoImages.Count; i++)
                if (photoImages[i] == image)
                //if (photoImages[i].Source == path)
                {
                    imageIndex = i;
                    break;
                }

            photoImages[imageIndex].Opacity = 1.0;

            int scrollWidth = (int)scvPhotos.Width;

            int internalWidth = photoImages.Count * 80;
            if (internalWidth > 0)
                internalWidth -= 10;
            if (internalWidth < scrollWidth)
                internalWidth = scrollWidth;

            int targetX = -(scrollWidth / 2 - 35) + imageIndex * 80;
            if (targetX < 0)
                targetX = 0;
            if (targetX > internalWidth - scrollWidth)
                targetX = internalWidth - scrollWidth;

            await scvPhotos.ScrollToAsync((double)targetX, 0, true);
            selectedImageFilePath = image.Source.ToString();


            try
            {
                webView.Source = new HtmlWebViewSource { Html = "" };
                string html = await LoadAndShowImageInBrowser(image.Source.ToString());

                if (photoImages.Count > 0 && photoImages[selectedIndex].Source == image.Source)
                {
                    webView.Source = new HtmlWebViewSource { Html = html };
                }
            }
            catch
            {
            }
            isLoading = false;

        }


        public async Task<string> LoadAndShowImageInBrowser(string path)
        {
            try
            {
                string result = await System.Threading.Tasks.Task.Run(() =>
                {
                    //int d = App.Device.GetImageRotationFromExif (path);
                    string html =
                        "<html>" +
                        "<head>" +
                        "<style>* { margin:0; padding:0; }</style> " +
                        //"<style>img { -webkit-transform: rotate(" + d + "deg); transform: rotate(" + d + "deg); } </style>" +
                        "<meta name='viewport' content='width=device-width, initial-scale=1, minimum-scale=0.001, maximum-scale=100.0, user-scalable=1' />" +
                        "<body style='padding:0 0 0 0'><img src='data:image/png;base64," + App.Device.ReadFileIntoBase64String(path) +
                        "' style='width:" + this.Width.ToString() + "px' /></body></html>";
                    return html;
                });

                return result;
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Creates a image view to show the photo and insert it to the screen.
        /// </summary>
        /// <param name="imagePath">Image path.</param>
        private Image CreatePhotoImageAndInsertToScreen(string imagePath, bool selected, int index)
        {
            imagePath = Helpers.GetPlatformSpecificPath(imagePath, "photos/");

            int imageWidth = Helpers.ScaleSizeByWidth(70, 0.8);

            Image img = new Image()
            { HeightRequest = imageWidth, WidthRequest = imageWidth };
            img.Aspect = Aspect.AspectFill;

            if (selected)
                img.Opacity = 1;
            else
                img.Opacity = 0.5;

            stackPhotos.Children.Add(img);

            // Bug fix - set the image path only AFTER the width / height are set.
            img.Source = imagePath;

            var imgClick = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            imgClick.Tapped += (object sender, EventArgs e) =>
            {
                if (isLoading)
                    return;
                isLoading = true;

                int imageIndex = photoImages.IndexOf(img);
                if (imageIndex == selectedIndex)
                {
                    isLoading = false;
                    return;
                }

                for (int i = 0; i < photoImages.Count; i++)
                    if (photoImages[i].Opacity == 1.0)
                        photoImages[i].Opacity = 0.5;
                img.Opacity = 1;
                ShowImage(img);

                selectedIndex = imageIndex;

            };
            img.GestureRecognizers.Add(imgClick);

            return img;
        }



        protected override void OnAppearing()
        {
            base.OnAppearing();

            ShowImage(photoImages[selectedIndex]);

        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //ppcImage.StopAnimations ();
        }

    }
}

