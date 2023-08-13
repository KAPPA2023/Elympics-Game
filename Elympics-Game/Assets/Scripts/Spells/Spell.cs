using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using TMPro;
using UnityEditor.Experimental.GraphView;

public abstract class Spell : ElympicsMonoBehaviour, IUpdatable
{
    [SerializeField] protected float spellDamage;
    [SerializeField] protected float spellSpeed;
    [SerializeField] protected float lifeTime;

    private ElympicsFloat deathTimer = new ElympicsFloat(0.0f);
    protected Vector3 spellVelocity;
    protected int caster;
    private ElympicsBool shouldBeDestoyed = new ElympicsBool();
    public Action SpellHit = null;
    //if spell exploded on collision or smth
    //[SerializeField] private float spellRange;
    protected Rigidbody rb;

    public virtual void SpawnSpell(Vector3 position, Vector3 direction, int casterID, bool modified)
    {
        rb = GetComponent<Rigidbody>();
        rb.position = position;
        spellVelocity = direction.normalized * spellSpeed;
        caster = casterID;
        rb.velocity = spellVelocity;
        if (modified)
        {
            applyModifier();
        }
        
    }

    public void SetSpellHitCallback(Action spellHit)
    {
        SpellHit = spellHit;
    }

    protected virtual void OnTriggerEnter(Collider other)
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
        move();
        
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

    protected void DetonateProjectile()
    {
        SpawnChild();
        shouldBeDestoyed.Value = true;
    }

    protected virtual void SpawnChild()
    {
        
    }
    protected virtual void move()
    {
    }
    
}
