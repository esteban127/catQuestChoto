using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueManager))]
[RequireComponent(typeof(NPCComponent))]
public class NpcIneraction : MonoBehaviour {

    QuestManager qManager;
    DialogueManager dialogue;
    IACTOR npc;
    [SerializeField] GameObject dialogBox;
    DialogBoxComponent dBoxComp;
    private void Start()
    {
        dBoxComp = dialogBox.GetComponent<DialogBoxComponent>();
        npc = GetComponent<NPCComponent>().getActor();
        qManager = QuestManager.Instance;
        dialogue = GetComponent<DialogueManager>();
        qManager.OnNewQuestStart += checkQuest;
    }

    public void Interact()
    {
        if (!dialogBox.activeInHierarchy)
        {
            string key = "Base";
            for (int i = 0; i < qManager.ActiveQuestKey.Count; i++)
            {
                if (dialogue.CheckKey(qManager.ActiveQuestKey[i]))
                {
                    key = qManager.ActiveQuestKey[i];
                    transform.GetChild(1).gameObject.SetActive(false);
                }

            }
            qManager.OnInteract(npc);
            InitializeDialogBox(dialogue.getDialogue(key));
        }        
    }

    void checkQuest()
    {
        for (int i = 0; i < qManager.ActiveQuestKey.Count; i++)
        {
            if (dialogue.CheckKey(qManager.ActiveQuestKey[i]))
            {
                transform.GetChild(1).gameObject.SetActive(true);
            }

        }
    }

    private void InitializeDialogBox(string[] dialogue)
    {
        dialogBox.SetActive(true);
        dBoxComp.Initialize(dialogue, npc.getImage());
    }
}
