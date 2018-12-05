using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using LGSEApp.ViewModels;
using LGSEApp.Services.Models;
using Xamarin.Essentials;
using LGSEApp.Services.Services;
using System.Diagnostics;

namespace LGSEApp.View
{
    public partial class MainPage : ContentPage
    {
        PropertyViewModel viewModel;
        public MainPage(bool sync=false)
        {
            var vm = new PropertyViewModel(sync);
          //  vm.LoadItemsCommand.Execute(null);           
            BindingContext = viewModel = vm;
           
            //   SortingId.SelectedIndex = 1;
            InitializeComponent();
           
        }
       
        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as PropertyModel;
            if (item == null)
                return;

            await Navigation.PushAsync(new MPRNDetailPage(item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }


      
      

        protected override bool OnBackButtonPressed()
        {
            if (viewModel.IsFilter)
            {
                viewModel.IsFilter = false;
                viewModel.IsListEnabled = true;
                return true;
            }
            else
            {
                Navigation.PopToRootAsync();
                //   base.OnBackButtonPressed();
                return false;
            }
        }

       
    }


}