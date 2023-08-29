using System;
using UnityEngine;
using Elympics;


public enum Spells
{
    Empty = -1,
    Fireball = 0,
    Lightbolt = 1,
    WaterBlast = 2,
    SandGranade = 3,
    Tornado = 4,
    IceSpike = 5
}
public class ActionHandler : ElympicsMonoBehaviour, IUpdatable
{
    [SerializeField] private SpellSpawner spellSpawner;
    [SerializeField] private GameObject viewController;
    [SerializeField] private float spellCooldown = 0.8f;
    public bool modified = false;
    public static event Action OnBadSpell = null;
    public static event Action OnGoodSpell = null;


    protected ElympicsFloat currentTimeBetweenShoots = new ElympicsFloat();
    protected bool canCast => currentTimeBetweenShoots.Value >= spellCooldown;
    
    private Spells _selectedSpell = Spells.Empty;
    public ElympicsInt _remainingUses = new ElympicsInt(0);
    public ElympicsArray<ElympicsInt> stashedSpells = new ElympicsArray<ElympicsInt>(3, () => new ElympicsInt(-1));
    public void HandleActions(bool attack)
    {
        if (attack && canCast)
        {
            CastSpell(viewController.transform.forward);
        }
    }
    
    private void CastSpell(Vector3 direction)
    {
        if (_selectedSpell != Spells.Empty && _remainingUses.Value > 0)
        {
            spellSpawner.TrySpawningSpell(_selectedSpell,direction, GetComponent<PlayerData>().PlayerId, modified);
            _remainingUses.Value--;
        }
        else 
        {
            CastBasicAttack(direction);
        }

        currentTimeBetweenShoots.Value = 0.0f;
    }

    public void CastBasicAttack(Vector3 direction)
    {
        spellSpawner.TrySpawningSpell(Spells.Empty, direction, GetComponent<PlayerData>().PlayerId, false);
    }

    public void ChooseSpell(Spells drawnSpell)
    {
        if (drawnSpell != Spells.Empty)
        {
            for (int i = 0; i < 3; i++)
            {
                if (stashedSpells.Values[i].Value == (int)drawnSpell)
                {
                    OnGoodSpell?.Invoke();
                    _selectedSpell = drawnSpell;
                    stashedSpells.Values[i].Value = (int)Spells.Empty;
                    _remainingUses.Value = GetSpellUses(drawnSpell);
                    break;
                }
                else
                {
                    OnBadSpell?.Invoke();
                }
            }
        }

    }
    public bool AddSpell(Spells spellType)
    {
        Debug.Log(spellType);
        for (int i = 0; i < 3; i++)
        {
            if (stashedSpells.Values[i].Value == (int)Spells.Empty)
            {
                stashedSpells.Values[i].Value = (int)spellType;
                return true;
            }
        }
        return false;
    }

    public void ElympicsUpdate()
    {
        if (!canCast)
        {
            currentTimeBetweenShoots.Value += Elympics.TickDuration;
        }    
    }
    private int GetSpellUses(Spells spell)
    {
        switch (spell)
        {
            case Spells.Empty: return 0;
            case Spells.Fireball: return 3;
            case Spells.Lightbolt: return 2;
            case Spells.SandGranade: return 1;
            case Spells.WaterBlast: return 2;
            case Spells.Tornado: return 2;
            case Spells.IceSpike: return 2;
            default: return 0;
        }
    }

    public void ResetActionHandler()
    {
        _remainingUses.Value = 0;
        for (int i = 0; i < 3; i++)
        {
            stashedSpells.Values[i].Value = (int)Spells.Empty;
        }
    }
}