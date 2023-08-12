using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoSpell : Spell
{
    public float bulletSpeed = 10f;
    public float knockbackForce = 20f;

    private Rigidbody rb;

    // Start is called before the first frame update
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        var playerRigidbody = other.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            // Oblicz wektor odrzutu
            Vector3 knockbackDirection = spellVelocity;
            knockbackDirection.y = 0.1f;

            // Dodaj siłę odrzutu do gracza
            playerRigidbody.AddForce(knockbackDirection * 25, ForceMode.Impulse);
        }

    }
}