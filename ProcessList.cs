using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;

namespace Lab5ParkhomenkoCSharp2019
{
    public class ProcessList
    {
        public int Id { get;private set; }
        public string ProcessName { get; private set; }
        public string FileName { get; private set;}
        public int Threads { get; private set; }
        public double Cpu { get; private set; }
        public double RamPercent{ get; private set; }
        public long RamVolume { get; private set; }
        public string UserName { get; private set; }
        public DateTime StartTime { get; private set; }
        public string Responding { get; private set; }

        public ProcessList(Process process)
        {
            Id = process.Id;
            ProcessName = process.ProcessName;
            var counter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
            counter.NextValue();
            Cpu = (Math.Round(counter.NextValue()/Environment.ProcessorCount, 1));
            try
            {
                FileName = process.MainModule.FileName;
            }
            catch { }

            Threads = process.Threads.Count;
            RamVolume = process.PrivateMemorySize64;
            try
            {
                StartTime = process.StartTime;
            }catch{ }

            IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;
            int WTS_UserName = 5;
            if (process.ProcessName != "Idle")
            {
                IntPtr AnswerBytes;
                IntPtr AnswerCount;
                if (WTSQuerySessionInformationW(WTS_CURRENT_SERVER_HANDLE,
                    process.SessionId,
                    WTS_UserName,
                    out AnswerBytes,
                    out AnswerCount))
                {
                    UserName = Marshal.PtrToStringUni(AnswerBytes);
                }
            }

            Responding = process.Responding ? "Yes" : "No";
        }
        [DllImport("Wtsapi32.dll")]
        private static extern bool WTSQuerySessionInformationW(
            IntPtr hServer,
            int SessionId,
            int WTSInfoClass,
            out IntPtr ppBuffer,
            out IntPtr pBytesReturned);
    }

}