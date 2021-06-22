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
        public static extern uint SendInput(uint numberInput, INPUT[] input, int structSize);

        [DllImport("user32.dll")]
        static internal extern int MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        static internal extern int ToUnicode(uint virtualKeyCode, uint scanCode, // Convert virtual key to current language
           byte[] keyboardState,
           [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)]
            StringBuilder receivingBuffer,
           int bufferSize, uint flags);

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public uint type;
            public InputUnion union;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)]
            public MouseInput mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HardwareInput hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT // структура для эмуляции клавиатуры
        {
            public uint wVk;
            public uint wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HardwareInput
        {
            internal int uMsg;
            internal short wParamL;
            internal short wParamH;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseInput
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [Flags]
        public enum KEYEVENTF : uint
        {
            KEYDOWN = 0x0000,
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            SCANCODE = 0x0008,
            UNICODE = 0x0004
        }
    }
}
