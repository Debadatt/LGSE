using LGSEApp.Services.Models;
using LGSEApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace LGSEApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapWebViewPage : ContentPage
	{
		public MapWebViewPage (PropertyModel items)
		{
            var vm = new MapViewModel(items);
            BindingContext = vm;
            InitializeComponent ();
            var position = new Position(vm.PropertyModel.Latitude, vm.PropertyModel.Longitude); // Latitude, Longitude
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = position,
                Label = vm.PropertyModel.BuildingNumber + (vm.PropertyModel.BuildingNumber.Length == 0 ? "" : (vm.PropertyModel.BuildingName.Length == 0 ? "" : ", ")) + vm.PropertyModel.BuildingName,
                Address = vm.PropertyModel.PrincipalStreet + (vm.PropertyModel.PrincipalStreet.Length == 0 ? "" : (Convert.ToString(vm.PropertyModel.Postcode).Length == 0 ? "" : ", ")) + vm.PropertyModel.Postcode
            };
            // webMap.Source = string.Format("bingmaps:?where={0}", position);
            //https://www.google.com/maps/embed/v1/directions?key=AIzaSyBDeLLKSqV4q8cKyMIZfPq2_FBnIqfepxk&origin=47.4242683,8.5542138&destination=47.2572183,8.60678729999995
            //https://www.google.com/maps/embed/v1/view?key=AIzaSyBDeLLKSqV4q8cKyMIZfPq2_FBnIqfepxk&center=-33.8569,151.2152&zoom=18&maptype=satellite
            // webMap.Source = string.Format("https://www.google.com/maps/embed/v1/view?key=AIzaSyA5Kp3TdfKsWYGDc8n_Sia4FdMNnwoXUNg&center=-33.8569,151.2152&zoom=18&maptype=satellite");
           // webMap.Source = string.Format("https://www.bing.com/maps?cp={0}~{1}&lvl=18&style=r", vm.PropertyModel.Latitude, vm.PropertyModel.Longitude);
           Device.OpenUri(new Uri(string.Format("bingmaps:?where={0}", Uri.EscapeDataString(position.ToString()))));
        }
    }
}