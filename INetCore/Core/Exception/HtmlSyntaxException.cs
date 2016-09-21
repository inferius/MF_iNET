namespace INetCore.Core.Exception
{
    class HtmlSyntaxException : System.Exception
    {
        private int _line;
        private string _tag;

        public HtmlSyntaxException()
        {

        }

        public HtmlSyntaxException(string message)
            : base(message)
        {

        }

        public HtmlSyntaxException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }

        public HtmlSyntaxException(string message, int line, string tag)
            : base(message)
        {
            _line = line;
            _tag = tag;
        }

        public HtmlSyntaxException(string message, int line, string tag, System.Exception innerException)
            : base(message, innerException)
        {
            _line = line;
            _tag = tag;
        }

        public int Line
        {
            get { return this._line; }
        }

        public string Tag
        {
            get { return _tag; }
        }
    }
}
