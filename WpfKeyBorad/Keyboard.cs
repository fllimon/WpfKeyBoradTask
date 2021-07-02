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
        public const int MILTIPLY_VALUE = 2;
        public const int MILTIPLY_SPACEKEY_VALUE = 10;

        private List<ButtonBase> _buttons;
        private List<Key> _keys;
        private bool _isShiftClicked = false; 

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
            _keys.Add(new Key(KeybordKey.KEY_P));
            _keys.Add(new Key(KeybordKey.OEM_4));
            _keys.Add(new Key(KeybordKey.OEM_6));
            _keys.Add(new Key(KeybordKey.BACK));
            _keys.Add(new Key(KeybordKey.NUMPAD7));
            _keys.Add(new Key(KeybordKey.NUMPAD8));
            _keys.Add(new Key(KeybordKey.NUMPAD9));
            _keys.Add(new Key(KeybordKey.TAB));
            _keys.Add(new Key(KeybordKey.KEY_A));
            _keys.Add(new Key(KeybordKey.KEY_S));
            _keys.Add(new Key(KeybordKey.KEY_D));
            _keys.Add(new Key(KeybordKey.KEY_F));
            _keys.Add(new Key(KeybordKey.KEY_G));
            _keys.Add(new Key(KeybordKey.KEY_H));
            _keys.Add(new Key(KeybordKey.KEY_J));
            _keys.Add(new Key(KeybordKey.KEY_K));
            _keys.Add(new Key(KeybordKey.KEY_L));
            _keys.Add(new Key(KeybordKey.OEM_1));
            _keys.Add(new Key(KeybordKey.OEM_7));
            _keys.Add(new Key(KeybordKey.RETURN));
            _keys.Add(new Key(KeybordKey.NUMPAD4));
            _keys.Add(new Key(KeybordKey.NUMPAD5));
            _keys.Add(new Key(KeybordKey.NUMPAD6));
            _keys.Add(new Key(KeybordKey.SHIFT));
            _keys.Add(new Key(KeybordKey.KEY_Z));
            _keys.Add(new Key(KeybordKey.KEY_X));
            _keys.Add(new Key(KeybordKey.KEY_C));
            _keys.Add(new Key(KeybordKey.KEY_V));
            _keys.Add(new Key(KeybordKey.KEY_B));
            _keys.Add(new Key(KeybordKey.KEY_N));
            _keys.Add(new Key(KeybordKey.KEY_M));
            _keys.Add(new Key(KeybordKey.OEM_COMMA));
            _keys.Add(new Key(KeybordKey.OEM_PERIOD));
            _keys.Add(new Key(KeybordKey.OEM_2));
            _keys.Add(new Key(KeybordKey.OEM_5));
            _keys.Add(new Key(KeybordKey.NUMPAD1));
            _keys.Add(new Key(KeybordKey.NUMPAD2));
            _keys.Add(new Key(KeybordKey.NUMPAD3));
            //_keys.Add(new Key(KeybordKey.OEM_3));
            _keys.Add(new Key(KeybordKey.OEM_8));
            _keys.Add(new Key(KeybordKey.SPACE));
            _keys.Add(new Key(KeybordKey.LEFT));
            _keys.Add(new Key(KeybordKey.RIGHT));
            _keys.Add(new Key(KeybordKey.OEM_PLUS));
            _keys.Add(new Key(KeybordKey.NUMPAD0));
            _keys.Add(new Key(KeybordKey.DECIMAL));

            ButtonClickCommand = new RelayCommand(o =>
            {
                Send((KeybordKey)o);
            });

            ShiftClickCommand = new RelayCommand(o =>
            {
                KEYEVENTF keyFlag;

                if (!IsShiftClicked)
                {
                    keyFlag = KEYEVENTF.KEYDOWN;

                    IsShiftClicked = true;
                }
                else
                {
                    keyFlag = KEYEVENTF.KEYUP;

                    IsShiftClicked = false;
                }

                SendShift((KeybordKey)o, keyFlag);
            });

            InitializeButton();
        }

        private void InitializeButton()
        {
            for (int i = 0; i < _keys.Count; i++)
            {
                if (_keys[i].SomeKey == KeybordKey.SHIFT)
                {
                    _buttons.Add(new ToggleButton());
                    _buttons[i].Command = ShiftClickCommand;
                }
                else
                {
                    _buttons.Add(new RepeatButton());
                    _buttons[i].Command = ButtonClickCommand;
                }

                _buttons[i].Content = GetKeyToChar(_keys[i].SomeKey, _isShiftClicked, false);
                _buttons[i].Height = _keys[i].Height;
                _buttons[i].Width = _keys[i].Width;
                _buttons[i].FontSize = 18;

                if (_keys[i].SomeKey == KeybordKey.OEM_PLUS)
                {
                    _buttons[i].Visibility = Visibility.Hidden;
                }

                if (_keys[i].SomeKey == KeybordKey.SPACE)
                {
                    _buttons[i].Content = _keys[i].SomeKey;
                    _buttons[i].Width = _keys[i].Width * MILTIPLY_SPACEKEY_VALUE;
                }
                else if (_keys[i].SomeKey == KeybordKey.SHIFT)
                {
                    _buttons[i].Content = _keys[i].SomeKey;
                    _buttons[i].Width = _keys[i].Width * MILTIPLY_VALUE + 4;
                    _buttons[i].SetValue(KeyboardPanel.LineBreakBeforeProperty, true);
                }
                else if (_keys[i].SomeKey == KeybordKey.BACK)
                {
                    _buttons[i].Content = _keys[i].SomeKey;
                }
                else if (_keys[i].SomeKey == KeybordKey.RETURN)
                {
                    _buttons[i].Content = "Enter";
                    _buttons[i].FontSize = 18;
                }
                else if (_keys[i].SomeKey == KeybordKey.LEFT)
                {
                    _buttons[i].Content = "<-";
                }
                else if (_keys[i].SomeKey == KeybordKey.RIGHT)
                {
                    _buttons[i].Content = "->";
                }
                else if (_keys[i].SomeKey == KeybordKey.MENU)
                {
                    _buttons[i].Content = _keys[i].SomeKey;
                    _buttons[i].Width = _keys[i].Width * MILTIPLY_VALUE;
                }
                else if (_keys[i].SomeKey == KeybordKey.TAB)
                {
                    _buttons[i].Content = _keys[i].SomeKey;
                    _buttons[i].Width = _keys[i].Width;
                    _buttons[i].SetValue(KeyboardPanel.LineBreakBeforeProperty, true);
                }
                else if (_keys[i].SomeKey == KeybordKey.NUMPAD0)
                {
                    _buttons[i].Width = _keys[i].Width * MILTIPLY_VALUE;
                }
                else if (_keys[i].SomeKey == KeybordKey.OEM_PLUS)
                {
                    _buttons[i].Width = 32;
                }
                else
                {
                    _buttons[i].Width = _keys[i].Width;
                }

                if (_keys[i].SomeKey == KeybordKey.OEM_8)
                {
                    _buttons[i].Content = "&123";
                    _buttons[i].SetValue(KeyboardPanel.LineBreakBeforeProperty, true);
                }

                _buttons[i].Margin = new Thickness(2);
                _buttons[i].Focusable = false;

                SetVirtualKeyValue(_buttons[i], _keys[i]);
                _buttons[i].CommandParameter = _keys[i].SomeKey;
            }
        }
        
        public List<ButtonBase> Buttons => _buttons;

        public bool IsShiftClicked
        {
            get => _isShiftClicked;
            set
            {
                _isShiftClicked = value;

                OnPropertyChanged();
            }
        }

        public ICommand ButtonClickCommand { get; set; }

        public ICommand ShiftClickCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public static void SetVirtualKeyValue(DependencyObject element, Key key)
        {
            element.SetValue(VirtualKeyProperty, key);
        }

        public static ButtonBase GetVirtualKeyValue(DependencyObject element)
        {
            return (ButtonBase)element.GetValue(VirtualKeyProperty);
        }

        public void Send(KeybordKey code)
        {
            INPUT[] inputs = new INPUT[1];
            INPUT Input = new INPUT();
            Input.type = 1; // 1 = Keyboard Input
            Input.union.ki.wVk = (uint)code;
            Input.union.ki.dwFlags = (uint)KEYEVENTF.KEYDOWN;
            inputs[0] = Input;

            var result = SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
            var error = Marshal.GetLastWin32Error();
        }

        public void SendShift(KeybordKey code, KEYEVENTF flag)
        {
            INPUT[] inputs = new INPUT[1];
            INPUT Input = new INPUT();
            Input.type = 1; // 1 = Keyboard Input
            Input.union.ki.wVk = (uint)code;
            Input.union.ki.dwFlags = (uint)flag;
            inputs[0] = Input;

            var result = SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
            var error = Marshal.GetLastWin32Error();
        }

        private string GetKeyToChar(KeybordKey keys, bool shift, bool altGr)
        {
            var buf = new StringBuilder(256);
            var keyboardState = new byte[256];
            if (shift)
                keyboardState[(int)KeybordKey.SHIFT] = 0xff;
            if (altGr)
            {
                keyboardState[(int)KeybordKey.CONTROL] = 0xff;
                keyboardState[(int)KeybordKey.MENU] = 0xff;
            }

            Win32.ToUnicode((uint)keys, 0, keyboardState, buf, 256, 0);

            return buf.ToString();
        }


        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
