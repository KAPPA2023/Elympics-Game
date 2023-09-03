using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonAnimatorController : MonoBehaviour
{
   [SerializeField] private MovementController movementController;
   [SerializeField] private DeathController deathController;
   
   private readonly int movementForwardParameterHash = Animator.StringToHash("MovementForward");
   private readonly int movementRightParameterHash = Animator.StringToHash("MovementRight");
   private readonly int deathTriggerParameterHash = Animator.StringToHash("DeathTrigger");
   private readonly int resetTriggerParameterHash = Animator.StringToHash("ResetTrigger");
   private readonly int jumpingTriggerParameterHash = Animator.StringToHash("JumpTrigger");
   private readonly int isGroundedParameterHash = Animator.StringToHash("IsGrounded");

   private Animator thirdPersonAnimator = null;
   private void Awake()
   {
      thirdPersonAnimator = GetComponent<Animator>();
      movementController.MovementValuesChanged += ProcessMovementValues;
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

   private void ProcessMovementValues(MovementState state,Vector3 movementDirection)
   {
      var localMovementDirection = movementController.transform.InverseTransformDirection(movementDirection);
      if (state != MovementState.air)
      {
         if (!thirdPersonAnimator.GetBool(isGroundedParameterHash))
         {
            thirdPersonAnimator.SetBool(isGroundedParameterHash, true);
         }
      }
      
      switch (state)
      {
         case MovementState.walking: 
            
            Debug.Log(localMovementDirection);
            thirdPersonAnimator.SetFloat(movementForwardParameterHash, localMovementDirection.z);
            thirdPersonAnimator.SetFloat(movementRightParameterHash, localMovementDirection.x);
            break;
         case MovementState.climbing: break;
         case MovementState.air: 
            thirdPersonAnimator.SetTrigger(jumpingTriggerParameterHash);
            thirdPersonAnimator.SetBool(isGroundedParameterHash, false); 
            break;
         default:
            throw new ArgumentOutOfRangeException(nameof(state), state, null);
      }
      
   }
}
