using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DefinedEnums;

public class LobbyManager : SingletonMonobehaviour<LobbyManager>
{
    [Header("포탈 VAR")]
    [SerializeField] GameObject SavePotalBox; // 세이브 UI 박스
    [SerializeField] GameObject ItemShopBox; // 아이템샵 박스
    [SerializeField] GameObject StatusShopBox; // 스텟 강화 박스

    [Header("카메라 및 알림문")]
    [SerializeField] CameraController _camera; // 카메라 연결
    [SerializeField] TextMeshProUGUI Notify; // 알림문

    [Header("아이템 상점")]
    [SerializeField] TextMeshProUGUI Potion_coin; // 포션 구입 비용
    [SerializeField] TextMeshProUGUI Potion_count; // 포션 구입할 개수
    [SerializeField] TextMeshProUGUI Scroll_coin; // 스크롤 구입 비용
    [SerializeField] TextMeshProUGUI Scroll_count; //  스크롤 구입할 개수
    [SerializeField] TextMeshProUGUI Enforce_coin;
    [SerializeField] TextMeshProUGUI Enforce_count; // 기본 총알 강화 획득/ 미획득 표시
    [SerializeField] TextMeshProUGUI Stun_coin; 
    [SerializeField] TextMeshProUGUI Stun_count; // 스턴 총알 획득 / 미획득
    [SerializeField] TextMeshProUGUI Item_TotalCoin; // 구입할 총 코인

    [Header("스텟 상점")]
    [SerializeField] TextMeshProUGUI Demage_coin; // 데미지 증가 구입 비용
    [SerializeField] TextMeshProUGUI Demage_value;
    int demage_Min;
    [SerializeField] TextMeshProUGUI BulletSpeed_coin; // 데미지 증가 구입 비용
    [SerializeField] TextMeshProUGUI BulletSpeed_value;
    float BulletSpeed_Min;
    [SerializeField] TextMeshProUGUI Health_coin; // 데미지 증가 구입 비용
    [SerializeField] TextMeshProUGUI Health_value;
    int Health_Min;
    [SerializeField] TextMeshProUGUI Energy_coin; // 데미지 증가 구입 비용
    [SerializeField] TextMeshProUGUI Energy_value;
    int Energy_Min;
    [SerializeField] TextMeshProUGUI PlayerSpeed_coin; // 데미지 증가 구입 비용
    [SerializeField] TextMeshProUGUI PlayerSpeed_value;
    [SerializeField] TextMeshProUGUI Enforce_TotalCoin;
    float PlayerSpeed_Min;

    int init_Potion = 15; // 기본 포션 가격
    int init_Scroll = 25; // 기본 스크롤 가격
    bool DecreaseCheck = false;

    void Start()
    {
        SavePotalBox.SetActive(false);
        ItemShopBox.SetActive(false);
        StatusShopBox.SetActive(false);
        NotifyClose();
    }


    #region ItemShopMethods

    public void ItemShopCameraPos()
    {
        if (_camera.NextDirection > 0)
            _camera.NextDirection = 0;

        OverallManager.Instance.p_Obj.GetComponent<PlayerController>().Next_move = true;
        _camera.Move_check = true;
    }

    public void NotifyText(string text)
    {
        Notify.text = text;
        Notify.transform.gameObject.SetActive(true);
    }

    public void NotifyClose()
    {
        Notify.transform.gameObject.SetActive(false);
    }

    #endregion

    /// <summary>
    /// UI 박스들을 관리하는 메서드
    /// </summary>
    #region BoxUisMethods

    /// <summary>
    /// Save, Item, Status 관리 Object Box UI를 활성화 시키는 함수들
    /// </summary>
    public void SavePotalBoxOpen()
    {
        SavePotalBox.SetActive(true);
    }

