using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ChapterInfo
{
    [SerializeField] int MainChapter;
    [SerializeField] int SubChapter;

    [SerializeField] string Savedate;

    [SerializeField] bool startCheck = true;
    [SerializeField] bool PlayCheck = true;


    public ChapterInfo()
    {
        MainChapter = 1;
        SubChapter = 1;
        Savedate = DateTime.Now.ToString("yyyy-MM-dd"); ;
    }

    public bool STARTCHECK { get { return startCheck; } set { startCheck = value; } }
    public bool PLAYCHECK { get { return PlayCheck; } set { PlayCheck = value; } }
    public int MCHAPTER { get { return MainChapter; } set { MainChapter = value; } }
    public int SCHAPTER { get { return SubChapter; } set { SubChapter = value; } }

    public string SAVEDATE { get { return Savedate; } set { Savedate = value; } }

}
