using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverManager : MonoBehaviour
{
    public Transform rightHand;
    public Transform leftHand;
    GameManager gameManager;

    public Transform tubeHandlePosition;
    public Renderer tubeRenderer;
    public Material originalTubeMat;

    Transform referenceElementPosition;
    Renderer referenceElementRenderer;
    public Material originalReferenceMat;
    public bool originalRefMatIsSet = false;

    Transform secondaryElementPosition;
    Renderer secondaryElementRenderer;
    public Material originalSecondaryMat;
    public bool originalSecMatIsSet = false;

    public Transform tabletPosition;
    public Renderer tabletRenderer;
    public Material originalTabletMat;

    public Material tabletHoverMat;
    public Material refCubeHoverMat;

    public Material secCubeHoverMat;
    public Material tubeHoverMat;

    public float distanceToHover = 0.075f;
    public float distanceToUnHover = 0.175f;



    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }
    void Update()
    {

        CheckDistanceToObject(tubeHandlePosition, tubeRenderer, originalTubeMat, tubeHoverMat);


        if (gameManager.referenceElement != null)
        {
            if (originalRefMatIsSet == false)
            {
                originalReferenceMat = gameManager.referenceElement.GetComponent<Renderer>().material;
                referenceElementPosition = gameManager.referenceElement.transform;
                referenceElementRenderer = gameManager.referenceElement.GetComponent<Renderer>();

                originalRefMatIsSet = true;
            }

            CheckDistanceToObject(referenceElementPosition, referenceElementRenderer, originalReferenceMat, refCubeHoverMat);
        }

        if (gameManager.secondaryElement != null)
        {

            if (originalSecMatIsSet == false)
            {
                originalSecondaryMat = gameManager.secondaryElement.GetComponent<Renderer>().material;
                secondaryElementPosition = gameManager.secondaryElement.transform;
                secondaryElementRenderer = gameManager.secondaryElement.GetComponent<Renderer>();

                originalSecMatIsSet = true;
            }

            secondaryElementPosition = gameManager.secondaryElement.transform;
            secondaryElementRenderer = gameManager.secondaryElement.GetComponent<Renderer>();
            CheckDistanceToObject(secondaryElementPosition, secondaryElementRenderer, originalSecondaryMat, secCubeHoverMat);
        }

        CheckDistanceToObject(tabletPosition, tabletRenderer, originalTabletMat, tabletHoverMat);
    }

    void CheckDistanceToObject(Transform transform, Renderer renderer, Material ogMaterial, Material hoverMat)
    {
        float distanceToTubeRightHand = Vector3.Distance(rightHand.transform.position, transform.position);
        float distanceToTubeLeftHand = Vector3.Distance(leftHand.transform.position, transform.position);

        if (distanceToTubeRightHand <= distanceToHover || distanceToTubeLeftHand <= distanceToHover)
        {
            ParticleManager.ChangeMaterial(renderer, hoverMat);

        }
        else if (distanceToTubeRightHand >= distanceToUnHover || distanceToTubeLeftHand >= distanceToUnHover)
        {
            ParticleManager.ChangeMaterial(renderer, ogMaterial);

        }
    }


}
