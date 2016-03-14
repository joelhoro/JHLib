using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace WD
{
    class WebDumper : WebSocketBehavior, IDisposable
    {
        static List<string> _messageQueue = new List<string>();
        static WebSocketServer _server = null;
        static Thread _thread;

        const int _port = 8080;
        const string _servicePath = "/webdump";
        public WebDumper()
        {
            InitializeServer();
        }

        static string _url { get 
        {
            var hostName = Dns.GetHostName();
            var numericIp = Dns.GetHostEntry(hostName)
                .AddressList
                .Single(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .ToString();

            return string.Format("ws://{0}:{1}", numericIp, _port ); 
        } }

        protected override void OnOpen()
        {
            foreach (var message in _messageQueue)
                Send(message);
        }

        protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
        {
            Console.WriteLine("Received {0} from the client {1}", e.Data, ID);
            SendMessage(string.Format("<li>Got a message: {0}", e.Data));
        }

        public static void SendMessage(string message)
        {
            InitializeServer();
            Console.WriteLine("Sending message: " + message);
            _server.WebSocketServices.Broadcast(message);
            _messageQueue.Add(message);
        }

        static void InitializeServer()
        {
            if (_server != null)
                return;

            _server = new WebSocketServer(_url);
            _server.AddWebSocketService<WebDumper>(_servicePath);

            Console.WriteLine("Starting server on " + _url);

            _thread = new Thread(new ThreadStart(() => _server.Start()));
            _thread.Start();
            WaitForServerStartup();
        }

        private static void WaitForServerStartup()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var timeout = 10000;
            while (!_server.IsListening && stopWatch.ElapsedMilliseconds < timeout)
                Thread.Sleep(1);
            Console.WriteLine("Listening on port {0}, and providing WebSocket services:", _server.Port);
            foreach (var path in _server.WebSocketServices.Paths)
                Console.WriteLine("- {0}", path);
        }

        public void Dispose()
        {
            Console.WriteLine("Stopping server");
            _server.Stop();
            _thread.Abort();
        }
    }    
}
