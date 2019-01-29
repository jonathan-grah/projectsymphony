using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public UnitDetails unitDetails;

    UnityEngine.AI.NavMeshAgent playerAgent;

    void Awake()
    {
        playerAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void Save()
    {
        unitDetails.position = transform.position.ToString();
        unitDetails.rotation = transform.rotation.ToString();
        unitDetails.localScale = transform.localScale.ToString();
    }

    public void Load(UnitDetails newDetails)
    {
        transform.position = SaveManager.StringToVector(newDetails.position);
        transform.rotation = SaveManager.StringToQuaternion(newDetails.rotation);
        transform.localScale = SaveManager.StringToVector(newDetails.localScale);
        unitDetails = newDetails;
    }

    void Update()
    {
        if (gameObject.GetComponent<SelectableUnit>().selectionCircle != null)
            if (Input.GetMouseButtonDown(1)) // right click
                move();
    }

    void move()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            playerAgent.destination = hit.point;
    }
}