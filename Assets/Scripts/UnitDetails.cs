using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitDetails
{
    public string id; // "prefabname_uniqueid"
    public int health = 20;
    [HideInInspector]
    public string position, rotation, localScale;
}