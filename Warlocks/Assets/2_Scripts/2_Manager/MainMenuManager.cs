using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using DefinedEnums;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    #region MainBtn
    [SerializeField] Button startbtn;
    [SerializeField] Button loadbtn;
    [SerializeField] Button settingbtn;
    [SerializeField] Button exitbtn;
    #endregion

    #region BoxUI
    [SerializeField] GameObject StartBox;
    [SerializeField] GameObject ExitBox;
    [SerializeField] GameObject LoadBox;
    [SerializeField] GameObject SettingBox;
    #endregion

    #region LoadBn
    [SerializeField] Image LoadData;
    #region LoadText
    [SerializeField] TextMeshProUGUI chapterText;
    [SerializeField] TextMeshProUGUI DateText;
    #endregion
    #endregion

    [SerializeField] bool NewCheck = false;
    PlayerInfo SaveFile;

    private void Start()
    {
        StartBox.SetActive(false);
        LoadBox.SetActive(false);
        SettingBox.SetActive(false);
        ExitBox.SetActive(false);
        startbtn.onClick.AddListener(() => StartBtn());
        loadbtn.onClick.AddListener(() => LoadBtn());
        settingbtn.onClick.AddListener(() => SettingBtn());
        exitbtn.onClick.AddListener(() => ExitBtn());
        SaveFile = OverallManager.Instance.checkSave<PlayerInfo>(ref NewCheck);
    }

    #region ButtonByMethods
    public void StartBtn()
    {
        if (!NewCheck)
        {
            SaveFile = new PlayerInfo();
            OverallManager.Instance.SaveBinary(SaveFile, SceneState.Lobby, true);
            NewCheck = false;
        }
        else
        {
            StartBox.SetActive(true);
        }

    }

    void LoadBtn()
    {

        if (NewCheck)
        {
            LoadBox.SetActive(true);
            chapterText.text = SaveFile.RECORD.MCHAPTER + " - " + SaveFile.RECORD.SCHAPTER;
            DateText.text = SaveFile.RECORD.SAVEDATE;
        }
        else
            LoadBox.SetActive(false);
    }

    void SettingBtn()
    {
        SettingBox.SetActive(true);
    }

    void ExitBtn()
    {
        ExitBox.SetActive(true);
        //Application.Quit();
    }
    #endregion

    #region BoxBtnMethods

    public void SOKBtn()
    {
        SaveFile = new PlayerInfo();
        OverallManager.Instance.SaveBinary(SaveFile, SceneState.Lobby, true);
    }

    public void SCANBtn()
    {
        StartBox.SetActive(false);
    }

    public void LOKBtn()
    {
        OverallManager.Instance.LoadBinary(SaveFile, SceneState.Lobby, NewCheck);
    }

    public void LCANBtn()
    {
        LoadBox.SetActive(false);
    }

    public void SECANBtn()
    {
        SettingBox.SetActive(false);
    }

    public void EOKBtn()
    {
        Application.Quit();
    }
    public void ECANBtn()
    {
        ExitBox.SetActive(false);
    }

    #endregion

    #region FileIOMethods

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

    #endregion
}
