using DummyClient;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    ServerSession _session = new ServerSession();

    public void Send(ArraySegment<byte> sendBuff)
    {
        _session.Send(sendBuff);
    }

    // Start is called before the first frame update
    void Start()
    {
        // DNS
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        // IPAddress ipAddr = ipHost.AddressList[0];
        IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7000);

        Connector connector = new Connector();

        connector.Connect(endPoint,
            () => { return _session; },
            1); // 테스트할 Client 개수

    }

    // Update is called once per frame
    void Update()
    {
        List<IPacket> list = PacketQueue.Instance.PopAll();
        foreach(IPacket packet in list)
            PacketManager.Instance.HandlePacket(_session, packet);
    }

}
