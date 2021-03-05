using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatRomm.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();

            client.Connect();

            Console.ReadLine();
        }

    }
    public class Client {

        private TcpClient client;
        private NetworkStream stream;
        string readData = null;

        public void Connect()
        {
            client = new System.Net.Sockets.TcpClient();
            stream = default(NetworkStream);
            client.Connect("127.0.0.1", 80);
            stream = client.GetStream();


            var receive = Task.Run(() =>
            {
                while (true)
                {
                    while (!stream.DataAvailable) ;

                    Byte[] bytes = new Byte[client.Available];

                    stream.Read(bytes, 0, bytes.Length);
                    //translate bytes of request to string
                    String data = Encoding.UTF8.GetString(bytes);

                    Console.WriteLine(data);
                }
            });
            var send = Task.Run(() =>
            {
                while (true)
                {
                    var text = Console.ReadLine();

                    byte[] outStream = System.Text.Encoding.UTF8.GetBytes(text);
                    stream.Write(outStream, 0, outStream.Length);
                    stream.Flush();

                }
            });

            Task.WaitAll(receive, send);

            //Thread ctThread = new Thread(getMessage);
            //ctThread.Start();
        }

        private void getMessage()
        {
            while (true)
            {
                stream = client.GetStream();
                int buffSize = 0;
                byte[] inStream = new byte[client.Available];
                buffSize = client.ReceiveBufferSize;
                stream.Read(inStream, 0, buffSize);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                readData = "" + returndata;
            }
        }
    }
}