    public void ItemShopBoxOpen()
    {
        PlayerController player = OverallManager.Instance.p_Obj.GetComponent<PlayerController>();
        ItemShopBox.SetActive(true);
        if (player.playerinfo.RECORD.SCHAPTER == 1
            && player.playerinfo.RECORD.MCHAPTER == 1)
        {
            Potion_coin.text = init_Potion.ToString();
            Scroll_coin.text = init_Scroll.ToString();
        }
        else
        {
            Potion_coin.text = (init_Potion + player.playerinfo.CoinIncrease).ToString();
            Scroll_coin.text = (init_Scroll + player.playerinfo.CoinIncrease).ToString();
        }

        if(player.playerinfo.LockBullet((int)BulletType.Enforce))
            Enforce_count.text = "획득";

        if (player.playerinfo.LockBullet((int)BulletType.Stun))
            Stun_count.text = "획득";
    }

    public void StatusShopBoxOpen()
    {
        PlayerController player = OverallManager.Instance.p_Obj.GetComponent<PlayerController>();
        StatusShopBox.SetActive(true);
        
        demage_Min = player.playerinfo.DEMAGE;
        BulletSpeed_Min = player.playerinfo.FIRE_TIME;
        Health_Min = player.playerinfo.MAXHEALTH;
        Energy_Min = player.playerinfo.MAXENERGY;
        PlayerSpeed_Min = player.playerinfo.SPEED;

        Demage_value.text = demage_Min.ToString();
        BulletSpeed_value.text = BulletSpeed_Min.ToString();
        Health_value.text = Health_Min.ToString();
        Energy_value.text = Energy_Min.ToString();
        PlayerSpeed_value.text = PlayerSpeed_Min.ToString();
    }

    #endregion

    /// <summary>
    /// UI 박스 안에서 발생하는 버튼들의 함수들
    /// </summary>
    #region BoxBtnMethods

    /// <summary>
    /// SavePotal Box
    /// </summary>
    public void SavePotalSaveBtn()
    {
        SavePotalBox.SetActive(false);
        OverallManager.Instance.LobbySaveBinary();
    }
    
    public void SavePotalSExitBtn()
    {
        SavePotalBox.SetActive(false);
        OverallManager.Instance.LobbySaveBinary(true);
    }
    ///
    
    /// <summary>
    /// ItemShopMehods
    /// </summary>
    public void ItemShopExitBtn()
    {
        PlayerController player = OverallManager.Instance.p_Obj.GetComponent<PlayerController>(); ;
        ItemShopBox.SetActive(false);
        Potion_count.text = "0";
        Scroll_count.text = "0";
        Item_TotalCoin.text = "0";

        if (player.playerinfo.LockBullet((int)BulletType.Enforce))
            Enforce_count.text = "획득";
        else
            Enforce_count.text = "미획득";

        if (player.playerinfo.LockBullet((int)BulletType.Stun))
            Stun_count.text = "획득";
        else
            Stun_count.text = "미획득";
    }

    public void ItemShopBuyBtn()
    {
        PlayerController player = OverallManager.Instance.p_Obj.GetComponent<PlayerController>();
        if (int.Parse(Item_TotalCoin.text) > player.playerinfo.POINT)
        {
            //구매할 수 없습니다.
        }
        else
        {
            player.playerinfo.POINT -= int.Parse(Item_TotalCoin.text);
            player.playerinfo.LOBBYPOTION += int.Parse(Potion_count.text);
            player.playerinfo.ESCAPESCROLL += int.Parse(Scroll_count.text);
            player.status.SetScrollnum();
            player.status.SetLPotionnum();
            player.status.SetCoinnum();
            if (Enforce_count.text.Equals("구매") || Stun_count.text.Equals("구매"))
            {
                if (Enforce_count.text.Equals("구매"))
                {
                    player.playerinfo.ChangeBullet((int)BulletType.Basic);
                    player.playerinfo.UnLockBullet((int)BulletType.Enforce);
                    Enforce_count.text = "획득";
                }
                if (Stun_count.text.Equals("구매"))
                {
                    player.playerinfo.UnLockBullet((int)BulletType.Stun);
                    Stun_count.text = "획득";
                }
                player.bullettype.bullet_ReSetting();
            }

            Potion_count.text = "0";
            Scroll_count.text = "0";
            Item_TotalCoin.text = "0";
            //구매가 완료되었습니다.
        }
    }

