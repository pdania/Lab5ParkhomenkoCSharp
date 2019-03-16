using System;

namespace Lab3ParkhomenkoCSharp2019.ViewModels.Date
{
    public class PersonDiedException : ArgumentException
    {

        public int Value { get; }
        public PersonDiedException(string message, int val)
            : base(message)
        {
            Value = val;
        }
    }
}