using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Android.Util;
using Android.Support.V4.App;
using Android.Locations;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using System.Threading.Tasks;
using System.IO;
using Android.Content;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Acr.UserDialogs;
using Plugin.LocalNotifications;

namespace hyphenApp.Droid
{
    [Activity(Label = "iControl Eczema Plus", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, 
        ScreenOrientation = ScreenOrientation.Portrait, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static readonly int RC_REQUEST_LOCATION_PERMISSION = 1000;
        static readonly string TAG = "MainActivity";
        static readonly string[] REQUIRED_PERMISSIONS = { Manifest.Permission.AccessFineLocation };
        /**
     	* Id to identify a camera permission request.
     	*/
        static readonly int REQUEST_CAMERA = 0;

        /**
     	* Id to identify a contacts permission request.
     	*/
        static readonly int REQUEST_CONTACTS = 1;

        /**
     	* Permissions required to read and write contacts. Used by the ContactsFragment.
     	*/
        static string[] PERMISSIONS_CONTACT = {
            Manifest.Permission.ReadContacts,
            Manifest.Permission.WriteContacts
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.app_logo;

            App.ScreenWidth = (Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
            App.ScreenHeight = (Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);

            // This event fires when the ServiceConnection lets the client (our App class) know that
            // the Service is connected. We use this event to start updating the UI with location
            // updates from the Service
            AppAndroid.Current.LocationServiceConnected += (sender, e) =>
            {
                Log.Debug(TAG, "ServiceConnected Event Raised");
                // notifies us of location changes from the system
                AppAndroid.Current.LocationService.LocationChanged += HandleLocationChanged;
                //notifies us of user changes to the location provider (ie the user disables or enables GPS)
                AppAndroid.Current.LocationService.ProviderDisabled += HandleProviderDisabled;
                AppAndroid.Current.LocationService.ProviderEnabled += HandleProviderEnabled;
                // notifies us of the changing status of a provider (ie GPS no longer available)
                AppAndroid.Current.LocationService.StatusChanged += HandleStatusChanged;
            };

            base.OnCreate(savedInstanceState);

            UserDialogs.Init(this);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            //Start the location service:
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == (int)Permission.Granted)
            {
                Log.Debug(TAG, "User already has granted permission.");
                AppAndroid.StartLocationService();
            }
            else
            {
                Log.Debug(TAG, "Have to request permission from the user. ");
                RequestLocationPermission();
            }
            // Check if the Camera permission is already available.
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != (int)Permission.Granted)
            {

                // Camera permission has not been granted
                RequestCameraPermission();
            }
            else
            {
                // Camera permissions is already available, show the camera preview.
                Log.Info(TAG, "CAMERA permission has already been granted. Displaying camera preview.");
            }
        }

        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        //{
        //    var layout = FindViewById(Android.Resource.Id.Content);
        //    if (requestCode == RC_REQUEST_LOCATION_PERMISSION)
        //    {
        //        if (grantResults.Length == 1 && grantResults[0] == Permission.Granted)
        //        {
        //            Log.Debug(TAG, "User granted permission for location.");
        //            AppAndroid.StartLocationService();
        //        }
        //        else
        //        {
        //            Log.Warn(TAG, "User did not grant permission for the location.");
        //        }
        //    }
        //    if (requestCode == REQUEST_CAMERA)
        //    {
        //        // Received permission result for camera permission.
        //        Log.Info(TAG, "Received response for Camera permission request.");

        //        // Check if the only required permission has been granted
        //        if (grantResults.Length == 1 && grantResults[0] == Permission.Granted)
        //        {
        //            // Camera permission has been granted, preview can be displayed
        //            Log.Info(TAG, "CAMERA permission has now been granted. Showing preview.");
        //            Snackbar.Make(layout, "CAMERA permission has now been granted. Showing preview.", Snackbar.LengthShort).Show();
        //        }
        //        else
        //        {
        //            Log.Info(TAG, "CAMERA permission was NOT granted.");
        //            Snackbar.Make(layout, "CAMERA permission was NOT granted.", Snackbar.LengthShort).Show();
        //        }
        //    }
        //    //else if (requestCode == REQUEST_CONTACTS)
        //    //{
        //    //    Log.Info(TAG, "Received response for contact permissions request.");

        //    //    // We have requested multiple permissions for contacts, so all of them need to be
        //    //    // checked.
        //    //    if (PermissionUtil.VerifyPermissions(grantResults))
        //    //    {
        //    //        // All required permissions have been granted, display contacts fragment.
        //    //        Snackbar.Make(layout, "Contacts permissions were granted.", Snackbar.LengthShort).Show();
        //    //    }
        //    //    else
        //    //    {
        //    //        Log.Info(TAG, "Contacts permissions were NOT granted.");
        //    //        Snackbar.Make(layout, "Contacts permissions were NOT granted.", Snackbar.LengthShort).Show();
        //    //    }
        //    //}
        //    else
        //    {
        //        PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //        //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //    }
           
        //}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            var layout = FindViewById(Android.Resource.Id.Content);
            if (requestCode == RC_REQUEST_LOCATION_PERMISSION)
            {
                if (grantResults.Length == 1 && grantResults[0] == Permission.Granted)
                {
                    Log.Debug(TAG, "User granted permission for location.");
                    AppAndroid.StartLocationService();
                }
                else
                {
                    Log.Warn(TAG, "User did not grant permission for the location.");
                }
            }
            if (requestCode == REQUEST_CAMERA)
            {
                // Received permission result for camera permission.
                Log.Info(TAG, "Received response for Camera permission request.");

                // Check if the only required permission has been granted
                if (grantResults.Length == 1 && grantResults[0] == Permission.Granted)
                {
                    // Camera permission has been granted, preview can be displayed
                    Log.Info(TAG, "CAMERA permission has now been granted. Showing preview.");
                    Snackbar.Make(layout, "CAMERA permission has now been granted. Showing preview.", Snackbar.LengthShort).Show();
                }
                else
                {
                    Log.Info(TAG, "CAMERA permission was NOT granted.");
                    Snackbar.Make(layout, "CAMERA permission was NOT granted.", Snackbar.LengthShort).Show();
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }

        void RequestCameraPermission()
        {
            var layout = FindViewById(Android.Resource.Id.Content);
            Log.Info(TAG, "CAMERA permission has NOT been granted. Requesting permission.");

            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.Camera))
            {
                // Provide an additional rationale to the user if the permission was not granted
                // and the user would benefit from additional context for the use of the permission.
                // For example if the user has previously denied the permission.
                Log.Info(TAG, "Displaying camera permission rationale to provide additional context.");

                Snackbar.Make(layout, "Displaying camera permission rationale to provide additional context.",
                    Snackbar.LengthIndefinite).SetAction("ok", new Action<View>(delegate (View obj) {
                        ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera }, REQUEST_CAMERA);
                    })).Show();
            }
            else
            {
                // Camera permission has not been granted yet. Request it directly.
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera }, REQUEST_CAMERA);
            }
        }

        void RequestLocationPermission()
        {
            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
            {
                var layout = FindViewById(Android.Resource.Id.Content);
                Snackbar.Make(layout,
                              "This app requires access to the device location in order to work",
                              Snackbar.LengthIndefinite)
                        .SetAction("Ok",
                                   new Action<View>(delegate
                                   {
                                       ActivityCompat.RequestPermissions(this, REQUIRED_PERMISSIONS,
                                                                         RC_REQUEST_LOCATION_PERMISSION);
                                   })
                                  ).Show();
            }
            else
            {
                ActivityCompat.RequestPermissions(this, REQUIRED_PERMISSIONS, RC_REQUEST_LOCATION_PERMISSION);
            }
        }

        async void PopOutAlert(String e)
        {
            await App.Current.MainPage.DisplayAlert("Warning", e, "OK");
        }
        /// <summary>
        ///     Updates UI with location data
        /// </summary>
        public void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            var location = e.Location;
            Log.Debug(TAG, "Foreground updating");

            // these events are on a background thread, need to update on the UI thread
            RunOnUiThread(() =>
            {
                //PopOutAlert(location.Latitude.ToString());
                App.Current.Properties["Latitude"] = location.Latitude.ToString();
                App.Current.Properties["Longitude"] = location.Longitude.ToString();
                //PopOutAlert(location.Latitude.ToString());
            });
        }

        public void HandleProviderDisabled(object sender, ProviderDisabledEventArgs e)
        {
            Log.Debug(TAG, "Location provider disabled event raised");
        }

        public void HandleProviderEnabled(object sender, ProviderEnabledEventArgs e)
        {
            Log.Debug(TAG, "Location provider enabled event raised");
        }

        public void HandleStatusChanged(object sender, StatusChangedEventArgs e)
        {
            Log.Debug(TAG, "Location status changed, event raised");
        }
    }
}