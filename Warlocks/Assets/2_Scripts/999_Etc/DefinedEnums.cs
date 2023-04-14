using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefinedEnums
{
    public enum BulletType
    {
        Basic       = 0,
        Enforce,
        Sniper,
        Stun,
        MAX
    }
    public enum SceneState
    {
        MainMenu    = 0,
        Lobby,
        Play,
        Boss
    }
    public enum PlayerMotion
    {
        None = -1,
        Idle,
        Move,
        Die,
        Hit,
        Max
    }

    public enum MonsterMotion
    {
        None = -1,
        Idle,
        Move,
        Chase,
        Hit,
        Attack,
        Die,
        Max
    }

    public enum BossMotion
    {
        None = -1,
        Idle,
        Move,
        Hit,
        Die,
        Attack1,
        Attack2,
        Attack3,
        Charge,
        Neutral
    }


    public enum MonBehaviourState
    {
        Idle,
        Chase,
        Patrol,
        Hit,
        Die,
        Max
    } // Monster의 행동에 따라 입력해주기 위해

    public enum BossBehaviourState
    {
        Idle,
        Attack,
        Neutral,
        Chase,
        Charge,
        Die,
        Max
    } // Monster의 행동에 따라 입력해주기 위해
}
