using System;

namespace Orleans.Security
{
    [Serializable]
    public class OrleansClusterUnauthorizedAccessException : Exception
    {
        // ReSharper disable once UnusedMember.Global
        public OrleansClusterUnauthorizedAccessException()
        {
        }

        public OrleansClusterUnauthorizedAccessException(string message) : base(message)
        {
        }

        public OrleansClusterUnauthorizedAccessException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
