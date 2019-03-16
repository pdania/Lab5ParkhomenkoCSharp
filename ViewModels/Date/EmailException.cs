using System;

namespace Lab3ParkhomenkoCSharp2019.ViewModels.Date
{
    public class EmailException : ArgumentException
    {
        public string Value { get; }
        public EmailException(string message, string val)
            : base(message)
        {
            Value = val;
        }
    }
}