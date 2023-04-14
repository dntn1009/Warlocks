using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterActive : MonoBehaviour
{
    public void Init_monster(float x, float y)
    {
        gameObject.transform.position = new Vector2(x, y);
        gameObject.SetActive(true);
    }
}
