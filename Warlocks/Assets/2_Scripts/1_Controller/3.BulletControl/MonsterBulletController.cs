using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBulletController : MonoBehaviour
{
    [SerializeField]
    GameObject m_bullet;

    [SerializeField]
    GameObject m_explo;

    float m_lifeTime = 2f;

    [SerializeField]
    float m_speed = 6f;

    Vector3 m_dir;
    Vector3 m_PrevPos;
    int m_demage;
    Rigidbody2D m_rigid;

    // Start is called before the first frame update
    private void Start()
    {
        m_rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(Bullet_LifeTime(m_lifeTime));// life_time 후 총알 삭제
    }
    void Update()
    {
        m_PrevPos = transform.position;
        var moveValue = m_speed * Time.deltaTime;
        transform.position += m_dir * moveValue;
        var dir = transform.position - m_PrevPos;
        var hit = Physics2D.Raycast(m_PrevPos, dir.normalized, moveValue, 1 << LayerMask.NameToLayer("Background") | 1 << LayerMask.NameToLayer("Enemy"));
        if (hit.collider != null)
        {
            transform.position = hit.point;
        }
    }

    public void SetBullet(Vector3 pos, Vector3 dir, int demage)
    {
        m_dir = dir;
        transform.position = pos;
        m_demage = demage;
    }

    IEnumerator Bullet_LifeTime(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Player") || collision.transform.CompareTag("Box"))
        {
            m_dir = new Vector3(0, 0, 0);
            m_bullet.SetActive(false);
            m_explo.SetActive(true);
            Destroy(gameObject, 0.05f);
            if (collision.transform.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().setDemage(m_demage);
            }
        }
    }
}
