using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public abstract class Spell : ElympicsMonoBehaviour, IUpdatable
{
    [SerializeField] protected float spellDamage;
    [SerializeField] protected float spellSpeed;
    [SerializeField] protected float lifeTime;

    private ElympicsFloat deathTimer = new ElympicsFloat(0.0f);
    private Vector3 spellVelocity;
    private int caster;
    private ElympicsBool shouldBeDestoyed = new ElympicsBool();
    public Action SpellHit = null;
    //if spell exploded on collision or smth
    //[SerializeField] private float spellRange;

    public void SpawnSpell(Vector3 position, Vector3 direction, int casterID, bool modified)
    {
        //we can use tick to setup timers - for example fireball could explode after 2 seconds in air
        transform.position = position;
        spellVelocity = direction.normalized * spellSpeed;
        caster = casterID;

        if (modified)
        {
            applyModifier();
        }
    }

    public void SetSpellHitCallback(Action spellHit)
    {
        SpellHit = spellHit;
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerInfo = other.GetComponent<PlayerData>();
        if (playerInfo != null)
        {
            SpellHit?.Invoke();
            playerInfo.DealDamage(spellDamage, caster);
        }
        DetonateProjectile();
    }

    public void ElympicsUpdate()
    {
        if(shouldBeDestoyed.Value) ElympicsDestroy(gameObject);
        transform.position += spellVelocity;
        
        deathTimer.Value += Elympics.TickDuration;
        if (deathTimer >= lifeTime)
        {
            DetonateProjectile();
        }
    }
    
    public virtual void applyModifier()
    {
        Debug.Log("spell modified");
    } 

    private void DetonateProjectile()
    {
        shouldBeDestoyed.Value = true;
    }
    
}
