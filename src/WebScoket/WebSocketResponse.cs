using System;
using System.Text;

namespace WebScoket
{
    public class WebSocketResponse
    {
        private StringBuilder _content = new StringBuilder();


        public string Body => _content.ToString();

        public void Write(string text)
        {
            _content.Append(text);
        }

        public void WriteLine(string text)
        {
            _content.AppendLine(text);
        }

    }
}
