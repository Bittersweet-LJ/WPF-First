using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace testTreeView.Models
{
    public class PartType : ObservableObject
    {

        public PartType(string code, PartType parent)
        {
            Code = code;
            Parent = parent;
        }

        public string Code { get; }
        public PartType Parent { get; }

        private string _displayName;

        public string DisplayName
        {
            get { return _displayName; }
            set { SetProperty(ref _displayName, value); }
        }


        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }


        private string _codeRules;
        public string CodeRules
        {
            get { return _codeRules; }
            set { SetProperty(ref _codeRules, value); }
        }


        private string _nameRules;
        public string NameRules
        {
            get { return _nameRules; }
            set { SetProperty(ref _nameRules, value); }
        }


        private bool _isReplaceable;
        public bool IsReplaceable
        {
            get { return _isReplaceable; }
            set { SetProperty(ref _isReplaceable, value); }
        }


        private bool _isAssembleable;
        public bool IsAssembleable
        {
            get { return _isAssembleable; }
            set { SetProperty(ref _isAssembleable, value); }
        }
        private ObservableCollection<PartType> _children = new ObservableCollection<PartType>();


        public ObservableCollection<PartType> Children
        {
            get { return _children; }
            set { SetProperty(ref _children, value); }
        }

        
    }

    public class PartTypeTree
    {

        public ObservableCollection<PartType> RootPartTypeCollection { get; set; } = new ObservableCollection<PartType>();

    }

}
