using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTriggerMultiple : MonoBehaviour
{
    public Text tutorialText;
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
