using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int fireballamount = 0;
    private int lightamount=0;
    // Start is called before the first frame update
    void Start()
    {
        fireballamount = 0;
    }
    
    public bool canCastFireball()
    {
        if (fireballamount > 0)
        {
            fireballamount--;
            return true;
        }

        return false;
    }
    public bool canCastLightning()
    {
        if (lightamount > 0)
        {
            lightamount--;
            return true;
        }

        return false;
    }
    private bool canAddSpell()
    {
        if (fireballamount + lightamount < 3) return true;
        return false;
    }
    public bool addItem(int spellID)
    {
        if (canAddSpell())
        {
            switch (spellID)
            {
                case 0: fireballamount++;
                    Debug.Log(fireballamount);
                    break;
                case 1: lightamount++;
                    Debug.Log(lightamount);
                    break;

            }
            
            return true;
        }

        return false;

    }
}
