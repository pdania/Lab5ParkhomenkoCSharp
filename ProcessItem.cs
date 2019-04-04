using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;
using System.Windows;

namespace Lab5ParkhomenkoCSharp2019
{
    public class ProcessItem
    {
        public int Id { get; private set; }
        public string ProcessName { get; private set; }
        public string FileName { get; private set; }
        public int Threads { get; private set; }
        public double Cpu { get; private set; }
        public double RamPercent { get; private set; }
        public double RamVolume { get; private set; }
        public string UserName { get; private set; }
        public string StartTime { get; private set; }
        public string Responding { get; private set; }
        public Process Process { get; private set; }

        private PerformanceCounter CpuCounter { get; }
        private PerformanceCounter RamCounter { get; }

        public ProcessItem(Process process)
        {
            Process = process;
            Id = process.Id;
            ProcessName = process.ProcessName;
            CpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName, true);
            RamCounter = new PerformanceCounter("Process", "Working Set", process.ProcessName, true);

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
                    if (UserName == "")
                    {
                        UserName = "System";
                    }
                }
            }

            Responding = process.Responding ? "Yes" : "No";
        }

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
                RamVolume = Math.Round(RamCounter.NextValue(), 5);
            }
            catch (InvalidOperationException) { }

            try
            {
                RamPercent =
                    Math.Round(
                        (double) RamCounter.NextValue() /
                        new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory * 100, 5);
            }
            catch (InvalidOperationException) { }
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