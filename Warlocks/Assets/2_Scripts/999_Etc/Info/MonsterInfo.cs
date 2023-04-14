using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInfo
{
    float hp; // 몬스터 HP
    float c_speed; // 추격할 떄 속도
    float m_speed; // 돌아다닐 때 속도

    bool preemptiveCheck; // 한번 맞으면 따라가게 하기 위함.
    bool DelCheck; // Monster Pool SetActive(false)
    bool DieCheck; // Die motion 설정

    public MonsterInfo()
    {

    }


}
