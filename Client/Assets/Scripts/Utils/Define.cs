using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
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
public enum Layer
    {
        Monster = 8,
        Ground = 9,
        Block = 10,
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
