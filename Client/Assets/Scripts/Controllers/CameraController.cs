using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 _delta;
    GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _delta = new Vector3(0, 6, -5);
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            _player = GameObject.Find("MyPlayer");
            transform.position = _player.transform.position + _delta;
            transform.LookAt(_player.transform);
        }
        catch (NullReferenceException ex)
        {
            Debug.Log($"MyPlayer not found: {ex}");
        }
    }
}
