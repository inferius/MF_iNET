using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mime;
using System.Web;

namespace INetCore.Core.Net
{
    public class Request
    {
        private string _head;
        private ContentType _contentType;
        private System.Net.HttpListener listener = new System.Net.HttpListener();
        private System.Net.WebRequest _httpRequest = System.Net.HttpWebRequest.Create("www.request.com");
        private System.Net.HttpVersion _httpVersion = new System.Net.HttpVersion();
    }

    public enum RequestType
    {
        Options,
        Get,
        Head,
        Post,
        Put,
        Delete,
        Trace,
        Connect
    }
}
