using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPickup : MonoBehaviour
{
    [SerializeField] protected int spellID = -1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ActionHandler>() != null)
        {
            if(other.GetComponent<ActionHandler>().addSpell(spellID))
            {gameObject.SetActive(false);}
        }
    }
}
