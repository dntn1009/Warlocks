using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefinedEnums;

public class BossManager : SingletonMonobehaviour<BossManager>
{
    [SerializeField] GameObject[] ChapterByRoom;
    [SerializeField] GameObject[] ChapterByBoss;

    [SerializeField] GameObject Exit_Hole;

    #region DIE UI Var

    [SerializeField] GameObject DieUI;

    #endregion

    public GameObject boss;
    public PlayerController player;

    private void Start()
    {
        player = OverallManager.Instance.p_Obj.GetComponent<PlayerController>();
        var obj = Instantiate(ChapterByRoom[player.playerinfo.RECORD.MCHAPTER - 1], transform.position, Quaternion.identity);
        obj.transform.parent = GameObject.Find("Map").transform;
        boss = Instantiate(ChapterByBoss[player.playerinfo.RECORD.MCHAPTER - 1], new Vector2(0, 2.5f), Quaternion.identity);
    }

    private void Update()
    {
        DieChangeScene();
    }

    public void CreateExitHole()
    {
        Instantiate(Exit_Hole, this.transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity);
    }

    #region DieUI Methods

    public void SetDieUI()
    {
        DieUI.SetActive(true);
    }

    public void DieChangeScene()
    {
        if (DieUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OverallManager.Instance.ChangeCurrentState(SceneState.MainMenu);
                Destroy(OverallManager.Instance.p_Obj);
            }
        }
    }

    #endregion

}
