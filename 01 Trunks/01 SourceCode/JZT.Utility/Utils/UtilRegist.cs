using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;

namespace JZT.Utility.Service
{
    public static class UtilRegist
    {
        public static void WriteRegist(string exePath)
        {
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey run = hklm.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

            try
            {
                run.SetValue(exePath, exePath);
                hklm.Close();
            }

            catch (Exception my)
            {
                MessageBox.Show(my.Message.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void DelRegist(string exePath)
        {
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey run = hklm.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

            try
            {
                run.DeleteValue(exePath, false);
                hklm.Close();
            }

            catch (Exception my)
            {
                MessageBox.Show(my.Message.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
