using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workstation : MonoBehaviour
{
    public ElementController elementOnWorkstation;
    public GameObject spawnedParticleElement;
    public GameObject spawnedParticleElementParent;
    public GameObject cubeParent;

    [SerializeField]
    List<GameObject> cubeParticleViews = new List<GameObject>();
    Dictionary<int, Vector3> cubeOffsets = new Dictionary<int, Vector3>
    {
        { 1, Vector3.zero },
        { 2, Vector3.one * 0.5f},
        { 3, Vector3.one }
    };

    Dictionary<int, float> cameraZAxisOffsets = new Dictionary<int, float>
    {
        { 1, 0.15f },
        { 2, 0.25f},
        { 3, 0.3f }
    };


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Element")
        {
            elementOnWorkstation = col.gameObject.GetComponent<ElementController>();
            if (cubeParent != null)
            {
                Destroy(cubeParent.gameObject);
                RenderParticleView(elementOnWorkstation, GetSubDiv(elementOnWorkstation.size));
            }
            else
            {
                RenderParticleView(elementOnWorkstation, GetSubDiv(elementOnWorkstation.size));
            }

        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Element")
        {
            elementOnWorkstation = null;
        }

    }

    private int GetSubDiv(float size)
    {
        int subdiv = 1;
        if (elementOnWorkstation.size == 0.5f)
        {
            subdiv = 1;

        }
        else if (elementOnWorkstation.size == 0.75f)
        {
            subdiv = 2;
        }
        else
        {
            subdiv = 3;

        }

        return subdiv;
    }

    private void RenderParticleView(ElementController element, int subdiv)
    {

        spawnedParticleElementParent.transform.rotation = Quaternion.identity;

        cubeParent = new GameObject("cubeParent");
        cubeParent.transform.parent = spawnedParticleElementParent.transform;
        cubeParent.transform.localPosition = Vector3.zero;
        cubeParent.transform.localRotation = Quaternion.identity;


        for (int x = 0; x < subdiv; x++)
        {
            for (int y = 0; y < subdiv; y++)
            {
                for (int z = 0; z < subdiv; z++)
                {
                    Vector3 pos = (new Vector3(x, y, z) - cubeOffsets[subdiv]) * ElementInfo.baseSize;
                    spawnedParticleElement = Instantiate(cubeParticleViews[elementOnWorkstation.density - 1], pos, Quaternion.identity);

                    spawnedParticleElement.gameObject.name = x + "/" + y + "/" + z;
                    //Debug.LogError(x + " " + y);

                    spawnedParticleElement.transform.parent = cubeParent.transform;
                    spawnedParticleElement.transform.localPosition = pos;


                }
            }
        }

        GameObject particleCam = GameObject.FindGameObjectWithTag("ParticleCam");
        particleCam.transform.localPosition = new Vector3(0, 0, -cameraZAxisOffsets[subdiv]);

    }
}
