using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefinedEnums;

public class WitchController : BossAnimationController
{
    public bool storyCheck = true;
    bool IdleCheck = false;
    bool NeturalCheck = false;
    bool PatternCheck = false;

    BossBehaviourState B_state;

    [SerializeField] public BossData bossinfo;
    [SerializeField] public int hp, hpMax;
    [SerializeField] public int np;
    [SerializeField] int demage;
    [SerializeField] GameObject AttackRange;
    [SerializeField] GameObject BombSpawn;

    UIBossStatusController status;

    Rigidbody2D m_rigidbody;
    Vector2 MotionVector;
    Transform player;

    float ChargeDuration = 10f;
    float ChargeTime = 0f;

    float BombDuration = 4f;
    float BombTime = 0f;

    float PBombDuration = 2f;
    int Bombnum = 1;


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
        if (collision.transform.CompareTag("Player"))
        {
            m_rigidbody.velocity = Vector3.zero;
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

        if (PatternCheck)
        {
            Play(BossMotion.Charge);
            SetState(BossBehaviourState.Charge);
            return;
        }

        if(Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(player.position.x, 0)) <= 4f && Vector2.Distance(new Vector2(0, transform.position.y), new Vector2(0, player.position.y)) <= 1f)
        {
            SetState(BossBehaviourState.Attack);
        }
        else
        {
            Play(BossMotion.Move);
            SetState(BossBehaviourState.Chase);
        }
    }

    //Attack
    void BehaviourState_Attack()
    {
        SetState(BossBehaviourState.Idle);
        Play(BossMotion.Attack1);
        m_rigidbody.velocity = Vector2.zero;
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
        if (Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(player.position.x, 0)) <= 4f && Vector2.Distance(new Vector2(0, transform.position.y), new Vector2(0, player.position.y)) <= 1f)
        {
            SetState(BossBehaviourState.Attack);
        }
        else
        {
            BombTime += Time.deltaTime;
            if(BombTime >= BombDuration)
            {
                var obj = Instantiate(BombSpawn, player.position, Quaternion.identity);
                obj.GetComponent<BombSpawnController>().SetDemage(demage * 2);
                BombTime = 0f;
            }
            MotionVector = (player.transform.position - transform.position).normalized;
            m_rigidbody.velocity = MotionVector * bossinfo.SPEED;
        }
    }

    void BehaviourState_Charge()
    {
        ChargeTime += Time.deltaTime;
        if (ChargeTime >= ChargeDuration)
        {
            PatternCheck = false;
            ChargeTime = 0;
            Bombnum = 1;
            SetState(BossBehaviourState.Idle);
            Play(BossMotion.Idle);
        }

        if (ChargeTime >= PBombDuration * Bombnum)
        {
            Bombnum++;
            var obj = Instantiate(BombSpawn, player.position, Quaternion.identity);
            obj.GetComponent<BombSpawnController>().SetDemage(demage * 2);
            
            for(int i = 0; i < 2; i++)
            {
                var obj2 = Instantiate(BombSpawn, player.position + new Vector3(Random.Range(-2, 3), Random.Range(-2, 3)), Quaternion.identity);
                obj2.GetComponent<BombSpawnController>().SetDemage(demage * 2);
            }
        }

    }

    //Die
    void BehaviourState_Die()
    {
        storyCheck = true;
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

        if ((rehp > hpMax / 4 * 3 && hp <= hpMax / 4 * 3) || (rehp > hpMax / 4 * 2 && hp <= hpMax / 4 * 2))
        {
            PatternCheck = true;
            Play(BossMotion.Idle);
            SetState(BossBehaviourState.Idle);
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

    public void AttackExit()
    {
        AttackRange.SetActive(false);
        SetIdle();
    }

    public void SetActiveAttackRange()
    {
        AttackRange.SetActive(true);
    }
    public void AttackDemage()
    {
        if (AttackRange.GetComponent<AttackUnitFind>().PlayerCheck)
        {
            player.GetComponent<PlayerController>().BossSetDemage(demage);
            Vector2 hitVector = (player.transform.position - transform.position).normalized;
            StartCoroutine(player.GetComponent<PlayerController>().KnockBack(hitVector, 6f));
        }
    }
    void SetState(BossBehaviourState state)
    {
        B_state = state;
    }// 행동 변화를 주기위한 것 FSM
    #endregion
}
