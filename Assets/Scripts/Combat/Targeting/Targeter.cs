using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cineTargetGroup;

    private List<Target> targets = new();
    public Target CurrentTarget { get; private set; }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Target>(out Target target))
        {
            targets.Add(target);
            target.OnDestroyed += RemoveTarget;
        }        
    }

    private void OnTriggerExit(Collider other)
    {       
        if(other.TryGetComponent<Target>(out Target target))
        {
            RemoveTarget(target);
        }        
    }

    public bool SelectTarget()
    {
        if(targets.Count != 0)
        {
            CurrentTarget = targets[0];
            cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

            return true;
        }
        else
        {
            return false;
        }
        
    }

    public void Cancel()
    {
        if(CurrentTarget != null)
        {
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

    } 

    private void RemoveTarget(Target target)
    {
        if(CurrentTarget == target)
        {
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }
}
