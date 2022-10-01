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
        Vector3 movement = new Vector3();

        movement.x = stateMachine.InputReader.MovementValue.x;
        movement.y = 0;
        movement.z = stateMachine.InputReader.MovementValue.y;
                
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
}

