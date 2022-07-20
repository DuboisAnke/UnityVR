using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ElementController : MonoBehaviour, IClickSelectable
{

    public int density;
    public float size;

    Vector3 newCubePosition;
    public Transform positionRef;
    public ElementController element;
    public Cannister cannister;
    public Renderer tubeRenderer;
    public Renderer cannisterTubeRenderer;
    public Transform tubeBottomPositionRef;

    public GameManager gameManager;

    [SerializeField]
    private Oculus.Interaction.PokeInteractable workstationButtonInteractable;
    [SerializeField]
    private Oculus.Interaction.PokeInteractable spawnButtonInteractable;




    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        tubeRenderer = GameObject.Find("Hose").GetComponent<Renderer>();
        cannister = GameObject.Find("TestTubeTrigger").GetComponent<Cannister>();

        cannisterTubeRenderer = GameObject.Find("Hose02").GetComponent<Renderer>();
        workstationButtonInteractable = GameObject.Find("TeleportPokeInteractable").GetComponent<Oculus.Interaction.PokeInteractable>();
        spawnButtonInteractable = GameObject.Find("SpawnPokeInteractable").GetComponent<Oculus.Interaction.PokeInteractable>();
    }

    public void MoveCube()
    {
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.25f));
        //Vector3 offset = hitObject.transform.position - cursorPosition;

        this.transform.position = cursorPosition;
    }

    public void OnSelectionStart()
    {

        gameManager.GrabbedElement(this);

    }

    public void UpdateSelection()
    {
        // this.MoveCube();
    }

    public void OnSelectionEnd()
    {

    }

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Tube" && gameManager.playerIsHoldingTube && cannister.CannisterElement == null)
        {

            if (density == 1f)
            {
                StartCoroutine(AnimateTube(0f));
            }
            else if (density == 2f)
            {
                StartCoroutine(AnimateTube(0.1f));
            }
            else
            {
                StartCoroutine(AnimateTube(0.2f));
            }
        }
        else
        {
            return;
        }

    }

    IEnumerator AnimateTube(float height)
    {
        workstationButtonInteractable.Disable();
        spawnButtonInteractable.Disable();

        ParticleManager.TeleportParticle("PoofClouds", this.gameObject);
        this.GetComponent<Renderer>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = true;

        float lumpValue = 0;
        float lumpSpeed = 1.35f;

        while (lumpValue < 1)
        {
            lumpValue += Time.deltaTime * lumpSpeed;
            tubeRenderer.material.SetFloat("_LumpMiddle", lumpValue);
            yield return null;
        }

        lumpValue = 1;
        tubeRenderer.material.SetFloat("_LumpMiddle", lumpValue);


        float cannisterLumpValue = 1;
        float cannisterLumpSpeed = 1.35f;


        while (cannisterLumpValue > 0)
        {
            cannisterLumpValue -= Time.deltaTime * cannisterLumpSpeed;
            cannisterTubeRenderer.material.SetFloat("_LumpMiddle", cannisterLumpValue);
            yield return null;
        }

        this.GetComponent<Renderer>().enabled = true;
        cannister.EnterElement(this.GetComponent<ElementController>());

        yield return new WaitForSeconds(0.75f);

        workstationButtonInteractable.Enable();
        spawnButtonInteractable.Enable();

    }

}

