﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace testeWindowsActivity.Models
{
    public class Processos
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public static void run()
        {
            string processoAnterior = string.Empty;

            while (true)
            {
                Process currentProcess = GetActiveProcess();

                if (currentProcess == null)
                    continue;
                try
                {
                    if (currentProcess.BasePriority == 8 && !currentProcess.ProcessName.Equals("explorer") && !currentProcess.MainModule.FileVersionInfo.FileDescription.Equals(processoAnterior) && !currentProcess.ProcessName.Equals("Idle"))
                    {
                        processoAnterior = currentProcess.MainModule.FileVersionInfo.FileDescription;
                        Console.WriteLine(processoAnterior + " - " + DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss"));
                    }
                }
                catch{}
            }
        }

        private static Process GetActiveProcess()
        {
            IntPtr hwnd = GetForegroundWindow();

            return hwnd != null ? GetProcessByHandle(hwnd) : null;
        }

        private static Process GetProcessByHandle(IntPtr hwnd)
        {
            try
            {
                uint processID;
                GetWindowThreadProcessId(hwnd, out processID);
                return Process.GetProcessById((int)processID);
            }
            catch { return null; }
        }
    }
}
