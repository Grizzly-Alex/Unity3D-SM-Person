using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int LokomotionBlendTreeHash = Animator.StringToHash("Lokomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float CrossFadeDuration = 0.1f; 
    private const float AnimatorDampTime = 0.1f; 

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LokomotionBlendTreeHash, CrossFadeDuration);   
        stateMachine.Animator.SetFloat(SpeedHash, 0);       
    }

    public override void Tick(float deltaTime)
    {
        if (!IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }
        else if (IsInAttackRange())
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
        }
        else
        {
            MoveToPlayer(deltaTime);
            FacePlayer();
            stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime); 
        }   
    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    private bool IsInAttackRange()
    {
        if(stateMachine.Player.IsDead) { return false; }
        
        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;    
    }

    private void MoveToPlayer(float deltaTime)
    {
        if (stateMachine.Agent.isOnNavMesh)
        {
            stateMachine.Agent.destination = stateMachine.Player.transform.position;

            Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        }
        
        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }
}