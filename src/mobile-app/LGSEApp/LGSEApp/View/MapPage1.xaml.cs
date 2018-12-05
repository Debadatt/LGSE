using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace LGSEApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage1 : ContentPage
	{
		public MapPage1 ()
		{
			InitializeComponent ();
            MyMap.MoveToRegion(
    MapSpan.FromCenterAndRadius(
        new Position(18.500621, 73.821985), Distance.FromMiles(0.1)));
        }
	}
}