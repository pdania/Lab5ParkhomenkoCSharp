using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Lab5ParkhomenkoCSharp2019
{
    public class ProcessItem
    {
        public int Id { get; }
        public string ProcessName { get; }
        public string FileName { get; }
        public int Threads { get; private set; }
        public double Cpu { get; private set; }
        public double RamPercent { get; private set; }
        public double RamVolume { get; private set; }
        public string UserName { get; }
        public string StartTime { get; }
        public string Responding { get; }
        public Process Process { get; }
        private readonly ulong _totalMemory;

        private PerformanceCounter CpuCounter { get; }
        private PerformanceCounter RamCounter { get; }
        
        public ProcessItem(Process process)
        {
            Process = process;
            Id = process.Id;
            ProcessName = process.ProcessName;
            CpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName, true);
            RamCounter = new PerformanceCounter("Process", "Working Set", process.ProcessName, true);
            _totalMemory = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
            try
            {
                CpuCounter.NextValue();
            }
            catch (InvalidOperationException)
            {
            }

            try
            {
                RamCounter.NextValue();
            }
            catch (InvalidOperationException)
            {
            }

            RefreshMetadata();
            try
            {
                FileName = process.MainModule.FileName;
            }
            catch
            {
                FileName = "Access denied";
            }

            try
            {
                StartTime = process.StartTime.ToString();
            }
            catch
            {
                StartTime = "Access denied";
            }

            IntPtr wtsCurrentServerHandle = IntPtr.Zero;
            int WTS_UserName = 5;
            if (process.ProcessName != "Idle")
            {
                IntPtr AnswerBytes;
                IntPtr AnswerCount;
                if (WTSQuerySessionInformationW(wtsCurrentServerHandle,
                    process.SessionId,
                    WTS_UserName,
                    out AnswerBytes,
                    out AnswerCount))
                {
                    UserName = Marshal.PtrToStringUni(AnswerBytes);
                    if (UserName == "")
                    {
                        UserName = "System";
                    }
                }
            }
            else
            {
                UserName = "System";
            }

            Responding = process.Responding ? "Yes" : "No";
        }

        /*
         * Function that refresh cpu, ram and ram %
         */
        public void RefreshMetadata()
        {
            try
            {
                Cpu = (Math.Round(CpuCounter.NextValue() / Environment.ProcessorCount, 5));
            }
            catch (InvalidOperationException)
            {
            }

            Threads = Process.Threads.Count;
            try
            {
                RamVolume = Math.Round(RamCounter.NextValue() / 1024 / 1024, 5);
            }
            catch (InvalidOperationException)
            {
            }

            try
            {
                RamPercent =
                    Math.Round(
                        (double) RamCounter.NextValue() /
                        _totalMemory * 100, 5);
            }
            catch (InvalidOperationException)
            {
            }
        }

        [DllImport("Wtsapi32.dll")]
        private static extern bool WTSQuerySessionInformationW(
            IntPtr hServer,
            int sessionId,
            int wtsInfoClass,
            out IntPtr ppBuffer,
            out IntPtr pBytesReturned);
    }
}