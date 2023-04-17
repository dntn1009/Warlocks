using DefinedEnums;
using System.Collections;
using UnityEngine;

public class KingMushController : BossAnimationController
{
    public bool storyCheck = true;
    bool IdleCheck = false;
    bool NeturalCheck = false;
    bool CrashCheck = false;
    bool RushCheck = false;

    BossBehaviourState B_state;

    [SerializeField] public BossData bossinfo;
    [SerializeField] float Rush = 7f;
    [SerializeField] public int hp, hpMax;
    [SerializeField] public int np;
    [SerializeField] int demage;
    [SerializeField] GameObject[] AttackRange;
    [SerializeField] Transform SmokePos;
    [SerializeField] GameObject ChargeSmoke;
    [SerializeField] GameObject RushSmoke;
    GameObject NowRush;

    UIBossStatusController status;

    Rigidbody2D m_rigidbody;
    Vector2 MotionVector;
    Transform player;

    float ChargeDuration = 4f;
    float ChargeTime = 0f;

    float WallDuration = 4f;
    float WallTime = 0f;
    bool WallCheck = false;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        player = OverallManager.Instance.p_Obj.transform;
        demage = bossinfo.DEMAGE + player.GetComponent<PlayerController>().playerinfo.BOSSPLUSDEMAGE;
        hp = hpMax = bossinfo.HP + player.GetComponent<PlayerController>().playerinfo.BOSSPLUSHP;
        np = bossinfo.NP;
        UIStatus_Tag();
    }


    void Update()
    {
        if (storyCheck)
            return;

        BehaviourProcess();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CrashCheck && collision.transform.CompareTag("Player"))
        {
            player.GetComponent<PlayerController>().BossSetDemage(demage * 2);
            Vector2 hitVector = (player.transform.position - transform.position).normalized;
            StartCoroutine(player.GetComponent<PlayerController>().KnockBack(hitVector, 10f));
            m_rigidbody.velocity = Vector3.zero;
            return;
        }

        if (CrashCheck && collision.transform.CompareTag("Wall"))
        {
            CrashCheck = false;
            Destroy(NowRush);
            ChargeTime = 0f;
            StartCoroutine(SetNeturalDuration(8f));
            Play(BossMotion.Neutral);
            SetState(BossBehaviourState.Neutral);
            m_rigidbody.velocity = Vector3.zero;
            return;
        }
    }

    #region BehaviourProcessMethods

    void BehaviourProcess()
    {
        switch (B_state)
        {
            case BossBehaviourState.Idle:
                BehaviourState_Idle();
                break;
            case BossBehaviourState.Attack:
                BehaviourState_Attack();
                break;
            case BossBehaviourState.Neutral:
                BehaviourState_Neutral();
                break;
            case BossBehaviourState.Chase:
                BehaviourState_Chase();
                break;
            case BossBehaviourState.Charge:
                BehaviourState_Charge();
                break;
            case BossBehaviourState.Die:
                BehaviourState_Die();
                break;
        }

        if (MotionVector.x < 0)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        else if (MotionVector.x > 0)
            transform.rotation = Quaternion.Euler(0f, 0, 0f);
    }

    //Idle
    void BehaviourState_Idle()
    {
        if (IdleCheck)
            return;

        if (Vector2.Distance(transform.position, player.position) > 2.5f)
        {
            if (Vector2.Distance(transform.position, player.position) <= 3.5f)
            {
                WallTime += Time.deltaTime;

                if (WallTime >= WallDuration)
                {
                    WallCheck = true;
                    SetState(BossBehaviourState.Attack);
                }
                return;
            }
            WallTime = 0f;
            Play(BossMotion.Move);
            SetState(BossBehaviourState.Chase);
        }
        else
        {
            WallTime = 0f;
            SetState(BossBehaviourState.Attack);
        }
    }

    //Attack
    void BehaviourState_Attack()
    {
        int per = Random.Range(0, 101);

        if (WallCheck)
            Play(BossMotion.Attack2);
        else if (per < 70)
            Play(BossMotion.Attack1);
        else
            Play(BossMotion.Attack2);

        m_rigidbody.velocity = Vector2.zero;
        SetState(BossBehaviourState.Idle);
        StartCoroutine(SetIdleDuration(1f));
    }

    //Neutralize
    void BehaviourState_Neutral()
    {
        if (!NeturalCheck)
        {
            np = bossinfo.NP;
            SetNP();
            SetState(BossBehaviourState.Idle);
        }
    }

    //Chase
    void BehaviourState_Chase()
    {
        if(RushCheck)
        {
            if (Vector2.Distance(transform.position, new Vector2(0, transform.position.y)) > 0.1f)
            {
                MotionVector = (new Vector3(0, transform.position.y, 0) - transform.position).normalized;
                m_rigidbody.velocity = MotionVector * Rush;
            }
            else
            {
                m_rigidbody.velocity = Vector2.zero;
                Play(BossMotion.Charge);
                SetState(BossBehaviourState.Charge);
                var smokeObj = Instantiate(ChargeSmoke, this.transform.position + new Vector3(0f, -0.3f, 0f), Quaternion.identity);
                smokeObj.transform.SetParent(SmokePos);
                Destroy(smokeObj, ChargeDuration);
            }
            return;
        }

        if (Vector2.Distance(transform.position, player.position) <= 3.5f)
        {
            WallTime += Time.deltaTime;

            if (WallTime >= WallDuration)
            {
                WallCheck = true;
                SetState(BossBehaviourState.Attack);
            }
            return;
        }
        if (Vector2.Distance(transform.position, player.position) <= 2.5f)
        {
            WallTime = 0f;
            SetState(BossBehaviourState.Attack);
        }
        else
        {
            MotionVector = (player.transform.position - transform.position).normalized;
            m_rigidbody.velocity = MotionVector * bossinfo.SPEED;
        }
    }

    void BehaviourState_Charge()
    {
        ChargeTime += Time.deltaTime;
        if (ChargeTime >= ChargeDuration)
        {
            if(RushCheck)
            {
                Play(BossMotion.Move);
                CrashCheck = true;
                RushCheck = false;
                NowRush = Instantiate(RushSmoke, SmokePos.position, Quaternion.identity);
                NowRush.transform.SetParent(SmokePos);
            }
            MotionVector = (player.transform.position - transform.position).normalized;
            m_rigidbody.velocity = MotionVector * Rush;
        }
    }

    //Die
    void BehaviourState_Die()
    {
        storyCheck = true;
        BossManager.Instance.CreateExitHole();
        OverallManager.Instance.SetBossDieCheck();
    }
    #endregion

    #region ATTACK & DEMAGE METHODS

    public void BulletypeEffect_Demage(BulletType bullettype, int demage)
    {
        int netur = 0;
        switch (bullettype)
        {
            case BulletType.Basic:
                netur = demage / 2;
                break;
            case BulletType.Enforce:
                netur = (int)(demage / 1.5);
                break;
            case BulletType.Sniper:
                netur = demage;
                break;
            case BulletType.Stun:
                netur = (int)(demage / 1.5f);
                break;
        }
        setDemage(demage, netur);
    }

    public void setDemage(int demage, int netur_demage)
    {
        int rehp = hp;
        hp -= demage;
        np -= netur_demage;

        if((rehp > hpMax / 4 * 3 && hp <= hpMax / 4 * 3) || (rehp > hpMax / 4 * 2 && hp <= hpMax / 4 * 2))
        {
            RushCheck = true;
            Play(BossMotion.Move);
            SetState(BossBehaviourState.Chase);
        }

        if (hp <= 0)
        {
            hp = 0;
            SetHP();
            m_rigidbody.velocity = Vector2.zero;
            StopAllCoroutines();
            Destroy(m_rigidbody);
            this.GetComponent<CapsuleCollider2D>().enabled = false;
            if (B_state != BossBehaviourState.Die)
            {
                Play(BossMotion.Die);
                SetState(BossBehaviourState.Die);
            }
            return;
        }
        else
            SetHP();

        if (np <= 0)
        {
            np = 0;
            SetNP();
            m_rigidbody.velocity = Vector2.zero;
            if (B_state != BossBehaviourState.Neutral || B_state != BossBehaviourState.Charge)
            {
                StartCoroutine(SetNeturalDuration(4f));
                Play(BossMotion.Neutral);
                SetState(BossBehaviourState.Neutral);
            }
        }
        else
            SetNP();
    }

    #endregion

    #region Coroutine_DelayMethods
    IEnumerator SetIdleDuration(float second)
    {
        IdleCheck = true;
        yield return new WaitForSeconds(second);
        IdleCheck = false;
    }

    IEnumerator SetNeturalDuration(float second)
    {
        NeturalCheck = true;
        yield return new WaitForSeconds(second);
        NeturalCheck = false;
    }

    #endregion

    #region UIBossStatusMethods

    void UIStatus_Tag()
    {
        status = GameObject.FindGameObjectWithTag("BossStatus").GetComponent<UIBossStatusController>();
        SetHP();
        SetNP();
    }

    public void SetHP()
    {
        float normalizedHP = hp / (float)hpMax;
        status.HPBAR.value = normalizedHP;
        if (normalizedHP <= 0f)
            status.HPBAR.value = 0f;

        status.HPText.text = hp + " / " + hpMax;
    }

    public void SetNP()
    {

        float normalizeNP = np / (float)bossinfo.NP;
        status.NPBAR.value = normalizeNP;
        if (normalizeNP <= 0f)
            status.NPBAR.value = 0f;
        status.NPText.text = np + " / " + bossinfo.NP;

    }

    #endregion

    #region etc_Animation Methods
    public void SetIdle()
    {
        Play(BossMotion.Idle);
    }
    public void AttackExit(int number)
    {
        AttackRange[number - 1].SetActive(false);
        SetIdle();
    }
    public void SetActiveAttackRange(int number)
    {
        AttackRange[number - 1].SetActive(true);
    }
    public void AttackDemage(int number)
    {
        if (AttackRange[number - 1].GetComponent<AttackUnitFind>().PlayerCheck)
        {
            switch (number)
            {
                case 1:
                    player.GetComponent<PlayerController>().BossSetDemage(demage);
                    break;
                case 2:
                    player.GetComponent<PlayerController>().BossSetDemage((int)(demage * 1.5f));
                    break;
            }
        }
    }
    void SetState(BossBehaviourState state)
    {
        B_state = state;
    }// 행동 변화를 주기위한 것 FSM
    #endregion

}
