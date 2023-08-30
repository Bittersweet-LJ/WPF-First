using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using testTreeView.Models;
using testTreeView.Services;
using testTreeView.Views;

namespace testTreeView.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly PartTypeService _partTypeService;
        private readonly PropertyDefineService _propertyDefineService;
        private readonly LocationModeDefineService _locationModeDefineService;

        public MainWindowViewModel() { }
        public MainWindowViewModel(PartTypeService partTypeService,PropertyDefineService propertyDefineService,LocationModeDefineService locationModeDefineService)
        {
            //服务
            _partTypeService = partTypeService;
            _partTypeTree = _partTypeService.GetPartTypeTree();
            
            _propertyDefineService = propertyDefineService;
            _locationModeDefineService = locationModeDefineService;


            //命令
            AddPartTypeCommand = new RelayCommand(AddPartTypeCommandAction);
            RemovePartTypeCommand = new RelayCommand(RemovePartTypeCommandAction);
            SaveCommand = new RelayCommand(SaveCommandAction);
        }

        private PartType _currentPartType;

        public PartType CurrentPartType
        {
            get { return _currentPartType; }
            set { SetProperty( ref _currentPartType,value); }
        }


        private PartTypeTree _partTypeTree;
        public PartTypeTree PartTypeTree
        {
            get { return _partTypeTree; }
            set
            {            
                SetProperty(ref _partTypeTree, value);
            }
        }

        public RelayCommand AddPartTypeCommand { get; }
        private void AddPartTypeCommandAction()
        {
            //1. 显示窗体
            //2. 获得编辑框内容TypeCode (Binding)
            //3. 根据当前所选节点和code新建零件类型
            //4. 将内容显示到页面 (Binding)
            InputPartTypeCodeWindow inputPartTypeCodeWindow = new InputPartTypeCodeWindow();
            inputPartTypeCodeWindow.CodeSubmit += InputPartTypeCodeCheck;
            if (inputPartTypeCodeWindow.ShowDialog() == true)
            {
                _ = _partTypeService.CreatePartType(inputPartTypeCodeWindow.TypeCode, CurrentPartType);
                CurrentPartType = null;
            }
        }

        private void InputPartTypeCodeCheck(object sender, string typeCode)
        {
            InputPartTypeCodeWindow inputPartTypeCodeWindow = sender as InputPartTypeCodeWindow;
            if (_partTypeService.NewPartTypeCodeCanUse(typeCode, out int errCode))
            {             
                inputPartTypeCodeWindow.DialogResult = true;
            }
            else
            {
                switch (errCode)
                {
                    case 1:
                        inputPartTypeCodeWindow.ErrorMessage = errCode.ToString() + " 命名重复";
                        break;
                    case 2:
                        inputPartTypeCodeWindow.ErrorMessage = errCode.ToString() + " 命名格式错误";
                        break;
                    default:
                        break;
                }
            }
        }

        public RelayCommand RemovePartTypeCommand { get; }

        private void RemovePartTypeCommandAction()
        {
            if (CurrentPartType != null)
            {
                _partTypeService.DeletePartType(CurrentPartType);
                if (_partTypeTree.RootPartTypeCollection == null)
                {
                    CurrentPartType = null;
                }
            }
              


        }

        public RelayCommand SaveCommand { get; }

        private void SaveCommandAction()
        {
            _partTypeService.SavePartTypes();
            _propertyDefineService.SaveProperties();
            _locationModeDefineService.SaveLocations();
        }

        
    }
}
