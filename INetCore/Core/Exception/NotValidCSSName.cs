namespace INetCore.Core.Exception
{
    class NotValidCSSName : System.Exception
    {
        public NotValidCSSName()
        {

        }

        public NotValidCSSName(string message)
            : base(message)
        {

        }

        public NotValidCSSName(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
