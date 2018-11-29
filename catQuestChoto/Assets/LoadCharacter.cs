using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadCharacter : MonoBehaviour {

    [SerializeField] ConfirmPanel confirm;
    [SerializeField] Text[] slotsText;
    [SerializeField] ErroPanel errorWindow;
    CharacterActor[] existentCharacters;
    SaveLoad sLManager;
    int slot;
    bool slotSelected = false;
    // Use this for initialization
    void Start () {
        sLManager = SaveLoad.Instance;               
        AsignSlots();
	}

    private void AsignSlots()
    {
        existentCharacters = sLManager.getAllCharacters();
        for (int i = 0; i < existentCharacters.Length; i++)
        {
            slotsText[i].text = existentCharacters[i].Name + "\n" + existentCharacters[i].Class + "  Lvl: " + existentCharacters[i].Level;
        }
    }

    public void selectSlot(int slotNum)
    {
        slot = slotNum;
        slotSelected = true;
    }

    public void TryToDelet()
    {
        if (slotSelected)
        {
            if (slot < existentCharacters.Length)
            {
                StartCoroutine(WaitForConfirmDelet(existentCharacters[slot].Name));
            }
            else
            {
                errorWindow.Error("Save slot is empty");
            }
        }
        else
        {
            errorWindow.Error("Save slot not selected");
        }
    }

    IEnumerator WaitForConfirmDelet(string nameToDelete)
    {
        confirm.Ask();
        while(confirm.ConfirmResult == "Null")
        {
            yield return null;
        }
        if(confirm.ConfirmResult == "Yes")
        {
            sLManager.DeleteSave(nameToDelete);
        }
    }

    public void TryToConfirm()
    {
        if (slotSelected)
        {
            if (slot < existentCharacters.Length)
            {

            }
            else
            {
                errorWindow.Error("Save slot is empty");
            }
        }
        else
        {
            errorWindow.Error("Save slot not selected");
        }
    }
}
