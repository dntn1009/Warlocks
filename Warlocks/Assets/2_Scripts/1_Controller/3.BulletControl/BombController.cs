using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    int demage;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().BossSetDemage(demage);
        }
    }

    public void SetBomb(int b_demage)
    {
        demage = b_demage;
    }

    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
