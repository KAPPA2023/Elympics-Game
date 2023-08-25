using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonAnimatorController : MonoBehaviour
{
   [SerializeField] private MovementController movementController;
   [SerializeField] private DeathController deathController;
   
   private readonly int movementForwardParameterHash = Animator.StringToHash("MovementForward");
   private readonly int deathTriggerParameterHash = Animator.StringToHash("DeathTrigger");
   private readonly int resetTriggerParameterHash = Animator.StringToHash("ResetTrigger");
   private readonly int jumpingTriggerParameterHash = Animator.StringToHash("JumpTrigger");
   private readonly int isGroundedParameterHash = Animator.StringToHash("IsGrounded");

   private Animator thirdPersonAnimator = null;
   private void Awake()
   {
      thirdPersonAnimator = GetComponent<Animator>();
      movementController.MovementValuesChanged += ProcessMovementValues;
      movementController.IsJumping.ValueChanged += ProcessJumping;
      deathController.IsDead.ValueChanged += ProcessDeathState;
   }
   
   private void ProcessDeathState(bool lastValue, bool newValue)
   {
      if (newValue)
      {
         thirdPersonAnimator.SetTrigger(deathTriggerParameterHash);
      }
      else
      {
         thirdPersonAnimator.SetTrigger(resetTriggerParameterHash);
      }
   }

   private void ProcessJumping(bool oldVal, bool newVal)
   {
      if (newVal)
      {
         thirdPersonAnimator.SetTrigger(jumpingTriggerParameterHash);
         thirdPersonAnimator.SetBool(isGroundedParameterHash, false);
      }
      else
      {
         thirdPersonAnimator.SetBool(isGroundedParameterHash, true);
      }
      
   }

   private void ProcessMovementValues(Vector3 movementDirection)
   {
      var localMovementDirection = movementDirection.magnitude;
      thirdPersonAnimator.SetFloat(movementForwardParameterHash, localMovementDirection);
   }
}
