using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMap : MonoBehaviour
{
    public int NextDirection;
    // = 1 Botoom = 2 Top = 3 Left =4 Right

    CameraController m_camera;

    int Room_num = 0; // Entry Room = 0
    // 배열로 정해진 방의 숫자를 확인하기 위해서 만듬.

    private void Start()
    {
        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && collision.GetComponent<PlayerController>().Next_move == false)
        {
            collision.GetComponent<PlayerController>().Next_pos = collision.transform.position;
            m_camera.NextDirection = NextDirection;
            collision.GetComponent<PlayerController>().NextDirection = NextDirection;
            collision.GetComponent<PlayerController>().Next_move = true;
            m_camera.Move_check = true;
        }
        else if(collision.CompareTag("Player") && collision.GetComponent<PlayerController>().Next_move == true)
        {
            StartCoroutine(CountMonster_Spawn());
        }

    }

    IEnumerator CountMonster_Spawn()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject Room = transform.parent.parent.gameObject;
        Room_num = RoomTemplates.Instance.rooms.IndexOf(Room);
        RoomTemplates.Instance.Monster_Spawn(Room_num);
        /*if(Room_num == RoomTemplates.Instance.rooms.Count - 1)
        {
           
        }*/
        Debug.Log("RoomNum : " + Room_num);
    }
}
