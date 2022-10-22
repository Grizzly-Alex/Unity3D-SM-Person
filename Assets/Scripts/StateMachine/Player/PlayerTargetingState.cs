using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private Vector2 dodgingDirectionInput;
    private float remainingDodgeTime;
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");
    private const float CrossFadeDuration = 0.1f; 

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.InputReader.TargetingEvent += OnCancle;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
        }
        else if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
        }
        else if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
        else
        {
            Vector3 movement = CalculateMovement(deltaTime);
            Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);
            UpdateAnimator(deltaTime);
            FaceTarget();
        }
    }

    public override void Exit()
    {        
        stateMachine.InputReader.TargetingEvent -= OnCancle;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
    }

    private void OnDodge()
    {
        if (Time.time - stateMachine.PreviousDodgeTime < stateMachine.DodgeCooldown) { return; }

        stateMachine.SetDodgeTime(Time.time);
        dodgingDirectionInput = stateMachine.InputReader.MovementValue;
        remainingDodgeTime = stateMachine.DodgeDuration;
    }

    private void OnCancle()
    {
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private Vector3 CalculateMovement(float deltaTime)
    {
        Vector3 movement = new Vector3();

        if (remainingDodgeTime > 0f)
        {
            movement += stateMachine.transform.right * dodgingDirectionInput.x * stateMachine.DodgeLength  /
            stateMachine.DodgeDuration;
            movement += stateMachine.transform.forward * dodgingDirectionInput.y * stateMachine.DodgeLength  /
            stateMachine.DodgeDuration;

            remainingDodgeTime = Mathf.Max(remainingDodgeTime - deltaTime, 0f);
        }
        else
        {
            movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
            movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;
        }

        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        if (stateMachine.InputReader.MovementValue.y == 0)
        {
            stateMachine.Animator.SetFloat(TargetingForwardHash, 0, 0.1f, deltaTime);
        }
        else
        {
            float value = stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingForwardHash, value, 0.1f, deltaTime);
        }

        if (stateMachine.InputReader.MovementValue.x == 0)
        {
            stateMachine.Animator.SetFloat(TargetingRightHash, 0,  0.1f, deltaTime);
        }
        else
        {
            float value = stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingRightHash, value,  0.1f, deltaTime);
        }       
    }
}
