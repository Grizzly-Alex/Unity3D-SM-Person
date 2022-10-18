using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int health;

    public event Action OnTakeDamage;
    public event Action OnDie;

    private void Start()
    {
        health = maxHealth;
    }

    public void DealDamage(int damage)
    {
        if (health > 0)
        {
            health = Mathf.Max(health - damage, 0);
            OnTakeDamage?.Invoke();
            Debug.Log(health);
        }
        else
        {
            OnDie?.Invoke();
        }  
    }
}
