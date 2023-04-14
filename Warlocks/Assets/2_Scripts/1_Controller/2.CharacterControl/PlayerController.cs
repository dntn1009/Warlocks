using DefinedEnums;
using System.Collections;
using UnityEngine;

public class PlayerController : PlayerAnimationController
{
    // var

    #region Bullet & Explo By Var
    [Header("총알 & 폭발 관련 변수")]
    [SerializeField] GameObject[] m_bulletPrefab;
    [SerializeField] GameObject m_zeroEnergy;

    //GameObject m_bulletPrefab;
    //총알

    #endregion


    #region Fire By Var
    [Header("Bullet 발사 변수")]
    [SerializeField]
    Transform m_firePos;// 위치
    bool fire_check = true;
    #endregion

    #region Move Control By Var

    Vector2 motionVector; // 움직임 속도

    [Header("움직임 관련 변수")]
    //맵 이동시 자동 이동
    public bool Next_move = false;
    public int NextDirection;
    public Vector2 Next_pos;
    bool Hit_move = false;
    bool Die_move = false;
    #endregion

    #region Hit By Var
    [Header("피격 및 무적 관련 변수")]
    bool Invin_check = false;
    public bool invin_check { get { return Invin_check; } } // 무적시간
    #endregion

    #region Status && etc_Var
    Rigidbody2D m_rigidbody; // 충돌 + 움직임

    public UIStatusController status;
    bool statusNullcheck = false;
    //HP&ENERGY&ITEM UI
    public UIBulletController bullettype;
    bool bulletNullcheck = false;
    //BUlletUI
    float recover_time;//에너지 기력회복
    float Energy_recover = 10f;
    
    public bool StoryCheck = false;

    #endregion

    #region 플레이어 중요 정보
    [Header("플레이어 정보")]
    public PlayerInfo playerinfo;
    #endregion


    //https://greenteacat.tistory.com/24

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        // playerinfo = new PlayerInfo();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!playerinfo.ENERGYRECOVERCHECK)
        {
            recover_time += Time.deltaTime;

            if (recover_time >= Energy_recover)
            {
                playerinfo.ENERGY++;
                recover_time = 0;
                if (playerinfo.ENERGY >= playerinfo.MAXENERGY)
                {
                    playerinfo.ENERGY = playerinfo.MAXENERGY;
                }
                status.SetENERGY();
            }
        }

        if (StoryCheck)
        {
            if(m_rigidbody.velocity != Vector2.zero)
            {
                Play(PlayerMotion.Idle);
            }
            m_rigidbody.velocity = Vector2.zero;
            return;
        }

