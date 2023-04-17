using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    //1 --> need bottom door ( 1 = 아래 번호 )
    //2 --> need top door
    //3 --> need left door
    //4 --> need right door

    private int rand; // 임의의 방을 가져오기 위한 랜덤값을 위해 선언
    private bool spawned = false;

    public float waitTime = 3f; // boss

    GameObject Room;

    void Start()
    {
        Destroy(gameObject, waitTime);
        Invoke("Spawn", 0.1f);
    }

    void Spawn()
    {
        if (spawned == false)
        {
            switch (openingDirection)
            {
                case 1:
                    // Need to spawn a room with a BOTTOM Door
                    RoomInstantiate(Room, RoomTemplates.Instance.bottomRooms);
                    break;
                case 2:
                    // Need to spawn a room with a TOP Door
                    RoomInstantiate(Room, RoomTemplates.Instance.topRooms);
                    break;
                case 3:
                    // Need to spawn a room with a LEFT Door
                    RoomInstantiate(Room, RoomTemplates.Instance.leftRooms);
                    break;
                case 4:
                    // Need to spawn a room with a RIGHT Door
                    RoomInstantiate(Room, RoomTemplates.Instance.rightRooms);
                    break;
            }
            spawned = true;
        }
    }

    void RoomInstantiate(GameObject Room, GameObject[] templateRoom)
    {
        rand = Random.Range(0, templateRoom.Length);
        Room = Instantiate(templateRoom[rand], transform.position, templateRoom[rand].transform.rotation);
        Room.transform.parent = GameObject.Find("Map").transform;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       if(other.CompareTag("SpawnPoint"))
        {
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            { // spawn walls blocking off any openings
                Room = Instantiate(RoomTemplates.Instance.SpecialRoom, transform.position, Quaternion.identity);
                Room.transform.parent = GameObject.Find("Map").transform;
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
