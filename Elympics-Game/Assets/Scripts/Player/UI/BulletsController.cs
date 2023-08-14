using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletsController : MonoBehaviour
{
    [SerializeField] private PlayerProvider playersProvider = null;
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;
    private ActionHandler actionHandler = null;

    private void Start()
    {
        actionHandler = playersProvider.ClientPlayer.GetComponent<ActionHandler>();
        actionHandler._remainingUses.ValueChanged += ChangeNumberOfBullets;
        image.gameObject.SetActive(false);
    }

    private void ChangeNumberOfBullets(int oldVal, int newVal) 
    {
        switch (newVal)
        {
            case 0:
                image.gameObject.SetActive(false);
                break;
            case 1:
                image.gameObject.SetActive(true);
                image.sprite = sprites[0];
                break;

            case 2:
                image.gameObject.SetActive(true);
                image.sprite = sprites[1];
                break;

            case 3:
                image.gameObject.SetActive(true);
                image.sprite = sprites[2];
                break;

            default:
                break;
        }

    }

}
