using System.Collections;
using System;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class SandSpell : Spell
{
    private float bounciness = 0.5f;
    private GameObject Sandstorm;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Sandstorm = new GameObject("SandGranate");
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {

            if (Elympics.IsServer)
            {
                SandStorm ss =  ElympicsInstantiate(Sandstorm.name,ElympicsPlayer.World).GetComponent<SandStorm>();
                ss.SpawnSpell(this.transform.position+ new Vector3(0,0.1f,0),caster);
            }
            DetonateProjectile();
           
        }
        var playerInfo = other.GetComponent<PlayerData>();
        if (playerInfo != null)
        {
            SpellHit?.Invoke();
            playerInfo.DealDamage(spellDamage, caster);
            DetonateProjectile();
        }
        
    }

    protected override void move()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 gravity = Physics.gravity;
        float drag = 0.1f;
        spellVelocity *= (1 - drag * Elympics.TickDuration);
        spellVelocity += gravity * Elympics.TickDuration;
    
        Ray ray = new Ray(rb.position, spellVelocity.normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, spellVelocity.magnitude * Elympics.TickDuration))
        {
            Vector3 reflectDirection = Vector3.Reflect(spellVelocity.normalized, hit.normal);
            spellVelocity = reflectDirection * spellVelocity.magnitude * bounciness;

            rb.position = hit.point + hit.normal * 0.05f;
            rb.velocity = spellVelocity;
        }
        else
        {
            rb.velocity += spellVelocity * Elympics.TickDuration;
        }
    }
}
