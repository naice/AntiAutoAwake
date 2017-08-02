using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCFGUtil
{
    public class CMD : IDisposable
    {
        private readonly Process _process;

        public ProcessStartInfo StartInfo { get { return _process.StartInfo; } }
        public string[] Lines { get; private set; }

        public CMD(string fileName) : this(fileName, null)
        {

        }
        public CMD(string fileName, string arguments)
        {
            _process = new Process();
            _process.StartInfo.FileName = fileName;
            _process.StartInfo.Arguments = arguments;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.CreateNoWindow = true;
            _process.StartInfo.RedirectStandardOutput = true;
        }

        public void Execute()
        {
            _process.Start();
            string strOutput = _process.StandardOutput.ReadToEnd();

            var lineSep = Environment.NewLine;
            Lines = strOutput.Split(new string[] { lineSep }, StringSplitOptions.None);

            _process.WaitForExit();
        }

        public void Dispose()
        {
            if (_process != null)
                _process.Dispose();
        }
    }
}
