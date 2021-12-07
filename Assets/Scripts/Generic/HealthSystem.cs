using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]private float MaxHealth=100.0f;

    [SerializeField]private float currentHealth=0.0f;
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
        //If the current health ever goes above maxhealth just reset it back to the max health
        if(currentHealth>MaxHealth)
        {
            currentHealth=MaxHealth;
        }

        //fraction of both the values gives us the desired amount since it is 0 to 1
        if (UIManager.instance != null)
        {
            UIManager.instance.ModifyHealthAmount(currentHealth / MaxHealth);
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

    //These will return the current and the max health
    public float ReturnCurrentHealth()
    {
        return currentHealth;
    }
    public float ReturnMaxHealth()
    {
        return MaxHealth;
    }
}
