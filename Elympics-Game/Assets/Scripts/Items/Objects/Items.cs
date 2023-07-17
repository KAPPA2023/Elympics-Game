using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Items : MonoBehaviour
{
    protected int spellID = -1;
    protected int max_use = 0;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.GetComponent<Inventory>() != null)
        {
            
            if(other.GetComponent<Inventory>().addItem(spellID))
            {gameObject.SetActive(false);}
            
           
        }
        
    }
}
