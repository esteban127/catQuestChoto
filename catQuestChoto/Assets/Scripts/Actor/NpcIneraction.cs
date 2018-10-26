using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueManager))]
public class NpcIneraction : MonoBehaviour {

    //questmanager
    DialogueManager dialogue;


    private void Start()
    {
        //instanciate el questmanajor
        dialogue = GetComponent<DialogueManager>();
    }
    public void Interact()
    {
        /*
            string key = "Base";
            chequeo en el quest manager
            interface de dialogo
            for (int i = 0; i < questKey.lengt; i++)
            {
                if(dialogue.CheckKey(questKey[i]))
                    key = questKey[i];
            }
            dialogue.getDialogue(key)
        */
    }
}
