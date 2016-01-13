using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DLL
{
    public static partial class Hook
    {
        private delegate int HookProc(int nCode, int wParam, IntPtr lParam);
        private static HookProc s_KeyboardDelegate;
        private static int s_KeyboardHookHandle;
        private static int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)//опр-ет какая  клавиша нажата
        {
            bool handled = false;
            if (nCode >= 0)
            {
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                if (s_KeyPress != null && wParam == WM_KEYDOWN)
                {
                    bool isDownShift = ((GetKeyState(VK_SHIFT) & 0x80) == 0x80 ? true : false);
                    bool isDownCapslock = (GetKeyState(VK_CAPITAL) != 0 ? true : false);
                    Encoding enc = Encoding.Default;
                    byte[] keyState = new byte[256];
                    GetKeyboardState(keyState);
                    byte[] inBuffer = new byte[2];
                    if (ToAscii(MyKeyboardHookStruct.VirtualKeyCode, MyKeyboardHookStruct.ScanCode, keyState, inBuffer, MyKeyboardHookStruct.Flags) == 1)
                    {
                        byte[] a = new byte[1];
                        a[0] = inBuffer[0];
                        if (a[0] > 31)
                        {
                            char key = Convert.ToChar(enc.GetString(a));
                            if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key))
                                key = Char.ToUpper(key);
                            KeyPressEventArgs e = new KeyPressEventArgs(key);
                            s_KeyPress.Invoke(null, e);
                            handled = handled || e.Handled;
                        }
                    }
                }
            }
            if (handled)
                return -1;
            return CallNextHookEx(s_KeyboardHookHandle, nCode, wParam, lParam);
        }

        private static void SetHook()//установление хука на клаву
        {
            if (s_KeyboardHookHandle == 0)
            {
                s_KeyboardDelegate = KeyboardHookProc;
                IntPtr hInstance = LoadLibrary("User32");
                s_KeyboardHookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, s_KeyboardDelegate, hInstance, 0);
            }
        }
        private static void UnHook()
        {
            if (s_KeyboardHookHandle != 0)
            {
                int result = UnhookWindowsHookEx(s_KeyboardHookHandle);
                s_KeyboardHookHandle = 0;
                s_KeyboardDelegate = null;
                if (result == 0)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode);
                }
            }
        }
    }
}
