using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using DefinedEnums;

public class MonsterAnimationController : AnimationController
{
    StringBuilder m_sb = new StringBuilder();
    MonsterMotion m_state;
    public MonsterMotion GetAnimState()
    {
        return m_state;
    }// 1. 이걸로 현재 Animation의 Motion값을 얻는다

    public void Play(MonsterMotion motion, bool isBlend = true)
    {
        m_sb.Append(motion);
        m_state = motion;
        Play(m_sb.ToString(), isBlend);
        m_sb.Clear();
    } //2. Idle인데 움직임이 생길시 Walk 이런식으로 변경하기위해
}
