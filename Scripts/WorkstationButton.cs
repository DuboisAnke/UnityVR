using System.Collections;
using System.Collections.Generic;
using System.IO;
using OculusSampleFramework;
using UnityEngine;

public class WorkstationButton : MonoBehaviour, IClickButton
{
    public Cannister cannister;
    public Workstation workstation;
    public Renderer tubeRenderer;
    public Renderer cannisterTubeRenderer;
    public GameObject hoseBottomPositionRef;
    public GameObject workStationPositionRef;
    public GameObject cannisterTopPositionRef;

    [SerializeField]
    private Oculus.Interaction.PokeInteractable workstationButtonInteractable;
    [SerializeField]
    private Oculus.Interaction.PokeInteractable spawnButtonInteractable;
    Vector3 newCubePosition;

    ElementController element;
    GameManager gameManager;

    public float cubeSpeed;
    bool elementIsInTube = false;
    public AudioSource audioSource;


    public void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cannister = GameObject.Find("TestTubeTrigger").GetComponent<Cannister>();
        workstation = GameObject.Find("WorkstationTrigger").GetComponent<Workstation>();
    }

    public void Update() 
    {
        audioSource.enabled = elementIsInTube;
    }

    public void OnClick()
    {
        if (workstation.elementOnWorkstation != null)
        {
            return;
        }
        else
        {
            HandleMovement();
        }
    }

    void HandleMovement()
    {

        if (cannister.CannisterElement != null)
        {
            StartCoroutine(AnimateTube(cannister.CannisterElement));
        }
        else
        {
            element = gameManager.ElementToBeTeleported();
            if (element == null)
            {
                return;
            }
            else
            {
                StartCoroutine(AnimateTeleport(element));
            }


        }
    }

    void TeleportCube(ElementController element, GameObject positionRef)
    {

        element.GetComponent<Rigidbody>().isKinematic = true;
        element.transform.position = positionRef.transform.position;

    }


    IEnumerator AnimateTube(ElementController element)
    {

        cannister.canBeAnimated = false;
        workstationButtonInteractable.Disable();
        spawnButtonInteractable.Disable();

        elementIsInTube = true;


        float timeElapsed = 0;
        Vector3 startPosition = element.transform.position;
        while (timeElapsed < cubeSpeed)
        {
            element.transform.position = Vector3.Lerp(startPosition, cannisterTopPositionRef.transform.position, timeElapsed / cubeSpeed);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        

        
        element.GetComponent<Renderer>().enabled = false;
        TeleportCube(element, hoseBottomPositionRef);


        float cannisterLumpValue = 0;
        float cannisterLumpSpeed = 1.35f;


        while (cannisterLumpValue < 1)
        {
            cannisterLumpValue += Time.deltaTime * cannisterLumpSpeed;
            cannisterTubeRenderer.material.SetFloat("_LumpMiddle", cannisterLumpValue);
            yield return null;
        }


        float lumpValue = 1;
        float lumpSpeed = 1.35f;

        while (lumpValue > 0)
        {
            lumpValue -= Time.deltaTime * lumpSpeed;
            tubeRenderer.material.SetFloat("_LumpMiddle", lumpValue);
            yield return null;
        }

        lumpValue = 1;
        tubeRenderer.material.SetFloat("_LumpMiddle", lumpValue);

        elementIsInTube = false;

        ParticleManager.TeleportParticle("PoofClouds", element.gameObject);
        element.GetComponent<Rigidbody>().isKinematic = false;
        element.GetComponent<Renderer>().enabled = true;

        cannister.EmptyElement();
        cannister.canBeAnimated = true;

        yield return new WaitForSeconds(0.75f);


        workstationButtonInteractable.Enable();
        spawnButtonInteractable.Enable();

    }

    IEnumerator AnimateTeleport(ElementController element)
    {
        workstationButtonInteractable.Disable();
        spawnButtonInteractable.Disable();

        ParticleManager.TeleportParticle("Circle", element.gameObject);
        yield return new WaitForSeconds(0.75f);

        element.GetComponent<Renderer>().enabled = false;

        TeleportCube(element, workStationPositionRef);

        ParticleManager.TeleportParticle("Circle", element.gameObject);
        yield return new WaitForSeconds(0.75f);

        element.GetComponent<Renderer>().enabled = true;
        element.GetComponent<Rigidbody>().isKinematic = false;

        yield return new WaitForSeconds(0.75f);

        workstationButtonInteractable.Enable();
        spawnButtonInteractable.Enable();
    }




}
