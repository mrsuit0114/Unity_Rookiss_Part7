using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressed = false;
    float _pressedTime = 0;
    public void OnUpdate()  // monobehaviour가 아니라 우리가 따로 호출 하니까 보기 좋게 이름도 바꿈
    {
        if (EventSystem.current.IsPointerOverGameObject())  //UI 클릭된 상황은 이동을 안하도록
        {
            return;
        }


        // KeyAction에 이벤트 리스너를 달아주기 때문에 null이 아니고 Invoke로 해당 리스너 메서드를 실행
        if(Input.anyKey && KeyAction != null)  // 마우스를 때는 것도 이벤트인데 anyKey == false상황에서 씹힐 수
                                               // 있기 때문에 키보드에만 영향가도록 하나의 조건문에 넣도록 변경
        {
            KeyAction.Invoke();
        }
        if(MouseAction != null)
        {
            if(Input.GetMouseButton(0))
            {
                if(!_pressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else
            {
                if (_pressed)
                {
                    if(Time.time > _pressedTime + 0.2f)
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);

                }
                _pressed = false;
                _pressedTime = 0;
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
