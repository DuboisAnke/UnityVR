using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryBoxTrigger : MonoBehaviour
{
    public ElementController elementInPrimaryBox;
    GameManager manager;
    SecondaryBoxTrigger secondaryBoxTrigger;

    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip inCorrectSound;
    int lastExitFrame = -1;

    //public GameObject boxVisual;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        secondaryBoxTrigger = GameObject.Find("RightBoxTrigger").GetComponent<SecondaryBoxTrigger>();

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag != "Element" || Time.frameCount == lastExitFrame)
        {
            return;
        }

        // iTween.ScaleFrom(boxVisual, iTween.Hash(
        //                         "x", 1f,
        //                         "y", 0.85f,
        //                         "z", 0.7f,
        //                         "time", 1.5f,
        //                         "easetype", iTween.EaseType.easeOutElastic
        //     ));

        elementInPrimaryBox = col.gameObject.GetComponent<ElementController>();

        if (secondaryBoxTrigger.elementInSecondaryBox != null)
        {


            if (elementInPrimaryBox.density == secondaryBoxTrigger.elementInSecondaryBox.density)
            {
                StartCoroutine(WinAnimation(2f));
            }
            else
            {
                StartCoroutine(LoseAnimation(2f));
            }
        }
        else
        {
            return;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Element" && Time.frameCount != lastExitFrame)
        {
            lastExitFrame = Time.frameCount;
            elementInPrimaryBox = null;
        }

    }

    IEnumerator WinAnimation(float seconds)
    {

        manager.equalSign.GetComponent<SpriteRenderer>().enabled = false;
        manager.correctUI.GetComponent<SpriteRenderer>().enabled = true;

        audioSource.clip = correctSound;
        audioSource.Play();

        // yield return new WaitForSeconds(0.75f);

        // ParticleManager.TeleportParticle("Poof", manager.referenceElement.gameObject);

        // Destroy(manager.referenceElement.gameObject);
        // manager.referenceElement = null;


        // manager.referenceElement = manager.InitReferenceElement();

        // ParticleManager.TeleportParticle("Poof", manager.secondaryElement.gameObject);

        // Destroy(manager.secondaryElement.gameObject);
        // manager.secondaryElement = null;

        // manager.secondaryElement = manager.InitSecondaryElement();

        yield return new WaitForSeconds(seconds);

        manager.equalSign.GetComponent<SpriteRenderer>().enabled = true;
        manager.correctUI.GetComponent<SpriteRenderer>().enabled = false;

    }

    IEnumerator LoseAnimation(float seconds)
    {
        manager.equalSign.GetComponent<SpriteRenderer>().enabled = false;
        manager.inCorrectUI.GetComponent<SpriteRenderer>().enabled = true;

        audioSource.clip = inCorrectSound;
        audioSource.Play();

        yield return new WaitForSeconds(seconds);

        manager.equalSign.GetComponent<SpriteRenderer>().enabled = true;
        manager.inCorrectUI.GetComponent<SpriteRenderer>().enabled = false;


    }
}
