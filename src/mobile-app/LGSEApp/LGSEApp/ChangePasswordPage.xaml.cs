using LGSEApp.Behaviors;
using LGSEApp.Services.Models;
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
	public partial class ChangePasswordPage : ContentPage
	{
		public ChangePasswordPage()
		{
            var vm = new ChangePasswordViewModel();
            this.BindingContext = vm;
            InitializeComponent ();          

        }
      

    }
}