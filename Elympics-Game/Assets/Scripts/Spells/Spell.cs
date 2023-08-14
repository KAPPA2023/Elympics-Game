using System;
using UnityEngine;
using Elympics;

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
            ApplyModifier();
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
            if (playerInfo.PlayerId == caster) return;
            SpellHit?.Invoke();
            playerInfo.DealDamage(spellDamage, caster);
        }
        DetonateProjectile();
    }

    public void ElympicsUpdate()
    {
        if(shouldBeDestoyed.Value) ElympicsDestroy(gameObject);
        
        
        deathTimer.Value += Elympics.TickDuration;
        if (deathTimer >= lifeTime)
        {
            DetonateProjectile();
        }
    }
    
    public virtual void ApplyModifier()
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
}
