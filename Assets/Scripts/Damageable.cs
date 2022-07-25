using UnityEngine;
using UnityEngine.Events;

public class Damageable
{
    [SerializeField] private int health = 5;
    [SerializeField] private int maxHealth = 5;

    public UnityEvent<int> onHealthChanged;
    public UnityEvent<int> onMaxHealthChanged;
    public UnityEvent onDestroy;

    public int Health 
    {
        get => health;
        set 
        {
            health = value;

            if (health <= 0) 
                onDestroy?.Invoke();
            else 
                onHealthChanged?.Invoke(health);
        } 
    }
    
    public int MaxHealth 
    { 
        get => maxHealth; 
        set => maxHealth = value; 
    }

    public void SetDamage(int value) => Health -= value;
}