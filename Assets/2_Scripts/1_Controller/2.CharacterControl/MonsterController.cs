using System.Collections;
using UnityEngine;
using DefinedEnums;

public class MonsterController : MonsterAnimationController
{
 
    MonBehaviourState M_state;

    #region Monster Info // 몬스터 정보
    [SerializeField] MonsterData monsterdata;
    float m_hp;
    #endregion

    #region PlayerReactVar // 반응을 위한 변수
    public bool fir_demageCheck = false; // 첫 공격시 따라가게하기 위해 bool 설정
    bool Del_check = false; // Monster Pool Setactive(false)
    bool die_check = false; // Die Motion 설정
    bool stun_check = false;
    #endregion

    #region MonsterTypeVar // 원거리 & 근접별 변수
    [Header("원거리 몬스터")]
    [SerializeField]
    GameObject m_bulletPrefab;//총알
    [SerializeField]
    Transform m_firePos;//사격 위치
    float m_fireTime;//사격 쿨타임
    bool fire_check = false;

    [Header("근접 몬스터")]
    bool crash_check = false;
    #endregion

    #region etcVar // 플레이어 및 리지드바디 연결 변수
    Transform player; // 플레이어와의 거리 계산
    Rigidbody2D m_rigidbody;
    GameObject Movespot; // 패트롤할때 필요한 spot
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        Movespot = transform.parent.Find("MoveSpot").gameObject;
        Movespot.transform.position = new Vector2(Random.Range(monsterdata.minX + RoomTemplates.Instance.spawnposition.x, monsterdata.maxX + RoomTemplates.Instance.spawnposition.x), Random.Range(monsterdata.minY + RoomTemplates.Instance.spawnposition.y, monsterdata.maxY + RoomTemplates.Instance.spawnposition.y));
        m_rigidbody = GetComponent<Rigidbody2D>();
        player = OverallManager.Instance.p_Obj.transform;

        if (player.GetComponent<PlayerController>().playerinfo.RECORD.MCHAPTER == 1 && player.GetComponent<PlayerController>().playerinfo.RECORD.SCHAPTER == 1)
            m_hp = monsterdata.HP;
        else
            m_hp = monsterdata.HP + player.GetComponent<PlayerController>().playerinfo.RECORDHARDVALUE;

