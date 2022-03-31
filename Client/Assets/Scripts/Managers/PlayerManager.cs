using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    MyPlayer _myPlayer;
    Dictionary<int, Player> _players = new Dictionary<int, Player>();

    public static PlayerManager Instance { get; } = new PlayerManager();

    public void Add(S_PLAYERLIST packet)
    {
        Object obj = Resources.Load("Prefabs/unitychan");

        foreach (S_PLAYERLIST.Player p in packet.players)
        {
            if (p.isSelf)
            {
                GameObject go = Object.Instantiate(obj) as GameObject;
                go.name = "MyPlayer";
                MyPlayer myPlayer = go.AddComponent<MyPlayer>();
                myPlayer.PlayerId = p.playerId;
                myPlayer.transform.position = new Vector3(p.posX, p.posY, p.posZ);
                _myPlayer = myPlayer;
            }
            else
            {
                GameObject go = Object.Instantiate(obj) as GameObject;
                Player player = go.AddComponent<Player>();
                player.PlayerId = p.playerId;
                player.transform.position = new Vector3(p.posX, p.posY, p.posZ);
                _players.Add(p.playerId, player);
            }
        }
    }

    public void Move(S_BROADCASTMOVE packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
        {

        }
        else
        {
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                player._destPos = new Vector3(packet.posX, packet.posY, packet.posZ);
                player._state = Player.PlayerState.Running;
            }
        }
    }

    public void EnterGame(S_BROADCASTENTERGAME packet)
    {
        if (packet.playerId == _myPlayer.PlayerId)
            return;

        Object obj = Resources.Load("Prefabs/unitychan");
        GameObject go = Object.Instantiate(obj) as GameObject;

        Player player = go.AddComponent<Player>();
        player.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        _players.Add(packet.playerId, player);
    }

    public void LeaveGame(S_BROADCASTLEAVEGAME packet)
    {
        if(_myPlayer.PlayerId == packet.playerId)
        {
            GameObject.Destroy(_myPlayer.gameObject);
            _myPlayer = null;
        }
        else
        {
            Player player = null;
            if(_players.TryGetValue(packet.playerId, out player)){
                GameObject.Destroy(player.gameObject);
                _players.Remove(packet.playerId);
            }
        }
    }

    public void PlayAnimation(S_BROADCASTANIMATION packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
        {

        }
        else
        {
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                player._animNum = (Define.Animation)packet.animationId;
                player._state = Player.PlayerState.Animation;
            }
        }
    }
}
