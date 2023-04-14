using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DefinedEnums;


public class UILoadingController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI LoadingText;
    [SerializeField] TextMeshProUGUI TipText;
    [SerializeField] TextMeshProUGUI CompleteText;

    float loadingduration = 0.5f;
    float loadingtime = 0f;

    bool SceneCheck = false;
    bool firstCheck = false;
    private void Start()
    {
        CompleteText.gameObject.SetActive(false);
        int rand = Random.Range(0, OverallManager.Instance.TipList.Count);
        TipText.text = OverallManager.Instance.TipList[rand];
    }

    private void Update()
    {
        loadingtime += Time.deltaTime;
        if (loadingtime >= loadingduration)
        {
            if (LoadingText.text.Equals("LOADING..."))
                LoadingText.text = "LOADING";
            else
                LoadingText.text += ".";

            loadingtime = 0f;
        }

        if (!firstCheck)
        {
            Invoke("NextScene", 0.3f);
            firstCheck = true;
        }

        if (SceneCheck && Input.GetKeyDown(KeyCode.Space))
        {
            CompleteText.gameObject.SetActive(false);
            gameObject.SetActive(false);
            SceneCheck = false;
            firstCheck = false;
            int rand = Random.Range(0, OverallManager.Instance.TipList.Count);
            TipText.text = OverallManager.Instance.TipList[rand];
        }

    }

    public void NextScene()
    {
        SceneCheck = true;
        CompleteText.gameObject.SetActive(true);
    }

}
