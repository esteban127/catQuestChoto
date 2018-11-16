using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestButtonManager : MonoBehaviour {

    IQUEST quest;
    QuestManager qManager;

    private void Start()
    {
        qManager = QuestManager.Instance;
    }
    public void setQuest(IQUEST newQuest)
    {
        quest = newQuest;
    }
    public IQUEST getQuest()
    {
        return quest;
    }

    public void onClick()
    {
        qManager.OnQuestSelected(quest);
    }


}