    public void ItemListPlusBtn(TextMeshProUGUI count)
    {
        int num = int.Parse(count.text) + 1;
        count.text = num.ToString();
    }

    public void TotalCoinIncrease(TextMeshProUGUI Coin)
    {
        int total = int.Parse(Item_TotalCoin.text) + int.Parse(Coin.text);
        Item_TotalCoin.text = total.ToString();
    }

    public void ItemListMinusBtn(TextMeshProUGUI count)
    {

        int num = int.Parse(count.text) - 1;
        if(num < 0)
        {
            count.text = "0";
            DecreaseCheck = true;
        }
        else
            count.text = num.ToString();
    }

    public void TotalCoinDecrease(TextMeshProUGUI Coin)
    {
        int total = int.Parse(Item_TotalCoin.text) - int.Parse(Coin.text);
        if (!DecreaseCheck)
            Item_TotalCoin.text = total.ToString();
        else
            DecreaseCheck = false;
    }

    // ItemShopBullet
    public void ItemListEnforceGetBtn()
    {
        if (Enforce_count.text.Equals("미획득"))
        {
            Enforce_count.text = "구매";
            int total = int.Parse(Item_TotalCoin.text) + int.Parse(Enforce_coin.text);
            Item_TotalCoin.text = total.ToString();
        }
    }
    public void ItemListEnforceNotBtn()
    {
        if (Enforce_count.text.Equals("구매"))
        {
            Enforce_count.text = "미획득";
            int total = int.Parse(Item_TotalCoin.text) - int.Parse(Enforce_coin.text);
            Item_TotalCoin.text = total.ToString();
        }
    }
    public void ItemListStunGetBtn()
    {
        if (Stun_count.text.Equals("미획득"))
        {
            Stun_count.text = "구매";
            int total = int.Parse(Item_TotalCoin.text) + int.Parse(Stun_coin.text);
            Item_TotalCoin.text = total.ToString();
        }
    }
    public void ItemListStunNotBtn()
    {
        if (Stun_count.text.Equals("구매"))
        {
            Stun_count.text = "미획득";
            int total = int.Parse(Item_TotalCoin.text) - int.Parse(Stun_coin.text);
            Item_TotalCoin.text = total.ToString();
        }
    }
    ///
    
    /// <summary>
    /// StatEnforceShopMethods
    /// </summary>
    public void StatEnforceShopExitBtn()
    {
        StatusShopBox.SetActive(false);
        Enforce_TotalCoin.text = "0";
    }

    public void StatEnforceShoppBuyBtn()
    {
        PlayerController player = OverallManager.Instance.p_Obj.GetComponent<PlayerController>();
        if (int.Parse(Enforce_TotalCoin.text) > player.playerinfo.POINT)
        {
            //구매할 수 없습니다.
        }
        else
        {
            player.playerinfo.DEMAGE = demage_Min = int.Parse(Demage_value.text);
            player.playerinfo.FIRE_TIME = BulletSpeed_Min = float.Parse(BulletSpeed_value.text);
            player.playerinfo.MAXHEALTH = Health_Min = int.Parse(Health_value.text);
            player.playerinfo.MAXENERGY = Energy_Min = int.Parse(Energy_value.text);
            player.playerinfo.SPEED = PlayerSpeed_Min = float.Parse(PlayerSpeed_value.text);

            player.playerinfo.POINT -= int.Parse(Enforce_TotalCoin.text);

            Enforce_TotalCoin.text = "0";
        }
    }

