using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefixMap : MonoBehaviour
{
    string SpawnPoint;
    // 스폰 포인트의 방향을 확인한후 그 방향의 방을 확인하기 위해 필요함.

    AddRoom addroom;
    // 바꾸려는 방이 저장되어있는 리스트에 새로운 방을 넣어줘야 함.

    private bool LeftWay = false;
    private bool RightWay = false;
    private bool TopWay = false;
    private bool BottomWay = false;
    // 방의 통로가 true인 방을 만들기 위함.

    int Refixnum = 0;
    // 1 = 방의 통로가 1개인 것으로 변경
    // 2 = 방의 통로가 잘 연결되어있어 해당 gameobect만 파괴
    // 3 = 다시 수정되면 잘 연결되어 있기 때문에 gameobject만 파괴

    GameObject Room;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("RefreshMap", 2f);
        //Invoke("RefreshMap", 4.5f);
        addroom = this.transform.parent.GetComponent<AddRoom>();   
    }

    public void RefreshMap()
    {
        switch(Refixnum)
        {
            case 1:
                Direction_By_Create();
                break;
            case 2:
                Destroy(gameObject);
                break;
            case 3:
                Destroy(gameObject);
                break;
        }
    }

    void Direction_By_Create()
    {
        if (RightWay)
        {
            Room = Instantiate(RoomTemplates.Instance.rightRoom, transform.position, Quaternion.identity);
            Room.transform.parent = GameObject.Find("Map").transform;
            RoomTemplates.Instance.rooms[addroom.roomsindex - 1] = Room;
            Destroy(this.transform.parent.gameObject);
        }
        if (BottomWay)
        {
            Room = Instantiate(RoomTemplates.Instance.bottomRoom, transform.position, Quaternion.identity);
            Room.transform.parent = GameObject.Find("Map").transform;
            RoomTemplates.Instance.rooms[addroom.roomsindex - 1] = Room;
            Destroy(this.transform.parent.gameObject);
        }
        if (LeftWay)
        {
            Room = Instantiate(RoomTemplates.Instance.leftRoom, transform.position, Quaternion.identity);
            Room.transform.parent = GameObject.Find("Map").transform;
            RoomTemplates.Instance.rooms[addroom.roomsindex - 1] = Room;
            Destroy(this.transform.parent.gameObject);
        }
        if (TopWay)
        {
            Room = Instantiate(RoomTemplates.Instance.topRoom, transform.position, Quaternion.identity);
            Room.transform.parent = GameObject.Find("Map").transform;
            RoomTemplates.Instance.rooms[addroom.roomsindex - 1] = Room;
            Destroy(this.transform.parent.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
              SpawnPoint = other.name;
            if (SpawnPoint.Equals("SpawnPoint_L"))
                RightWay = true;
            else if (SpawnPoint.Equals("SpawnPoint_R"))
                LeftWay = true;
            else if (SpawnPoint.Equals("SpawnPoint_T"))
                BottomWay = true;
            else if (SpawnPoint.Equals("SpawnPoint_B"))
                TopWay = true;
                    Refixnum++;
        }//가운데에 충돌하는 스폰 포인트의 개수를 파악하여 방을 새로 바꿔주려고 함.
    }
}
