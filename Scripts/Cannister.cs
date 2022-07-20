using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannister : MonoBehaviour
{

    private ElementController cannisterElement;



    Vector3 targetPosition;
    public bool canBeAnimated = true;
    public float floatRange = 0.01f;

    public float floatSpeed = 0.5f;

    public AudioSource audioSource;

    public ElementController CannisterElement { get => cannisterElement; }

    public void EmptyElement()
    {
        cannisterElement = null;
    }

    public void EnterElement(ElementController element)
    {
        cannisterElement = element;

        if (cannisterElement != null)
        {
            Transform positionRef = GameObject.Find("tubeBottomRef").transform;
            cannisterElement.transform.position = positionRef.transform.position;

            if (cannisterElement.density == 1f)
            {
                targetPosition = positionRef.position - new Vector3(0, 0.1f, 0);
            }
            else if (cannisterElement.density == 2f)
            {
                targetPosition = positionRef.position - new Vector3(0, 0.2f, 0);
            }
            else
            {
                targetPosition = positionRef.position - new Vector3(0, 0.3f, 0);
            }

        }

    }
    private void Update()
    {
        if (cannisterElement != null && canBeAnimated)
        {

            Vector3 actualTargetPosition = targetPosition + Vector3.up * Mathf.Sin(Time.time * (2 * Mathf.PI) * floatSpeed) * floatRange;

            cannisterElement.transform.position = Vector3.Lerp(cannisterElement.transform.position, actualTargetPosition, Time.deltaTime);
        }

    }


}
