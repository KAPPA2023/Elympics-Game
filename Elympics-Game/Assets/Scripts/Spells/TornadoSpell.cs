using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoSpell : Spell
{
    public float bulletSpeed = 10f;
    public float knockbackForce = 20f;

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


            playerRigidbody.AddForce(knockbackDirection * 15, ForceMode.Impulse);
        }
    }
}