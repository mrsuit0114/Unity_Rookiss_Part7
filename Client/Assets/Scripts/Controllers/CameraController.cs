using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuarterView;

    [SerializeField]
    Vector3 _delta;

    [SerializeField]
    GameObject _player;

    public void SetPlayer(GameObject player) { _player = player; }
    void Start()
    {
        
    }

    void LateUpdate()
    {
        if(_mode == Define.CameraMode.QuarterView)
        {
            if (_player.IsValid() == false)
                return;

            RaycastHit hit;
            //해당 장애물들 중에 가장 플레이어와 가까운 것을 해야하니까 플레이어로부터 쏘는게 합리적
            if(Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Block")))
            {
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                transform.position = _player.transform.position + _delta.normalized * dist;
            }
            else
            {
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform);  // 카메라의 delta값이 변경된다해도 항상 플레이어를 바라보도록
                // 위에서는 같은방향으로 이동만하니까 필요없긴함
            }


        }
    }

    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }
}
