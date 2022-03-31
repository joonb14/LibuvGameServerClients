using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : Player
{
    NetworkManager  _network;
    float           _speed = 10.0f;
    PlayerState     _state = PlayerState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CoSendPacket");
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
    }


    // Update is called once per frame
    void Update()
    {
        if (Managers.Input._keyPressed == false)
            _state = PlayerState.Idle;

        switch (_state)
        {
            case PlayerState.Die:
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Running:
                UpdateRunning();
                break;
            case PlayerState.Jumping:
                break;
            case PlayerState.Waving:
                break;
        }
    }


    void OnKeyboard()
    {
        if (_state == PlayerState.Die)
            return;

        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += Vector3.forward * Time.deltaTime * _speed;
            _state = PlayerState.Running;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += Vector3.back * Time.deltaTime * _speed;
            _state = PlayerState.Running;
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += Vector3.left * Time.deltaTime * _speed;
            _state = PlayerState.Running;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += Vector3.right * Time.deltaTime * _speed;
            _state = PlayerState.Running;
        }
        if (Input.GetKey(KeyCode.G))
        {
            _state = PlayerState.Waving;
        }
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
            yield return new WaitForSeconds(0.1f);
            try
            {
                C_MOVE movePacket = new C_MOVE();
                movePacket.posX = transform.position.x;
                movePacket.posY = transform.position.y;
                movePacket.posZ = transform.position.z;

                _network.Send(movePacket.Write());
            }
            catch (NullReferenceException ex)
            {
                Debug.Log($"MyPlayer not found: {ex}");
            }
        }
    }
}
