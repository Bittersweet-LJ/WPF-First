using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using testTreeView.Models;
using testTreeView.ViewModels;

namespace testTreeView.Views.Converters
{
    public class PartToTypeDefineVM : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PartType partType)
            {
                var result = new TypeDefineViewModel
                {
                    PartType = partType
                };
                return result;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PartToPropertyDefineVM : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PartType partType)
            {
                var result = new PropertyDefineViewModel(new Services.PropertyDefineService())
                {
                    PartType = partType
                };
                return result;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PartToLocationDefineVM : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is PartType partType)
            {
                LocationModeDefineViewModel viewModel = new LocationModeDefineViewModel(new Services.LocationModeDefineService())
                {
                    PartType = partType
                };
                return viewModel;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
