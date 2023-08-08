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
        clientPlayerData.GetComponent<ActionHandler>().stashedSpellChanged += changeSlot;
    }
    private void changeSlot(int index, int newVal)
    {
        switch ((Spells)newVal)
        {
            case Spells.Empty:
                spells[index].sprite = sprites[0]; 
                break;
            case Spells.Fireball:
                spells[index].sprite = sprites[1];
                break;
            case Spells.Lightbolt:
                spells[index].sprite = sprites[2];
                break;
        }
    }
}
