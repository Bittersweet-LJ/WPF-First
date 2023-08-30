using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testTreeView.Models
{
    public class Property : ObservableObject 
    {

        public Property(string name)
        {
            Name = name;
        }
        public string Name { get;  }

        private string _displayName = "新建类型";
        public string DisplayName
        {
            get { return _displayName; }
            set { SetProperty(ref _displayName, value); }
        }

        private int _valueType;
        public int ValueType
        {
            get { return _valueType; }
            set { SetProperty(ref _valueType, value); }
        }

        private bool _isReadOnly;
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { SetProperty(ref _isReadOnly, value); }
        }

        private string _defaultValue;
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { SetProperty(ref _defaultValue, value); }
        }

        private bool _isRequired;
        public bool IsRequired
        {
            get { return _isRequired; }
            set { SetProperty(ref _isRequired, value); }
        }

        private bool _isLimitedInput;
        public bool IsLimitedInput
        {
            get { return _isLimitedInput; }
            set { SetProperty(ref _isLimitedInput, value); }
        }

        private string _valueList;
        public string ValueList
        {
            get { return _valueList; }
            set { SetProperty(ref _valueList, value); }
        }

        private int _minValue;
        public int MinValue
        {
            get { return _minValue; }
            set { SetProperty(ref _minValue, value); }
        }

        private int _maxValue;
        public int MaxValue
        {
            get { return _maxValue; }
            set { SetProperty(ref _maxValue, value); }
        }

        private string _validateStr;
        public string ValidateStr
        {
            get { return _validateStr; }
            set { SetProperty(ref _validateStr, value); }
        }

        private string _validateErrTips;
        public string ValidateErrTips
        {
            get { return _validateErrTips; }
            set { SetProperty(ref _validateErrTips, value); }
        }

        private string _bindingAction;
        public string BindingAction
        {
            get { return _bindingAction; }
            set { SetProperty(ref _bindingAction, value); }
        }
    
    }

    public class PropertyList
    {
        public PropertyList()
        {
        }
        public PropertyList(ObservableCollection<Property> propertyCollection)
        {
            PropertyCollection = propertyCollection;
        }

        public ObservableCollection<Property> PropertyCollection { get; set; } = new ObservableCollection<Property>();
    }
}
