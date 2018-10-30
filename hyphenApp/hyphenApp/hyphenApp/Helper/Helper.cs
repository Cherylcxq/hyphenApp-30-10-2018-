using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace hyphenApp
{
    public class Helpers
    {
        static Dictionary<string, string> shortDateFormat = new Dictionary<string, string>();


        public static string GetShortDateFormat(string cultureCode)
        {
            if (shortDateFormat.Count == 0)
            {
                shortDateFormat["en"] = "dd-MMM-yyyy";
                shortDateFormat["fil"] = "dd-MMM-yyyy";
                shortDateFormat["id"] = "dd-MMM-yyyy";
                shortDateFormat["vi"] = "dd-MMM-yyyy";
                shortDateFormat["zh-CN"] = "dd-MM-yyyy";
                shortDateFormat["zn-TW"] = "dd-MM-yyyy";
                shortDateFormat["ms"] = "dd-MMM-yyyy";

            }

            if (shortDateFormat.ContainsKey(cultureCode))
                return shortDateFormat[cultureCode];
            return "dd-MMM-yyyy";
        }




        public static string GenerateNewPictureFileName(string fileName)
        {
            string extension = "";
            int index = fileName.LastIndexOf('.');
            if (index >= 0)
                extension = fileName.Substring(index + 1);

            return "F_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + "." + extension;
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <returns>The file name.</returns>
        /// <param name="filePath">File path.</param>
        public static string GetFileName(string filePath)
        {
            // Extract only the filename
            //
            if (filePath == null)
                return "";
            int index = filePath.LastIndexOf('/');
            if (index < 0)
                return filePath;
            filePath = filePath.Substring(index + 1);

            return filePath;
        }


        /// <summary>
        /// Gets the full path of the platform specific documents folder.
        /// </summary>
        /// <returns>The platform specific path.</returns>
        /// <param name="filePath">File path.</param>
        /// <param name="destFolder">Destination folder.</param>
        public static string GetPlatformSpecificPath(string filePath, string destFolder)
        {
            filePath = GetFileName(filePath);
            return App.Device.GetDocumentsFolder() + "/" + destFolder + filePath;
        }


        /// <summary>
        /// Allows the user to select photo from a gallery or takes a photo.
        /// 
        /// The file name of the photo will be returned.
        /// </summary>
        /// <returns>The photo.</returns>
        /// <param name="page">Page.</param>
        /// <param name="takePhoto">If set to <c>true</c> take photo.</param>
        public static async Task<string> PickPhoto(string prompt, Page page)
        {
            try
            {
                string newPicturePath = "";
                string result = await page.DisplayActionSheet(
                    prompt, AppResources.Helper_Cancel, null,
                    new string[] { AppResources.Helper_TakePhoto, AppResources.Helper_ChooseFromGallery });

                MediaFile mediaFile;
                if (result == AppResources.Helper_TakePhoto)
                {
                    mediaFile = await App.MediaPicker.TakePhotoAsync(
                        new CameraMediaStorageOptions
                        {
                            DefaultCamera = CameraDevice.Front,
                            MaxPixelDimension = 1000
                        });

                    string fileName = GetFileName(mediaFile.Path);

                    if (Device.OS == TargetPlatform.iOS)
                    {
                        newPicturePath = GetPlatformSpecificPath(GenerateNewPictureFileName(fileName), "photos/");

                        // In iOS the media picker saves the file into the Documents folder.
                        // We will want to move it into the photos folder.
                        //
                        App.Device.MoveFile(mediaFile.Path, newPicturePath);
                    }
                    else if (Device.OS == TargetPlatform.Android)
                    {
                        newPicturePath = GetPlatformSpecificPath(GenerateNewPictureFileName(fileName), "photos/");
                        App.Device.CopyFile(mediaFile.Path, newPicturePath);
                    }
                }
                else if (result == AppResources.Helper_ChooseFromGallery)
                {
                    mediaFile = await App.MediaPicker.SelectPhotoAsync(
                        new CameraMediaStorageOptions());

                    string fileName = GetFileName(mediaFile.Path);
                    if (Device.OS == TargetPlatform.iOS)
                    {
                        newPicturePath = GetPlatformSpecificPath(GenerateNewPictureFileName(fileName), "photos/");

                        // In iOS the media picker copies the file into a temp
                        // folder but it will delete it after a while. So we have
                        // to copy the file into our own folder
                        //
                        App.Device.CopyFile(mediaFile.Path, newPicturePath);
                    }
                    else if (Device.OS == TargetPlatform.Android)
                    {
                        newPicturePath = GetPlatformSpecificPath(GenerateNewPictureFileName(fileName), "photos/");
                        App.Device.CopyFile(mediaFile.Path, newPicturePath);
                    }
                }
                return newPicturePath;
            }
            catch (Exception ex)
            {
            }
            return "";
        }

        public static async Task<string> ScanPhoto(string prompt, Page page)
        {
            try
            {
                string newPicturePath = "";
                //string result = await page.DisplayActionSheet(
                //    prompt, AppResources.Helper_Cancel, null,
                //    new string[] { AppResources.Helper_TakePhoto, AppResources.Helper_ChooseFromGallery });

                MediaFile mediaFile;
                //if (result == AppResources.Helper_TakePhoto)
                //{
                //    mediaFile = await App.MediaPicker.TakePhotoAsync(
                //        new CameraMediaStorageOptions
                //        {
                //            DefaultCamera = CameraDevice.Front,
                //            PercentQuality = 100,
                //            MaxPixelDimension = 1000
                //        });

                //    string fileName = GetFileName(mediaFile.Path);
                //    if (Device.OS == TargetPlatform.iOS)
                //    {
                //        newPicturePath = GetPlatformSpecificPath(GenerateNewPictureFileName(fileName), "photos/");

                //        // In iOS the media picker saves the file into the Documents folder.
                //        // We will want to move it into the photos folder.
                //        //
                //        App.Device.MoveFile(mediaFile.Path, newPicturePath);
                //    }
                //    else if (Device.OS == TargetPlatform.Android)
                //    {
                //        newPicturePath = GetPlatformSpecificPath(GenerateNewPictureFileName(fileName), "photos/");
                //        App.Device.CopyFile(mediaFile.Path, newPicturePath);
                //    }
                //}
                //else if (result == AppResources.Helper_ChooseFromGallery)
                {
                    mediaFile = await App.MediaPicker.SelectPhotoAsync(
                        new CameraMediaStorageOptions());

                    string fileName = GetFileName(mediaFile.Path);
                    if (Device.OS == TargetPlatform.iOS)
                    {
                        newPicturePath = GetPlatformSpecificPath(GenerateNewPictureFileName(fileName), "photos/");

                        // In iOS the media picker copies the file into a temp
                        // folder but it will delete it after a while. So we have
                        // to copy the file into our own folder
                        //
                        App.Device.CopyFile(mediaFile.Path, newPicturePath);
                    }
                    else if (Device.OS == TargetPlatform.Android)
                    {
                        newPicturePath = GetPlatformSpecificPath(GenerateNewPictureFileName(fileName), "photos/");
                        App.Device.CopyFile(mediaFile.Path, newPicturePath);
                    }
                }
                return newPicturePath;
            }
            catch (Exception ex)
            {
            }
            return "";
        }

        public static int FindStringInArray(string[] array, string stringToFind)
        {
            for (int i = 0; i < array.Length; i++)
                if (array[i] == stringToFind)
                    return i;
            return 0;
        }


        ///// <summary>
        ///// Checks if there is already a modal above the specified page.
        ///// </summary>
        ///// <returns><c>true</c>, if has modal page above was alreadyed, <c>false</c> otherwise.</returns>
        ///// <param name="page">Page.</param>
        //public static bool AlreadyHasModalPageAbove(Page page)
        //{
        //    bool encounteredPage = false;
        //    int count = 0;
        //    foreach (var modalPage in page.Navigation.ModalStack)
        //    {
        //        if (modalPage == page)
        //            encounteredPage = true;
        //        else
        //        {
        //            if (encounteredPage)
        //                count++;
        //        }
        //    }

        //    return count > 0;
        //}


        ///// <summary>
        ///// Checks if there is already a modal above the specified page.
        ///// </summary>
        ///// <returns><c>true</c>, if has modal page above was alreadyed, <c>false</c> otherwise.</returns>
        ///// <param name="page">Page.</param>
        //public static Page TopmostModalPage()
        //{
        //    Page topMostModalPage = null;
        //    //foreach (var modalPage in App.myApp.MainPage.Navigation.ModalStack)
        //    //    topMostModalPage = modalPage;

        //    return topMostModalPage;
        //}



        private static int displayWidth = -1;

        public static int ScaleSizeByWidth(int baseValue, double factor)
        {
            if (displayWidth == -1)
                displayWidth = (int)App.ScreenWidth;
                //displayWidth = App.Device.GetDisplayResolution().Width;

            if (displayWidth <= 400)
                return baseValue;

            return baseValue + (int)(baseValue * factor * (displayWidth - 400) / 400);
        }
    }
}



