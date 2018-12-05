using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkLordinteraction : MonoBehaviour {


    [SerializeField] GameObject player;
    [SerializeField] GameObject DarkLord;
    DialogueManager dialogue;
    IACTOR npc;
    [SerializeField] GameObject dialogBox;
    DialogBoxComponent dBoxComp;
    private void Start()
    {
        dBoxComp = dialogBox.GetComponent<DialogBoxComponent>();        
        npc = GetComponent<NPCStats>().getActor();            
        dialogue = GetComponent<DialogueManager>();
            
    }

    public void Interact()
    {
        if (!dialogBox.activeInHierarchy)
        {
            string key = "Base";                
            InitializeDialogBox(dialogue.getDialogue(key));
        }
    }

       

    private void InitializeDialogBox(string[] dialogue)
    {
        dBoxComp.onDialogEnd += SpawnDarkLord;
        dialogBox.SetActive(true);
        dBoxComp.Initialize(dialogue, npc.getImage());
    }

    private void SpawnDarkLord()
    {
        DarkLord.SetActive(true);
        DarkLord.GetComponent<SimpleEnemyIA>().Initialize(DarkLord.transform.position, player, npc.Level);
        dBoxComp.onDialogEnd -= SpawnDarkLord;
    }
}
