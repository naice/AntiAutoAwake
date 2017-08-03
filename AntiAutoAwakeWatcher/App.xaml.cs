using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AntiAutoAwakeWatcher
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        WpfNotifyIcon notifyIcon;
        USBDeviceWatcher usbDeviceWatcher;
        bool IsAutomaticDeviceDisablingActivated;

        protected override void OnStartup(StartupEventArgs e)
        {
            IsAutomaticDeviceDisablingActivated = true;
            notifyIcon = new WpfNotifyIcon("AntiAutoAwake Watcher", new WpfNotifyIconAction[] {
                new WpfNotifyIconAction() { Text = "Automatic Device Disabling", IsChecked = IsAutomaticDeviceDisablingActivated, IsCheckedChanged=(isChecked)=>IsAutomaticDeviceDisablingActivated = isChecked},
                new WpfNotifyIconAction() { Type = WpfNotifyIconActionType.Seperator },
                new WpfNotifyIconAction() { Text = "Deactivate Watcher", Action = ()=> Shutdown(0) }
            });
            usbDeviceWatcher = new USBDeviceWatcher(
                () => PerformAwakeScan());
            base.OnStartup(e);
        }

        private void PerformAwakeScan()
        {
            //ToDo: go on.
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (notifyIcon != null)
                notifyIcon.Dispose();
            if (usbDeviceWatcher != null)
                usbDeviceWatcher.Dispose();

            base.OnExit(e);
        }
    }
}
