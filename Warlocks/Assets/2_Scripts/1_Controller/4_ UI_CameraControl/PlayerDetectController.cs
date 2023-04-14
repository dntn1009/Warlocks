using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectController : MonoBehaviour
{

    [SerializeField] int ShopNumber = 0;
    // 0 : 아이템 상점
    // 1 : 플레이어 스테이터스 강화


    bool triggerCheck = false;


    void Update()
    {
        if (triggerCheck)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ShopByState();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            triggerCheck = true;
            NotifyReact();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            triggerCheck = false;
            LobbyManager.Instance.NotifyClose();
        }
    }

    public void ShopByState()
    {
        switch (ShopNumber)
        {
            case 0:
                LobbyManager.Instance.ItemShopBoxOpen();
                break;

            case 1:
                LobbyManager.Instance.StatusShopBoxOpen();
                break;
        }
    }

    public void NotifyReact()
    {
        switch (ShopNumber)
        {
            case 0:
                LobbyManager.Instance.NotifyText("아이템 상점");
                break;

            case 1:
                LobbyManager.Instance.NotifyText("스텟 강화");
                break;
        }
    }

}
