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

    public List<Vector3> waypoints = new List<Vector3>();
    NavMeshPath currentPath;
    int currentCorner = 0;
    bool navStatus = false;

    NavMeshAgent unitAgent;


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

    IEnumerator nextCorner()
    {
        navStatus = true;
        while (Vector3.Distance(transform.position, currentPath.corners[currentCorner]) > 0.5f) // while not near corner
        {
            Vector3 newDir = currentPath.corners[currentCorner] - transform.position;
            Quaternion rotation = Quaternion.LookRotation(newDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 0.9f); // look at next corner

            yield return new WaitForEndOfFrame();

            if (Vector3.Dot(transform.TransformDirection(Vector3.forward), (currentPath.corners[currentCorner] - transform.position).normalized) > 0.75)
            {
                unitAgent.destination = currentPath.corners[currentCorner];
            }
        }
        if (currentPath.corners.Length == (currentCorner + 1)) // if last corner
            navStatus = false;
    }

    void Update()
    {
        if (gameObject.GetComponent<SelectableUnit>().selectionCircle != null)
            if (navStatus && Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(1)) // shift + right click
            {
                StartCoroutine(createPath());
            }
            else if (Input.GetMouseButtonDown(1)) // right click
            {
                navStatus = false;
                waypoints.Clear();
                StartCoroutine(createPath());
            }

        if (navStatus)
        {
            if (currentPath != null && currentPath.corners.Length > (currentCorner + 1))
            {
                if (Vector3.Distance(transform.position, currentPath.corners[currentCorner]) <= 0.5f) // if reached corner
                {
                    currentCorner += 1;
                    StartCoroutine(nextCorner()); // start towards next corner
                }
            }
        }
        else if (!navStatus && waypoints.Count > 0) // if not navigating and there's another path to navigate to
        {
            currentPath = new NavMeshPath(); // adds oldest path to current path
            unitAgent.CalculatePath(waypoints[0], currentPath);
            waypoints.RemoveAt(0); // removes from paths backlog
            currentCorner = 0; // reset corner
            StartCoroutine(nextCorner());
        }
    }

    IEnumerator createPath()
    {
        RaycastHit hit;
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(raycast.origin, raycast.direction, out hit, Mathf.Infinity))
        {
            if (hit.transform.tag != "Obstacle")
            {
                navTarget.transform.position = hit.point;
                navTarget.GetComponent<Projector>().enabled = true;
                waypoints.Add(hit.point);
                yield return new WaitForSeconds(2);
                navTarget.GetComponent<Projector>().enabled = false;
            }
        }
    }
}