using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Raycast : MonoBehaviour
{
    private Vector3 targetPosition; // target position that player will move to

    NavMeshAgent playerAgent;

    void Awake()
    {
        playerAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (gameObject.GetComponent<SelectableUnit>().selectionCircle != null)
        {
            if (Input.GetMouseButtonDown(1)) // right click
                SetTargetPosition();
            MovePlayer();
        }
    }

    void SetTargetPosition()
    {
        Plane plane = new Plane(Vector3.up, transform.position);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float point = 0f;

        if (plane.Raycast(ray, out point))
            targetPosition = ray.GetPoint(point);
    }

    void MovePlayer()
    {
        playerAgent.SetDestination(targetPosition);
    }



}
