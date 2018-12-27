﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    bool isSelecting = false;
    Vector3 initialMousePosition;

    public GameObject selectionCirclePrefab;

    public bool IsWithinSelectionBounds(GameObject gameObject)
    {
        if (!isSelecting)
            return false;

        var camera = Camera.main;
        var viewportBounds = Utils.GetViewportBounds(camera, initialMousePosition, Input.mousePosition);

        return viewportBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // when click is pressed down
        {
            isSelecting = true;
            initialMousePosition = Input.mousePosition;

            foreach (var selectableUnit in FindObjectsOfType<SelectableUnit>())
            {
                if (selectableUnit.selectionCircle != null && initialMousePosition != Input.mousePosition) // ensures dragging and not direct clicking
                    deselectUnit(selectableUnit);

            }
        }

        if (Input.GetMouseButtonUp(0)) // when click is released
        {
            isSelecting = false;
        }

        if (isSelecting)
        {
            foreach (var selectableUnit in FindObjectsOfType<SelectableUnit>())
            {
                if (IsWithinSelectionBounds(selectableUnit.gameObject))
                    if (selectableUnit.selectionCircle == null)
                        selectUnit(selectableUnit);
                else if (initialMousePosition != Input.mousePosition) // ensures dragging and not direct clicking
                    if (selectableUnit.selectionCircle != null)
                        deselectUnit(selectableUnit);
            }
        }
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            var rect = Utils.GetScreenRect(initialMousePosition, Input.mousePosition);
            Utils.DrawScreenRectBorder(rect, 1, Color.black);
        }
    }

    public void selectUnit(SelectableUnit selectableUnit)
    {
        selectableUnit.isSelected = true;
        selectableUnit.selectionCircle = Instantiate(selectionCirclePrefab);
        selectableUnit.selectionCircle.transform.SetParent(selectableUnit.transform, false);
        selectableUnit.selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);
    }

    public void deselectUnit(SelectableUnit selectableUnit)
    {
        selectableUnit.isSelected = false;
        Destroy(selectableUnit.selectionCircle.gameObject);
        selectableUnit.selectionCircle = null;
    }

}
