using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public UnitDetails unitDetails;

    NavMeshPath path;
    NavMeshAgent unitAgent;
    public int currentCorner = 0;

    void Awake()
    {
        unitAgent = GetComponent<NavMeshAgent>();
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

    private IEnumerator RotateAndMove()
    {
        while (Vector3.Distance(transform.position, path.corners[currentCorner]) > 0.5f)
        {
            Vector3 newDir = path.corners[currentCorner] - transform.position;
            Quaternion rotation = Quaternion.LookRotation(newDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 0.6f);

            yield return new WaitForEndOfFrame();
            if (Vector3.Dot(transform.forward, (path.corners[currentCorner] - transform.position).normalized) > 0.9)
                unitAgent.destination = path.corners[currentCorner];
        }
    }

    void Update()
    {
        if (gameObject.GetComponent<SelectableUnit>().selectionCircle != null)
            if (Input.GetMouseButtonDown(1)) // right click
                move(); // generates path
        if (path != null && path.corners.Length > (currentCorner + 1))
        {
            if (Vector3.Distance(transform.position, path.corners[currentCorner]) <= 0.5f)
            {
                currentCorner += 1;
                StartCoroutine(RotateAndMove());
            }
        }
    }

    void move()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            path = new NavMeshPath();
            currentCorner = 0;
            unitAgent.CalculatePath(hit.point, path);
        }
    }
}