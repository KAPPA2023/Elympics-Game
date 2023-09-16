using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpells : MonoBehaviour
{
    [SerializeField] private PlayerProvider playersProvider = null;
    [SerializeField] private Image[] spells;
    [SerializeField] private Sprite[] sprites;
    private bool _modifier;

    private void Start()
    {
        spells[0].sprite = sprites[0];
        spells[1].sprite = sprites[0];
        spells[2].sprite = sprites[0];
        
        if (playersProvider.IsReady)
        {
            SubscribeToActionHandler();
        }
        else
        {
            playersProvider.IsReadyChanged += SubscribeToActionHandler;
        }
    }

    private void SubscribeToActionHandler()
    {
        var clientPlayerData = playersProvider.ClientPlayer;
        clientPlayerData.TheMagician += SetModifier;
        clientPlayerData.GetComponent<ActionHandler>().stashedSpells.Values[0].ValueChanged += ChangeSlot0;
        clientPlayerData.GetComponent<ActionHandler>().stashedSpells.Values[1].ValueChanged += ChangeSlot1;
        clientPlayerData.GetComponent<ActionHandler>().stashedSpells.Values[2].ValueChanged += ChangeSlot2;
    }
    
    private void ChangeSlot0(int oldVal, int newVal) { ChangeSlot(0, newVal); }
    private void ChangeSlot1(int oldVal, int newVal) { ChangeSlot(1, newVal); }
    private void ChangeSlot2(int oldVal, int newVal) { ChangeSlot(2, newVal); }

    private void ChangeSlot(int index, int newVal)
    {
        if (!_modifier)
        {
            spells[index].sprite = sprites[newVal + 1]; 
        }
        else
        {
            if (newVal != -1)
            {
                spells[index].sprite = sprites[7]; 
            }
            else
            {
                spells[index].sprite = sprites[0]; 
            }
            
        }
    }

    private void SetModifier()
    {
        _modifier = true;
    }
}
