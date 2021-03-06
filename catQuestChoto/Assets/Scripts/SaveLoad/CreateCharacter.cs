﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CreateCharacter : MonoBehaviour {

    c_class cClass;
    SaveLoad sLManager;
    bool classSelected = false;
    [SerializeField] ErroPanel errorWindow;
    string characterName = "";
    CharacterActor[] existentCharacters;
    private void Start()
    {
        sLManager = SaveLoad.Instance;        
    }

    public void classSelect(string newClass)
    {
        switch (newClass)
        {
            case "Warrior":
                cClass = c_class.Warrior;
                classSelected = true;
                break;
            case "Mage":
                cClass = c_class.Mage;
                classSelected = true;
                break;
            case "Ranger":
                cClass = c_class.Ranger;
                classSelected = true;
                break;
        }        
    }
    public void setCharacterName(string newName)
    {
        characterName = newName;
    }

    public void TryToConfirm()
    {
        existentCharacters = sLManager.getAllCharacters();
        bool valid = true;
        if (classSelected)
        {
            if(characterName != "")
            {
                for (int i = 0; i < existentCharacters.Length; i++)
                {                    
                    if (existentCharacters[i].Name == characterName)
                        valid = false;
                }
                if (valid)
                {
                    if (existentCharacters.Length < 4)
                    {
                        sLManager.NewCharacter(characterName, cClass);
                    }
                    else
                    {
                        errorWindow.Error("Maximum num of characters reached");
                    }
                }
                else
                {
                    errorWindow.Error("Character name already exist");
                }
            }
            else
            {
                errorWindow.Error("Invalid character name");
            }
        }
        else
        {
            errorWindow.Error("Class not selected");
        }
    }
}
