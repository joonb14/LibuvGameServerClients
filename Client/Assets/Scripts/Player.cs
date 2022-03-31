using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerState
    {
        Die,
        Idle,
        Running,
        Jumping,
        Animation
    }

    public int PlayerId { get; set; }
    public PlayerState _state = PlayerState.Idle;
    float _wait_run_ratio = 0;
    public float _speed = 10.0f;
    public Vector3 _destPos;
    public Define.Animation _animNum = Define.Animation.WIN;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_state == PlayerState.Running) // PlayerManager Move()에서 _state와 _destPos값을 설정해줌
        {
            Vector3 dir = _destPos - transform.position;
            if (dir.magnitude < 0.0001f) // 도착
            {
                _state = PlayerState.Idle;
            }
            else
            {
                float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
                transform.position += dir.normalized * moveDist;
                transform.LookAt(_destPos);
            }
        }

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
            case PlayerState.Animation:
                UpdateAnimation(_animNum);
                break;
        }
    }

    protected void UpdateIdle()
    {
        Animator anim = GetComponent<Animator>();
        _wait_run_ratio = Mathf.Lerp(_wait_run_ratio, 0, 10.0f * Time.deltaTime);
        anim.SetFloat("wait_run_ratio", _wait_run_ratio);
        anim.Play("WAIT_RUN");
    }

    protected void UpdateRunning()
    {
        Animator anim = GetComponent<Animator>();
        _wait_run_ratio = Mathf.Lerp(_wait_run_ratio, 1, 10.0f * Time.deltaTime);
        anim.SetFloat("wait_run_ratio", _wait_run_ratio);
        anim.Play("WAIT_RUN");
    }

    protected void UpdateAnimation(Define.Animation animationNum)
    {
        Animator anim = GetComponent<Animator>();
        anim.Play(string.Format("ANIM_{0:D2}", (int)animationNum));
    }
}
