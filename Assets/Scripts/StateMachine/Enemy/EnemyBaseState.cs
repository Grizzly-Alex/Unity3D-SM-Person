using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : IState
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Tick(float deltaTime);
}
