using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawnController : MonoBehaviour
{
    [SerializeField] GameObject Bomb;

    float DestroyTime = 1f;
    int demage;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("BombSpawn", DestroyTime);
    }

    void BombSpawn()
    {
        Destroy(this.gameObject);
        var obj = Instantiate(Bomb, this.transform.position, Quaternion.identity);
        obj.GetComponent<BombController>().SetBomb(demage);
    }

    public void SetDemage(int p_demage)
    {
        demage = p_demage;
    }
}
