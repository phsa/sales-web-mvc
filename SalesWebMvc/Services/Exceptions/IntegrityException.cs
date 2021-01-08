using System;
using System.Runtime.Serialization;

namespace SalesWebMvc.Services
{
    [Serializable]
    internal class IntegrityException : Exception
    {

        public IntegrityException(string message) : base(message)
        {
        }
    }
}