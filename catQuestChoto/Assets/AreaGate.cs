using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaGate : MonoBehaviour {

    [SerializeField] string sceneName;
    SaveLoad sLManager;
    [SerializeField] string questRequiered;
    QuestManager qManager;    
    private void Start()
    {
        sLManager = SaveLoad.Instance;
        qManager = QuestManager.Instance;
    }

    private void OnTriggerEnter(Collider collide)
    {
        if(collide.tag == "Player")
        {
            if(questRequiered == ""|| CheckQuest())
                sLManager.ChangeScene(sceneName);

        }        
    }

    private bool CheckQuest()
    {
        for (int i = 0; i < qManager.ActiveQuestKey.Count; i++)
        {
            if (questRequiered == qManager.ActiveQuestKey[i])
            {
                return true;
            }
        }
        return false;
    }
}
