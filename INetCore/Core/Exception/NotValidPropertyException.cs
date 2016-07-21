namespace INetCore.Core.Exception
{
    class NotValidPropertyException : System.Exception
    {
        public NotValidPropertyException()
        {

        }

        public NotValidPropertyException(string message)
            : base(message)
        {

        }

        public NotValidPropertyException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
