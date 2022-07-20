using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeHandle : MonoBehaviour, IClickSelectable
{
    Vector3 initialHandGrabPos;
    bool isInitialPosSet = false;

    public Transform hoseBottom;
    public Transform handGrabInt;
    public Transform hand;

    public float tubeSpeed;
    public GameManager gameManager;
    public AudioSource audioSource;

    Renderer handleRenderer;
    public Material originalMat;
    public Material hoverMat;

    bool interactableSelected = false;

    private void Start()
    {
        handleRenderer = GameObject.Find("Handle").GetComponent<Renderer>();

        if (!isInitialPosSet)
        {
            initialHandGrabPos = handGrabInt.position;
            isInitialPosSet = true;
        }


    }

    private void Update()
    {
        UpdateSelection();
    }

    public void OnSelectionEnd()
    {

        interactableSelected = false;
        StartCoroutine(MoveTube(initialHandGrabPos));
        handGrabInt.position = initialHandGrabPos;
        gameManager.playerIsHoldingTube = false;
        handleRenderer.material = originalMat;

    }

    public void OnSelectionStart()
    {
        interactableSelected = true;
        handleRenderer.material = hoverMat;
        gameManager.playerIsHoldingTube = true;

    }


    public void UpdateSelection()
    {
        audioSource.enabled = interactableSelected;


        if (interactableSelected)
        {

            if (hand.transform.position.y >= 1.18 && hand.transform.position.y <= initialHandGrabPos.y)
            {
                hoseBottom.position = new Vector3(initialHandGrabPos.x, hand.position.y, initialHandGrabPos.z);
                handleRenderer.material = hoverMat;
            }

        }
    }

    IEnumerator MoveTube(Vector3 targetPosition)
    {
        float timeElapsed = 0;
        Vector3 startPosition = hoseBottom.position;
        while (timeElapsed < tubeSpeed)
        {
            hoseBottom.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / tubeSpeed);
            timeElapsed += Time.deltaTime;
            // iTween.MoveTo(hoseBottom.gameObject, iTween.Hash(
            //                     "position", targetPosition,
            //                     "time", 1.0f,
            //                     "easetype", iTween.EaseType.easeOutBounce));

            yield return null;
        }

        hoseBottom.position = targetPosition;
        handGrabInt.position = targetPosition;
    }


}
