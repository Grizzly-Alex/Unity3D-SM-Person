using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cineTargetGroup;

    private Camera mainCamera;

    private List<Target> targets = new();
    public Target CurrentTarget { get; private set; }

    private void Start()
    {
        mainCamera = Camera.main;
    }
    
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
            Target closesTarget = null;
            float closestTargetDistance = Mathf.Infinity;



            foreach (Target target in targets)
            {
                Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

                if(target.GetComponentInChildren<Renderer>().isVisible)
                {
                    Vector2 toCenter = viewPos = new Vector2(0.5f, 0.5f);

                    if(toCenter.sqrMagnitude < closestTargetDistance)
                    {
                        closesTarget = target;
                        closestTargetDistance = toCenter.sqrMagnitude;
                    }
                }              
            }

            if(closesTarget == null)            
            { 
                return false;
            }
            else
            {
                CurrentTarget = closesTarget;
                cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

                return true;
            }            
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
