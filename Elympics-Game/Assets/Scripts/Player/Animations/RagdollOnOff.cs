using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class RagdollOnOff : MonoBehaviour
{
    public CapsuleCollider mainCollider;
    public GameObject thisRig;
    public Animator thisAnimator = null;
    private Collider[] ragdollColliders;
    //private readonly int resetTriggerParameterHash = Animator.StringToHash("ResetTrigger");
    private Rigidbody[] limbsRigidbodies;
    [SerializeField] private DeathController deathController;
    Rigidbody rbRoot;
    private Rigidbody[] limbsRigidbodiescopy;
    private Collider[] ragdollColliderscopy;
    
    // Start is called before the first frame update
    void Start()
    {
        
        GetRagdollBits();
        limbsRigidbodiescopy = limbsRigidbodies;
        ragdollColliderscopy = ragdollColliders;
        ragdollModeOff();

    }
    
    
    private void Awake()
    {
        Transform mt = transform;
        Transform root = mt.root;
        rbRoot=root.GetComponent<Rigidbody>();

        GetRagdollBits();
        limbsRigidbodiescopy = limbsRigidbodies;
        ragdollColliderscopy = ragdollColliders;
        ragdollModeOff();
        thisAnimator = GetComponent<Animator>();
        deathController.IsDead.ValueChanged += ProcessDeathState;
    }
    private void ProcessDeathState(bool lastValue, bool newValue)
    {
        if (newValue)
        {
            ragdollModeOn();
        }
        else
        {
            ragdollModeOff();
        }
    }
   
    // Update is called once per frame
    void Update()
    {
     
    }

    private void OnCollisionEnter(Collision other)
    {
       
    }

    void GetRagdollBits()
    {
        ragdollColliders = thisRig.GetComponentsInChildren<Collider>();
        limbsRigidbodies = thisRig.GetComponentsInChildren<Rigidbody>();
    }
    
  public  void ragdollModeOn()
    {
        thisAnimator.enabled = false;
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rigid in limbsRigidbodies)
        {
            rigid.isKinematic = false;
        }

        mainCollider.enabled = false;
        rbRoot.isKinematic = true;

    }
    
   public void ragdollModeOff()
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
        Transform mt = transform;
        Transform root = mt.root;
        Rigidbody rbRoot=root.GetComponent<Rigidbody>();
        rbRoot.isKinematic = false;
        for(int i=0; i< limbsRigidbodiescopy.Length;i++)
        {
            limbsRigidbodies[i].rotation = limbsRigidbodiescopy[i].rotation;
            limbsRigidbodies[i].position = limbsRigidbodiescopy[i].position;
            limbsRigidbodies[i].velocity = limbsRigidbodiescopy[i].velocity;
        }
       
       
        // thisAnimator.ResetTrigger(resetTriggerParameterHash);
        

    }
}
