using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerTestState : PlayerBaseState
{
    private float timer;
    
    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enter");
        stateMachine.InputReader.JumpEvent += OnJump;
    }

    public override void Tick(float deltaTime)
    {
        Debug.Log($"Tick {timer}");
        timer += deltaTime;
       
    }

    public override void Exit()
    {
        Debug.Log("Exut");
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerTestState(stateMachine));
    }
}

