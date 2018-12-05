using LGSEApp.Services.Services;
using LGSEApp.View;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LGSEApp
{
    public partial class App : Application
    {
        //   public static bool IsUserLoggedIn { get; set; }
        public App()
        {

            InitializeComponent();
           
            MainPage = new NavigationPage(new SplashPage());
            var seconds = TimeSpan.FromSeconds(20);
            Xamarin.Forms.Device.StartTimer(seconds, () =>
                {
                    return CheckConnection();
                }
            );
        }

        private bool CheckConnection()
        {
            var networkAccess = Connectivity.NetworkAccess;
            if (networkAccess == NetworkAccess.Internet)
            {
                Debug.WriteLine("Pending Data:{0}",Client.PendingCount);
                if (Client.PendingCount != 0)
                {
                    DataSync();
                    Client.PendingCount = 0;
                    Debug.WriteLine("Data sync on server");
                }
                Debug.WriteLine("Pending Data:{0}", Client.PendingCount);
            }
            else
            {
                Debug.WriteLine("Pending Data:{0}", Client.PendingCount);
                Debug.WriteLine("Data check Internet not avaliable sync on server");
               
            }
            return true;
        }
        public async void DataSync()
        {
            await AllSyncService.PushDataAsync();
        }



        protected override void OnStart()
        {
            base.OnStart();
            Debug.WriteLine("OnStart");

        }

        protected override void OnSleep()
        {
            base.OnSleep();
            Debug.WriteLine("OnSleep");

        }

        protected override void OnResume()
        {
            base.OnResume();
            Debug.WriteLine("OnResume");



        }

    }
}
