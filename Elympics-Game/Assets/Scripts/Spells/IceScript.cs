using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceScript : Spell
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.GetComponent<MovementController>() != null)
        {
            other.GetComponent<MovementController>().Slow(6);
        }
    }
}
