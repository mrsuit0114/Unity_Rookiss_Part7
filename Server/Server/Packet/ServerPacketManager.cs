using Google.Protobuf;
using Google.Protobuf.Examples.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;

//Unity

public class PacketManager
{
    #region Singleton
    static PacketManager _instance = new PacketManager();
    public static PacketManager Instance { get { return _instance; } }
    #endregion

    PacketManager()
    {
        Register();
    }

    Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IMessage>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IMessage>>();
    Dictionary<ushort, Action<PacketSession, IMessage>> _handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();


    public void Register()
    {
        _makeFunc.Add((ushort)MsgId.CChat, MakePacket<C_Chat>);
        _handler.Add((ushort)PacketID.C_LeaveGame, PacketHandler.C_LeaveGameHandler);

    }

    public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IMessage> onRecvCallback = null)
    {
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        Func < PacketSession, ArraySegment<byte>, IMessage> func = null;
        if(_makeFunc.TryGetValue(id, out func))
        {
            IMessage packet = func.Invoke(session, buffer);
            if(onRecvCallback != null)
                onRecvCallback.Invoke(session,packet);
            else
            HandlePacket(session,packet);
        }
    }
        
    // T는 new가 가능해야한다고 말하면서 이렇게 사용함
    T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IMessage, new()
    {
        T p = new T();
        p.Read(buffer);
        return p;
    }

    public void HandlePacket(PacketSession session, IMessage packet)
    {
        Action<PacketSession, IMessage> action = null;
        if (_handler.TryGetValue(packet.Protocol, out action))
            action.Invoke(session, packet);
    }
}