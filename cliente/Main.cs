using Godot;
using Network.Receiver;
using System;
using System.Net.Sockets;
using System.Threading;

public partial class Main : Control
{
    private TcpClient client;
    private NetworkStream stream;
    private readonly DataReceiver dataReceiver;

    public Main()
    {
        dataReceiver = new DataReceiver();
    }

    public void Connect(string serverIP, int port)
    {
        try
        {
            client = new TcpClient(serverIP, port);
            stream = client.GetStream();

            Thread receiveThread = new Thread(ReceiverData);
            receiveThread.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao conectar ao servidor: {ex.Message}");
        }
    }

    private void ReceiverData()
    {
        try
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    byte[] receivedData = new byte[bytesRead];
                    Array.Copy(buffer, receivedData, bytesRead);

                    dataReceiver.ReceiverData(client, receivedData);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao receber dados do servidor: {ex.Message}");
        }
    }
}
