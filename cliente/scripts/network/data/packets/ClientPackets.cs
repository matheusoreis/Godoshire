using Godot;
using System;

namespace Network.Packets.Client;

public enum ClientPackets
{
    checkPing
}

public static class ClientPacketsUtils
{
    public static int GetCount()
    {
        return Enum.GetValues(typeof(ClientPackets)).Length;
    }
}