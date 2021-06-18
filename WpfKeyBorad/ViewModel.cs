using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfKeyBorad
{
    class ViewModel : INotifyPropertyChanged
    {
        private bool _isKeyVisible = false;

        public ViewModel()
        {
            ButtonClickCommand = new RelayCommand(o => 
            {
               
            });
        }

        //public ICommand TextBoxFocusedCommand { get; set; }

        public ICommand ButtonClickCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
