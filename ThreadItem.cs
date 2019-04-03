using System;
using System.Diagnostics;

namespace Lab5ParkhomenkoCSharp2019
{
    public class ThreadItem
    {
        public int Id { get;private set; }
        public ThreadState State { get;private set; }
        public DateTime StartTime { get; private set; }

        public ThreadItem(ProcessThread thread)
        {
            Id = thread.Id;
            try
            {
                StartTime = thread.StartTime;
            }
            catch
            {

            }

            State = thread.ThreadState;
        }
    }
}