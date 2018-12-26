using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnClick : MonoBehaviour
{

    public GameObject tank;
    public GameObject unit2;

    public GameObject spawnPoint;

    public void SpawnUnitOne() // called by button 1 to create unit1 at the HQ spawn point
    {
        Instantiate(tank, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }

    public void SpawnUnitTwo() // called by button 2 to create unit2 at the HQ spawn point
    {
        Instantiate(unit2, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }
}
