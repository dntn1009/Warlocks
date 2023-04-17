using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    [SerializeField]
    protected int item_number;
    // 0 : Coin
    // 1 : HPAPotion
    // 2 : HPBPotion
    // 3 : AttackPressPotion
    // 4 : 

    private void OnTriggerEnter2D(Collider2D collision)
    {
            Item_Effect(collision);
    }

    public virtual void Item_Effect(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (item_number)
            {
                case 0: // 코인
                    CoinPoint(collision, 2);
                    break;
                case 1: // 회복 A포션
                    collision.GetComponent<PlayerController>().playerinfo.FPotionHealing(2);
                    collision.GetComponent<PlayerController>().status.SetHP();
                    break;
                case 2: // 회복 B포션
                    collision.GetComponent<PlayerController>().playerinfo.FPotionHealing(4, true);
                    collision.GetComponent<PlayerController>().status.SetHP();
                    break;
                case 3: // 자동 공격 포션
                    collision.GetComponent<PlayerController>().playerinfo.PRESS_ATTACK = true;
                    break;
            }
            Destroy(gameObject);
        }
    }

    void CoinPoint(Collider2D collision, int point)
    {
        collision.GetComponent<PlayerController>().playerinfo.POINT += point;
    }
}
