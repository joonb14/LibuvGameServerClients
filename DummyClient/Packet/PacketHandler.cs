using DummyClient;
using ServerCore;
using System;

class PacketHandler
{
    public static void S_BROADCASTENTERGAMEHandler(PacketSession session, IPacket packet)
    {
        S_BROADCASTENTERGAME pkt = packet as S_BROADCASTENTERGAME;
        ServerSession serverSession = session as ServerSession;
        S_PLAYERLIST.Player player = new S_PLAYERLIST.Player();
        player.isSelf = false;
        player.playerId = pkt.playerId;
        player.posX = pkt.posX;
        player.posY = pkt.posY;
        player.posZ = pkt.posZ;
        if (serverSession.room.players.ContainsKey(player.playerId) == false)
        {
            serverSession.room.players.Add(player.playerId, player);
            Console.WriteLine($"Player {player.playerId} add to {serverSession}, total players: {serverSession.room.players.Count}");
        }
    }

    public static void S_BROADCASTLEAVEGAMEHandler(PacketSession session, IPacket packet)
    {
        S_BROADCASTLEAVEGAME pkt = packet as S_BROADCASTLEAVEGAME;
        ServerSession serverSession = session as ServerSession;
    }

    public static void S_PLAYERLISTHandler(PacketSession session, IPacket packet)
    {
        S_PLAYERLIST pkt = packet as S_PLAYERLIST;
        ServerSession serverSession = session as ServerSession;

        foreach (S_PLAYERLIST.Player player in pkt.players)
        {
            if(serverSession.room.players.ContainsKey(player.playerId) == false)
                serverSession.room.players.Add(player.playerId, player);
        }
    }

    public static void S_BROADCASTMOVEHandler(PacketSession session, IPacket packet)
    {
        S_BROADCASTMOVE pkt = packet as S_BROADCASTMOVE;
        ServerSession serverSession = session as ServerSession;
    }
}
