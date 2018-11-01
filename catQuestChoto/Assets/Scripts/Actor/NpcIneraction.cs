using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueManager))]
public class NpcIneraction : MonoBehaviour {

    QuestManager qManager;
    DialogueManager dialogue;


    private void Start()
    {
        qManager = QuestManager.Instance;
        dialogue = GetComponent<DialogueManager>();
    }

    public void Interact()
    {    
        string key = "Base";
        for (int i = 0; i < qManager.ActiveQuestKey.Count; i++)
        {
            if (dialogue.CheckKey(qManager.ActiveQuestKey[i]))
            {
                Debug.Log("MAATCH");
                key = qManager.ActiveQuestKey[i];
            }
                
        }
        Debug.Log(dialogue.getDialogue(key));
        qManager.OnInteract(gameObject);
        //interface de dialogo
    }
}
