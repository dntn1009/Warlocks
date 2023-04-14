using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefinedEnums;
public class BulletController : MonoBehaviour
{
    [SerializeField] GameObject ExploObj;
    [SerializeField] BulletType type;
    [SerializeField] float m_lifeTime;
    [SerializeField] float m_speed;
    [SerializeField] Transform Explo_Pos;

    Vector3 m_dir = Vector3.zero;
    Vector3 m_PrevPos;

    int demage;

    void Start()
    {
        StartCoroutine(Bullet_LifeTime(m_lifeTime));
    }

    void Update()
    {
        m_PrevPos = transform.position;
        var moveValue = m_speed * Time.deltaTime;
        transform.position += m_dir * moveValue;
        var dir = transform.position - m_PrevPos;
    }

    public void SetBullet(Vector3 pos, Vector3 dir, int pdemage)
    {
        demage = pdemage;
        BulletDemage(type);
        m_dir = dir;
        bulletRotation(m_dir);
        transform.position = pos;
    }

    void BulletDemage(BulletType type)
    {
        switch (type)
        {
            case BulletType.Basic:
                break;
            case BulletType.Enforce:
                demage = (int)(demage * 1.5f);
                break;
            case BulletType.Sniper:
                demage *= 3;
                break;
            case BulletType.Stun:
                demage = (int)(demage * 1.25f);
                break;
        }
        
    }

    void bulletRotation(Vector3 dir)
    {
        int pos = 1;
        bool XYCheck = XYRotation(dir, ref pos);
        switch(XYCheck)
        {
            case true:
                if(pos == 1)
                    transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                break;
            case false:
                if (pos == -1)
                    transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                else
                    transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;
        }


    }

    bool XYRotation(Vector3 dir, ref int pos)
    {
        if (dir == Vector3.right || dir == Vector3.left)
        {
            pos = (int)dir.x;
            return true;
        }
        else
        {
            pos = (int)dir.y;
            return false;
        }
    }

    IEnumerator Bullet_LifeTime(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("Boss") || collision.transform.CompareTag("Box"))
        {

            m_dir = Vector3.zero;
            Destroy(gameObject);
            Instantiate(ExploObj, Explo_Pos.position, transform.rotation);
            if (collision.transform.CompareTag("Enemy"))
            {
                var enemy = collision.gameObject.GetComponent<MonsterController>();
                enemy.BulletypeEffect_Demage(type, demage);
                enemy.fir_demageCheck = true;
            }
            else if(collision.transform.CompareTag("Boss"))
            {
                int chapter = OverallManager.Instance.p_Obj.GetComponent<PlayerController>().playerinfo.RECORD.MCHAPTER;
                switch (chapter)
                {
                    case 1:
                        var enemy = collision.gameObject.GetComponent<KingMushController>();
                        enemy.BulletypeEffect_Demage(type, demage);
                        break;
                    case 2:
                        var enemy2 = collision.gameObject.GetComponent<DarkanController>();
                        enemy2.BulletypeEffect_Demage(type, demage);
                        break;
                    case 3:
                        var enemy3 = collision.gameObject.GetComponent<WitchController>();
                        enemy3.BulletypeEffect_Demage(type, demage);
                        break;
                }

            }
        }
    }
}
