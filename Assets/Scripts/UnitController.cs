using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent playerAgent;

    void Awake()
    {
        playerAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        if (gameObject.GetComponent<SelectableUnit>().selectionCircle != null)
        {
            if (Input.GetMouseButtonDown(1)) // right click
                move();
        }
    }

    void move()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            playerAgent.destination = hit.point;
        }
    }
}