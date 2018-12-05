using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterFrame : MonoBehaviour {

    [SerializeField] GameObject playerRef;
    CharacterStats playerStats;
    Clock timer;
    [SerializeField] HealtManaBars healtBar;
    [SerializeField] HealtManaBars manaBar;
    [SerializeField] Transform healtBarTransform;
    [SerializeField] Tooltip toolTip;
    [SerializeField] ActiveBuffDebuff activeBnD;
    [SerializeField] GameObject damageTextPrefab;
    float lastHealt;
    private void Start()
    {
        timer = Clock.Instance;
        LoadSystem.OnEndLoading += EndLoad;
    }

    private void EndLoad()
    {
        playerStats = playerRef.GetComponent<CharacterStats>();
        lastHealt = playerStats.CurrentHealth;
        timer.OnTick += Actualize;
        activeBnD.setStatus(playerStats.status);
    }
    private void OnDisable()
    {
        LoadSystem.OnEndLoading -= EndLoad;
    }
    public void Actualize(float time)
    {
        if (playerStats.CurrentHealth - lastHealt > 1)
            GenerateDamageText((int)(playerStats.CurrentHealth - lastHealt), Color.green);
        else
            if (playerStats.CurrentHealth - lastHealt < -1)
            GenerateDamageText((int)(playerStats.CurrentHealth - lastHealt), Color.red);  
        lastHealt = playerStats.CurrentHealth;
        healtBar.UpdateFillBar(playerStats.CurrentHealth / playerStats.MaxHealth());        
        manaBar.UpdateFillBar(playerStats.CurrentMana / playerStats.MaxMana());        
    }

    private void GenerateDamageText(int amount, Color color)
    {
        string text = "";
        if (amount > 0)
            text += "+";
        text += amount;        
        DamageText dmgText = Instantiate(damageTextPrefab, healtBarTransform).GetComponent<DamageText>();
        dmgText.Create(text, true, color);
    }    

    public void ShowToolTip(barType bar)
    {
        string text = "<b>";
        

        switch (bar)
        {
            case barType.Health:
                text += (int)playerStats.CurrentHealth + " / " + (int)playerStats.MaxHealth();
                break;
            case barType.Mana:
                text += (int)playerStats.CurrentMana + " / " + (int)playerStats.MaxMana();
                break;
        }
        text += "</b>";
        toolTip.ShowToolTip(text);      
    }


    public void HideToolTip()
    {
        toolTip.Hide();
    }

}
