using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : SingletonMonobehaviour<HealthManager>
{
    
    public GameObject HP_BAR;


    PlayerController player;

    void Start()
    {
        Invoke("playertag", 0.1f);
        Invoke("HeartCreate", 0.101f);
    }

    // 카메라 흔들림효과
    //https://ncube-studio.tistory.com/entry/%EC%9C%A0%EB%8B%88%ED%8B%B0-%EC%B9%B4%EB%A9%94%EB%9D%BC-%ED%9D%94%EB%93%A4%EA%B8%B0%EC%89%90%EC%9D%B4%ED%81%AC-%ED%9A%A8%EA%B3%BC-%EA%B5%AC%ED%98%84-%EC%A7%80%EC%A7%84-%ED%8F%AD%EB%B0%9C-%EC%8A%88%ED%8C%85%EC%8B%9C-%EC%9C%A0%EC%9A%A9%ED%95%9C-%ED%9A%A8%EA%B3%BC-Unity-C-ScriptCamera-Shake-Invoke-InvokeRepeating
    // 

    #region Heart_CreateOrChangeMethods

  /*  public void HeartCreate()
    {
        hearts = new Image[player.playerinfo.MAXHEART];
        float xPos = 0;
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i] = Instantiate(heart);
            hearts[i].rectTransform.SetParent(HP_BAR.gameObject.transform, false);
            hearts[i].rectTransform.anchoredPosition = new Vector3(xPos, 0, 0);
            xPos += 80;
        }
    }
    public bool Health_Decrease(int demage)
    {
        player.playerinfo.Health -= demage;
        if (player.playerinfo.Health <= 0)
        {
            player.playerinfo.Health = 0;
            hearts[0].sprite = emptyHeart;
            return false;
        }

        int heartnum = player.playerinfo.CHANGEHEART;

        if (player.playerinfo.Health % 2 == 0)
            hearts[heartnum].sprite = emptyHeart;
        else if (player.playerinfo.Health % 2 == 1)
            hearts[heartnum].sprite = halfHeart;
        StartCoroutine(RDemage_Shake(heartnum));

        return true;
    }

    public void Health_Increase(int healing)
    {
        if (player.playerinfo.Health == player.playerinfo.MAXHEALTH)
            return;

        int initheart = player.playerinfo.HEART;
        player.playerinfo.Health += healing;
        int heartpos = player.playerinfo.Health % 2;
        int heartnum = player.playerinfo.Health;

        if (heartpos == 1)
        {
            for (int i = initheart; i <= heartnum; i++)
            {
                if (i == heartnum)
                    hearts[i].sprite = halfHeart;
                else
                    hearts[i].sprite = fullHeart;
                StartCoroutine(RDemage_Shake(heartnum - 1));
            }
        }
        else
        {
            for (int i = initheart; i < heartnum; i++)
            {
                hearts[i].sprite = fullHeart;
                StartCoroutine(RDemage_Shake(heartnum - 1));
            }
        }
    }

    public void Health_Increase(int healing, bool shakeCheck)
    {
        if (player.playerinfo.HEART * 2 <= player.playerinfo.Health || healing == 0)
            return;
        else if (player.playerinfo.HEART * 2 > player.playerinfo.Health && healing != 0)
        {
            healing--;
            Health_Increase(healing, true);
        }
        player.playerinfo.Health += 1;
        int heartnum = player.playerinfo.Health / 2;

        if (player.playerinfo.Health % 2 == 0)
            hearts[heartnum - 1].sprite = fullHeart;
        else if (player.playerinfo.Health % 2 == 1)
            hearts[heartnum].sprite = halfHeart;
    }


    public void HeartRefix()
    {
        player.playerinfo.MAXHEALTH += 2;
        Array.Resize(ref hearts, player.playerinfo.MAXHEART);
        hearts[hearts.Length - 1] = Instantiate(heart);
        hearts[hearts.Length - 1].rectTransform.SetParent(HP_BAR.gameObject.transform, false);
        hearts[hearts.Length - 1].rectTransform.anchoredPosition = new Vector3(hearts[hearts.Length - 2].transform.position.x + 80, 0, 0);
    }*/

    #endregion

    #region etcMethods
    void playertag()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }// 플레이어 TAG(태그) 찾기

    #endregion

    #region Coroutine_DelayMethods

    IEnumerator RDemage_Shake(int heartnum)
    {
        //hearts[heartnum].rectTransform.localRotation = Quaternion.Euler(0, 0, -7);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(LDemage_Shake(heartnum));
    }
    IEnumerator LDemage_Shake(int heartnum)
    {
        //hearts[heartnum].rectTransform.localRotation = Quaternion.Euler(0, 0, 7);
        yield return new WaitForSeconds(0.1f);
       // hearts[heartnum].rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    #endregion

}
