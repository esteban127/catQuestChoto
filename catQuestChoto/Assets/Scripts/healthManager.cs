using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthManager : MonoBehaviour {
    
    [SerializeField] int maxHealth;
    [SerializeField] GameObject hpBar;
    [SerializeField] bool isPlayer;
    int currentHealth;
    bool alive = true;
    public int MaxHealth { get { return maxHealth; } }
    public int CurrentHealth { get { return currentHealth; } }


    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Heal(int healAmount)   
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        hpBar.GetComponent<ProgressionBar>().SetProgression((float)currentHealth / (float)maxHealth);
        hpBar.GetComponent<ProgressionBar>().SetText(currentHealth.ToString() + " / " + maxHealth.ToString());
    }
    public void Death()    
    {
        gameObject.GetComponent<soundPlayer>().playSoud(Sounds.Die);
        if (isPlayer)
            gameObject.GetComponent<GameEnding>().DeadEnd();
        
        alive = false;
    }

    public void getDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            hpBar.SetActive(false);
            Death();
        }
        else
        {
            gameObject.GetComponent<soundPlayer>().playSoud(Sounds.Dmg);
            hpBar.GetComponent<ProgressionBar>().SetProgression((float)currentHealth / (float)maxHealth);
            hpBar.GetComponent<ProgressionBar>().SetText(currentHealth.ToString() + " / " + maxHealth.ToString());
        }
    }
	public void Revive()
    {
        
        currentHealth = maxHealth;
        alive = true;
    }

    public void UpdateMaxHP(int newMaxHP)
    {
        if (newMaxHP > 0)
            maxHealth = newMaxHP;
    }

    public bool isAlive()
    {
        return alive;
    }
}
