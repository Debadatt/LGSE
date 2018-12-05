using LGSEApp.Services.Models;
using LGSEApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace LGSEApp.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        public PropertyModel _propertyModel { get; set; }
        public PropertyModel PropertyModel
        {
            get { return _propertyModel; }
            set
            {
                _propertyModel = value;
                RaisePropertyChanged(() => PropertyModel);
            }
        }

        public MapViewModel(PropertyModel item)
        {         

            PropertyModel = item;
           
        }

    }
}
