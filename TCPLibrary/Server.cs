using System;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public delegate void CallbackInput(int foward, int turn);

public class Server : MonoBehaviour
{
    private TcpListener _tcpListener;
    private Thread _tcpListenerThread;
    private TcpClient _connectedTcpClient;

    private static Server _instance = null;
    public static Server Instance
    {
        get
        {
            if(_instance == null)
                return null;
            return _instance;
        }
    }

    CallbackInput _callbackInput;

    private void Awake()
    {
        _instance = this;

        _tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequest));
        _tcpListenerThread.IsBackground = true;
        _tcpListenerThread.Start();
    }

    public void SetInputCallback(CallbackInput callback)
    {
        if(_callbackInput == null )
            _callbackInput = callback;
        else
            _callbackInput += callback;
    }

    private void ListenForIncommingRequest()
    {
        try
        {
            _tcpListener = new TcpListener(IPAddress.Parse("192.168.0.11"), 50001);
            _tcpListener.Start();

            while (true)
            {
                using (_connectedTcpClient = _tcpListener.AcceptTcpClient())
                {
                    using (NetworkStream stream = _connectedTcpClient.GetStream())
                    {
                        do
                        {
                            var bytesTypeOfService = new byte[4];
                            var bytesDisplayId = new byte[4];
                            var bytesPayloadLength = new byte[4];

                            int lengthTypeOfService = stream.Read(bytesTypeOfService, 0, 4);
                            int lengthDisplayId = stream.Read(bytesDisplayId, 0, 4);
                            int lengthPayloadLength = stream.Read(bytesPayloadLength, 0, 4);

                            if (lengthTypeOfService <= 0 && lengthDisplayId <= 0 && lengthPayloadLength <= 0)
                                break;

                            if(BitConverter.IsLittleEndian == false)
                            {
                                Array.Reverse(bytesTypeOfService);
                                Array.Reverse(bytesDisplayId);
                                Array.Reverse(bytesPayloadLength);
                            }

                            int typeOfService = BitConverter.ToInt32(bytesTypeOfService, 0);
                            int displayId = BitConverter.ToInt32(bytesDisplayId, 0);
                            int payloadLength = BitConverter.ToInt32(bytesPayloadLength, 0);

                            if (typeOfService == 3)
                                payloadLength = 1012;

                            var bytes = new byte[payloadLength];

                            HandleIncommingRequest(typeOfService, displayId, payloadLength, bytes);
                        } while (true);
                    }
                }
            }
        }
        catch (SocketException e)
        {
            Debug.Log(e);
        }
    }

    private void HandleIncommingRequest(int typeOfService, int displayId, int payloadLength, byte[] bytes)
    {
        switch(typeOfService)
        {
            case 0:
                InputHandler(displayId, payloadLength, bytes);
                break;
        }
    }

    private void InputHandler(int displayId, int payloadLength, byte[] bytes)
    {
        int fowardAxis = BitConverter.ToInt32(bytes, 0);
        int turnAxis = BitConverter.ToInt32(bytes, 4);
        _callbackInput?.Invoke(fowardAxis, turnAxis);
    }
}