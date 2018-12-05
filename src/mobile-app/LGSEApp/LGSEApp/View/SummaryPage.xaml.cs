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
	public partial class SummaryPage : ContentPage
	{
        SummaryViewModel viewModel;
        public SummaryPage (PropertyDetailViewModel viewModel)
		{
            var vm = new SummaryViewModel(viewModel);
            BindingContext = this.viewModel = vm;
            
            InitializeComponent ();
           
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingPopupPage());
        }
    }
}