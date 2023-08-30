using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testTreeView.Models;
using testTreeView.Services;
using testTreeView.Views;
namespace testTreeView.ViewModels
{
    public class LocationModeDefineViewModel : ObservableObject
    {
        private readonly LocationModeDefineService _locationModeDefineService;
        public LocationModeDefineViewModel(LocationModeDefineService locationModeDefineService)
        {
            _locationModeDefineService = locationModeDefineService;



            AddLocationCommand = new RelayCommand(AddLocationCommandAction);
            DelLocationCommand = new RelayCommand(DelLocationCommandAction);
            SetDefaultCommand = new RelayCommand(SetDefaultCommandAction);
            SelectBasePartCommand = new RelayCommand(SelectBasePartCommandAction);
        }


        private PartType _partType;
        public PartType PartType
        {
            get { return _partType; }
            set 
            {
                SetProperty(ref _partType, value);
                LocationList = _locationModeDefineService.GetLocationListFromDict(PartType);
            }
        }

        private LocationList  _locationList;
        public LocationList LocationList
        {
            get { return _locationList; }
            set { SetProperty(ref _locationList ,value); }
        }

        private Location _currentLocation;
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set { SetProperty(ref _currentLocation, value); }
        }

        public RelayCommand AddLocationCommand { get; }
        public void AddLocationCommandAction()
        {
            InputLocationModelNameWindow inputLocationModelNameWindow = new InputLocationModelNameWindow();
            inputLocationModelNameWindow.NameSubmit += InputLocationModelNameWindowCommit;
            if (inputLocationModelNameWindow.ShowDialog() == true)
            {
                if(inputLocationModelNameWindow.LocationName !="")
                    _ = _locationModeDefineService.CreateLocation(PartType, inputLocationModelNameWindow.LocationName);
            }
        }

        private void InputLocationModelNameWindowCommit(object sender, EventArgs e)
        {
            if(sender is InputLocationModelNameWindow window)
            {
                window.DialogResult = true;
            }
        }

        public RelayCommand DelLocationCommand { get; }
        public void DelLocationCommandAction()
        {
            _locationModeDefineService.DelLocation(PartType, CurrentLocation);
        }

        public RelayCommand SetDefaultCommand { get; }

        public void SetDefaultCommandAction()
        {
            foreach(var location in LocationList.LocationCollection)
            {
                location.IsDefault = false;
            }
            CurrentLocation.IsDefault = true;
            //_locationModeDefineService.SetDefaultLocation(LocationList,CurrentLocation);
        }

        public RelayCommand SelectBasePartCommand { get; }

        public void SelectBasePartCommandAction()
        {
            SelectLocationBasePartWindow window = new SelectLocationBasePartWindow();
            window.BasePartSelectSubmit += Window_BasePartSelectSubmit;
            if (window.ShowDialog() == true)
            {
                CurrentLocation.BasePartCode = window.BasePartType?.Code;
            }
        }

        private void Window_BasePartSelectSubmit(object sender, EventArgs e)
        {
            if(sender is SelectLocationBasePartWindow window)
            {
                window.DialogResult = true;
            }
        }
    }
}
