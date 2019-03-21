using System;

namespace Lab4ParkhomenkoCSharp2019.ViewModels.Date
{
    public class PersonTooYoungException : Exception
    {
        public PersonTooYoungException(string message)
            : base(message)
        { }
    }
}