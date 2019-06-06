using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    Vector3 startPosition;
    public bool isSelecting;

    public GameObject selectionCirclePrefab;

    public bool IsWithinSelectionBounds(GameObject gameObject)
    {
        var camera = Camera.main;
        var viewportBounds = Utils.GetViewportBounds(camera, startPosition, Input.mousePosition);

        return viewportBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
            SelectableUnit.DeselectAll(new BaseEventData(EventSystem.current));
        startPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.LeftAlt)) isSelecting = true;
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            var rect = Utils.GetScreenRect(startPosition, Input.mousePosition);
            Utils.DrawScreenRectBorder(rect, 1, Color.black);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isSelecting = false;
        foreach (SelectableUnit selectable in SelectableUnit.allMySelectables)
            if (IsWithinSelectionBounds(selectable.gameObject))
                selectable.OnSelect(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // click selection only with left-clicking
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        Ray ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var unit = hit.collider.GetComponent<SelectableUnit>();
            if (unit == null)
            {
                // deselect all, did not click a selectable
                if (SelectableUnit.currentlySelected.Any())
                    SelectableUnit.DeselectAll(eventData);

                return;
            }

            if (SelectableUnit.currentlySelected.Any())
            {
                if (unit.selectionCircle == null && Input.GetKey(KeyCode.LeftControl))
                    unit.OnSelect(eventData);
                else if (unit.selectionCircle != null && !Input.GetKey(KeyCode.LeftControl))
                    unit.OnDeselect(eventData);
                else
                {
                    SelectableUnit.DeselectAll(eventData);
                    unit.OnSelect(eventData);
                }
            }
            else
                unit.OnSelect(eventData);
        }
    }
}
