using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Items : MonoBehaviour
{
    protected int spellID = -1;
    protected static int max_use;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.GetComponent<Inventory>() != null)
        {
            
            if(other.GetComponent<Inventory>().addItem(spellID))
            {gameObject.SetActive(false);}
            
           
        }
        
    }

   
}
