using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleViewController : MonoBehaviour
{
    float rotationSpeed = 5f;
    Workstation workstation;

    void Update()
    {
        Tablet tablet = GameObject.Find("Tablet01").GetComponent<Tablet>();
        workstation = GameObject.Find("WorkstationTrigger").GetComponent<Workstation>();

        if (tablet.elementParticleViewVisible)
        {
            if (workstation.cubeParent != null)
            {
                transform.RotateAround(workstation.cubeParent.transform.position, new Vector3(0.1f, 0.1f, 0f), rotationSpeed * Time.deltaTime);

            }
        }
    }

}
