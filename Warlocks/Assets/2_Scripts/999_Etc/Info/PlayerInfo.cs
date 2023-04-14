using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    [SerializeField] int mybullet;
    //나의 총알( 0 = 기본 총알 1 = 기본 총알(강화)  2 = 스나이처 총알 3 = 스턴 총알 )
    [SerializeField] int demage;
    [SerializeField] bool[] obtain_bullet;

    [SerializeField] int max_health; //HP
    [SerializeField] int m_health;
    //기본 체력

    [SerializeField] int max_energy;
    [SerializeField] int m_energy;
    //기본 기력

    [SerializeField] float fire_time;
    [SerializeField] bool Press_Attack; //키를 계속 누르고있어도 공격이 가능하게끔 하는 아이템 획득 시  True

    [SerializeField] float m_speed; // 캐릭터 속도
    [SerializeField] int Point; // 캐릭터 코인
    [SerializeField] int EscpaeScroll; // 탈출스크롤 개수
    [SerializeField] int LobbyPotion; // 로비에서 구입한 포션 개수


    [SerializeField] ChapterInfo player_Record;

    public PlayerInfo()
    {
        obtain_bullet = new bool[4] { true, false, false, false };
        mybullet = 0;
        demage = 1;
        max_health = m_health = 20;
        max_energy = m_energy = 16;
        fire_time = 1f;
        Press_Attack = false;
        m_speed = 3.6f;
        Point = 0;
        EscpaeScroll = 1;
        LobbyPotion = 1;
        player_Record = new ChapterInfo();
    }


    #region PlayerProperty
    public int MYBULLET { get { return mybullet; } set { mybullet = value; } }
    public int DEMAGE { get { return demage; } set { demage = value; } }
    public int MAXHEALTH { get { return max_health; } set { max_health = value; } }
    public int Health { get { return m_health; } set { m_health = value; } }
    public int MAXENERGY { get { return max_energy; } set { max_energy = value; } }
    public int ENERGY { get { return m_energy; } set { m_energy = value; } }
    public bool ENERGYRECOVERCHECK { get { if (MAXENERGY <= ENERGY) return true; else return false; } }
    public float FIRE_TIME { get { return fire_time; } set { fire_time = value; } }
    public bool PRESS_ATTACK { get { return Press_Attack; } set { Press_Attack = value; } }
    public float SPEED { get { return m_speed; } set { m_speed = value; } }

    public int POINT { get { return Point; } set { Point = value; } }

    public int ESCAPESCROLL { get { return EscpaeScroll; } set {  EscpaeScroll = value; } }

    public int LOBBYPOTION { get { return LobbyPotion; } set { LobbyPotion = value; } }

    public ChapterInfo RECORD { get { return player_Record; } set { player_Record = value; } }

    public int RECORDNORMALVALUE { get { return (int)(player_Record.SCHAPTER * 0.5f) + (int)(player_Record.MCHAPTER - 1); } }
    public int RECORDHARDVALUE { get { return (int)(player_Record.SCHAPTER * 1f) + (int)(player_Record.MCHAPTER * 1.5); } }

    public int BOSSPLUSDEMAGE { get { return (int)((player_Record.SCHAPTER - 2) * 0.5f); } }
    public int BOSSPLUSHP { get { return (player_Record.SCHAPTER - 2) * 15; } }
    public int CoinIncrease { get { return (int)(player_Record.SCHAPTER * 1.5f) + (int)(player_Record.MCHAPTER * 4); } }
    #endregion


    #region Item Effect Methods
    public void LPotionUse()
    {
        if(LOBBYPOTION > 0)
        {
            if(Health < MAXHEALTH)
            {
                LOBBYPOTION--;
                Health += 5;
                if (Health > MAXHEALTH)
                    Health = MAXHEALTH;
            }
        }
    }

    public void FPotionHealing(int num, bool Sort = false)
    {
        if(Health < MAXHEALTH)
        {
            if (!Sort)
                Health += num + RECORDNORMALVALUE;
            else
                Health += num + RECORDHARDVALUE;

            if (Health > MAXHEALTH)
                Health = MAXHEALTH;
        }
    }

    #endregion

    #region Chapter Manager Methods

    public void subCheapterIncrease()
    {
        player_Record.SCHAPTER += 1;
    }

    public void MainCheapterIncrease()
    {
        player_Record.MCHAPTER += 1;
        player_Record.SCHAPTER = 1;
    }

    #endregion

    #region BulletLOCK&UNLOCK Methods
    public bool LockBullet(int num)
    {
        return obtain_bullet[num];
    }

    public void ChangeBullet(int num)
    {
        obtain_bullet[num] = false;
    }

    public void UnLockBullet(int num)
    {
        obtain_bullet[num] = true;
    }
    public void SetSpeed(float speed)
    {
        m_speed += speed;
    }
    #endregion

}
