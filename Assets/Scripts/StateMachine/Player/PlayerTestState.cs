using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerTestState : PlayerBaseState
{   
    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enter");

    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();
               
        if(stateMachine.InputReader.MovementValue != Vector2.zero)
        {
           stateMachine.Controller.Move(movement * stateMachine.FreeLookMovementSpeed * deltaTime); 
           stateMachine.transform.rotation = Quaternion.LookRotation(movement);
           stateMachine.Animator.SetFloat("FreeLookSpeed", 1, 0.1f, deltaTime);
        }
        else
        {
            stateMachine.Animator.SetFloat("FreeLookSpeed", 0, 0.1f, deltaTime);
        }       
    }

    public override void Exit()
    {
        Debug.Log("Exit");
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
}

