// See https://aka.ms/new-console-template for more information

using Google.Protobuf;
using Google.Protobuf.Examples.Protocol;
using Server;
using Server.Session;
using ServerCore;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static Google.Protobuf.Examples.Protocol.Person.Types;

class Program{
    static Listener _listener = new Listener();

    static void FlushRoom()
    {
        JobTimer.Instance.Push(FlushRoom, 250);
    }

    static void Main(string[] args)
    {
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);


        _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
        Console.WriteLine("Listening...");

        //FlushRoom();
        JobTimer.Instance.Push(FlushRoom);

        while (true)
        {
            JobTimer.Instance.Flush();
        }
    }
}