        StartCoroutine(mAttack_Delay());
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("BehaviourProcess", 0.1f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 hitVector = (player.transform.position - transform.position).normalized;
            switch (monsterdata.NUMBER)
            {
                case 0:
                    StartCoroutine(mCrash_Type_Delay(1f));
                    collision.GetComponent<PlayerController>().setDemage(monsterdata.DEMAGE);
                    break;
            }
            StartCoroutine(player.GetComponent<PlayerController>().KnockBack(hitVector, 3f));
        }
    }

    #region BehaviourState_Type_Mathods
    void BehaviourProcess()
    {
        switch (M_state)
        {
            case MonBehaviourState.Idle:
                BehaviourState_Idle();
                break;
            case MonBehaviourState.Patrol: // 돌아다니기
                BehaviourState_Patrol();
                break;
            case MonBehaviourState.Chase: // 플레이어 쫒기
                BehaviourState_Chase();
                break;
            case MonBehaviourState.Hit:
                //BehaviourState_Hit();
                break;
            case MonBehaviourState.Die:
                BehaviourState_Die();
                break;
        }
    } // 몬스터의 행동

    void BehaviourState_Idle()
    {
        switch(monsterdata.NUMBER)
        {
            case 0:
                if (Vector2.Distance(transform.position, player.transform.position) <= 5)//플레이어와 몬스터 거리가 5 안에 있을 시에
                {
                    Play(MonsterMotion.Chase);
                    SetState(MonBehaviourState.Chase);
                }
                else
                {
                    if (fir_demageCheck)
                    {
                        Play(MonsterMotion.Chase);
                        SetState(MonBehaviourState.Chase);
                    }
                    else
                    {
                        Play(MonsterMotion.Move);
                        SetState(MonBehaviourState.Patrol);
                    }
                }
                break;
            case 1:
                Play(MonsterMotion.Move);
                SetState(MonBehaviourState.Patrol);
                break;
        }
    } // Idle
    void BehaviourState_Patrol()
    {
        switch(monsterdata.NUMBER)
        {
            case 0:
                if (Vector2.Distance(transform.position, player.transform.position) > 5) //플레이어와 거리가 정해진 거리보다 멀면 MoveSpot 쪽으로 움직임
                {
                    if (fir_demageCheck)
                    {
                        Play(MonsterMotion.Idle);
                        SetState(MonBehaviourState.Idle);
                    }
                    else
                    {
                        if(!stun_check)
                            transform.position = Vector2.MoveTowards(transform.position, Movespot.transform.position, monsterdata.NORMALSPEED * Time.deltaTime);

                         if (Vector2.Distance(transform.position, Movespot.transform.position) < 0.2f)// 무브스팟과의 거리가 가까워지면
                                Movespot.transform.position = new Vector2(Random.Range(monsterdata.minX + RoomTemplates.Instance.spawnposition.x, monsterdata.maxX + RoomTemplates.Instance.spawnposition.x), Random.Range(monsterdata.minY + RoomTemplates.Instance.spawnposition.y, monsterdata.maxY + RoomTemplates.Instance.spawnposition.y));
                                // 무브스팟의 위치를 다시정함.
                    }
                }
                else
                {
                    Play(MonsterMotion.Idle);
                    SetState(MonBehaviourState.Idle);
                }
                break;
            case 1:
                if (!stun_check)
                {
                    if (fire_check)
                    {
                        bulletAttack();
                        StartCoroutine(mAttack_Delay());
                    }
                    transform.position = Vector2.MoveTowards(transform.position, Movespot.transform.position, monsterdata.NORMALSPEED * Time.deltaTime);
                }
                if (Vector2.Distance(transform.position, Movespot.transform.position) < 0.2f)// 무브스팟과의 거리가 가까워지면
                {
                    Movespot.transform.position = new Vector2(Random.Range(monsterdata.minX + RoomTemplates.Instance.spawnposition.x, monsterdata.maxX + RoomTemplates.Instance.spawnposition.x), Random.Range(monsterdata.minY + RoomTemplates.Instance.spawnposition.y, monsterdata.maxY + RoomTemplates.Instance.spawnposition.y));
                    // 무브스팟의 위치를 다시정함.
                }
                break;
        }
    } // Patrol
    void BehaviourState_Chase()
    {
        if (Vector2.Distance(transform.position, player.transform.position) > 5)
        {
            if (fir_demageCheck)
            {
                if(!stun_check)
                transform.position = Vector2.MoveTowards(transform.position, player.position, Time.deltaTime * monsterdata.CHASESPEED);
            }
            else
            {
                Play(MonsterMotion.Idle);
                SetState(MonBehaviourState.Idle);
            }
        }
        else
        {
            switch(crash_check)
            {
                case true:
                    Play(MonsterMotion.Idle);
                    break;
                case false:
                    if(!stun_check)
                    transform.position = Vector2.MoveTowards(transform.position, player.position, Time.deltaTime * monsterdata.CHASESPEED);
                    break;
            }
            
        }
        // 부딪히고 나서 안뭉치게 어케할까
    } // Chase
    void BehaviourState_Die()
    {
        if (Del_check)
        {
            int per = Random.Range(1, 101);
            int D_num;
            if (per < 70)
                D_num = 0;
            else if(per < 85)
                D_num = 1;
            else if(per < 93)
                D_num = 2;
            else
                D_num = 3;
                
            RoomTemplates.Instance.check_monnum += 1;
            die_check = false;
            Del_check = false;
            Instantiate(RoomTemplates.Instance.FieldItem[D_num], this.transform.position, Quaternion.identity).transform.SetParent(RoomTemplates.Instance.Item.transform);
            this.transform.parent.gameObject.SetActive(false);
        }
    } // Die
    void BehaviourState_Hit()
    {
    } // Hit
    #endregion

    #region ATTACK & DEMAGE Methods
    void CreateBullet(Vector3 euler)
    {
        var obj = Instantiate(m_bulletPrefab);// 1. 총알 날리기 쉬운방법
        var bullet = obj.GetComponent<MonsterBulletController>();
        bullet.SetBullet(m_firePos.position, euler, monsterdata.DEMAGE);
        //                  총알 위치          y가 180도 돌면 right 아니면 left 이다.
    }
    void bulletAttack()
    {
        Vector2 target_pos = (player.transform.position - m_firePos.position).normalized;
        CreateBullet(target_pos);
    }
    public void setDemage(int demage)
    {
        m_hp -= demage;
        if (m_hp <= 0)
        {
            Destroy(m_rigidbody);
            if (!die_check)
            {
                StopAllCoroutines();
                Play(MonsterMotion.Die);
                SetState(MonBehaviourState.Die);
                StartCoroutine(mDelect_Delay());
                switch(monsterdata.NUMBER)
                {
                    case 0:
                        this.GetComponent<CircleCollider2D>().enabled = false;
                        break;

                    case 1:
                        this.GetComponent<CapsuleCollider2D>().enabled = false;
                        break;
                }
            }
        }
        else
        {
            switch(monsterdata.NUMBER)
            {
                case 0:// 근접 몬스터일 경우 적용
                    Play(MonsterMotion.Hit);
                    SetState(MonBehaviourState.Hit);
                    StartCoroutine(Idle_Duration(0.5f));
                    break;
            }
        }
    }

    public void BulletypeEffect_Demage(BulletType bullettype,int demage)
    {
        switch(bullettype)
        {
            case BulletType.Basic:
                    break;
            case BulletType.Enforce:
                break;
            case BulletType.Sniper:
                break;
            case BulletType.Stun:
                StartCoroutine(mAttack_Stun());
                break;
        }
        setDemage(demage);
    }
    #endregion

    #region Coroutine_DelayMethods

    IEnumerator mAttack_Stun()
    {
        stun_check = true;
        yield return new WaitForSeconds(1f);
        stun_check = false;
    }

    IEnumerator mAttack_Delay()
    {
        fire_check = false;
        m_fireTime = Random.Range(2.5f, 3f);
        yield return new WaitForSeconds(m_fireTime);
        fire_check = true;
    }

    IEnumerator Idle_Duration(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SetState(MonBehaviourState.Idle);
    }

    IEnumerator mDelect_Delay()
    {
        die_check = true;
        yield return new WaitForSeconds(1.5f);
        Del_check = true;
    }

    IEnumerator mCrash_Type_Delay(float seconds)
    {
        crash_check = true;
        yield return new WaitForSeconds(seconds);
        Play(MonsterMotion.Chase);
        crash_check = false;
    }
    #endregion

    #region etc_Methods
    void SetState(MonBehaviourState state)
    {
        M_state = state;
    }// 행동 변화를 주기위한 것 FSM
    #endregion

}
