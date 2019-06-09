using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{

    public GameObject assaultGun;

    private bool landCooldownActive = false;
    private bool seaCooldownActive = false;
    private bool airCooldownActive = false;

    public Transform garage;
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
            var newUnit = Instantiate(unit, garage.position, landSpawnPoint.transform.rotation);
            newUnit.transform.parent = GameObject.Find("Units").transform;
            newUnit.GetComponent<UnitController>().unitDetails.id = $"{prefabName}_{Guid.NewGuid().ToString()}";
            newUnit.GetComponent<UnitController>().waypoints.Add(landSpawnPoint.transform.position);
            StartCoroutine(cooldown());
        }
    }

    public void SpawnBasicTank()
    {
        spawnLandUnit(assaultGun, "temptank");
    }

    public void ToggleFlagAdjustment()
    {
        if (landSpawnPoint.activeSelf) landSpawnPoint.SetActive(false);
        else landSpawnPoint.SetActive(true);
    }

}
