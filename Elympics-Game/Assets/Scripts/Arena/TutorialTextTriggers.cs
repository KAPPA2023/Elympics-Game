using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextTriggers : MonoBehaviour
{
    public TMP_Text tutorialText;
    public float textSize;
    public string triggerMessage;
    public GameObject image;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialText.fontSize = textSize;
            tutorialText.text = triggerMessage;
            image.SetActive(true);
            tutorialText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialText.gameObject.SetActive(false);
            image.SetActive(false);
        }

    }
}
