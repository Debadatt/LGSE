using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net;
using Android.OS;
using LGSEApp.View;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Permissions;
using System;
using Xamarin.Essentials;

namespace LGSEApp.Droid
{
  [Activity(Label = "LGSE Support", Icon = "@drawable/Icon", Theme = "@style/MainTheme",  MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //[Activity(Label = "LGSE App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
  //  [Activity(Label = "LGSEApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        bool hasNotified = false;

        private System.Timers.Timer timer = new System.Timers.Timer();
        
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
           ToolbarResource = Resource.Layout.Toolbar;
            //SetTheme(0);
            base.OnCreate(bundle);
            // Initialize Azure Mobile Apps
            CurrentPlatform.Init();
            //  Rg.Plugins.Popup.Popup.Init(this, bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
            LoadApplication(new App());
          
            Xamarin.FormsMaps.Init(this, bundle);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        private void Timer_Elapsed(object sender, EventArgs e)
        {

            timer.Stop();
           
         var   networkAccess = Connectivity.NetworkAccess;
            if (networkAccess == NetworkAccess.Internet)
            {
                hasNotified = true;
                Xamarin.Forms.MessagingCenter.Send<MainPage>(new MainPage(), "Internet connection has been lost");
            }
            timer.Start();
        }
    }
}

