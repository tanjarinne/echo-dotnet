using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
  static void Main(string[] args)
  {
    TcpListener server = new TcpListener(IPAddress.Loopback, 8080);
    server.Start();
    Console.WriteLine("Server listening on port 8080");

    while (true)
    {
      TcpClient client = server.AcceptTcpClient();
      HandleRequest(client);
      client.Close();
    }
  }

  static void HandleRequest(TcpClient client)
  {
    byte[] buffer = new byte[1024];
    int bytesRead = client.GetStream().Read(buffer, 0, buffer.Length);
    string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);
    string headers = request.Split("\r\n\r\n")[0];

    string response = $@"HTTP/1.1 200 OK
Content-Type: text/plain
Server: Echo

{request}";

    byte[] responseBytes = Encoding.ASCII.GetBytes(response);
    client.GetStream().Write(responseBytes, 0, responseBytes.Length);
  }
}
