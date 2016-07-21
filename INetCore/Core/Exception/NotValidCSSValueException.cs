namespace INetCore.Core.Exception
{
    class NotValidCSSValueException: System.Exception
    {
        public NotValidCSSValueException()
        {

        }

        public NotValidCSSValueException(string message)
            : base(message)
        {

        }

        public NotValidCSSValueException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
