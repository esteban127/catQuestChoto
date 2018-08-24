using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProgression : MonoBehaviour {
    int currentLvl = 0;
    [SerializeField] GameObject xpBar;
    [SerializeField] GameObject lvlDisplay;
    private int experiencie = 0;
    private int nextLevelXp = 100;
    private healthManager myHealthManager;

    public int Lvl {get { return currentLvl;  } }


    private void Start()
    {
        myHealthManager= gameObject.GetComponent<healthManager>();
       
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
        gameObject.GetComponent<soundPlayer>().playSoud(Sounds.LvlUp);
        currentLvl++;
        experiencie -= nextLevelXp;        
        nextLevelXp = (int)(nextLevelXp *1.3f);
        myHealthManager.UpdateMaxHP(myHealthManager.MaxHealth + (int)(10* currentLvl*1.5));
        myHealthManager.Heal((int)Mathf.Round(myHealthManager.MaxHealth * 0.25f));       
        lvlDisplay.GetComponentInChildren<Text>().text = (currentLvl + 1).ToString();

        if (currentLvl >= 19)
            gameObject.GetComponent<GameEnding>().WinEnd();
    }
}
