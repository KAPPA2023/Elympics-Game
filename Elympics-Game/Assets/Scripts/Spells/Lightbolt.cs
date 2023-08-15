using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightbolt : Spell
{
    [SerializeField] private int bounces;
    [SerializeField] private float Radius;
    private void OnCollisionEnter(Collision other)
    {
        
        bounces--;
        if (bounces <= 0)
        {
            DetonateProjectile();
        }
        else
        {
            var playerInfo = other.gameObject.GetComponent<PlayerData>();
            if (playerInfo != null) {
                SpellHit?.Invoke();
                playerInfo.DealDamage(spellDamage, caster); 
                DetonateProjectile();
            }
            else
            {
                Collider[] objectsInRange = Physics.OverlapSphere(transform.position, Radius);
                foreach (var obj in objectsInRange)
                {
                    var playerData = obj.GetComponent<PlayerData>();
                    if (playerData != null)
                    {
                        if (playerData.PlayerId != caster)
                        {
                            if(rb == null) return;
                            float speed = rb.velocity.magnitude;
                            Vector3 direction = (playerData.GetComponent<Rigidbody>().position-transform.position).normalized;
                            RaycastHit hit;
                            if(Physics.Raycast(transform.position,direction,out hit,Radius))
                            {
                                if (hit.collider.gameObject == playerData.gameObject)
                                {
                                    rb.velocity = direction * speed; 
                                }
                                
                            }
                        }
                    }
                }
               
            }
        }
            
            
       
        
        
    }
}
