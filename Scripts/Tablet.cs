using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablet : MonoBehaviour, IClickSelectable
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip scanningLoop;
    [SerializeField]
    private AudioClip scanningSuccess;

    Vector3 cursorPosition;
    Vector3 initialObjectPos;
    Quaternion initialObjectRot;
    public Material particleRenderScreen;
    public Material screenStandby;
    public Material screenScanning;
    public Material seeThrough;
    RaycastHit hit;
    Workstation workstation;
    public bool elementParticleViewVisible = false;
    Rigidbody tabletBody;
    bool tabletIsSelected;

    public float tabletSpeed;
    bool isInitialPosSet = false;

    Renderer visualScreen;
    Renderer visualCase;
    Material originalCaseMat;


    private bool successBeepOnce = false;

    private void Start()
    {
        if (!isInitialPosSet)
        {
            initialObjectPos = this.transform.position;
            initialObjectRot = this.transform.rotation;
            isInitialPosSet = true;

        }

        visualCase = GameObject.Find("Case").GetComponent<Renderer>();
        visualScreen = GameObject.Find("TabletScreen").GetComponent<Renderer>();
        workstation = GameObject.Find("WorkstationTrigger").GetComponent<Workstation>();
        originalCaseMat = visualCase.sharedMaterial;

    }

    public void MoveTablet()
    {
        cursorPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.20f));

        this.transform.position = cursorPosition;
    }

    public void OnSelectionEnd()
    {
        //this.transform.position = initialObjectPos;
        elementParticleViewVisible = false;
        tabletIsSelected = false;
        successBeepOnce = false;
        StartCoroutine(TeleportTablet(tabletSpeed));
    }

    public void OnSelectionStart()
    {
        tabletBody = this.GetComponent<Rigidbody>();
        tabletIsSelected = true;
        successBeepOnce = false;
        visualScreen.material = screenScanning;
    }


    public void Update()
    {
        audioSource.enabled = tabletIsSelected;

        if (!elementParticleViewVisible)
        {
            successBeepOnce = false;
            // if (!audioSource.isPlaying)
            // {
            //     audioSource.Play();
            // }
            // else
            // {
            //     audioSource.Stop();
            // }
        }
        else
        {
            //audioSource.Stop();
        }

        if (tabletIsSelected && workstation.elementOnWorkstation != null)
        {
            UpdateSelection();
        }
    }

    IEnumerator TeleportTablet(float speed)
    {

        // visualCase.sharedMaterial = seeThrough;
        // visualScreen.sharedMaterial = seeThrough;
        // Debug.LogError("output material is" + visualCase.sharedMaterial.name);
        this.transform.rotation = initialObjectRot;

        float timeElapsed = 0;
        Vector3 startPosition = this.transform.position;
        while (timeElapsed < speed)
        {
            this.transform.position = Vector3.Lerp(startPosition, initialObjectPos, timeElapsed / speed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // visualCase.sharedMaterial = originalCaseMat;
        visualScreen.sharedMaterial = screenStandby;
        // Debug.LogError("output material is" + visualCase.sharedMaterial.name);
    }

    public void UpdateSelection()
    {

        if (Physics.SphereCast(this.transform.position, 0.05f, -this.transform.up, out hit))
        {
            //Debug.LogError("tablet raycast hit " + hit.transform.name);
            if (hit.transform.name == workstation.elementOnWorkstation.name)
            {
                if (!successBeepOnce)
                {
                    audioSource.PlayOneShot(scanningSuccess);
                    successBeepOnce = true;
                }

                //Debug.LogError("tablet raycast hit element");
                elementParticleViewVisible = true;
                Renderer renderer = GameObject.Find("TabletScreen").GetComponent<Renderer>();
                renderer.material = particleRenderScreen;
            }
        }

    }


}
