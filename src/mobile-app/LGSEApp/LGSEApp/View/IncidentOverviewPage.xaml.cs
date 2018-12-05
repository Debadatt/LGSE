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
	public partial class IncidentOverviewPage : ContentPage
	{
		public IncidentOverviewPage ()
		{
            var vm = new IncidentOverviewViewModel();        
            this.BindingContext = vm;
            InitializeComponent ();
		}
        protected override bool OnBackButtonPressed()
        {
           
            return true;
        }


    }
}
