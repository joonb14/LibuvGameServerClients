using DummyClient;
using ServerCore;
using System;
using UnityEngine;

class PacketHandler
{
    public static void S_BROADCASTENTERGAMEHandler(PacketSession session, IPacket packet)
    {
        S_BROADCASTENTERGAME pkt = packet as S_BROADCASTENTERGAME;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.EnterGame(pkt);
    }

    public static void S_BROADCASTLEAVEGAMEHandler(PacketSession session, IPacket packet)
    {
        S_BROADCASTLEAVEGAME pkt = packet as S_BROADCASTLEAVEGAME;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.LeaveGame(pkt);
    }

    public static void S_PLAYERLISTHandler(PacketSession session, IPacket packet)
    {
        S_PLAYERLIST pkt = packet as S_PLAYERLIST;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.Add(pkt);
    }

    public static void S_BROADCASTMOVEHandler(PacketSession session, IPacket packet)
    {
        S_BROADCASTMOVE pkt = packet as S_BROADCASTMOVE;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.Move(pkt);
    }

    public static void S_BROADCASTANIMATIONHandler(PacketSession session, IPacket packet)
    {
        S_BROADCASTANIMATION pkt = packet as S_BROADCASTANIMATION;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.PlayAnimation(pkt);
    }
}