using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LGSEApp.ViewModels;
using LGSEApp.Services.Models;
using LGSEApp.Services.Services;
using LGSEApp.Services.Tables;

namespace LGSEApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MPRNDetailPage : ContentPage
    {
        PropertyDetailViewModel viewModel;
        public MPRNDetailPage(PropertyModel viewModel)
        {
           
                var vm = new PropertyDetailViewModel(viewModel);
                //  vm.LoadItemsCommand.Execute(null);
                //  vm.PropertyModel = viewModel.PropertyModel;
                //  vm = viewModel;
                this.BindingContext= this.viewModel = vm;
                InitializeComponent();
           
     
        }
       


      
        private void ListViewStatus_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as PropertyStatusMstr;
            if (item == null)
                return;
            viewModel.Status = item;
            viewModel.SubStatus = null;
            viewModel.ExecuteSubStatusLoadItemsCommand(viewModel);
        }       

        private void ListViewSubStatus_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as PropertySubStatusMstr;
            if (item == null)
                return;
            viewModel.SubStatus = item;
            viewModel.ExecuteSubStatusSelectedItemsCommand(viewModel);
        }
        protected override bool OnBackButtonPressed()
        {
            if (viewModel.IsSubStatus)
            {
                viewModel.IsSubStatus = false;
                viewModel.IsStatus = true;
                return true;
            }
            else if (viewModel.IsStatus)
            {
                viewModel.IsStatus = false;
                viewModel.IsBlur = "#FFFFFF";
                viewModel.IsBusy = false;
                return true;
            }
            else
            {               
                return false;
            }
        }
    }
}