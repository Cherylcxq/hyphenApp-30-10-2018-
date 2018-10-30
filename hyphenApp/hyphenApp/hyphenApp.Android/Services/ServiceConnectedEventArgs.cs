using System;

using Android.OS;

namespace hyphenApp.Droid.Services
{
    public class ServiceConnectedEventArgs : EventArgs
    {
        public IBinder Binder { get; set; }
    }
}