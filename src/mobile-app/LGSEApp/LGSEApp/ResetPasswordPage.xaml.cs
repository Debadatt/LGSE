using LGSEApp.Behaviors;
using LGSEApp.Services.Models;
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
	public partial class ResetPasswordPage : ContentPage
	{
		public ResetPasswordPage(string email)
		{
            var vm = new ResetPasswordViewModel(email);
            this.BindingContext = vm;
            InitializeComponent ();          

        }

        protected override bool OnBackButtonPressed()
        {
            //  Navigation.PopToRootAsync();
            //   base.OnBackButtonPressed();
            return true;
        }
    }
}