using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class TornadoSpell : Spell
{
    [SerializeField] float knockbackForce = 5f;

    // Start is called before the first frame update
    protected override void OnTriggerEnter(Collider other)
    {
        var playerData = other.GetComponent<PlayerData>();
        if(playerData == null) return;
        if (playerData.PlayerId == caster) return;
        base.OnTriggerEnter(other);
        var playerRigidbody = other.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {

            Vector3 knockbackDirection = spellVelocity;
            knockbackDirection.y = 0.1f;
            
            playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
    }

    public override void ApplyModifier()
    {
        base.ApplyModifier();
        knockbackForce *= 3f;
    }
}