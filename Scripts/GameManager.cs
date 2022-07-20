using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Everything we need to spawn in an element
    public List<ElementInfo> elementList;
    ElementInfo chosenElementInfo;
    public GameObject spawnedElement;

    ElementController elementController;

    // positionreferences and prefabs to instantiate
    public GameObject cube;
    public Transform leftBoxPosRef;
    public Transform rightBoxPosRef;
    public Transform workstationPosRef;
    public int subdiv;

    public GameObject correctUI;
    public GameObject inCorrectUI;
    public GameObject equalSign;

    public bool playerIsHoldingTube = false;

    public ElementController referenceElement;
    public Material referenceCubeMaterial;
    public ElementController secondaryElement;
    public Workstation workstation;
    public Cannister cannister;
    public PrimaryBoxTrigger primaryBoxTrigger;
    public SecondaryBoxTrigger secondaryBoxTrigger;

    public bool gameRunning = false;
    public AudioSource audioSource;

    void Start()
    {


        // initialising an element from list of scriptableObjects

        referenceElement = InitReferenceElement();
        correctUI.GetComponent<SpriteRenderer>().enabled = false;
        inCorrectUI.GetComponent<SpriteRenderer>().enabled = false;
        equalSign.GetComponent<SpriteRenderer>().enabled = true;

        primaryBoxTrigger = GameObject.Find("LeftBoxTrigger").GetComponent<PrimaryBoxTrigger>();
        secondaryBoxTrigger = GameObject.Find("RightBoxTrigger").GetComponent<SecondaryBoxTrigger>();
        cannister = GameObject.Find("TestTubeTrigger").GetComponent<Cannister>();
    }

    public ElementController InitReferenceElement()
    {

        gameRunning = true;

        HoverManager hoverManager = GetComponent<HoverManager>();
        if (hoverManager.originalReferenceMat != null)
        {
            hoverManager.originalReferenceMat = null;
            hoverManager.originalRefMatIsSet = false;
        }


        // initialisation of the the reference element with density, scale and a seperate material
        // gets a random element out of our list of elements, this simply contains the info we need from the scriptable objects
        chosenElementInfo = elementList[Random.Range(0, elementList.Count)];

        // we have to instantiate the element (the cube is added in the editor, so we know what 'object' to spawn)
        spawnedElement = Instantiate(this.cube, leftBoxPosRef.transform.position, Quaternion.identity);
        spawnedElement.name = "ReferenceElement";


        // next we need to find the elementcontroller attached to this object and get the density
        elementController = spawnedElement.GetComponent<ElementController>();
        elementController.density = chosenElementInfo.substance.density;
        elementController.size = chosenElementInfo.size;

        // we need to set the local size of the object to the size the element should have
        spawnedElement.transform.localScale = Vector3.one * chosenElementInfo.size;

        // In order to set the material, we need to get the Renderer
        Renderer renderer = spawnedElement.GetComponent<Renderer>();
        // set the material 
        renderer.material = referenceCubeMaterial;
        spawnedElement = null;
        chosenElementInfo = null;

        return elementController;

    }

    public ElementController InitSecondaryElement()
    {
        HoverManager hoverManager = GetComponent<HoverManager>();
        if (hoverManager.originalSecondaryMat != null)
        {
            hoverManager.originalSecondaryMat = null;
            hoverManager.originalSecMatIsSet = false;
        }

        chosenElementInfo = elementList[Random.Range(0, elementList.Count)];


        spawnedElement = Instantiate(this.cube, workstationPosRef.transform.position, Quaternion.identity);
        spawnedElement.name = "SecondaryElement";

        elementController = spawnedElement.GetComponent<ElementController>();
        elementController.density = chosenElementInfo.substance.density;
        elementController.size = chosenElementInfo.size;

        spawnedElement.transform.localScale = Vector3.one * chosenElementInfo.size;


        Renderer renderer = spawnedElement.GetComponent<Renderer>();
        renderer.material = chosenElementInfo.substance.material;

        // Here we 'calculate'/set how many subdivisions the material should have
        subdiv = 1;
        if (chosenElementInfo.size == 0.5f)
        {
            subdiv = 1;

        }
        else if (chosenElementInfo.size == 0.75f)
        {
            subdiv = 2;
        }
        else
        {
            subdiv = 3;

        }
        renderer.material.SetInt("_Division", subdiv);
        spawnedElement = null;
        chosenElementInfo = null;
        return elementController;
    }

    public void GrabbedElement(ElementController element)
    {


        if (secondaryElement != null)
        {
            if (element.name == "ReferenceElement" && primaryBoxTrigger.elementInPrimaryBox != secondaryElement
            && secondaryBoxTrigger.elementInSecondaryBox != secondaryElement && cannister.CannisterElement != secondaryElement)
            {
                secondaryElement.transform.position = rightBoxPosRef.position;

            }
            else if (element.name == "SecondaryElement" && primaryBoxTrigger.elementInPrimaryBox != referenceElement
            && secondaryBoxTrigger.elementInSecondaryBox != referenceElement && cannister.CannisterElement != referenceElement)
            {
                referenceElement.transform.position = leftBoxPosRef.position;
            }
        }
    }

    public void MoveElement(ElementController element)
    {
        if (element == referenceElement)
        {
            referenceElement.transform.position = leftBoxPosRef.position;
        }
        else if (element == secondaryElement)
        {
            secondaryElement.transform.position = rightBoxPosRef.position;
        }
        else
        {
            return;
        }


    }

    public ElementController ElementToBeTeleported()
    {
        workstation = GameObject.Find("WorkstationTrigger").GetComponent<Workstation>();

        if (primaryBoxTrigger.elementInPrimaryBox != referenceElement &&
            secondaryBoxTrigger.elementInSecondaryBox != referenceElement &&
            workstation.elementOnWorkstation != referenceElement)
        {
            return referenceElement;
        }
        else if (secondaryBoxTrigger.elementInSecondaryBox != secondaryElement &&
            secondaryBoxTrigger.elementInSecondaryBox != secondaryElement &&
            workstation.elementOnWorkstation != secondaryElement)
        {
            return secondaryElement;
        }
        else
        {
            return null;
        }
    }

}

