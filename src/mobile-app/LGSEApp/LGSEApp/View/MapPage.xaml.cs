using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using LGSEApp.ViewModels;
using LGSEApp.Services.Models;

namespace LGSEApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
        //PropertyModel items;
        Map map;
       
        Geocoder geoCoder;
        public MapPage (PropertyModel items)
		{
            var vm = new MapViewModel(items);            
            BindingContext =  vm;
			InitializeComponent ();

            //geoCoder = new Geocoder();

            //    map = new Map(MapSpan.FromCenterAndRadius(new Position(vm.PropertyModel.Latitude, vm.PropertyModel.Longitude), Distance.FromKilometers(0.1)))
            //{
            //    IsShowingUser = true,
            //    VerticalOptions = LayoutOptions.FillAndExpand

            //};


            var position = new Position(vm.PropertyModel.Latitude, vm.PropertyModel.Longitude); // Latitude, Longitude
            //var pin = new Pin
            //{
            //    Type = PinType.Place,
            //    Position = position,
            //    Label = vm.PropertyModel.BuildingNumber + (vm.PropertyModel.BuildingNumber.Length == 0 ? "" : (vm.PropertyModel.BuildingName.Length == 0 ? "" : ", ")) + vm.PropertyModel.BuildingName,
            //    Address = vm.PropertyModel.PrincipalStreet + (vm.PropertyModel.PrincipalStreet.Length == 0 ? "" : (Convert.ToString(vm.PropertyModel.Postcode).Length == 0 ? "" : ", ")) + vm.PropertyModel.Postcode
            //};
            //map.Pins.Add(pin);
            //var stack = new StackLayout { Spacing = 0.5, Orientation = StackOrientation.Vertical, };            
            //stack.Children.Add(map);
            //Content = stack;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    Device.OpenUri(
                      new Uri(string.Format("http://maps.apple.com/?q={0}", WebUtility.UrlEncode("warje"))));
                    break;
                case Device.Android:
                    Device.OpenUri(
                      new Uri(string.Format("geo:0,0?q={0}", WebUtility.UrlEncode("Satara"))));
                    break;
                case Device.UWP:
                    Device.OpenUri(
       new Uri(string.Format("bingmaps:?where={0}", Uri.EscapeDataString("warje"))));//position.ToString()
                    break;
            }
        }
      

       
    }
}