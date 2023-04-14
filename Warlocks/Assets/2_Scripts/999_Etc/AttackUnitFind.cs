using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUnitFind : MonoBehaviour
{
    public bool PlayerCheck = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            PlayerCheck = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerCheck = false;
        }
    }

    public void SetActiveFalse()
    {
        this.gameObject.SetActive(false);
    }
}
