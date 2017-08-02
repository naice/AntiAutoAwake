using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiAutoAwake
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<PowerCFGUtil.PowerCFGAwakeDevice> Devices { get; private set; }
        public RelayCommand DisableAwake { get; private set; }
        public RelayCommand RefreshDevices { get; private set; }
        public RelayCommand InvertSelectedDevices { get; private set; }

        public MainViewModel()
        {
            Devices = new ObservableCollection<PowerCFGUtil.PowerCFGAwakeDevice>();
            PopulateDevices();
            CreateCommands();
        }

        private void PopulateDevices()
        {
            foreach (var device in PowerCFGUtil.PowerCFGAwakeDevices.GetDevices())
            {
                Devices.Add(device);
            }
        }

        private void CreateCommands()
        {
            DisableAwake = new RelayCommand(() => 
            {
                PowerCFGUtil.PowerCFGAwakeDevices.DisableAwakeFor(Devices.Where(device => device.IsSelected));
                Devices.Clear();
                PopulateDevices();
            });
            RefreshDevices = new RelayCommand(() => { Devices.Clear(); PopulateDevices(); });
            InvertSelectedDevices = new RelayCommand(() => { Devices.ToList().ForEach((device) => device.IsSelected = !device.IsSelected); });
        }
    }
}
