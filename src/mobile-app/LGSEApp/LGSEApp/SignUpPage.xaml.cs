using LGSEApp.Behaviors;
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
	public partial class SignUpPage : ContentPage
	{
        
        public SignUpPage ()
		{
          
            var vm = new SignUpViewModel();           
            vm.LoadItemsCommand.Execute(null);
            this.BindingContext = vm;
            InitializeComponent();
		    var forgetPassword_tap = new TapGestureRecognizer();
		    forgetPassword_tap.Tapped += (s, e) =>
		    {
		        DisplayAlert("Accept Terms and Conditions", "This transmission and any attachments to it are strictly confidential and are intended solely for the person or organisation to whom it is addressed. Its contents may contain legal professional or other privileged information. If you are not the intended recipient, please notify us immediately and delete it, without retaining it, copying it, disclosing its contents to anyone or acting upon it. You must ensure that you have appropriate virus protection before you open or detach any documents from this transmission. We accept no responsibility for viruses. We may monitor replies to emails for operational or lawful business reasons. The views or opinions expressed in this email are the author's own and may not, unless expressly stated to the contrary, reflect the views or opinions of Wales & West Utilities Limited, its affiliates or subsidiaries. Unless expressly stated to the contrary, neither Wales & West Utilities Limited, its affiliates or subsidiaries, their respective directors, officers or employees make any representation about, or accept any liability for, the accuracy or completeness of such views or opinions. Wales & West Utilities Limited Registered office: Wales & West House, Spooner Close, Celtic Springs, Coedkernew, NEWPORT NP10 8FZ Registered in England and Wales No 5046791", "Ok");

            };
		    lblTerms.GestureRecognizers.Add(forgetPassword_tap);
        }

	  
	}
}