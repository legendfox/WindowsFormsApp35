using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp35
{
    public partial class Form1 : Form
    {
        private delegate int KeyboardHookProc(int code, int wParam, ref KeyboardHookStruct lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance, uint threadId);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref KeyboardHookStruct lParam);

        private readonly IntPtr _hook;

        private struct KeyboardHookStruct
        {
            public int VkCode;
            public int ScanCode;
            public int Flags;
            public int Time;
            public int DwExtraInfo;
        }

        public Form1()
        {
            InitializeComponent();
            var hInstance = LoadLibrary("User32");
            _hook = SetWindowsHookEx(13, HookProc, hInstance, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        Keys _OncekiKey;
        private int HookProc(int code, int wParam, ref KeyboardHookStruct lParam)
        {
            if (code >= 0 && lParam.Flags == 0)
            {
                var key = (Keys)lParam.VkCode;
                    textBox1.Text += key.ToString() + "" + Environment.NewLine;

                if (key == Keys.B&& _OncekiKey==Keys.LControlKey)
                {
                    SendKeys.Send("Hasan Bey");

                }
                if (key == Keys.Q )
                {
                    SendKeys.Send("cem Bey");

                }
                _OncekiKey = key;
            }
            return CallNextHookEx(_hook, code, wParam, ref lParam);
        }
    }
}
