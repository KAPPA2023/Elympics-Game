using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollOnOff : MonoBehaviour
{
    public CapsuleCollider mainCollider;
    public GameObject thisRig;
    public Animator thisAnimator;

    private Collider[] ragdollColliders;

    private Rigidbody[] limbsRigidbodies;
    // Start is called before the first frame update
    void Start()
    {
        Transform mt = transform;
        Transform root = mt.root;
        Rigidbody rbRoot=root.GetComponent<Rigidbody>();;

        GetRagdollBits();
        ragdollModeOff();
     

    }

    // Update is called once per frame
    void Update()
    {
     
    }

    private void OnCollisionEnter(Collision other)
    {
        throw new NotImplementedException();
    }

    void GetRagdollBits()
    {
        ragdollColliders = thisRig.GetComponentsInChildren<Collider>();
        limbsRigidbodies = thisRig.GetComponentsInChildren<Rigidbody>();
    }
    
    void ragdollModeOn()
    {
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rigid in limbsRigidbodies)
        {
            rigid.isKinematic = false;
        }

        mainCollider.enabled = false;
        thisAnimator.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

    }
    
    void ragdollModeOff()
    {
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rigid in limbsRigidbodies)
        {
            rigid.isKinematic = true;
        }

        
        thisAnimator.enabled = true;
        mainCollider.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;

    }
}
