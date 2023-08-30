using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testTreeView.Models;


namespace testTreeView.ViewModels
{
    public class TypeDefineViewModel : ObservableObject
    {
        public TypeDefineViewModel()
        {
            SelectIconCommand = new RelayCommand(SelectIconCommandAction);
        }

        private PartType _partType;
        public PartType PartType
        {
            get { return _partType; }
            set { SetProperty(ref _partType, value); }
        }

        public RelayCommand SelectIconCommand { get; }

        private void SelectIconCommandAction()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true&&File.Exists(dialog.FileName))
            {
                PartType.Icon = dialog.FileName;
            }
        }

        public RelayCommand BuildCodeRulesCommand { get; }

        public void BuildCodeRulesCommandAction() { }

        public RelayCommand BuildNameRulesCommand { get; }

        public void BuildNameRulesCommandAction() { }


    }
}
