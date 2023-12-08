﻿using System;

namespace Capstone.Exceptions
{
    public class UserInfoRetrievalErrorException : Exception
    {
        public UserInfoRetrievalErrorException() : base() { }
        public UserInfoRetrievalErrorException(string message) : base(message) { }
        public UserInfoRetrievalErrorException(string message, Exception inner) : base(message, inner) { }
    }
}
