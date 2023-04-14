using DefinedEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTemplates : SingletonMonobehaviour<RoomTemplates>
{
    #region Room Create Var

    [Header("방생성목록")]
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject SpecialRoom;
    // 초기 방생성에 필요함

    [Header("재생성방목록")]
    public GameObject bottomRoom;
    public GameObject topRoom;
    public GameObject leftRoom;
    public GameObject rightRoom;


    // 방을 재생성할 방

    #endregion

    #region Monster & Player Var
    [Header("캐릭터 배치")]
    [SerializeField]
    GameObject Player;

    [Header("몬스터")]
    GameObjectPool<MonsterActive> m_slimePool;
    [SerializeField]
    GameObject SlimePrefab;
    GameObjectPool<MonsterActive> m_spiderPool;
    [SerializeField]
    GameObject SpiderPrefab;

    #endregion

    #region Spawn & Door By Var
    [Header("Spawn & Door By")]
    RoomMonCheck nowRoom;
    public List<GameObject> rooms;
    int Max_monster = 3;// 슬라임 or 거미 최대 생성갯수
    int Room_Mon = 5; // 방안에 최대 몬스터 생성개수
    public int check_monnum = 0; // 몬스터 죽을때마다 증가. 5가되면 문 열림

    public Vector2 spawnposition = new Vector2(0, 0);
    // 몬스터 patrol시 movespot 위치 변화
    // EntryRoom 진입시 0,0

    public float Exit_STime;
    public GameObject Exit_Hole; // 마지막 방 탈출구

     #endregion

    #region ItemVar
    [Header("아이템 드랍 및 획득")]
    [SerializeField]
    public GameObject[] FieldItem;

    public GameObject[] BoxItem;

    public GameObject Item;

    #endregion

    #region DIE UI Var

    [SerializeField] GameObject DieUI;

    #endregion

    private void Start()
    {
        Pool_setting();
     
    }
    private void Update()
    {    
        Reset_DoorOpen();
        DieChangeScene();
    }


    #region Door_ByMethods
    void Reset_DoorOpen()
    {
        if (check_monnum == 5)
        {
            if (nowRoom != null)
            {
                nowRoom.m_Door.SetActive(false);
                if (nowRoom.gameObject == rooms[rooms.Count - 1])
                    Exit_HoleCreate();
            }
            else
                rooms[0].GetComponent<RoomMonCheck>().m_Door.SetActive(false);
            check_monnum = 0;
        }
    }
    
    void Exit_HoleCreate()
    {
        StartCoroutine(Exit_HoleSpawnDelay(Exit_STime));
    }
    #endregion

    #region Monster_SpawnMethods
    void Pool_setting()
    {
        m_slimePool = new GameObjectPool<MonsterActive>(Max_monster, () =>
        {
            var obj = Instantiate(SlimePrefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            var mon = obj.GetComponent<MonsterActive>();
            return mon;
        });// 슬라임 몬스터 오브젝트 풀
        m_spiderPool = new GameObjectPool<MonsterActive>(Max_monster, () =>
        {
            var obj = Instantiate(SpiderPrefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            var mon = obj.GetComponent<MonsterActive>();
            return mon;
        });//스파이더 몬스터 오브젝트 풀
    }

    public void CreateMonster()
    {
        int first = Random.Range(1, 3);
        for (int i = 0; i < first; i++)
        {
            float x = Random.Range(-6, 6);
            float y = Random.Range(-6, 6);
            var slime = m_slimePool.Get();
            slime.Init_monster(spawnposition.x + x, spawnposition.y + y);
        }
        for (int i = 0; i < Room_Mon - first; i++)
        {
            float x = Random.Range(-6, 6);
            float y = Random.Range(-6, 6);
            var spider = m_spiderPool.Get();
            spider.Init_monster(spawnposition.x + x, spawnposition.y + y);
        }
    }

    public void Monster_Spawn(int Room_Number)
    {
        float ItemX =Random.Range(-1.5f, -5f);
        nowRoom = rooms[Room_Number].GetComponent<RoomMonCheck>();
        spawnposition = rooms[Room_Number].transform.position;
        if (nowRoom.Check_Spawn == false)
        {
            nowRoom.Check_Spawn = true;
            nowRoom.m_Door.SetActive(true);
            Box_Spawn(spawnposition + new Vector2(ItemX, 6.5f));
            CreateMonster();
        }
    }
    #endregion

    #region BoxItemMehtods

    public void Box_Spawn(Vector2 SpawnPosition)
    {
        int random = Random.Range(0, 101);

        if(random > 40)
            Instantiate(BoxItem[0], SpawnPosition, Quaternion.identity).transform.SetParent(Item.transform);
    }

    #endregion

    #region DieUI Methods

    public void SetDieUI()
    {
        DieUI.SetActive(true);
    }

    public void DieChangeScene()
    {
        if(DieUI.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                OverallManager.Instance.ChangeCurrentState(SceneState.MainMenu);
                Destroy(OverallManager.Instance.p_Obj);
            }
        }
    }

    #endregion

    #region Coroutine_DelayMethods

    IEnumerator Exit_HoleSpawnDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Instantiate(Exit_Hole, rooms[rooms.Count - 1].transform.position, Quaternion.identity);
    }// Room 배열의 마지막 방에 탈출구 생성
    #endregion
}
