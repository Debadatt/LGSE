using LGSEApp.Services.Services;
using LGSEApp.View;
using LGSEApp.ViewModels.Base;
using Plugin.Permissions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LGSEApp.ViewModels
{
  public  class SplashViewModel : ViewModelBase
    {       
    
    public  async Task Splash()
        {
            //switch (Device.RuntimePlatform)
            //{
            //    case Device.Android:
            //     await   Permission();
            //        break;
            //    case Device.UWP:
            //     await   Permission();
            //        break;
            //}

            NavigationOnResume();
        }
            async Task Permission()
        {
           
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Location);
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    //if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Location))
                    //{
                    //    await DisplayAlert("Need location", "Gunna need that location", "Allow");
                    //}

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Plugin.Permissions.Abstractions.Permission.Location });
                    status = results[Plugin.Permissions.Abstractions.Permission.Location];
                }

                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    // var results = await Plugin.Permissions.Abstractions.PermissCrossGeolocator.Current.GetPositionAsync(10000);
                    //  LabelGeolocation.Text = "Lat: " + results.Latitude + " Long: " + results.Longitude;
                }
                else if (status != Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
                {
                  //  await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                // LabelGeolocation.Text = "Error: " + ex;
            }

        }
        async void NavigationOnResume()
        {
            try
            {
                //   Handle when your app starts
                if (Application.Current.Properties.ContainsKey("IsUserLoggedIn")) //Check if property is sat
                {
                    bool PropName = Convert.ToBoolean(Application.Current.Properties["IsUserLoggedIn"]);

                    if (PropName)
                    {
                        GetApplicationData();
                         NavigationSync();
                        Application.Current.MainPage = new NavigationPage(new MainPage(true));
                        //  await Application.Current.MainPage.Navigation.PushAsync(new MainPage(true));

                        //Page you want the user to see if he is logged in
                    }
                    else
                    {
                        navigationOnLogin();
                        //Login page
                    }
                }
                else
                {
                    navigationOnLogin();
                    //Login page
                }
            }
            catch(Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }
        Page page = new Page();
        private  async void NavigationSync()
        {
            networkAccess = Connectivity.NetworkAccess;
            if (networkAccess == NetworkAccess.Internet)
            {
                await AllSyncService.DataAsync();
            }
            else
            {
                await page.DisplayAlert("Alert", ResponceCode.customErrorFunction(651, null), "Ok");
            }
        }

        async void navigationOnLogin()
        {
          Application.Current.MainPage = new NavigationPage(new LoginPage());
          //  await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }
    }
}
