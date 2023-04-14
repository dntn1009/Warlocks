using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DefinedEnums;
using System.IO;

public class PortalController : MonoBehaviour
{
    [SerializeField] int portalNum;
    [SerializeField] bool TriggerCheck = false;

    bool Shopgo = false;



    void Update()
    {
        //
        if (TriggerCheck)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                PotalbyState();
        }
        //
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            TriggerCheck = true;
            NotifyReact();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            TriggerCheck = false;
            LobbyManager.Instance.NotifyClose();
        }
    }

    #region PotalbyStateMethods

    void PotalbyState()
    {

        switch (portalNum)
        {
            case 0:
                zeroItemShop();
                if (!Shopgo)
                    Shopgo = true;
                else if(Shopgo)
                    Shopgo = false;
                break;
            case 1:
                oneSaveFile();
                break;
            case 2:
                twoBossRoom();
                break;
            case 3:
                threeRandomMap();
                break;
        }
    }
    void zeroItemShop()
    {
        LobbyManager.Instance.ItemShopCameraPos();
    }

    void oneSaveFile()
    {
        LobbyManager.Instance.SavePotalBoxOpen();
    }

    void twoBossRoom()
    {
        //조건 달아야함
        PlayerController player = OverallManager.Instance.p_Obj.GetComponent<PlayerController>();
        int subChapter = player.playerinfo.RECORD.SCHAPTER;

        if (subChapter >= 3)
        {
            OverallManager.Instance.ChangeCurrentState(SceneState.Boss);
        }
    }

    void threeRandomMap()
    {
        //챕터 별 서브챕터 숫자 늘리고
        OverallManager.Instance.ChangeCurrentState(SceneState.Play);
        OverallManager.Instance.p_Obj.transform.position = Vector3.zero;
    }

    void NotifyReact()
    {
        switch (portalNum)
        {
            case 0:
                if (!Shopgo)
                    LobbyManager.Instance.NotifyText("상점 & 강화");
                else if(Shopgo)
                    LobbyManager.Instance.NotifyText("로비");
                break;
            case 1:
                LobbyManager.Instance.NotifyText("저장하기");
                break;
            case 2:
                PlayerController player = OverallManager.Instance.p_Obj.GetComponent<PlayerController>();
                int mChapter = player.playerinfo.RECORD.MCHAPTER;
                int sChapter = player.playerinfo.RECORD.SCHAPTER;
                string subtext = string.Empty;
                if (sChapter >= 3)
                {
                    subtext = " PLAY";
                }
                else
                {
                    subtext = " (" + mChapter + "-3 부터 가능)";
                }
                LobbyManager.Instance.NotifyText("보스룸" + subtext);
                break;
            case 3:
                LobbyManager.Instance.NotifyText("미로 PLAY");
                break;
        }
    }

    #endregion
}
