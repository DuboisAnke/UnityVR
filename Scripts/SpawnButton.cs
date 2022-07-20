using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour, IClickButton
{
    GameManager manager;
    Workstation workstation;

    public void OnClick()
    {
        this.SpawnNewElement();
    }

    void SpawnNewElement()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        workstation = GameObject.Find("WorkstationTrigger").GetComponent<Workstation>();


        if (manager.secondaryElement == null)
        {
            if (workstation.elementOnWorkstation != null)
            {
                manager.MoveElement(workstation.elementOnWorkstation);
                manager.secondaryElement = manager.InitSecondaryElement();
                workstation.elementOnWorkstation = manager.secondaryElement;
            }
            else
            {
                manager.secondaryElement = manager.InitSecondaryElement();
                workstation.elementOnWorkstation = manager.secondaryElement;
            }

        }
        else
        {
            if (workstation.elementOnWorkstation != null)
            {
                manager.MoveElement(workstation.elementOnWorkstation);
                Destroy(manager.secondaryElement.gameObject);
                manager.secondaryElement = null;
                manager.secondaryElement = manager.InitSecondaryElement();
            }
            else
            {
                Destroy(manager.secondaryElement.gameObject);
                manager.secondaryElement = null;
                manager.secondaryElement = manager.InitSecondaryElement();
            }


        }


    }


}
