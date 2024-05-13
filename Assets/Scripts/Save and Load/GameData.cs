using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    
    public int currency;//金钱
    public List<string> hasKOBoss;

    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;

    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckpointId;

    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    public SerializableDictionary<string, float> volumeSettings;
    

    public GameData()
    {
        this.currency = 0;
        hasKOBoss = new List<string>();

        skillTree = new SerializableDictionary<string, bool>();
        inventory=new SerializableDictionary<string, int> ();
        equipmentId=new List<string> ();

        closestCheckpointId = string.Empty;
        checkpoints=new SerializableDictionary<string, bool>();


        volumeSettings=new SerializableDictionary<string, float> ();
    }
}
