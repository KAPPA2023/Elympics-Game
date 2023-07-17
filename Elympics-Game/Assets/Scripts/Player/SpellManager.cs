using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int _selectedSpell = -1;
    private int _remainingUses = 0;
    [SerializeField] private GameObject CubePrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setRemainingUses(int amount)
    {
        _remainingUses = amount;
    }
    public void castSpell()
    {
        if (_selectedSpell != -1 && _remainingUses > 0)
        {
            Instantiate(CubePrefab, transform.position + transform.forward, transform.rotation);
            _remainingUses--;
        }
        else
        {
            castBasicAttack();
        }
    }

    public void castBasicAttack()
    {
        
    }

    public void chooseSpell(int drawn_spell)
    {
        if (drawn_spell >= 0)
        {
            
            _selectedSpell = drawn_spell;

        }
    }

    private bool isInInventory(int drawn_spell)
    {
        switch (drawn_spell)
        {
            case 0:
                return this.gameObject.GetComponent<Inventory>().canCastFireball();
                    
                
            case 1:
                 return this.gameObject.GetComponent<Inventory>().canCastLightning();
                     
            default: return false;     
        }

        
    }
}

