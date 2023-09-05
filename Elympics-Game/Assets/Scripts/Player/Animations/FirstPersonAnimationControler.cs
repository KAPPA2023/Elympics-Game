using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonAnimationControler : MonoBehaviour
{
   [SerializeField] private MovementController movementController;
   [SerializeField] private DeathController deathController;
   [SerializeField] private ActionHandler actionController;
   
   private readonly int movementForwardParameterHash = Animator.StringToHash("VelocityZ");
   private readonly int movementRightParameterHash = Animator.StringToHash("VelocityX");
   private readonly int movementUpParameterHash = Animator.StringToHash("VelocityY");
   private readonly int castTriggerParameterHash = Animator.StringToHash("CastTrigger");
   private readonly int resetTriggerParameterHash = Animator.StringToHash("ResetTrigger");
   private readonly int climbingTriggerParameterHash = Animator.StringToHash("ClimbTrigger");
   private readonly int isClimbing = Animator.StringToHash("IsClimbing");
   private readonly int isGroundedParameterHash = Animator.StringToHash("IsGrounded");
   

   private Animator thirdPersonAnimator = null;
   private void Awake()
   {
      thirdPersonAnimator = GetComponent<Animator>();
      movementController.MovementValuesChanged += ProcessMovementValues;
      deathController.IsDead.ValueChanged += ProcessDeathState;
      actionController.SpellCasted += castState;
   }

   private void castState()
   {
      thirdPersonAnimator.SetTrigger(castTriggerParameterHash);
   }
   private void ProcessDeathState(bool lastValue, bool newValue)
   {
      if (newValue)
      {
         
      }
      else
      {
         thirdPersonAnimator.SetTrigger(resetTriggerParameterHash);
      }
   }

   private void ProcessMovementValues(MovementState state,Vector3 movementDirection)
   {
      var localMovementDirection = movementController.transform.InverseTransformDirection(movementDirection);
      
     
      
      switch (state)
      {
         case MovementState.walking:
            thirdPersonAnimator.SetBool(isClimbing, false);
            thirdPersonAnimator.SetFloat(movementForwardParameterHash,  localMovementDirection.z );
            thirdPersonAnimator.SetFloat(movementRightParameterHash,  localMovementDirection.x );
            break;
         case MovementState.climbing:
               thirdPersonAnimator.SetTrigger(climbingTriggerParameterHash);
               thirdPersonAnimator.SetFloat(movementUpParameterHash, localMovementDirection.normalized.y);
               thirdPersonAnimator.SetBool(isClimbing, true); 
            
  
            break;
         case MovementState.air: 
            thirdPersonAnimator.SetBool(isClimbing, false);

            break;
            
         default:
            throw new ArgumentOutOfRangeException(nameof(state), state, null);
      }
      
   }
}
