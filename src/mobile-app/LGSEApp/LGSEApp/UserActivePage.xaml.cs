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
	public partial class UserActivePage : ContentPage
	{
		public UserActivePage(string email)
		{
            var vm = new UserActiveViewModel(email);
            this.BindingContext = vm;
            InitializeComponent ();          

        }
       

    }
}