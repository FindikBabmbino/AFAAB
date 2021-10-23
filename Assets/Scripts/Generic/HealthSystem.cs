using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]private float MaxHealth=100.0f;

    [SerializeField] private float currentHealth=0.0f;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth=MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth<=0)
        {
            CombatEventSystemManager.instance.RemoveEnemiesFromTheList(this.gameObject);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if(currentHealth<=0)
        {
            return;
        }
        currentHealth-=damageAmount;
    }

    public void Heal(float healAmount)
    {
        if (currentHealth>=MaxHealth)
        {
            return;
        }
        currentHealth+=healAmount;
    }
}
