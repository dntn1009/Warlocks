using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    private RoomTemplates templates;

    public int roomsindex = 0; // 새로만드는 방의 인덱스를 알기위해 필요함.

    private void Start()
    {
        RoomTemplates.Instance.rooms.Add(this.gameObject);
        roomsindex = RoomTemplates.Instance.rooms.Count;
    }
}
