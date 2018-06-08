using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthManager : MonoBehaviour {
    
    [SerializeField] int HealthPoints;
    [SerializeField] GameObject hpBar;
    int currentHealth;
    bool alive = true;
    public int HP {get { return HealthPoints; } }


    private void Start()
    {
        currentHealth = HealthPoints;
    }

    public void Heal(int healAmount)   
    {
        currentHealth += healAmount;
        if (currentHealth > HealthPoints)
            currentHealth = HealthPoints;

        hpBar.GetComponent<ProgressionBar>().SetProgression((float)currentHealth / (float)HealthPoints);
        hpBar.GetComponent<ProgressionBar>().SetText(currentHealth.ToString() + " / " + HealthPoints.ToString());
    }
    public void Death()    
    {
        Debug.Log(gameObject.name + ": Estoy murido");
        alive = false;
    }

    public void getDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            hpBar.SetActive(false);
            alive = false;
        }
        else
        {
            hpBar.GetComponent<ProgressionBar>().SetProgression((float)currentHealth / (float)HealthPoints);
            hpBar.GetComponent<ProgressionBar>().SetText(currentHealth.ToString() + " / " + HealthPoints.ToString());
        }
    }
	public void Revive()
    {
        
        currentHealth = HealthPoints;
        alive = true;
    }

    public void UpdateMaxHP(int newMaxHP)
    {
        if (newMaxHP > 0)
            HealthPoints = newMaxHP;
    }

    public bool isAlive()
    {
        return alive;
    }
}
