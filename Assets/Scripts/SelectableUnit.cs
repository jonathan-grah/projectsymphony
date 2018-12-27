using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableUnit : MonoBehaviour
{
    public GameObject selectionCircle;
    public bool isSelected;

    private GameObject MainCamera;

    private void Start()
    {
        MainCamera = GameObject.Find("MainCamera");
    }

    // BUG: Clicking to select a unit breaks after continuous triggering

    void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (!isSelected)
                MainCamera.GetComponent<SelectionManager>().selectUnit(this);
            else
                MainCamera.GetComponent<SelectionManager>().deselectUnit(this);
        }
    }

}
