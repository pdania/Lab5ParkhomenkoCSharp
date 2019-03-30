using System.Diagnostics;

namespace Lab5ParkhomenkoCSharp2019
{
    public class ProcessList
    {
        public int Id => Process.Id;
        public string ProcessName => Process.ProcessName;
        public bool KeepAlive { get; set; }
        public Process Process { get; }
        public string FileName { get; }
        public string Arguments { get; }

        public ProcessList(Process process)
        {
            Process = process;
            FileName = process.StartInfo.FileName;
            Arguments = process.StartInfo.Arguments;
        }
    }
}