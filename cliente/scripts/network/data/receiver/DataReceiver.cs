

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Godot;
using Interfaces;
using Network.Buffer;
using Network.Packets.Client;
using Network.Receiver.Messages;
namespace Network.Receiver;

public class DataReceiver
{
    private readonly Dictionary<ClientPackets, IReceiverMessage> _receiverDataMessage;
    private readonly ClientBuffer _buffer;

    public DataReceiver()
    {
        _receiverDataMessage = new Dictionary<ClientPackets, IReceiverMessage>();

        _buffer = new ClientBuffer();

        InitializeMessages();
    }

    private void InitializeMessages()
    {
        _receiverDataMessage.Add(ClientPackets.checkPing, new CheckPingReceiverMessage());
    }

    public void ReceiverData(TcpClient client, byte[] data)
    {
        if (data.Length < 4) return;

        _buffer.WriteBytes(data);
        _buffer.ReadInteger();

        int msgType = _buffer.ReadInteger();

        try
        {
            if (msgType < 0 || msgType >= ClientPacketsUtils.GetCount())
            {
                GD.Print($"msgType fora do intervalo válido: {msgType}");
            }

            if (_receiverDataMessage.TryGetValue((ClientPackets)msgType, out IReceiverMessage receiverMessage))
            {
                receiverMessage.Receiver(client, data);
            }
            else
            {
                GD.Print($"Não há manipulador de mensagem para o tipo de pacote: {msgType}");
            }
        }
        catch (Exception ex)
        {
            GD.Print($"Erro ao processar dados recebidos: {ex.Message}");
        }
    }
}
