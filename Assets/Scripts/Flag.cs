using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Flag : MonoBehaviour
{
    private Vector3 initialPos;

    SelectionManager selectionManager;

    void Awake()
    {
        selectionManager = GameObject.Find("Drag Handler").GetComponent<SelectionManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            initialPos = transform.position;
        }

        if (Input.GetMouseButton(0))
        {
            selectionManager.isSelecting = false;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity) && !Utils.IsPointerOverUI())
            {
                Vector3 newPos = new Vector3(hit.point.x, initialPos.y, hit.point.z);
                transform.position = newPos;
            }
        }
    }
}