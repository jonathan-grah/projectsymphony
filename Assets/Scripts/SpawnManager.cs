using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject assaultGun;

    private bool landCooldownActive = false;
    private bool seaCooldownActive = false;
    private bool airCooldownActive = false;

    public GameObject landSpawnPoint;

    private IEnumerator cooldown()
    {
        landCooldownActive = true;
        yield return new WaitForSeconds(3);
        landCooldownActive = false;
    }

    void Start()
    {
        landSpawnPoint.SetActive(false);
    }

    void spawnLandUnit(GameObject unit, string prefabName)
    {
        if (!landCooldownActive)
        {
            unit.GetComponent<UnitController>().unitDetails.id = $"{prefabName}_{Guid.NewGuid().ToString()}";
            var newUnit = Instantiate(unit, landSpawnPoint.transform.position, landSpawnPoint.transform.rotation);
            newUnit.transform.parent = GameObject.Find("Units").transform;
            StartCoroutine(cooldown());
        }
    }

    public void SpawnBasicTank()
    {
        spawnLandUnit(assaultGun, "temptank");
    }

    public void ToggleFlagAdjustment()
    {
        if (landSpawnPoint.activeSelf)
        {
            landSpawnPoint.SetActive(false);
        }
        else
        {
            landSpawnPoint.SetActive(true);
        }
    }

}
