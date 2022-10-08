using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerFreeLookState : PlayerBaseState
{  
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed"); 
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f; 

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.InputReader.TargetingEvent += OnTarget;
        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();
        if(stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
        }               
        else if(stateMachine.InputReader.MovementValue != Vector2.zero)
        {
            Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);
            FaceMovementDirection(movement, deltaTime);
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
        }
        else
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
        }       
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetingEvent -= OnTarget;
    }

    private void OnTarget()
    {
        if(stateMachine.Targeter.SelectTarget())
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
        
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y + right * stateMachine.InputReader.MovementValue.x;
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDamping);
         
    }

}

