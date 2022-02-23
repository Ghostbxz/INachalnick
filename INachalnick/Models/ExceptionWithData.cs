using System;
using System.Runtime.Serialization;

namespace INachalnickTinyRestApi.Models
{
    public class ExceptionWithData : Exception
    {
        public object ExtraData { get; set; }
        public ExceptionWithData()
        {
        }

        public ExceptionWithData(string? message) : base(message)
        {
        }

        public ExceptionWithData(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ExceptionWithData(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}