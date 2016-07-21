namespace INetCore.Core.Exception
{
    class NotValidCSSPropertyException: System.Exception
    {
        public NotValidCSSPropertyException()
        {

        }

        public NotValidCSSPropertyException(string message)
            : base(message)
        {

        }

        public NotValidCSSPropertyException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
