using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnClick : MonoBehaviour
{

    public GameObject tank;
    public bool landCooldownActive = false;
    public bool seaCooldownActive = false;
    public bool airCooldownActive = false;

    public Transform spawnPoint;

    private IEnumerator cooldown()
    {
        landCooldownActive = true;
        yield return new WaitForSeconds(3);
        landCooldownActive = false;
    }

    void spawnLandUnit(GameObject unit, string prefabName)
    {
        if (!landCooldownActive)
        {
            unit.GetComponent<UnitController>().unitDetails.id = $"{prefabName}_{Guid.NewGuid().ToString()}";
            var newUnit = Instantiate(unit, spawnPoint.position, spawnPoint.rotation);
            newUnit.transform.parent = GameObject.Find("Units").transform;
            StartCoroutine(cooldown());
        }
    }

    public void SpawnBasicTank()
    {
        spawnLandUnit(tank, "sturmgeschutz_iv");
    }

}
