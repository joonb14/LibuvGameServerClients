using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : Player
{
    NetworkManager  _network;
    float           _speed = 10.0f;
    PlayerState     _state = PlayerState.Idle;
    float           _wait_run_ratio = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CoSendPacket");
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
    }

    public enum PlayerState
    {
        Die,
        Idle,
        Running,
        Jumping,
        Waving
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

    void UpdateIdle()
    {
        Animator anim = GetComponent<Animator>();
        _wait_run_ratio = Mathf.Lerp(_wait_run_ratio, 0, 10.0f * Time.deltaTime);
        anim.SetFloat("wait_run_ratio", _wait_run_ratio);
        anim.Play("WAIT_RUN");
    }

    void UpdateRunning()
    {
        Animator anim = GetComponent<Animator>();
        _wait_run_ratio = Mathf.Lerp(_wait_run_ratio, 1, 10.0f * Time.deltaTime);
        anim.SetFloat("wait_run_ratio", _wait_run_ratio);
        anim.Play("WAIT_RUN");
    }

    void OnKeyboard()
    {
        if (_state == PlayerState.Die)
            return;

        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += Vector3.forward * Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += Vector3.back * Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += Vector3.left * Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += Vector3.right * Time.deltaTime * _speed;
        }
        _state = PlayerState.Running;
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
                C_TESTMOVE movePacket = new C_TESTMOVE();
                movePacket.position.x = transform.position.x;
                movePacket.position.y = transform.position.y;
                movePacket.position.z = transform.position.z;
                                        
                movePacket.rotation.w = transform.rotation.w;
                movePacket.rotation.x = transform.rotation.x;
                movePacket.rotation.y = transform.rotation.y;
                movePacket.rotation.z = transform.rotation.z;

                _network.Send(movePacket.Write());
            }
            catch (NullReferenceException ex)
            {
                Debug.Log($"MyPlayer not found: {ex}");
            }
        }
    }
}
