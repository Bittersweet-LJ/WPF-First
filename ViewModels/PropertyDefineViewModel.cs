using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testTreeView.Models;
using testTreeView.Services;
using testTreeView.Views;

namespace testTreeView.ViewModels
{
    public class PropertyDefineViewModel:ObservableObject
    {

        private readonly PropertyDefineService _propertyDefineService;


        public PropertyDefineViewModel(PropertyDefineService propertyDefineService)
        {
            _propertyDefineService = propertyDefineService;
            
            AddPropertyCommand = new RelayCommand(AddPropertyCommandAction);
            RemovePropertyCommand = new RelayCommand(RemovePropertyCommandAction);
        }

       

        private PartType _partType;
        public PartType PartType
        {
            get { return _partType; }
            set { SetProperty(ref _partType, value);
                CurrentPropertyList = _propertyDefineService.GetPropertyListFromDict(PartType);
                ParentPropertyList = _propertyDefineService.GetParentPropertyListFromDict(PartType);
            }
        }

        private Property _currentProperty;
        public Property CurrentProperty
        {
            get { return _currentProperty; }
            set { SetProperty(ref _currentProperty, value); }
        }

        private PropertyList _currentPropertyList;
        public PropertyList CurrentPropertyList
        {
            get { return _currentPropertyList; }
            set { SetProperty(ref _currentPropertyList, value); }
        }

        private PropertyList _parentPropertyList;
        public PropertyList ParentPropertyList
        {
            get { return _parentPropertyList; }
            set { SetProperty(ref _parentPropertyList, value); }
        }

        public RelayCommand AddPropertyCommand { get; }
        public void AddPropertyCommandAction()
        {
            InputPropertyNameWindow inputPropertyNameWindow = new InputPropertyNameWindow();
            inputPropertyNameWindow.NameSubmit += InputPropertyNameCheck;
            if (inputPropertyNameWindow.ShowDialog() == true)
            {
                _propertyDefineService.CreateProperty(PartType, inputPropertyNameWindow.PropertyName);

            }
        }
        public void InputPropertyNameCheck(object sender,string propertyName)
        {
            InputPropertyNameWindow inputPropertyNameWindow = sender as InputPropertyNameWindow;         
            if (_propertyDefineService.NewPropertyNameCanUse(propertyName, out int errCode))
            {
                inputPropertyNameWindow.DialogResult =true;
            }
            else
            {
                inputPropertyNameWindow.ErrorMessage = errCode.ToString() + "名称不合法";
            }
            
        }

        public RelayCommand RemovePropertyCommand { get; }
        public void RemovePropertyCommandAction()
        {
            if (CurrentProperty != null)
            {
                _ = _propertyDefineService.DeleteProperty(PartType, CurrentProperty);
            }
        }
    }
}
