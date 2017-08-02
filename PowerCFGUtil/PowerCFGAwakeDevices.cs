using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCFGUtil
{
    public static class PowerCFGAwakeDevices
    {
        public static List<PowerCFGAwakeDevice> GetDevices()
        {
            List<PowerCFGAwakeDevice> devices = new List<PowerCFGAwakeDevice>();

            using (CMD cmd = new CMD("powercfg", "-devicequery wake_armed"))
            {
                cmd.Execute();

                foreach (var item in cmd.Lines)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                        devices.Add(new PowerCFGAwakeDevice() { Name = item, IsSelected = true });
                }
            }

            return devices;
        }

        public static void DisableAwakeFor(IEnumerable<PowerCFGAwakeDevice> devices)
        {
            foreach (var item in devices)
            {
                using (CMD cmd = new CMD("powercfg", $"-DEVICEDISABLEWAKE \"{item.Name}\""))
                {
                    cmd.Execute();

                    item.Status = string.Join(" ", cmd.Lines);
                }
            }
        }
    }
}
