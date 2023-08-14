using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpells : MonoBehaviour
{
    [SerializeField] private PlayerProvider playersProvider = null;
    [SerializeField] private Image[] spells;
    [SerializeField] private Sprite[] sprites;

    private void Start()
    {
        spells[0].sprite = sprites[0];
        spells[1].sprite = sprites[0];
        spells[2].sprite = sprites[0];
        var clientPlayerData = playersProvider.ClientPlayer;
        
        clientPlayerData.GetComponent<ActionHandler>().stashedSpells.Values[0].ValueChanged += ChangeSlot0;
        clientPlayerData.GetComponent<ActionHandler>().stashedSpells.Values[1].ValueChanged += ChangeSlot1;
        clientPlayerData.GetComponent<ActionHandler>().stashedSpells.Values[2].ValueChanged += ChangeSlot2;
        
    }
    
    private void ChangeSlot0(int oldVal, int newVal) { ChangeSlot(0, newVal); }
    private void ChangeSlot1(int oldVal, int newVal) { ChangeSlot(1, newVal); }
    private void ChangeSlot2(int oldVal, int newVal) { ChangeSlot(2, newVal); }

    private void ChangeSlot(int index, int newVal)
    {
        //This will work as long as you setup sprites in order of spells in enum
        spells[index].sprite = sprites[newVal + 1]; 
        // switch ((Spells)newVal)
        // {
        //     case Spells.Empty:
        //         spells[index].sprite = sprites[0]; 
        //         break;
        //     case Spells.Fireball:
        //         spells[index].sprite = sprites[1];
        //         break;
        //     case Spells.Lightbolt:
        //         spells[index].sprite = sprites[2];
        //         break;
        //     case Spells.WaterBlast:
        //         spells[index].sprite = sprites[3];
        //         break;
        //     case Spells.SandGranade:
        //         spells[index].sprite = sprites[4];
        //         break;
        //     case Spells.Tornado:
        //         spells[index].sprite = sprites[5];
        //         break;
        //     case Spells.IceSpike:
        //         spells[index].sprite = sprites[6];
        //         break;
        // }
    }
}
