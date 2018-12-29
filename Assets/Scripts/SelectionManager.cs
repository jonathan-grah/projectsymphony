using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// current bugs:
// - you can create selection box with basically any mouse key
// - as soon as you start selecting, clicking doesn't work
// - ensure clicking works with multiple tanks

public class SelectionManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    Vector3 startPosition;
    bool isSelecting;

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
        {
            SelectableUnit.DeselectAll(new BaseEventData(EventSystem.current));
        }
        startPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isSelecting = true;
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
        {
            if (IsWithinSelectionBounds(selectable.gameObject))
            {
                selectable.OnSelect(eventData);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.GetComponent<SelectableUnit>() != null)
        {
            var unit = hit.collider.GetComponent<SelectableUnit>();
            if (unit.selectionCircle == null) unit.OnSelect(eventData);
            else if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
                unit.OnDeselect(eventData);
        }
    }
}
