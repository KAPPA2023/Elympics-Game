using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextTriggers : MonoBehaviour
{
    public TMP_Text tutorialText;
    public string triggerMessage; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialText.text = triggerMessage;
            tutorialText.gameObject.SetActive(true);
        }
    }

}
