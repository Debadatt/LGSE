using LGSEApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LGSEApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingPopupPage : ContentPage
	{
		public SettingPopupPage ()
		{
            var vm = new SettingViewModel();

            BindingContext = vm;
                InitializeComponent();
          
			
		}
        
        //private async void btnChangePassword_Clicked(object sender, EventArgs e)
        //{

        //    await Navigation.PushAsync(new ChangePasswordPage());
        // //   await PopupNavigation.PopAsync(true);
        //}

        //private async void btnLogOut_Clicked(object sender, EventArgs e)
        //{
        //    Application.Current.Properties.Clear();           
        //    await Navigation.PushAsync(new LoginPage());
        // //   await PopupNavigation.PopAsync(true);
        //}
       
    }
}