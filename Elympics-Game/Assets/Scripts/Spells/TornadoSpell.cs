using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoSpell : Spell
{
    public float bulletSpeed = 10f;
    public float knockbackForce = 5f;

    private Rigidbody rb;

    // Start is called before the first frame update
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        var playerRigidbody = other.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            // Oblicz wektor odrzutu
            Vector3 knockbackDirection = -transform.forward;

            // Dodaj siłę odrzutu do gracza
            playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }

    }
}