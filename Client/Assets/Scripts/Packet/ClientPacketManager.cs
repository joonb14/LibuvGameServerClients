using ServerCore;
using System;
using System.Collections.Generic;
using UnityEngine;

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
    Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
    Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();

    public void Register()
    {
        _makeFunc.Add((ushort)PacketID.S_BROADCASTENTERGAME, MakePacket<S_BROADCASTENTERGAME>);
        _handler.Add((ushort)PacketID.S_BROADCASTENTERGAME, PacketHandler.S_BROADCASTENTERGAMEHandler);
        _makeFunc.Add((ushort)PacketID.S_BROADCASTLEAVEGAME, MakePacket<S_BROADCASTLEAVEGAME>);
        _handler.Add((ushort)PacketID.S_BROADCASTLEAVEGAME, PacketHandler.S_BROADCASTLEAVEGAMEHandler);
        _makeFunc.Add((ushort)PacketID.S_PLAYERLIST, MakePacket<S_PLAYERLIST>);
        _handler.Add((ushort)PacketID.S_PLAYERLIST, PacketHandler.S_PLAYERLISTHandler);
        _makeFunc.Add((ushort)PacketID.S_BROADCASTMOVE, MakePacket<S_BROADCASTMOVE>);
        _handler.Add((ushort)PacketID.S_BROADCASTMOVE, PacketHandler.S_BROADCASTMOVEHandler);

    }

    public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
    {

        Debug.Log("ClientPacketManager::OnRecvPacket");
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
        if (_makeFunc.TryGetValue(id, out func))
        {
            IPacket packet = func.Invoke(session, buffer);
            if (onRecvCallback != null)
                onRecvCallback.Invoke(session, packet);
            // ServerSession.cs에서 onRecvCallback을 정의해주었기 때문에 얘만 실행됨
            else
                HandlePacket(session, packet);
        }
    }

    T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
    {
        T pkt = new T();
        pkt.Read(buffer);
        return pkt;
    }

    public void HandlePacket(PacketSession session, IPacket packet)
    {
        Action<PacketSession, IPacket> action = null;
        if (_handler.TryGetValue(packet.Protocol, out action))
            action.Invoke(session, packet);
    }
}