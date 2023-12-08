﻿using System;

namespace Capstone.Exceptions
{
    public class IncorrectEntryException : Exception
    {
        public IncorrectEntryException() : base() { }
        public IncorrectEntryException(string message) : base(message) { }
        public IncorrectEntryException(string message, Exception inner) : base(message, inner) { }
    }
}
