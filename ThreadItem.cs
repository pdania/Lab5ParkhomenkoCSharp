using System;
using System.Diagnostics;

namespace Lab5ParkhomenkoCSharp2019
{
    public class ThreadItem
    {
        public int Id { get;}
        public ThreadState State { get;}
        public DateTime StartTime { get;}

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