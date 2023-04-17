using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Data", menuName = "Scriptable Object/Boss Data", order = int.MaxValue)]
public class BossData : ScriptableObject
{
    [SerializeField] string name;
    public string NAME { get { return name; } }

    [SerializeField] int hp;
    public int HP { get { return hp; } }
    [SerializeField] int np;
    public int NP { get { return np; } }
    [SerializeField] int demage;
    public int DEMAGE { get { return demage; } }

    [SerializeField] float speed;
    public float SPEED { get { return speed; } }


}
