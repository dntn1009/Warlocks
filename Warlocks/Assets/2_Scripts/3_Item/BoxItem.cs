using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxItem : FieldItem
{
    protected int Demage = 0;

    public override void Item_Effect(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
            Box_Open();
    }

    void Box_Open()
    {
        Demage++;
        if (Demage >= 3)
        {
            Change_Item();
            Destroy(gameObject);
        }
        else
            StartCoroutine(RDemage_Shake());
    }

    public virtual void Change_Item()
    {
        switch (item_number)// 아이템 흩뿌리기
        {
            //FieldItem 0: coin 1: HPA 2: HPB 3: Auto
            case 0: // itembox
                for(int i = 0; i < 3; i++)
                {
                    float y = Random.Range(0, - 1.5f);
                    float x = Random.Range(-1.5f, 2f);
                    Vector3 Rpos = transform.position + new Vector3(x, y, 0);
                    int percentage = Random.Range(0, 101);
                    if (percentage < 10)
                        Instantiate(RoomTemplates.Instance.FieldItem[2], Rpos, Quaternion.identity);
                    else if (percentage < 25)
                        Instantiate(RoomTemplates.Instance.FieldItem[1], Rpos, Quaternion.identity);
                    else if (percentage < 101)
                    {
                        for (int j = 0; j < 3; j++)
                        Instantiate(RoomTemplates.Instance.FieldItem[0], Rpos + new Vector3(Random.Range(0, 1f), Random.Range(0, 1f), 0), Quaternion.identity);
                    }
                }
                break;
            case 1: // 미정
                break;
            case 2: // 미정
                break;
        }
    }


    #region Corutine_Methods
    IEnumerator RDemage_Shake()
    {
        this.transform.localRotation = Quaternion.Euler(0, 0, -7);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(LDemage_Shake());
    }
    IEnumerator LDemage_Shake()
    {
        this.transform.localRotation = Quaternion.Euler(0, 0, 7);
        yield return new WaitForSeconds(0.1f);
        this.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
    #endregion

}
