using System.Net;
using System.Net.Sockets;
using System.Text;

class Server {

    const ushort PORT = 6969;
    const ushort BUFF_SIZE = 2048;

    static async Task Main(){
        var ipEndPoint = new IPEndPoint(IPAddress.Loopback, PORT);

        using Socket listener = new(ipEndPoint.AddressFamily, 
                            SocketType.Stream, 
                            ProtocolType.Tcp);

        listener.Bind(ipEndPoint);
        listener.Listen(100);

        var running = true;
        int client_ident = 0;

        while(running){
            Socket handler = await listener.AcceptAsync();
            _ = HandleClientAsync(handler, client_ident++);
        } 
    }

    static async Task HandleClientAsync(Socket handler, int client_ident){
        bool running = true;
        RESPParser resp;

        using (handler){
            while(running){
                var buffer = new byte[BUFF_SIZE];
                var received = await handler.ReceiveAsync(buffer, SocketFlags.None);

                if(received == 0){
                    running = false;
                    continue;
                }
                
                Console.WriteLine("Reading");
                resp = new RESPParser(buffer, received);

                Value value = resp.Read();
                Console.WriteLine("Read");
                Console.WriteLine($"Client {client_ident}: type -> {value.type}");
                Console.WriteLine($"Client {client_ident}: str -> {value.str}");
                Console.WriteLine($"Client {client_ident}: num -> {value.num}");
                Console.WriteLine($"Client {client_ident}: bulk -> {value.bulk}");
                Console.WriteLine($"Client {client_ident}: array -> {value.array}");
            
                string response = "+OK\r\n";
                var response_bytes = Encoding.UTF8.GetBytes(response);
                await handler.SendAsync(response_bytes, 0);

            }

        }
    }
}
