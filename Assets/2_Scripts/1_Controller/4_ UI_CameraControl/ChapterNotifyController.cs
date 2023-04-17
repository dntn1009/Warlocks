using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChapterNotifyController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ChapterText;

    PlayerController player;

    void Start()
    {
        Invoke("ChapterInput", 0.1f);
    }

    void ChapterInput()
    {
        player_tag();
        ChapterText.text = player.playerinfo.RECORD.MCHAPTER + "-" + player.playerinfo.RECORD.SCHAPTER;
    }

    void player_tag()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
}
