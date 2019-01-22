using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string saveName;

    public GameData(StartProcessor data)
    {
        saveName = data.saveName;
    }
}
