using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefinedEnums;

public class BossController : BossAnimationController
{
    BossBehaviourState B_state;
    Vector2 motionVector; // 움직임 속도
    Rigidbody2D m_rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
            Play(BossMotion.Charge);
        else if(Input.GetKey(KeyCode.LeftArrow))
            Play(BossMotion.Idle);
        else if (Input.GetKey(KeyCode.UpArrow))
            Play(BossMotion.Neutral);
        else if (Input.GetKey(KeyCode.DownArrow))
            Play(BossMotion.Die);
        else if (Input.GetKey(KeyCode.A))
            Play(BossMotion.Move);

    }

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
            case BossBehaviourState.Die:
                BehaviourState_Die();
                break;
        }
    }

    void Speed()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal < 0)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        else if (horizontal > 0)
            transform.rotation = Quaternion.Euler(0f, 0, 0f);

        motionVector = new Vector2(horizontal, vertical);

    } // 속도 구하기
    void Move()
    {
        m_rigidbody.velocity = motionVector * 5;
    }//플레이어가 움직이게 함.
    #region BehaviourState_Type_Mehthods
    void BehaviourState_Idle()
    {

    }
    void BehaviourState_Attack()
    {

    }
    void BehaviourState_Die()
    {

    }

    void BehaviourState_Neutral()
    {

    }

    void BehaviourState_Hit()
    {

    }
    #endregion

    #region etc_Methods
    void SetState(BossBehaviourState state)
    {
        B_state = state;
    }// 행동 변화를 주기위한 것 FSM
    #endregion
}
