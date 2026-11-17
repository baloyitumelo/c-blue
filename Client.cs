/*
using System.Net;
using System.Net.Sockets;
using System.Text;

const ushort PORT = 6969;
const ushort BUFF_SIZE = 2048;

var ipEndPoint = new IPEndPoint(IPAddress.Loopback, PORT);
using Socket client = new (ipEndPoint.AddressFamily,
                             SocketType.Stream,
                             ProtocolType.Tcp);

await client.ConnectAsync(ipEndPoint);

bool running = true;

while(running){
    string msg = Console.ReadLine();
    var msgBytes = Encoding.UTF8.GetBytes(msg);

    client.SendAsync(msgBytes, SocketFlags.None);
}
*/
