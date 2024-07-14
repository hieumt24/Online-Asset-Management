﻿namespace AssetManagement.Infrastructure.Exceptions
{
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}