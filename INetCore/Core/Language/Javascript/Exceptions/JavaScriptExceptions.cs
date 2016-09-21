namespace INetCore.Core.Language.Javascript.Exceptions
{
    public class JavaScriptExceptions : System.Exception
    {
        public int Line { private set; get; }

        public JavaScriptExceptions()
        {

        }

        public JavaScriptExceptions(string message)
            : base(message)
        {

        }

        public JavaScriptExceptions(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }

        public JavaScriptExceptions(string message, int line)
            : base(message)
        {
            Line = line;
        }

        public JavaScriptExceptions(string message, int line, System.Exception innerException)
            : base(message, innerException)
        {
            Line = line;
        }

        
    }
}
