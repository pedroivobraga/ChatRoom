using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebScoket
{
    public class ClientRegistration
    {
        private readonly Func<ClientRegistration, string, Task> _handle;
        private readonly Action<string> _quit;
        private NetworkStream _stream;
        private bool _isClosed;
        private TcpClient client;
        private Func<string, Task> handleAsync;

        private TcpClient _client { get; set; }

        public string Id { get; private set; }



        public ClientRegistration(TcpClient client, Func<ClientRegistration, string, Task> handle, Action<string> quit, string welcome)
        {
            this.Id = Guid.NewGuid().ToString();
            this._client = client;
            this._handle = handle;
            this._quit = quit;

            _stream = _client.GetStream();

            Task.Run(() =>
            {
                Notify(welcome);
            });

            Task.Run(() =>
            {
                Watch();
            });
        }


        public ClientRegistration(TcpClient client, Func<string, Task> handleAsync, object quitAsync)
        {
            this.client = client;
            this.handleAsync = handleAsync;
        }

        public void Quit()
        {
            Close();

            _quit.Invoke(Id);
        }

        public void Notify(string text)
        {

            byte[] outStream = System.Text.Encoding.UTF8.GetBytes(text);

            _stream.Write(outStream, 0, outStream.Length);
            _stream.Flush();
        }

        public void Close()
        {
            _isClosed = true;
        }

        private void Watch()
        {
            while (!_isClosed)
            {
                while (!_stream.DataAvailable) ;

                Byte[] bytes = new Byte[_client.Available];

                _stream.Read(bytes, 0, bytes.Length);


                string data = Encoding.UTF8.GetString(bytes);

                _handle.Invoke(this, data);
            }

            _stream.Close();
            _stream.Dispose();
            _stream = null;
        }
    }
}
