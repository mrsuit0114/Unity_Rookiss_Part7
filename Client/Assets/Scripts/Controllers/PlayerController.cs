using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
    PlayerStat _stat;

    bool _stopSkill = false;
    int _mask = 1<<(int)Define.Layer.Ground | 1<<(int)Define.Layer.Monster;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _stat = GetComponent<PlayerStat>();
        /*Managers.Input.KeyAction -= OnKeyboard;  //두번 호출 방지
        Managers.Input.KeyAction += OnKeyboard;*/
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
        anim = GetComponent<Animator>();

        Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);

    }

    protected override void UpdateMoving()
    {
        // 몬스터가 내 사정거리보다 가까우면 공격
        if(_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_lockTarget.transform.position - transform.position).magnitude;
            if (distance <= 1.5f)
            {
                State = Define.State.Skill;
                return;
            }


        }


        Vector3 dir = _destPos - transform.position;
        dir.y = 0;
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.yellow);
            if(Physics.Raycast(transform.position, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if(Input.GetMouseButton(0)==false)
                    State = Define.State.Idle;
                return;
            }
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 14f);
        }

        //애니메이션
        
    }

    protected override void UpdateSkill()
    {
        if(_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);

        }
        Debug.Log("UpdateSkill");
    }

    void OnHitEvent()
    {
        if(_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            //PlayerStat myStat = (PlayerStat)gameObject.GetComponent<Stat>();  // 상속받은 자식 클래스를 찾을수가있구나
            // PlayerStat myStat = gameObject.GetComponent<PlayerStat>(); 
            targetStat.OnAttacked(_stat);
        }

        if (_stopSkill)
            State = Define.State.Idle;
        else
            State = Define.State.Skill;

    }



    

    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
                {
                    if(evt == Define.MouseEvent.PointerUp)
                        _stopSkill = true;
                }
                break;
        }
        
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  //메인 카메라 기준으로 해당 방향으로 쏜 ray
        bool raycastHit = Physics.Raycast(ray, out hit, 100f, _mask);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        _destPos = hit.point;
                        State = Define.State.Moving;
                        _stopSkill = false;
                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)  // 몬스터
                        {
                            Debug.Log("monsterclicked");
                            _lockTarget = hit.collider.gameObject;
                        }
                        else  // 땅
                        {
                            _lockTarget = null;
                        }
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {
                    if (_lockTarget == null && raycastHit)
                        _destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
        }
    }

    #region OnKeyBoard
    /*void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), Time.deltaTime * 14f);
            *//*transform.Translate(Vector3.forward * Time.deltaTime * _speed);*//*
            transform.position += Vector3.forward * Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), Time.deltaTime * 14f);
            *//*transform.Translate(Vector3.forward * Time.deltaTime * _speed);*//*
            transform.position += Vector3.back * Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), Time.deltaTime * 14f);
            *//*transform.Translate(Vector3.forward * Time.deltaTime * _speed);*//*
            transform.position += Vector3.right * Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), Time.deltaTime * 14f);
            *//*transform.Translate(Vector3.forward * Time.deltaTime * _speed);*//*
            transform.position += Vector3.left * Time.deltaTime * _speed;
        }
        _moveToDest = false;
    }*/
    #endregion
}
