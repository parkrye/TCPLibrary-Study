using System;
using System.Net.Sockets;
using UnityEngine;

public class ClientInterface
{
    private TcpClient _socketConnection;
    private static int _displayId = 0;
    private PacketManager _packetManaer = new PacketManager(_displayId);

    public void Start()
    {
        ConnectToTcpServer();
    }

    private void ConnectToTcpServer()
    {
        try
        {
            _socketConnection = new TcpClient("127.0.0.1", 50001);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void SendInput(int input)
    {
        SendMessage(_packetManaer.GetInputPacket(input));
    }

    private void SendMessage(byte[] buffer)
    {
        if (_socketConnection == null)
            return;

        try
        {
            var stream = _socketConnection.GetStream();
            if (stream.CanWrite)
            {
                stream.Write(buffer, 0, buffer.Length);
            }
        }
        catch (SocketException e)
        {
            Debug.Log(e);
        }
    }
}
