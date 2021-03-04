using System;

namespace WebScoket
{
    public class WebSocketContext
    {
        private readonly Action<string, string> _notifyClient;
        private readonly Action<string, string> _notifyEveryone;

        public WebSocketContext(WebSocketRequest request, ClientRegistration client, Action<string, string> notifyClient, Action<string, string> notifyEveryone)
        { 
            _notifyClient = notifyClient;
            _notifyEveryone = notifyEveryone;

            Request = request;
            ClientRegistration = client;

            Response = new WebSocketResponse();
        }

        public ClientRegistration ClientRegistration { get; private set; }

        public WebSocketRequest Request { get; private set; }

        public WebSocketResponse Response { get; private set; }

        public void Notify(string message)
        {
            _notifyEveryone(message, ClientRegistration.Id);

        }

        public void Notify(string message, string clientId)
        {
            _notifyClient(clientId, message);
        }
    }
}
