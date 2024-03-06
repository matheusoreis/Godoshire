using System.Collections.Generic;
using System.Net.Sockets;
using Godot;
using Interfaces;

namespace Network.Receiver.Messages;

public class CheckPingReceiverMessage : IReceiverMessage
{
    public void Receiver(TcpClient socket, byte[] data)
    {
        GD.Print("Dados Recebidos.");
    }
}
