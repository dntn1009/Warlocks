using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIStoryController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI Conversation;

    string name;
    List<string> conver;
    string CASE;
    string skip = " [SPACE]";
    int num = 0;

    string bossname;
    public string BossName { get { return bossname; } set { bossname = value; } }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CaseByPrint();
        }
    }

    public void CaseByPrint()
    {
        switch (CASE)
        {
            case "GameStart":
            case "PlayStart":
                if (num < conver.Count)
                {
                    Conversation.text = conver[num] + skip;
                    num++;
                }
                else
                {
                    num = 0;
                    OverallManager.Instance.p_Obj.GetComponent<PlayerController>().StoryCheck = false;
                    gameObject.SetActive(false);
                }
                break;
            case "B1Start":
            case "B1Die":
            case "B2Start":
            case "B2Die":
            case "B3Start":
            case "B3Die":
                if (num < conver.Count)
                {
                    string[] narrate = conver[num].Split('_');
                    if (narrate[0].Equals("p"))
                        Name.text = "정수레";
                    else
                        Name.text = BossName;
                    Conversation.text = narrate[1] + skip;
                    num++;
                }
                else
                {
                    num = 0;
                    OverallManager.Instance.p_Obj.GetComponent<PlayerController>().StoryCheck = false;
                    if(CASE.Equals("B1Start") || CASE.Equals("B2Start") || CASE.Equals("B3Start"))
                    switch (BossName)
                    {
                        case "킹머쉬":
                            GameObject.FindGameObjectWithTag("Boss").GetComponent<KingMushController>().storyCheck = false;
                            break;
                        case "다칸":
                            GameObject.FindGameObjectWithTag("Boss").GetComponent<DarkanController>().storyCheck = false;
                            break;
                        case "얼음마녀":
                            GameObject.FindGameObjectWithTag("Boss").GetComponent<WitchController>().storyCheck = false;
                            break;
                    }
                    gameObject.SetActive(false);
                }
                break;
        }
    }

    public void SetText(string Case, List<string> str)
    {
        CASE = Case;
        conver = str;
        if (CASE.Equals("GameStart") || CASE.Equals("PlayStart"))
            Conversation.text = conver[0] + skip;
        else
        {
            string[] narrate = conver[num].Split('_');
            if (narrate[0].Equals("p"))
                Name.text = "정수레";
            else
                Name.text = BossName;
            Conversation.text = narrate[1] + skip;
        }
    }
}
