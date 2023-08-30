using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testTreeView.Models
{
    public class Location:ObservableObject
    {

        public Location(string name)
        {
            Name = name;
        }
        public string Name { get;}

        private string _displayName;

        public string DisplayName
        {
            get { return _displayName; }
            set { SetProperty(ref _displayName, value); }
        }

        private string _basePartCode;

        public string BasePartCode
        {
            get { return _basePartCode; }
            set { SetProperty(ref _basePartCode, value); }
        }

        private int _mode;

        public int Mode
        {
            get { return _mode; }
            set { SetProperty(ref _mode, value); }
        }

        private bool _isDefault;
        public bool IsDefault
        {
            get { return _isDefault; }
            set { SetProperty(ref _isDefault, value); }
        }


    }

    public class LocationList
    {
        public string PartCode { get; }
        public ObservableCollection<Location> LocationCollection { get; } = new ObservableCollection<Location>();
    }
}
