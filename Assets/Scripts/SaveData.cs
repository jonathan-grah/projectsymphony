using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    private string id = Guid.NewGuid().ToString();
    public String saveName;
    public DateTime modifiedDate = DateTime.Now;
    public List<UnitDetails> Units = new List<UnitDetails>();

    public SaveData(string saveName)
    {
        this.saveName = saveName;
    }

    public void UpdateSaveName(string newSaveName)
    {
        this.saveName = newSaveName;
    }

    public void AddUnits()
    {
        GameObject[] AllUnits = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject Unit in AllUnits)
        {
            Unit.GetComponent<UnitController>().Save();
            Units.Add(Unit.GetComponent<UnitController>().unitDetails);
        }
    }

    public List<UnitDetails> GetUnits()
    {
        return (this.Units);
    }

}
