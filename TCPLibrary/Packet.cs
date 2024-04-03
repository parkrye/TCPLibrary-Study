using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
struct InputPacket
{
    [MarshalAs(UnmanagedType.I4)]
    public int typeOfService;
    [MarshalAs(UnmanagedType.I4)]
    public int displayId;
    [MarshalAs(UnmanagedType.I4)]
    public int payloadLength;
    [MarshalAs(UnmanagedType.I4)]
    public int input;
}

public class PacketManager
{
    private InputPacket _inputPacket = new InputPacket();

    private int _displayId;

    public PacketManager(int id)
    {
        _displayId = id;
    }

    public byte[] GetInputPacket(int input)
    {
        _inputPacket.typeOfService = 0;
        _inputPacket.displayId = _displayId;
        _inputPacket.payloadLength = 4;
        _inputPacket.input = input;

        return Serialize(_inputPacket);
    }

    private byte[] Serialize<T>(T packet)
    {
        var buffer = new byte[Marshal.SizeOf(typeof(T))];

        var gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        var pBuffer = gch.AddrOfPinnedObject();

        Marshal.StructureToPtr(packet, pBuffer, false);
        gch.Free();

        return buffer;
    }
}