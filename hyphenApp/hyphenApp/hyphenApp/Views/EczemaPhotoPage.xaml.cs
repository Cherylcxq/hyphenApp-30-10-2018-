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
	public partial class EczemaPhotoPage : ContentPage
	{
        List<Image> photoImages = new List<Image>();
        int selectedIndex = 0;
        string selectedImageFilePath;
        bool isLoading = false;

        public EczemaPhotoPage (int patientID, string path, bool showDeleteButton, DateTime pestDate)
		{
			InitializeComponent ();

            string date = pestDate.ToString("yyyy/MM/dd");

            LoadPhotosWithDate(patientID, date);

            try
            {
                webView.Source = new HtmlWebViewSource { Html = "" };
                string html = Task.Run(() => LoadAndShowImageInBrowser(path)).Result;

                //PopOutAlert(html);

                //if (photoImages.Count > 0 && photoImages[selectedIndex].Source == image.Source)
                {
                    webView.Source = new HtmlWebViewSource { Html = html };
                }
            }
            catch
            {
            }

            //ShowImage(path);
        }
        async void PopOutAlert(String e)
        {
            await App.Current.MainPage.DisplayAlert("", e, "OK");
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        /// <summary>
        /// Loads up a list of photos from the current patient
        /// within a given date range.
        /// 
        /// Returns the total number of photos loaded.
        /// </summary>
        private int LoadPhotosWithDate(int patientID, string date)
        {
            photoImages.Clear();
            stackPhotos.Children.Clear();

            // Load up and add photos of a given date range into the stack layout below.
            //
            List<dPestphoto> photoList = Task.Run(() => BLL.GetPestPhotos(patientID, date)).Result;

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

        private async void ShowImage(string path)
        {
            isLoading = true;
            var img = new Image { Source = path };
            //image.Source = img.Source;

            // Find index of photoImages with the same file path.
            //
            int imageIndex = 0;
            for (int i = 0; i < photoImages.Count; i++)
                if (photoImages[i].Source == img.Source)
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
            selectedImageFilePath = img.Source.ToString();


            try
            {
                webView.Source = new HtmlWebViewSource { Html = "" };
                string html = await LoadAndShowImageInBrowser(path);

                //if (photoImages.Count > 0 && photoImages[selectedIndex].Source == img.Source)
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
                Byte[] bytes = File.ReadAllBytes(path);
                String file = Convert.ToBase64String(bytes);
                string result = await System.Threading.Tasks.Task.Run(() =>
                {
                    //int d = App.Device.GetImageRotationFromExif (path);
                    string html =
                        "<html>" +
                        "<head>" +
                        "<style>* { margin:0; padding:0; }</style> " +
                        //"<style>img { -webkit-transform: rotate(" + d + "deg); transform: rotate(" + d + "deg); } </style>" +
                        "<meta name='viewport' content='width=device-width, initial-scale=1, minimum-scale=0.001, maximum-scale=100.0, user-scalable=1' />" +
                        "<body style='padding:0 0 0 0'><img src='data:image/png;base64," + file +
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
        /// 
        private void CreatePhotoImageTouchGesture(Image img, string imagePath)
        {
            var imgClick = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            imgClick.Tapped += async (object sender, EventArgs e) =>
            {
                try
                {
                    webView.Source = new HtmlWebViewSource { Html = "" };
                    string html = Task.Run(() => LoadAndShowImageInBrowser(imagePath)).Result;

                    //PopOutAlert(html);

                    //if (photoImages.Count > 0 && photoImages[selectedIndex].Source == image.Source)
                    {
                        webView.Source = new HtmlWebViewSource { Html = html };
                    }
                }
                catch
                {
                }
            };
            img.GestureRecognizers.Add(imgClick);
        }


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
                //ShowImage(img);

                selectedIndex = imageIndex;

            };
            img.GestureRecognizers.Add(imgClick);

            return img;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //ShowImage(photoImages[selectedIndex]);

        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //ppcImage.StopAnimations ();
        }
    }
}