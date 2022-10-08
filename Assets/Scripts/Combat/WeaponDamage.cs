using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    private int damage;
    private List<Collider> alreadyCollidedWith = new();

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    public void SetAttack(int damage) => this.damage = damage;

  
    private void OnTriggerEnter(Collider other) 
    {
        if (other == myCollider) { return; }
        
        if (alreadyCollidedWith.Contains(other)) { return; }

        alreadyCollidedWith.Add(other);

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damage);
        }       
    }
}
