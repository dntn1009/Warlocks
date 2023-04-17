using DefinedEnums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBulletController : MonoBehaviour
{

    [SerializeField] GameObject SelectBullet;
    [SerializeField] TextMeshProUGUI MyText;

    [SerializeField] Image[] NotgainBackground; 

    [SerializeField] public bool BulletCheck = false;
    PlayerController player;

    BulletType currentBullet;
    Color gainBackground;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("playertag", 0.1f);
        SelectBullet.SetActive(false);
        
    }

    void LockBulletCheck(BulletType bullettype)
    {
        if (player.playerinfo.LockBullet((int)bullettype))
        {
            currentBullet = bullettype;
            player.playerinfo.MYBULLET = (int)currentBullet;
            MyText.text = currentBullet.ToString();

            if(OverallManager.Instance._currentState == SceneState.Play || OverallManager.Instance._currentState == SceneState.Boss)
            {
                OverallManager.Instance.audio.SetSFX((int)bullettype);
            }
        }
    }

    public void BulletTypeChoice()
    {
        if (BulletCheck)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                LockBulletCheck(BulletType.Basic);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                LockBulletCheck(BulletType.Enforce);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                LockBulletCheck(BulletType.Sniper);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                LockBulletCheck(BulletType.Stun);
            }
        }
    }

    public void BulletSelectcheck()
    {
        if (!BulletCheck)
        {
            BulletCheck = true;
            SelectBullet.SetActive(true);
        }
        else
        {
            BulletCheck = false;
            SelectBullet.SetActive(false);
        }
    }

    public void GetNewBullet(int num)
    {
        player.playerinfo.UnLockBullet(num);
        NotgainBackground[num].color = gainBackground;
    }

    #region playerTag && BulletSet
    void playertag()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        bullet_Setting();
        if (player.playerinfo.LockBullet(0))
        {
            gainBackground = NotgainBackground[0].color;
        }
        else
        {
            gainBackground = NotgainBackground[1].color;
        }
    }// 플레이어 TAG(태그) 찾기

    void bullet_Setting()
    {
        
        for (int i = 0; i < (int)BulletType.MAX; i++)
        {
            if (!player.playerinfo.LockBullet(i))
                NotgainBackground[i].color = Color.black;
        }
        currentBullet = (BulletType)player.playerinfo.MYBULLET;
        MyText.text = currentBullet.ToString();
    }

    public void bullet_ReSetting()
    {
        for (int i = 0; i < (int)BulletType.MAX; i++)
        {
            if (!player.playerinfo.LockBullet(i))
            {
                if (i == 0 && player.playerinfo.MYBULLET == 0)
                {
                    currentBullet = BulletType.Enforce;
                    player.playerinfo.MYBULLET = (int)currentBullet;
                    MyText.text = currentBullet.ToString();
                }
                NotgainBackground[i].color = Color.black;
            }
            else
                NotgainBackground[i].color = gainBackground;
        }
    }
    //ItemShop Bullet
    #endregion
}
