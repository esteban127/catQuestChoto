using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsFrameManager : MonoBehaviour {

    [SerializeField] Text strength;
    [SerializeField] Text consitution;
    [SerializeField] Text dextery;
    [SerializeField] Text inteligence;
    [SerializeField] Text luck;

    [SerializeField] Text attributePoints;
    [SerializeField] Text actualXp;
    [SerializeField] Text nextLevelXp;
    [SerializeField] Text extraStats;
    [SerializeField] Text characterName;
    [SerializeField] Text characterLevel;

    [SerializeField] Button addStrengButton;
    [SerializeField] Button addConsitutionButton;
    [SerializeField] Button addDexteryButton;
    [SerializeField] Button addInteligenceButton;
    [SerializeField] Button addLuckButton;
    public void SetAttribute(int playerAmount, int equipmentAmouint, attribute stat)
    {

        string value = "";

        if (equipmentAmouint > 0)
        {
            value += "<color=orange>" + (playerAmount + equipmentAmouint) + "</color>";
        }
        else
        {
            value += playerAmount;
        }

        switch (stat)
        {
            case attribute.Strength:
                strength.text = value;
                break;
            case attribute.Constitution:
                consitution.text = value;
                break;
            case attribute.Dextery:
                dextery.text = value;
                break;
            case attribute.Inteligence:
                inteligence.text = value;
                break;
            case attribute.Luck:
                luck.text = value;
                break;
        }        
    }

    public void setExtraStats(string exStats)
    {
        extraStats.text = exStats;
    }

    public void setXp(int actlXp, int nextXp)
    {
        actualXp.text = "" + actlXp;
        nextLevelXp.text = "" + nextXp;
    }
    public void setLevel(int level)
    {
        characterLevel.text = "" + level;
    }
    public void setName(string name)
    {
        characterName.text = name;
    }

    public void setAttributePoints(int amount)
    {
        attributePoints.text = "" + amount;
        if (amount > 0)
        {
            addStrengButton.interactable = true;
            addConsitutionButton.interactable = true;
            addDexteryButton.interactable = true;
            addInteligenceButton.interactable = true;
            addLuckButton.interactable = true;
        }
        else
        {
            addStrengButton.interactable = false;
            addConsitutionButton.interactable = false;
            addDexteryButton.interactable = false;
            addInteligenceButton.interactable = false;
            addLuckButton.interactable = false;
        }
        
    }

}
