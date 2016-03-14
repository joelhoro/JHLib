using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WD
{
    class Program
    {
        static void TestWebDumper()
        {
            using (new WebDumper())
            {
                for (int i = 0; i < 50; i++)
                {
                    var html = string.Format("<li>Just received the data [{0}]", i);
                    WebDumper.SendMessage(html);
                    Thread.Sleep(1000);
                }
                Console.WriteLine("Press a key to stop the server...");
                Console.ReadKey(true);
            }
        }

        static void TestClient()
        {
            using (var ws = new WebSocket("ws://localhost:8080/test1"))
            {
                ws.OnMessage += (sender, e) =>
                  Console.WriteLine("Laputa says: " + e.Data);

                ws.Connect();
                ws.Send("BALUS");
                Console.ReadKey(true);
            }
        }

        static void Main(string[] args)
        {
            TestWebDumper();
            return;
        }
    }
}
