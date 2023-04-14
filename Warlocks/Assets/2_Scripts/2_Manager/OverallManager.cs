using DefinedEnums;
using LitJson;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverallManager : DonDestory<OverallManager>
{
#if UNITY_EDITOR && UNITY_STANDALONE_WIN
    string FilePath = Path.Combine(Application.streamingAssetsPath, "PlayerData.bin");
    string TipJsonPath = Path.Combine(Application.streamingAssetsPath, "Json/TipJson.json");
    string StoryJsonPath = Path.Combine(Application.streamingAssetsPath, "Json/StoryJson.json");
    /*#elif UNITY_STANDALONE_WIN
        string FilePath = Path.Combine(Application.persistentDataPath, "PlayerData.bin");*/
#endif


    #region 플레이어 및 로딩, 스토리 데이터 관련
    public SceneState _currentState; // 현재 나타내는 신

    [SerializeField] GameObject Loading;
    [SerializeField] GameObject Story;
    [SerializeField] GameObject Player;
    PlayerInfo playerData;
    public PlayerInfo PDATA { get { return playerData; } set { playerData = value; } }
    public GameObject p_Obj;

    #endregion

    #region Json관리 및 오디오
    public AudioController audio;
    public List<string> TipList = new List<string>();
    public Dictionary<string, List<string>> StoryList = new Dictionary<string, List<string>>();
    #endregion

    void Start()
    {
        _currentState = SceneState.MainMenu;
        ParsingJsonTipText(TipJsonPath, TipList);
        ParsingJsonStoryText(StoryJsonPath, StoryList);
    }

    #region SceneMethods
    public void ChangeCurrentState(SceneState state, bool create = false)
    {
        if (create)
            Invoke("CreateCharacter", 0.1f);


        switch (state)
        {
            case SceneState.MainMenu:
                Loading.SetActive(true);
                if (_currentState == SceneState.Lobby)
                {
                    RemoveCharacter();
                }
                SceneManager.LoadScene((int)SceneState.MainMenu);
                break;
            case SceneState.Lobby:
                Loading.SetActive(true);
                Invoke("SetStartCheck", 0.1f);
                SceneManager.LoadScene((int)SceneState.Lobby);
                Invoke("UI_Connect", 0.1f);
                Invoke("Audio_Connect", 0.1f);

                break;
            case SceneState.Play:
                Loading.SetActive(true);
                Invoke("SetPlayCheck", 0.1f);
                SceneManager.LoadScene((int)SceneState.Play);
                p_Obj.transform.position = Vector3.zero;
                Invoke("UI_Connect", 0.1f);
                Invoke("Audio_Connect", 0.1f);
                break;
            case SceneState.Boss:
                Loading.SetActive(true);
                SetBossCheck();
                SceneManager.LoadScene((int)SceneState.Boss);
                p_Obj.transform.position = Vector3.zero;
                Invoke("UI_Connect", 0.1f);
                Invoke("Audio_Connect", 0.1f);
                break;
        }
        _currentState = state;

    }

    #endregion

    #region Story By Methods

    void SetStartCheck()
    {
        if (p_Obj.GetComponent<PlayerController>().playerinfo.RECORD.STARTCHECK)
        {
            p_Obj.GetComponent<PlayerController>().StoryCheck = true;
            Story.SetActive(true);
            Story.GetComponent<UIStoryController>().SetText("GameStart", StoryList["GameStart"]);
            p_Obj.GetComponent<PlayerController>().playerinfo.RECORD.STARTCHECK = false;
        }
    }

    void SetPlayCheck()
    {
        if (p_Obj.GetComponent<PlayerController>().playerinfo.RECORD.PLAYCHECK)
        {
            p_Obj.GetComponent<PlayerController>().StoryCheck = true;
            Story.SetActive(true);
            Story.GetComponent<UIStoryController>().SetText("PlayStart", StoryList["PlayStart"]);
            p_Obj.GetComponent<PlayerController>().playerinfo.RECORD.PLAYCHECK = false;
        }
    }
    void SetBossCheck()
    {
        p_Obj.GetComponent<PlayerController>().StoryCheck = true;
        Story.SetActive(true);
        switch (p_Obj.GetComponent<PlayerController>().playerinfo.RECORD.MCHAPTER)
        {
            case 1:
                Story.GetComponent<UIStoryController>().SetText("B1Start", StoryList["B1Start"]);
                Story.GetComponent<UIStoryController>().BossName = "킹머쉬";
                break;
            case 2:
                Story.GetComponent<UIStoryController>().SetText("B2Start", StoryList["B2Start"]);
                Story.GetComponent<UIStoryController>().BossName = "다칸";
                break;
            case 3:
                Story.GetComponent<UIStoryController>().SetText("B3Start", StoryList["B3Start"]);
                Story.GetComponent<UIStoryController>().BossName = "얼음마녀";
                break;
        }
    }
    public void SetBossDieCheck()
    {
        p_Obj.GetComponent<PlayerController>().StoryCheck = true;
        Story.SetActive(true);
        switch (p_Obj.GetComponent<PlayerController>().playerinfo.RECORD.MCHAPTER)
        {
            case 1:
                Story.GetComponent<UIStoryController>().SetText("B1Die", StoryList["B1Die"]);
                Story.GetComponent<UIStoryController>().BossName = "킹머쉬";
                break;
            case 2:
                Story.GetComponent<UIStoryController>().SetText("B2Die", StoryList["B2Die"]);
                Story.GetComponent<UIStoryController>().BossName = "다칸";
                break;
            case 3:
                Story.GetComponent<UIStoryController>().SetText("B3Die", StoryList["B3Die"]);
                Story.GetComponent<UIStoryController>().BossName = "얼음마녀";
                break;
        }
    }
    #endregion
    #region CharacterMethods
    void CreateCharacter()
    {
        p_Obj = Instantiate(Player, transform.position, Quaternion.identity);
        p_Obj.GetComponent<PlayerController>().playerinfo = playerData;
    }

    void UI_Connect()
    {
        p_Obj.GetComponent<PlayerController>().UIStatus_Connect();
        p_Obj.GetComponent<PlayerController>().UIBullet_Connect();
    }

    void Audio_Connect()
    {
        audio = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioController>();
        audio.SetSFX(p_Obj.GetComponent<PlayerController>().playerinfo.MYBULLET);
    }

    void RemoveCharacter()
    {
        Destroy(p_Obj);
        Destroy(p_Obj.GetComponent<PlayerController>().status.transform.gameObject);
    }

    public void ChangePlayerBullet(int num)
    {
        PDATA.MYBULLET = num;
    }
    #endregion

    #region File I/O Methods

    public void LobbySaveBinary(bool check = false)
    {
        PDATA = p_Obj.GetComponent<PlayerController>().playerinfo;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(FilePath, FileMode.OpenOrCreate);
        binaryFormatter.Serialize(fileStream, PDATA);
        fileStream.Close();

        if (check)
            ChangeCurrentState(SceneState.MainMenu);
    }

    public void SaveBinary(PlayerInfo SaveFile, SceneState state, bool check)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(FilePath, FileMode.OpenOrCreate);
        binaryFormatter.Serialize(fileStream, SaveFile);
        fileStream.Close();
        PDATA = SaveFile;
        ChangeCurrentState(state, check);
    }



    public void LoadBinary(PlayerInfo LoadFile, SceneState state, bool NewCheck)
    {
        if (NewCheck)
        {
            PDATA = LoadFile;
            ChangeCurrentState(state, true);
        }
    }


    public T checkSave<T>(ref bool newCheck)
    {
        newCheck = File.Exists(FilePath);

        if (newCheck)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(FilePath, FileMode.Open);
            return (T)binaryFormatter.Deserialize(fileStream);
        }
        else
            return default(T);
    }



    public T ReadBinary<T>(string filePath)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(filePath, FileMode.Open);

        if (fileStream != null && fileStream.Length > 0)
            return (T)binaryFormatter.Deserialize(fileStream);

        else
        {
            // 저장된 데이터가 없습니다. 표시하기
            return default(T);
        }

    }

    void ParsingJsonTipText(string filepath, List<string> tiplist)
    {
        string jsonStr = File.ReadAllText(filepath);
        JsonData json = JsonMapper.ToObject(jsonStr);

        for (int i = 0; i < json.Count; i++)
        {
            tiplist.Add(json[i][1].ToString());
        }
    }

    void ParsingJsonStoryText(string filepath, Dictionary<string, List<string>> storylist)
    {
        string jsonStr = File.ReadAllText(filepath);
        JsonData json = JsonMapper.ToObject(jsonStr);

        for (int i = 0; i < json.Count; i++)
        {
            List<string> list = new List<string>();
            for (int j = 2; j < 11; j++)
            {
                if (json[i][j] == null)
                    break;
                else
                    list.Add(json[i][j].ToString());

                if(i == 0)
                Debug.Log(json[i][j].ToString());
            }
            storylist.Add(json[i][1].ToString(), list);
        }
    }

    #endregion
}
