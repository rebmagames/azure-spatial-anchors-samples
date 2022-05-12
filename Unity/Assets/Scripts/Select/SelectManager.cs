using System.Collections.Generic;
using UnityEngine;

///-------------------------------------------------------------------------------
/// Author: Amber Voskamp
/// <summary>
/// 
/// </summary>
///-------------------------------------------------------------------------------

public class SelectManager : MonoBehaviour
{
    private List<ObjectSelect> prefabSelect = new List<ObjectSelect>();

    [SerializeField]
    private Camera arCamera;

    public void Update()
    {
        SelectObjectAtTouchPoint();
    }

    private void SelectObjectAtTouchPoint()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
                    ObjectSelect objectSelect = hitObject.transform.GetComponent<ObjectSelect>();

                    if (objectSelect != null)
                    {
                        ChangeColor(objectSelect);
                    }
                }
            }
        }
    }

    private void ChangeColor(ObjectSelect selected)
    {
        foreach (ObjectSelect current in prefabSelect)
        {
            if (selected != current)
            {
                current.Selected = false;
                current.ObjectIsNotSelected();
            }
            else
            {
                current.Selected = true;
                current.ObjectIsSelected();
            }
        }
    }

    public void AddPlacementObjectToList(ObjectSelect PlacementObject)
    {
        prefabSelect.Add(PlacementObject);
    }
}