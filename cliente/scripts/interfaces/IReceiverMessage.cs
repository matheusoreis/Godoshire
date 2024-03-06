using System.Net.Sockets;

namespace Interfaces;

public interface IReceiverMessage
{
    public void Receiver(TcpClient client, byte[] data);
}
