using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProgression : MonoBehaviour {
    int currentLvl = 18;
    [SerializeField] GameObject xpBar;
    [SerializeField] GameObject lvlDisplay;
    private int experiencie = 0;
    private int nextLevelXp = 100;
    private healthManager myHealthManager;
    private Weapon myWeapon;

    public int Lvl {get { return currentLvl;  } }


    private void Start()
    {
        myHealthManager= gameObject.GetComponent<healthManager>();
        myWeapon = gameObject.GetComponentInChildren<Weapon>();
    }

  

    public void GainXp(int xp)
    {
        experiencie += xp;
        if (experiencie >= nextLevelXp)
            LvlUp();

        xpBar.GetComponent<ProgressionBar>().SetProgression((float)experiencie / (float)nextLevelXp);
        xpBar.GetComponent<ProgressionBar>().SetText(Mathf.Round(((float)experiencie / (float)nextLevelXp) * 100).ToString() + "%");
    }

    private void LvlUp()
    {
        currentLvl++;
        experiencie -= nextLevelXp;        
        nextLevelXp = (int)(nextLevelXp *1.6f);
        myHealthManager.UpdateMaxHP(myHealthManager.HP + (int)(10* currentLvl*1.5));
        myHealthManager.Heal((int)Mathf.Round(myHealthManager.HP * 0.5f));
        myWeapon.SetDamage(myWeapon.Damage + (1.8f * currentLvl));
        lvlDisplay.GetComponentInChildren<Text>().text = (currentLvl + 1).ToString();

        if (currentLvl >= 19)
            gameObject.GetComponent<GameEnding>().WinEnd();
    }
}
