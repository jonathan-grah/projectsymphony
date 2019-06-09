using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableUnit : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public static HashSet<SelectableUnit> selectableUnits = new HashSet<SelectableUnit>();
    public static HashSet<SelectableUnit> currentlySelected = new HashSet<SelectableUnit>();

    SelectionManager selectionManager;

    public GameObject selectionCircle;

    void Awake()
    {
        selectableUnits.Add(this);
        selectionManager = GameObject.Find("Drag Handler").GetComponent<SelectionManager>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        currentlySelected.Add(this);
        selectionCircle = Instantiate(selectionManager.selectionCirclePrefab);
        selectionCircle.transform.SetParent(transform, false);
        selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (selectionCircle) Destroy(selectionCircle.gameObject);
        selectionCircle = null;
    }

    public static void DeselectAll(BaseEventData eventData)
    {
        foreach (SelectableUnit selectable in currentlySelected)
            selectable.OnDeselect(eventData);
        currentlySelected.Clear();
    }
}
