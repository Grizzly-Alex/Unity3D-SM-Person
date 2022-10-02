using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{

    private List<Target> targets = new();
    public Target CurrentTarget { get; private set; }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Target>(out Target target))
        {
            targets.Add(target);
        }        
    }

    private void OnTriggerExit(Collider other)
    {       
        if(other.TryGetComponent<Target>(out Target target))
        {
            targets.Remove(target);
        }        
    }

    public bool SelectTarget() => targets.Count != 0 ? CurrentTarget = targets[0] : false;

    public void Cancel() => CurrentTarget = null;
}
