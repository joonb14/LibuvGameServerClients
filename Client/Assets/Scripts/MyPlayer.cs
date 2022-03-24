using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : Player
{
    NetworkManager _network;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CoSendPacket");
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnApplicationQuit()
    {
        C_LEAVEGAME leavePacket = new C_LEAVEGAME();
        _network.Send(leavePacket.Write());
    }

    IEnumerator CoSendPacket()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            C_MOVE movePacket = new C_MOVE();
            movePacket.posX = UnityEngine.Random.Range(-10, 10);
            movePacket.posY = 0;
            movePacket.posZ = UnityEngine.Random.Range(-10, 10);

            _network.Send(movePacket.Write());
        }
    }
}
