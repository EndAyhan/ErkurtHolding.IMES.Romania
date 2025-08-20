using ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Main;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel
{
    internal static class Program
    {
        // ---- Win32 ----
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private const int SW_RESTORE = 9;
        private const int SW_MAXIMIZE = 3;

        // ---- Constants ----
        private const string MutexName = @"Global\IMES_OperatorPanel_SingleInstance";

        // Tekrar eden Contains() çağrıları için O(1) erişim ve GC basıncı düşük olsun diye HashSet
        private static readonly HashSet<string> BypassMutexTypes =
            new HashSet<string>(StringComparer.Ordinal)
            {
            };

        /// <summary>Main entry point.</summary>
        [STAThread]
        private static void Main()
        {
            // Type okunurken yan etkiden kaçınmak için bir kez alıp sabitleyin
            var type = StaticValues.WorkCenterType;

            // Bazı tiplerde tek-instans kuralını devre dışı bırak
            if (BypassMutexTypes.Contains(type))
            {
                StartApp(type);
                return;
            }

            // Single-instance: createdNew=false ise başka bir proses zaten çalışıyor
            var createdNew = false;

            // initiallyOwned=false: Mutex’i beklemeden açar; tek-instans kontrolünü hızlı yapar
            using (var mutex = new Mutex(initiallyOwned: false, name: MutexName, createdNew: out createdNew))
            {
                if (createdNew)
                {
                    // Uygulamayı başlat
                    StartApp(type);
                }
                else
                {
                    // Zaten çalışıyorsa: mevcut pencereyi öne getir ve büyüt
                    TryActivateRunningInstance();
                    Environment.Exit(0);
                }
            }
        }

        private static void StartApp(string type)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                switch (type)
                {
                    default:
                    case "PANEL":
                        Application.Run(new FrmOperatorPanel());
                        break;

                    case "FILLBOX":
                        Application.Run(new FrmFillHandlingUnit());
                        break;
                }
            }
            catch (Exception ex)
            {
                // Minimum tanısal bilgi; isterseniz burada log altyapınıza yönlendirin
                MessageBox.Show(
                    $"Unexpected error:\n{ex.Message}",
                    "iMES Operator Panel",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Mevcut instance’ı bulup ön plana getirir ve (isteğe bağlı) büyütür.
        /// </summary>
        private static void TryActivateRunningInstance()
        {
            try
            {
                var current = Process.GetCurrentProcess();
                var procs = Process.GetProcessesByName(current.ProcessName);

                foreach (var p in procs)
                {
                    if (p.Id == current.Id) continue;

                    // WinForms ana form açıldıysa MainWindowHandle dolu olur
                    var hWnd = p.MainWindowHandle;
                    if (hWnd != IntPtr.Zero)
                    {
                        // Restore + öne getir (ya da büyütmek isterseniz SW_MAXIMIZE kullanın)
                        ShowWindow(hWnd, SW_RESTORE);
                        SetForegroundWindow(hWnd);
                        break;
                    }
                }
            }
            catch
            {
                // Sessiz yut: aktivasyon başarısız olsa bile uygulamayı kapatacağız
            }
        }
    }
}
