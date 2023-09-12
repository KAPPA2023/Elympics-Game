using Elympics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : ElympicsMonoBehaviour
{
    MovementController mc;
    public float jumpForce;
    [SerializeField] private AudioSource _audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mc = other.GetComponent<MovementController>();
            mc.OffDoubleJump();
            mc.rb.velocity = new Vector3(mc.rb.velocity.x, 0, mc.rb.velocity.z);
            mc.rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
    }

}
