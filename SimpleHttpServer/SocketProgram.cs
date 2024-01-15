using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SocketProgram
{
    public static void SocketMain()
    {
        // 创建一个TCP监听器
        TcpListener server = new TcpListener(IPAddress.Any, 8080);
        server.Start();

        Console.WriteLine("服务器已启动，等待连接...");

        // 接受客户端连接
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("客户端已连接");

        // 获取客户端的网络流
        NetworkStream stream = client.GetStream();

        // 从客户端接收消息
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Console.WriteLine("接收到消息: " + data);

        // 向客户端发送消息
        byte[] response = Encoding.UTF8.GetBytes("已收到消息");
        stream.Write(response, 0, response.Length);

        // 关闭连接
        client.Close();
        server.Stop();
    }
}