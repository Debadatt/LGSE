using LGSEApp.Services.Models;
using LGSEApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LGSEApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RolePage : ContentPage
	{
     
		public RolePage (ObservableCollection<RoleModel> roleModels)
		{
            var vm = new UserRolesViewModel();           
            vm.RoleList = roleModels;        
         
            this.BindingContext = vm;
            InitializeComponent ();         
           
        }

        protected override bool OnBackButtonPressed()
        {
            //base.OnBackButtonPressed();
            return true;
        }
    }
}