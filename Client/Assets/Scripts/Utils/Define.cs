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
        MaxCount,  // Sound의 갯수 어따쓰려고? 왜 얘만? -> 일단 bgm따로 effect 따로 오디오 소소를 구분함 나중에 필요할까봐
    }  // enum이라 index가 정해져있어서 MaxCount값이 자동으로 갯수만큼 되는구나
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
