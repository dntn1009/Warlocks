using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBossStatusController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI BossName;
    [SerializeField] public TextMeshProUGUI HPText;
    [SerializeField] public Slider HPBAR;
    [SerializeField] public TextMeshProUGUI NPText;
    [SerializeField] public Slider NPBAR;

    int chapter;
    void Start()
    {
        Invoke("SetBossStatus", 0.11f);
    }

    public void SetBossStatus()
    {
        chapter = BossManager.Instance.player.playerinfo.RECORD.MCHAPTER;
        switch (chapter)
        {
            case 1:
                BossName.text = "킹머쉬";
                break;
            case 2:
                BossName.text = "다칸";
                break;
            case 3:
                BossName.text = "얼음마녀";
                break;
        }

    }
}
