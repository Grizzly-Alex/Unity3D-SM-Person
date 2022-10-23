using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int LokomotionBlendTreeHash = Animator.StringToHash("Lokomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float CrossFadeDuration = 0.1f; 
    private const float AnimatorDampTime = 0.1f; 

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LokomotionBlendTreeHash, CrossFadeDuration);   
        stateMachine.Animator.SetFloat(SpeedHash, 0);    
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);


        if(IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }
        else
        {
            FacePlayer();
            
            stateMachine.Animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, deltaTime); 
        }             
    }

    public override void Exit()
    {
        
    }
}