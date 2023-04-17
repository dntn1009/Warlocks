using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefinedEnums;
using TMPro;
using UnityEngine.UI;

public class UIStatusController : MonoBehaviour
{
    [SerializeField] GameObject Inven;
    [SerializeField] GameObject Scroll;
    [SerializeField] GameObject LPotion;
    [SerializeField] GameObject Coin;

    [SerializeField] TextMeshProUGUI HPText;
    [SerializeField] Slider HPBAR;
    [SerializeField] TextMeshProUGUI ENERGYText;
    [SerializeField] Slider ENERGYBAR;
    [SerializeField] TextMeshProUGUI Scrollnum;
    [SerializeField] TextMeshProUGUI Lpotionnum;
    [SerializeField] TextMeshProUGUI Coinnum;


    [SerializeField] bool InvenCheck = false;
    PlayerController player;

    void Start()
    {
        Invoke("playertag", 0.1f);
        Inven.SetActive(true);
        Scroll.SetActive(false);
        LPotion.SetActive(false);
        Coin.SetActive(false);

    }


    public void SetHP()
    {
        float normalizedHP = player.playerinfo.Health / (float)player.playerinfo.MAXHEALTH;
        HPBAR.value = normalizedHP;
        if (normalizedHP <= 0f)
            HPBAR.value = 0f;
        HPText.text = player.playerinfo.Health + " / " + player.playerinfo.MAXHEALTH;
        //HPBAR VALUE 변경
    }

    public void SetENERGY()
    {
        float normalizeENERGY = player.playerinfo.ENERGY / (float)player.playerinfo.MAXENERGY;
        ENERGYBAR.value = normalizeENERGY;
        if (normalizeENERGY <= 0f)
            ENERGYBAR.value = 0f;
        ENERGYText.text = player.playerinfo.ENERGY + " / " + player.playerinfo.MAXENERGY;
    }

    public void SetScrollnum()
    {
        Scrollnum.text = player.playerinfo.ESCAPESCROLL.ToString();
    }

    public void SetLPotionnum()
    {
        Lpotionnum.text = player.playerinfo.LOBBYPOTION.ToString();
    }

    public void SetCoinnum()
    {
        Coinnum.text = player.playerinfo.POINT.ToString();
    }

    public void InvenOpencheck()
    {
        if(!InvenCheck)
        {
            InvenCheck = true;
            Inven.SetActive(false);
            Scroll.SetActive(true);
            LPotion.SetActive(true);
            Coin.SetActive(true);
            SetScrollnum();
            SetLPotionnum();
            SetCoinnum();
        }
        else
        {
            InvenCheck = false;
            Inven.SetActive(true);
            Scroll.SetActive(false);
            LPotion.SetActive(false);
            Coin.SetActive(false);
        }
    }



    #region playerTag
    void playertag()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        SetHP();
        SetENERGY();
    }// 플레이어 TAG(태그) 찾기
    #endregion

}
