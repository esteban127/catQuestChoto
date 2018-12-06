using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkLordinteraction : MonoBehaviour {


    [SerializeField] GameObject player;
    [SerializeField] GameObject DarkLord;
    IACTOR npc;
    [SerializeField] GameObject dialogBox;
    DialogBoxComponent dBoxComp;
    private void Start()
    {
        dBoxComp = dialogBox.GetComponent<DialogBoxComponent>();        
        npc = GetComponent<NPCStats>().getActor();   
        dBoxComp.onDialogEnd += SpawnDarkLord;
    }   
    private void SpawnDarkLord()
    {        
        DarkLord.SetActive(true);
        DarkLord.GetComponent<SimpleEnemyIA>().Initialize(DarkLord.transform.position, player, npc.Level);
        dBoxComp.onDialogEnd -= SpawnDarkLord;
        gameObject.SetActive(false);
    }
}
