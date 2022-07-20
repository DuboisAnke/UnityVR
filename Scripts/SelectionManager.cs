using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    RaycastHit hit;

    IClickSelectable currentSelectable = null;
    IClickButton currentClickable = null;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray();

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, (1 << LayerMask.NameToLayer("Element"))
                | (1 << LayerMask.NameToLayer("Tube"))
                | (1 << LayerMask.NameToLayer("Tablet"))))
            {
                Transform selection = hit.transform;
                if (hit.transform != null)
                {
                    currentSelectable = hit.transform.GetComponentInParent<IClickSelectable>();
                    if (currentSelectable != null)
                    {
                        currentSelectable.OnSelectionStart();
                    }
                }
            }

            if (Physics.Raycast(ray, out hit, (1 << LayerMask.NameToLayer("Button"))))
            {
                Transform selection = hit.transform;
                if (hit.transform != null)
                {
                    currentClickable = hit.transform.GetComponentInParent<IClickButton>();
                    if (currentClickable != null)
                    {
                        currentClickable.OnClick();
                    }
                }
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (currentSelectable != null)
            {
                currentSelectable.OnSelectionEnd();
            }
            currentSelectable = null;
        }

        if (currentSelectable != null)
        {
            currentSelectable.UpdateSelection();
        }
    }

}