    public void EnforceListDemagePlusBtn()
    {
        int MAXDEMAGE = 16;
        int PLUSDEMAGE = 1;
        int total = int.Parse(Enforce_TotalCoin.text) + int.Parse(Demage_coin.text);
        int demage = int.Parse(Demage_value.text) + PLUSDEMAGE;
        if (MAXDEMAGE >= demage)
        {
            Enforce_TotalCoin.text = total.ToString();
            Demage_value.text = demage.ToString();
        }
    }
    public void EnforceListDemageMinusBtn()
    {
        int MINUSDEMAGE = -1;
        int total = int.Parse(Enforce_TotalCoin.text) - int.Parse(Demage_coin.text);
        int demage = int.Parse(Demage_value.text) + MINUSDEMAGE;
        if (demage_Min <= demage)
        {
            Enforce_TotalCoin.text = total.ToString();
            Demage_value.text = demage.ToString();
        }
    }
    public void EnforceListBulletSpeedPlusBtn()
    {
        float MAXSPEED = 0.2f;
        float PLUSSPEED = -0.2f;
        int total = int.Parse(Enforce_TotalCoin.text) + int.Parse(BulletSpeed_coin.text);
        float speed = float.Parse(BulletSpeed_value.text) + PLUSSPEED;
        if (MAXSPEED <= speed)
        {
            Enforce_TotalCoin.text = total.ToString();
            BulletSpeed_value.text = speed.ToString();
        }
    }
    public void EnforceListBulletSpeedMinusBtn()
    {
        float MINUSSPEED = 0.2f;
        int total = int.Parse(Enforce_TotalCoin.text) - int.Parse(BulletSpeed_coin.text);
        float speed = float.Parse(BulletSpeed_value.text) + MINUSSPEED;
        if (BulletSpeed_Min >= speed)
        {
            Enforce_TotalCoin.text = total.ToString();
            BulletSpeed_value.text = speed.ToString();
        }
    }
    public void EnforceListHealthPlusBtn()
    {
        int MAXHEALTH = 70;
        int PLUSHEALTH = 2;
        int total = int.Parse(Enforce_TotalCoin.text) + int.Parse(Health_coin.text);
        int health = int.Parse(Health_value.text) + PLUSHEALTH;
        if (MAXHEALTH >= health)
        {
            Enforce_TotalCoin.text = total.ToString();
            Health_value.text = health.ToString();
        }
    }
    public void EnforceListHealthMinusBtn()
    {
        int MINUSDEMAGE = -2;
        int total = int.Parse(Enforce_TotalCoin.text) - int.Parse(Health_coin.text);
        int health = int.Parse(Health_value.text) + MINUSDEMAGE;
        if (Health_Min <= health)
        {
            Enforce_TotalCoin.text = total.ToString();
            Health_value.text = health.ToString();
        }
    }
    public void EnforceListEnergyPlusBtn()
    {
        int MAXENERGY = 50;
        int PLUSENERGY = 2;
        int total = int.Parse(Enforce_TotalCoin.text) + int.Parse(Energy_coin.text);
        int energy = int.Parse(Energy_value.text) + PLUSENERGY;
        if (MAXENERGY >= energy)
        {
            Enforce_TotalCoin.text = total.ToString();
            Energy_value.text = energy.ToString();
        }
    }
    public void EnforceListEnergyMinusBtn()
    {
        int MINUSHEALTH = -2;
        int total = int.Parse(Enforce_TotalCoin.text) - int.Parse(Health_coin.text);
        int health = int.Parse(Health_value.text) + MINUSHEALTH;
        if (Health_Min <= health)
        {
            Enforce_TotalCoin.text = total.ToString();
            Health_value.text = health.ToString();
        }
    }

    public void EnforceListSpeedPlusBtn()
    {
        float MAXSPEED = 6;
        float PLUSSPEED = 0.2f;
        int total = int.Parse(Enforce_TotalCoin.text) + int.Parse(PlayerSpeed_coin.text);
        float speed = float.Parse(PlayerSpeed_value.text) + PLUSSPEED;
        if (MAXSPEED >= speed)
        {
            Enforce_TotalCoin.text = total.ToString();
            PlayerSpeed_value.text = speed.ToString();
        }
    }
    public void EnforceListSpeedMinusBtn()
    {
        float MINUSSPEED = -0.2f;
        int total = int.Parse(Enforce_TotalCoin.text) - int.Parse(Demage_coin.text);
        float speed = float.Parse(PlayerSpeed_value.text) + MINUSSPEED;
        if (PlayerSpeed_Min <= speed)
        {
            Enforce_TotalCoin.text = total.ToString();
            PlayerSpeed_value.text = speed.ToString();
        }
    }
    #endregion
}
