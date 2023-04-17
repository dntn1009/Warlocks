using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletController : MonoBehaviour
{
    [SerializeField] GameObject ExploObj;
    [SerializeField] float m_lifeTime;
    [SerializeField] float m_speed;
    [SerializeField] Transform Explo_Pos;

    Vector3 m_PrevPos;
    Transform player;
    int demage;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Bullet_LifeTime(m_lifeTime));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, m_speed * Time.deltaTime);
        Vector2 dir = player.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
            Instantiate(ExploObj, Explo_Pos.position, Quaternion.identity);
            
            if(collision.transform.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().BossSetDemage(demage);
            }
        }
    }


    #region BulletMthods
    public void SetBullet(Transform player_pos, int pdemage)
    {
        player = player_pos;
        demage = pdemage;
    }

    #endregion

    #region CoroutineMethods
    IEnumerator Bullet_LifeTime(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
        Instantiate(ExploObj, Explo_Pos.transform.position, Quaternion.identity);
    }
    #endregion
}
