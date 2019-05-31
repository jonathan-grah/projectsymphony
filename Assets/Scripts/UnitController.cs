using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public UnitDetails unitDetails;
    GameObject navTarget;

    NavMeshPath path;
    NavMeshAgent unitAgent;
    private int currentCorner = 0;

    void Awake()
    {
        unitAgent = GetComponent<NavMeshAgent>();

        navTarget = GameObject.Find("Navigation Target");
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
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 0.9f); // look at next corner

            yield return new WaitForEndOfFrame();

            if (currentCorner != 0)
                if (Vector3.Dot(transform.TransformDirection(Vector3.forward), (path.corners[currentCorner] - transform.position).normalized) > 0.75)
                {
                    unitAgent.destination = path.corners[currentCorner];
                }
        }
    }

    void Update()
    {
        if (gameObject.GetComponent<SelectableUnit>().selectionCircle != null)
            if (Input.GetMouseButtonDown(1)) // right click
                StartCoroutine(startPath()); // generates path
        if (path != null && path.corners.Length > (currentCorner + 1))
        {
            if (Vector3.Distance(transform.position, path.corners[currentCorner]) <= 0) // if reached corner
            {
                currentCorner += 1;
                Debug.DrawLine(transform.position, path.corners[currentCorner], Color.red);
                StartCoroutine(RotateAndMove());
            }
        }
    }

    IEnumerator startPath()
    {
        RaycastHit hit;
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(raycast.origin, raycast.direction, out hit, Mathf.Infinity))
        {
            if (hit.transform.tag != "Obstacle")
            {
                navTarget.transform.position = hit.point;
                navTarget.GetComponent<Projector>().enabled = true;
                path = new NavMeshPath();
                currentCorner = 0;
                unitAgent.CalculatePath(hit.point, path);
                RotateAndMove();
                yield return new WaitForSeconds(2);
                navTarget.GetComponent<Projector>().enabled = false;
            }
        }

    }
}