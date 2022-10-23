using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbingState : PlayerBaseState
{
    private readonly int ClimbingHash = Animator.StringToHash("Climbing");
    private readonly Vector3 Offset = new Vector3(0f, 2.325f, 0.65f);
    private const float CrossFadeDuration = 0.1f;

    public PlayerClimbingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ClimbingHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(GetNormalizedTime(stateMachine.Animator, "Climbing") < 1f) { return; }

        stateMachine.Controller.enabled = false;
        stateMachine.transform.Translate(Offset, Space.Self);
        stateMachine.Controller.enabled = true;

        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine, false));
    }

    public override void Exit()
    {
        stateMachine.Controller.Move(Vector3.zero);
        stateMachine.ForceReceiver.Reset(); 
    }
}