using LGSEApp.Services.Models;
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
	public partial class PropertyHistoryPage : ContentPage
	{       
        public PropertyHistoryPage (PropertyModel viewModel)
		{
            var vm = new PropertyHistoryViewModel(viewModel);
            BindingContext = vm;
			InitializeComponent ();
		}
	}
}