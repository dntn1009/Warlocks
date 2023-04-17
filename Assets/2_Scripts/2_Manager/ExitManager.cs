using DefinedEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitManager : MonoBehaviour
{
    bool TriggerCheck = false;

    private void Update()
    {
        if(TriggerCheck && !OverallManager.Instance.p_Obj.GetComponent<PlayerController>().StoryCheck)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (OverallManager.Instance._currentState)
            {
                case SceneState.Play:
                    OverallManager.Instance.p_Obj.GetComponent<PlayerController>().playerinfo.subCheapterIncrease();
                    OverallManager.Instance.ChangeCurrentState(SceneState.Lobby);
                        OverallManager.Instance.p_Obj.GetComponent<PlayerController>().UIStatus_Connect();
                        OverallManager.Instance.p_Obj.GetComponent<PlayerController>().UIBullet_Connect();
                        OverallManager.Instance.p_Obj.transform.position = OverallManager.Instance.transform.position;
                    break;
                case SceneState.Boss:
                    OverallManager.Instance.ChangeCurrentState(SceneState.Lobby);
                    OverallManager.Instance.p_Obj.GetComponent<PlayerController>().playerinfo.MainCheapterIncrease();
                    break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            TriggerCheck = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerCheck = false;
        }
    }
}
