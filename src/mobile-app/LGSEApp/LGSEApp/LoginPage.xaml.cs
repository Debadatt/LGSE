using LGSEApp.Behaviors;
using LGSEApp.View;
using LGSEApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LGSEApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
      
        public LoginPage()
        {            
            var vm = new LoginViewModel();
            
         //   vm.UserName.Value = Convert.ToString(Application.Current.Properties["userId"]);
       //  vm.UserName.Value = "isolator@lgse.com"; //"abhijeetukalbhor@gmail.com";
      // vm.Password.Value = "Lgse@123";           
            this.BindingContext = vm;
            InitializeComponent();
           
        }
     
        protected override bool OnBackButtonPressed()
        {
            
          Navigation.PopToRootAsync();
             // base.OnBackButtonPressed();
            return false;
        }
      
    }
}