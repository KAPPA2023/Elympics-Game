using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int fireballamount = 0;
    private int lightamount=0;
    private int maxSpellamount = 3;
    // Start is called before the first frame update
    void Start()
    {
        fireballamount = 0;
    }

    // Update is called once per frame
    void Update()
    {
      
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
                    break;
                case 1: fireballamount++;
                    break;

            }
            Debug.Log(fireballamount);
            return true;
        }

        return false;

    }
}
