using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfKeyBorad
{
    class Win32
    {
        [DllImport("User32.dll")]
        public static extern uint SendInput(uint numberInput, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] INPUT[] input, int structSize);

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT // структура для эмуляции клавиатуры
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT
        {
            [FieldOffset(0)]
            public int type;
            [FieldOffset(4)]
            public KEYBDINPUT keyboard;
        };

        const uint KEYEVENTF_KEYUP = 0x0002; // событие Up
        const uint KEYEVENTF_SCANCODE = 0x0008; // событие Down

        private bool _isKeyVisible = false;
        private INPUT[] inputs = new INPUT[1];
    }
}
