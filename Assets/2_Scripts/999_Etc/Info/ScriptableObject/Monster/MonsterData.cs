using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Object/Monster Data", order = int.MaxValue)]
public class MonsterData : ScriptableObject
{
    [Header("몬스터 정보")]
    [SerializeField] string name;
    public string NAME { get { return name; } }
    [SerializeField] int number;
    public int NUMBER { get { return number; } }
    [SerializeField] int hp;
    public int HP { get { return hp; } }
    [SerializeField] int demage;
    public int DEMAGE { get { return demage; } }
    [SerializeField] float chase_speed;
    public float CHASESPEED { get { return chase_speed; } }
    [SerializeField] float normal_speed;
    public float NORMALSPEED { get { return normal_speed; } }

    [Header("방의 최대최소 좌표")]
    [SerializeField] float minx;
    public float minX { get { return minx; } }
    [SerializeField] float maxx;
    public float maxX { get { return maxx; } }
    [SerializeField] float miny;
    public float minY { get { return miny; } }
    [SerializeField] float maxy;
    public float maxY { get { return maxy; } }

}
