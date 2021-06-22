using KeyboradPanel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using static WpfKeyBorad.Win32;

namespace WpfKeyBorad
{
    class Keyboard : INotifyPropertyChanged
    {
        public static DependencyProperty VirtualKeyProperty = DependencyProperty.RegisterAttached("VirtualKey",
                                                             typeof(Key), typeof(Keyboard), new PropertyMetadata());

        private List<ButtonBase> _buttons;
        private List<Key> _keys;

        public Keyboard()
        {
            _keys = new List<Key>();
            _buttons = new List<ButtonBase>();

            _keys.Add(new Key(KeybordKey.KEY_Q));
            _keys.Add(new Key(KeybordKey.KEY_W));
            _keys.Add(new Key(KeybordKey.KEY_E));
            _keys.Add(new Key(KeybordKey.KEY_R));
            _keys.Add(new Key(KeybordKey.KEY_T));
            _keys.Add(new Key(KeybordKey.KEY_Y));
            _keys.Add(new Key(KeybordKey.KEY_U));
            _keys.Add(new Key(KeybordKey.KEY_I));
            _keys.Add(new Key(KeybordKey.KEY_O));

            ButtonClickCommand = new RelayCommand(o =>
            {
                uint code = (uint)(KeybordKey)o;

                int scanCode = Win32.MapVirtualKey(code, 0);

                Send((ScanCode)(ushort)scanCode);
            });

            InitializeButton();
        }

        private void InitializeButton()
        {
            for (int i = 0; i < _keys.Count; i++)
            {
                _buttons.Add(new RepeatButton());
                _buttons[i].Command = ButtonClickCommand;
                _buttons[i].Height = _keys[i].Height;
                _buttons[i].Width = _keys[i].Width;
                _buttons[i].Margin = new Thickness(2);
                _buttons[i].Content = _keys[i].SomeKey;
                _buttons[i].Focusable = false;

                SetVirtualKeyValue(_buttons[i], _keys[i]);

                _buttons[i].CommandParameter = _keys[i].SomeKey;
            }
        }

        public List<ButtonBase> Buttons => _buttons;

        public ICommand ButtonClickCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public static void SetVirtualKeyValue(DependencyObject element, Key key)
        {
            element.SetValue(VirtualKeyProperty, key);
        }

        public static ButtonBase GetVirtualKeyValue(DependencyObject element)
        {
            return (ButtonBase)element.GetValue(VirtualKeyProperty);
        }

        public void Send(ScanCode a)
        {
            INPUT[] inputs = new INPUT[1];
            INPUT Input = new INPUT();
            Input.type = 1; // 1 = Keyboard Input
            Input.union.ki.wScan = (uint)a;
            Input.union.ki.dwFlags = (uint)KEYEVENTF.KEYDOWN;
            inputs[0] = Input;

            var result = SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
            var error = Marshal.GetLastWin32Error();
        }

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
