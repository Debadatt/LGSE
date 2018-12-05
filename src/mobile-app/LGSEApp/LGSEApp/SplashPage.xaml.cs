using LGSEApp.Services.Services;
using LGSEApp.View;
using LGSEApp.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LGSEApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {   
            InitializeComponent();           
        }
        protected override void OnAppearing()
        {
            LoadMainAsync();
            base.OnAppearing();         
        }
        private async void LoadMainAsync()
        {
            await Task.Delay(2000);
            SplashViewModel splashViewModel = new SplashViewModel();
          await  splashViewModel.Splash();
        }
        protected override bool OnBackButtonPressed()
        {

            Navigation.PopToRootAsync();
            // base.OnBackButtonPressed();
            return false;
        }
    }
}