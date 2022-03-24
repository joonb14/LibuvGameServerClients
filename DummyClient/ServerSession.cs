using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;

namespace DummyClient
{
	

    class ServerSession : PacketSession
    {
        public float posX;
        public float posY;
        public float posZ;
        public Room room;

        public ServerSession()
        {
            posX = 0;
            posY = 0;
            posZ = 0;
            room = new Room();
        }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected: {endPoint}");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected: {endPoint}");
        }

        // 이동 패킷 ((3,2) 좌표로 이동하고 싶다!)
        // 15 3 2
        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numOfBytes)
        {
            // Console.WriteLine($"Transferred bytes: {numOfBytes }");
        }


    }
}
