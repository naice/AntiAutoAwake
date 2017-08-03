using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace AntiAutoAwakeWatcher
{
    class USBDeviceWatcher : IDisposable
    {
        //static readonly Guid GUID_DEVCLASS_USB = new Guid("{36fc9e60-c465-11cf-8056-444553540000}");
        readonly Action usbDeviceArrivedAction;
        readonly ManagementEventWatcher managementEventWatcher;
        bool isDisposed = false;

        public USBDeviceWatcher(Action deviceArrived)
        {
            WqlEventQuery query = new WqlEventQuery(
                "SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");
            
            managementEventWatcher = new ManagementEventWatcher();
            managementEventWatcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
            managementEventWatcher.Query = query;
            managementEventWatcher.Start();

            usbDeviceArrivedAction = deviceArrived;
        }

        private void watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            if (!isDisposed)
                usbDeviceArrivedAction?.Invoke();
        }

        public void Dispose()
        {
            managementEventWatcher.Dispose();
            isDisposed = true;
        }
    }
}