        if (Next_move || Die_move)
        {
            if (Die_move)
                zeroMove();
            if (Next_move)
                Move_Map();
            // 움직임을 제한할 경우
        }
        else
        {
            if (!Hit_move)
            {
                Speed();
                Move();
                Inven_Open();
                BulletType_Select();
                if (bulletNullcheck)
                    bullettype.BulletTypeChoice();

                LobbyPotionUse();

                if (OverallManager.Instance._currentState == SceneState.Play || OverallManager.Instance._currentState == SceneState.Boss)
                {
                    Attack();
                    EscapeScrollUse();
                }
                PauseOrProgress();
            }
        }
    }




    #region SpeedByMethods
    void Speed()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal < 0)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        else if (horizontal > 0)
            transform.rotation = Quaternion.Euler(0f, 0, 0f);

        motionVector = new Vector2(horizontal, vertical);
        SpeedTypeMotion(motionVector);

    } // 속도 구하기
    void SpeedTypeMotion(Vector2 motionVector)
    {
        if (motionVector != Vector2.zero && GetAnimState() == PlayerMotion.Idle)
        {
            Play(PlayerMotion.Move);
        }
        else if (motionVector == Vector2.zero && GetAnimState() == PlayerMotion.Move)
        {
            Play(PlayerMotion.Idle);
        }
    }//속도에 맞춰 idle, Move 정하기

    void Move_Map()
    {
        switch (NextDirection)
        {
            case 1:
                m_rigidbody.velocity = new Vector2(0, 1) * playerinfo.SPEED;
                if (this.transform.position.y >= Next_pos.y + 3.5f)
                {
                    Next_move = false;
                }
                break;
            case 2:
                m_rigidbody.velocity = new Vector2(0, -1) * playerinfo.SPEED;
                if (this.transform.position.y <= Next_pos.y - 3.5f)
                {
                    Next_move = false;
                }
                break;
            case 3:
                m_rigidbody.velocity = new Vector2(1, 0) * playerinfo.SPEED;
                if (this.transform.position.x >= Next_pos.x + 3f)
                {
                    Next_move = false;
                }
                break;
            case 4:
                m_rigidbody.velocity = new Vector2(-1, 0) * playerinfo.SPEED;
                if (this.transform.position.x <= Next_pos.x - 3f)
                {
                    Next_move = false;
                }
                break;
        }
    } // 맵 이동 시 플레이어가 못움직이게 하도록 고정

    #endregion

    #region MoveByMethods
    void zeroMove()
    {
        m_rigidbody.velocity = new Vector2(0, 0);
    }//죽은 경우 안움직이게 고정
    void Move()
    {
        m_rigidbody.velocity = motionVector * playerinfo.SPEED;
    }//플레이어가 움직이게 함.
    void Die_Stop()
    {
        Die_move = false;
        this.transform.gameObject.SetActive(false);

        switch (OverallManager.Instance._currentState)
        {
            case SceneState.Play:
                RoomTemplates.Instance.SetDieUI();
                break;
            case SceneState.Boss:
                BossManager.Instance.SetDieUI();
                break;
        }

    }
    #endregion

    #region AttackOrBullet_Methods

    void AttackByState()
    {
        if (!(Next_move || Die_move || Hit_move))
            Attack();
    }

    void Attack()
    {
        if (fire_check)
        {
            switch (playerinfo.PRESS_ATTACK)
            {
                case true:
                    if (Input.GetKey(KeyCode.RightArrow))
                        CreateBullet(Vector3.right);
                    else if (Input.GetKey(KeyCode.LeftArrow))
                        CreateBullet(Vector3.left);
                    else if (Input.GetKey(KeyCode.UpArrow))
                        CreateBullet(Vector3.up);
                    else if (Input.GetKey(KeyCode.DownArrow))
                        CreateBullet(Vector3.down);
                    break;
                case false:
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                        CreateBullet(Vector3.right);
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        CreateBullet(Vector3.left);
                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                        CreateBullet(Vector3.up);
                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                        CreateBullet(Vector3.down);
                    break;
            }

        }
    }// 키보드 화살표 공격

    void CreateBullet(Vector3 euler)
    {
        StartCoroutine(Attack_delay());
        if (playerinfo.MYBULLET > 1 && playerinfo.ENERGY - 2 >= 0 || playerinfo.MYBULLET <= 1)
        {
            if (playerinfo.MYBULLET > 1 && playerinfo.ENERGY - 2 >= 0)
            {
                playerinfo.ENERGY -= 2;
                status.SetENERGY();
            }
            var obj = Instantiate(m_bulletPrefab[playerinfo.MYBULLET]);
            var bullet = obj.GetComponent<BulletController>();
            bullet.SetBullet(m_firePos.position, euler, playerinfo.DEMAGE);
            //                  총알 위치          y가 180도 돌면 right 아니면 left 이다.

            OverallManager.Instance.audio.SFXPlay();
        }
        else
        {
                var obj = Instantiate(m_zeroEnergy, m_firePos.position + euler, Quaternion.identity);
                obj.transform.SetParent(m_firePos);
        }
    }// 촣알 생성

    public void setDemage(int demage)
    {
        int Ademage;
        if (playerinfo.RECORD.MCHAPTER == 1 && playerinfo.RECORD.SCHAPTER == 1)
            Ademage = demage;
        else
            Ademage = demage + playerinfo.RECORDNORMALVALUE;

        switch (Invin_check)
        {
            case false:
                playerinfo.Health -= Ademage;

                if (playerinfo.Health <= 0)
                {
                    playerinfo.Health = 0;
                    Die_move = true;
                    Play(PlayerMotion.Die);
                    Invoke("Die_Stop", 1.5f);
                }
                else
                    StartCoroutine(Invin_delay());
                status.SetHP();
                break;
        }
    }
    public void BossSetDemage(int demage)
    {
        
        switch (Invin_check)
        {
            case false:
                playerinfo.Health -= demage;

                if (playerinfo.Health <= 0)
                {
                    playerinfo.Health = 0;
                    Die_move = true;
                    Play(PlayerMotion.Die);
                    Invoke("Die_Stop", 1.5f);
                }
                else
                    StartCoroutine(Invin_delay());
                status.SetHP();
                break;
        }
    }

    #endregion

    #region Bullet && Status && Inven && Sound Methods

    public void UIStatus_Connect()
    {
        Invoke("UIStatus_Tag", 0.1f);
    }
    void UIStatus_Tag()
    {
        status = GameObject.FindGameObjectWithTag("Status").GetComponent<UIStatusController>();
        statusNullcheck = true;
    }

    void Inven_Open()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            status.InvenOpencheck();
        }
    }

    public void UIBullet_Connect()
    {
        Invoke("UIBullet_Tag", 0.1f);
    }

    void UIBullet_Tag()
    {
        bullettype = GameObject.FindGameObjectWithTag("BulletType").GetComponent<UIBulletController>();
        bulletNullcheck = true;
    }

    void BulletType_Select()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            bullettype.BulletSelectcheck();
        }
    }

    void LobbyPotionUse()
    {
        if (statusNullcheck)
            if (!bullettype.BulletCheck)
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    playerinfo.LPotionUse();
                    status.SetHP();
                    status.SetLPotionnum();
                }
    }

    void EscapeScrollUse()
    {
        if (statusNullcheck)
            if (!bullettype.BulletCheck)
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    if (OverallManager.Instance._currentState == SceneState.Play && playerinfo.ESCAPESCROLL > 0)
                    {
                        playerinfo.subCheapterIncrease();
                        playerinfo.ESCAPESCROLL--;
                        OverallManager.Instance.ChangeCurrentState(SceneState.Lobby);
                        UIStatus_Connect();
                        UIBullet_Connect();
                        transform.position = OverallManager.Instance.transform.position;
                        status.SetScrollnum();
                    }

                }
    }

    #endregion

    #region PauseMethods

    public void PauseOrProgress()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
    }

    #endregion

    #region Coroutine_DelayMethods
    IEnumerator Invin_delay()
    {
        Invin_check = true;
        yield return new WaitForSeconds(1.5f);
        Invin_check = false;
    }
    IEnumerator Attack_delay()
    {
        fire_check = false;
        yield return new WaitForSeconds(playerinfo.FIRE_TIME);
        fire_check = true;
    }

    public IEnumerator KnockBack(Vector2 vector, float speed)
    {
        Hit_move = true;
        m_rigidbody.velocity = vector * speed;
        yield return new WaitForSeconds(0.1f);
        Hit_move = false;
    }
    #endregion

}
