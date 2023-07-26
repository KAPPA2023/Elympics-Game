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
        spells[1].sprite = sprites[1];
        spells[2].sprite = sprites[2];
    }

    private void Update()
    {
        var clientPlayerData = playersProvider.ClientPlayer;
        var spellsNumbers = clientPlayerData.GetComponent<ActionHandler>().getSpells();

        for (int i = 0; i < spells.Length; i++)
        {
            switch (spellsNumbers[i])
            {
                case "empty":
                    spells[i].sprite = sprites[0]; 
                    break;
                case "fireBall":
                    spells[i].sprite = sprites[1];
                    break;
                case "lightningBolt":
                    spells[i].sprite = sprites[2];
                    break;
            }
        }
    }


}
