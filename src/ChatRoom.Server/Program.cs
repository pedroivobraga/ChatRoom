using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using WebScoket;

namespace ChatRoom.Server
{
    class Program
    {
        public static Hashtable clientsList = new Hashtable();

        static void Main(string[] args)
        {


            IPipelineBuilder _builder = new PipelineBuilder();

            _builder.Use(new IdentificationPipeline());
            _builder.Use(new ChatRoomPipeline());

            var executor = _builder.Build();

            var handler = new WebsocketHandler(_builder, new Logger());

            Console.WriteLine("Initializing server...");
            handler.Start("127.0.0.1", 80, "Welcome, please enter your nickname:");


        //TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);

        //    server.Start();
        //    Console.WriteLine("Server has started on 127.0.0.1:80.{0}Waiting for a connection...", Environment.NewLine);

        //    TcpClient client = server.AcceptTcpClient();

        //    Console.WriteLine("A client connected.");

        //    NetworkStream stream = client.GetStream();

        //    //enter to an infinite cycle to be able to handle every change in stream
        //    while (true)
        //    {
        //        Console.WriteLine("Aguardando mensagem...");
        //        while (!stream.DataAvailable) ;

        //        Byte[] bytes = new Byte[client.Available];

        //        stream.Read(bytes, 0, bytes.Length);
        //        //translate bytes of request to string
        //        String data = Encoding.UTF8.GetString(bytes);

        //        if (new Regex("^GET").IsMatch(data))
        //        {
        //            Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + Environment.NewLine
        //                + "Connection: Upgrade" + Environment.NewLine
        //                + "Upgrade: websocket" + Environment.NewLine);

        //            stream.Write(response, 0, response.Length);
        //        }
        //        else
        //        {
        //            byte[] outStream = System.Text.Encoding.UTF8.GetBytes("Mensagem recebida.");
        //            stream.Write(outStream, 0, outStream.Length);
        //            stream.Flush();

        //        }
        //    }

            Console.ReadLine();
        }

        public static void broadcast(string msg, string uName, bool flag)

        {

            foreach (DictionaryEntry Item in clientsList)

            {

                TcpClient broadcastSocket;

                broadcastSocket = (TcpClient)Item.Value;

                NetworkStream broadcastStream = broadcastSocket.GetStream();

                Byte[] broadcastBytes = null;



                if (flag == true)

                {

                    broadcastBytes = Encoding.ASCII.GetBytes(uName + " says : " + msg);

                }

                else

                {

                    broadcastBytes = Encoding.ASCII.GetBytes(msg);

                }



                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                broadcastStream.Flush();

            }

        }  //end broadcast function
    }

    public class Logger : ILogger
    {
        public void Error(string message)
        {
            Console.WriteLine(message);
        }

        public void Notify(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class handleClinet

    {

        TcpClient clientSocket;

        string clNo;

        Hashtable clientsList;



        public void startClient(TcpClient inClientSocket, string clineNo, Hashtable cList)

        {

            this.clientSocket = inClientSocket;

            this.clNo = clineNo;

            this.clientsList = cList;

            Thread ctThread = new Thread(doChat);

            ctThread.Start();

        }



        private void doChat()

        {

            int requestCount = 0;

            byte[] bytesFrom = new byte[10025];

            string dataFromClient = null;

            Byte[] sendBytes = null;

            string serverResponse = null;

            string rCount = null;

            requestCount = 0;



            while ((true))

            {

                try

                {

                    requestCount = requestCount + 1;

                    NetworkStream networkStream = clientSocket.GetStream();

                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);

                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);

                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                    Console.WriteLine("From client - " + clNo + " : " + dataFromClient);

                    rCount = Convert.ToString(requestCount);



                    Program.broadcast(dataFromClient, clNo, true);

                }

                catch (Exception ex)

                {

                    Console.WriteLine(ex.ToString());

                }

            }//end while

        }//end doChat

    } //end class handleClinet

}
