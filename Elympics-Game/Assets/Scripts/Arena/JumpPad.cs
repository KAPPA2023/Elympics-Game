using Elympics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : ElympicsMonoBehaviour
{
    Rigidbody rb;
    public float jumpForce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rb = other.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

}
