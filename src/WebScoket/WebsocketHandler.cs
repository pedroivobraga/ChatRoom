using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WebScoket
{
    public class WebsocketHandler
    {
        private bool _isClosed;

        private List<ClientRegistration> _clients;
        private readonly IPipelineBuilder _builder;
        private readonly ILogger _logger;
        private PipelineExecutor executor;

        public WebsocketHandler(IPipelineBuilder builder, ILogger logger)
        {
            _builder = builder;
            _logger = logger;
            _clients = new List<ClientRegistration>();

            executor = _builder.Build();
        }

        public void Start(string host, int port, string welcome = "")
        {
            TcpListener server = new TcpListener(IPAddress.Parse(host), port);

            server.Start();

            _logger.Notify("Server initialized");

            while (!_isClosed)
            {
                TcpClient client = server.AcceptTcpClient();

                var clientRegistration = new ClientRegistration(client, HandleAsync, QuitAsync, welcome);

                _logger.Notify($"A new client was registered '{clientRegistration.Id}'");

                _clients.Add(clientRegistration);
            }
        }

        private void QuitAsync(string clientId)
        {
                var client = _clients.Where(x => x.Id == clientId).FirstOrDefault();

                _clients.Remove(client);
        }

        protected async Task HandleAsync(ClientRegistration client, string content)
        {
            try
            {
                var request = new WebSocketRequest() { Body = content };
                //var request = JsonConvert.DeserializeObject<WebSocketRequest>(content);

                var context = new WebSocketContext(request, client, NotifyClient, NotifyEveryone);
                await executor.RunAsync(context);

                if(string.IsNullOrWhiteSpace(context.Response.Body) == false)
                {
                    client.Notify(context.Response.Body);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to parse request", ex.Message);
            }
        }

        public void NotifyClient(string clientId, string message)
        {
            var client = _clients.Where(x => x.Id == clientId).FirstOrDefault();

            if (client != null)
                client.Notify(message);
        }
        public void NotifyEveryone(string message, string except)
        {
            foreach (var client in _clients)
            {
                if(client.Id != except)
                    client.Notify(message);
            }
        }

        public void Close()
        {
            foreach (var client in _clients)
            {
                client.Close();
            }

            _isClosed = true;

        }
    }
}
