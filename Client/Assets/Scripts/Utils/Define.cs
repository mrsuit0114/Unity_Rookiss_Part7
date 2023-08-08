using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    // Part 7-
    
    public enum CreatureState
    {
        Idle,
        Moving,
        Skill,
        Dead,
    }

    public enum MoveDir
    {
        None,
        Up,
        Down,
        Left,
        Right,
    }

    // -Part 7

    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
    }
    public enum State
    {
        Die,
        Moving,
        Idle,
        Skill,
    }
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,  // Sound�� ���� ���������? �� �길? -> �ϴ� bgm���� effect ���� ����� �ҼҸ� ������ ���߿� �ʿ��ұ��
    }  // enum�̶� index�� �������־ MaxCount���� �ڵ����� ������ŭ �Ǵ±���
    public enum UIEvent
    {
        Click,
        Drag,
    }
    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
    }
    public enum CameraMode
    {
        QuarterView,
    }
}
