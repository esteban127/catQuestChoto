using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    [SerializeField] List<IQUEST> questList;
    List<IQUEST> activeQuest;
    List<string> activeQuestKey;
    int[] flagsTrack;
    public List<string> ActiveQuestKey { get { return activeQuestKey; } }

    static private QuestManager instance = null;
    static public QuestManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            activeQuest = new List<IQUEST>();
            activeQuestKey = new List<string>();
            AddQuest(questList[0]);

            //loadQuestStatus
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }  

    void AddQuest(IQUEST quest)
    {
        activeQuest.Add(quest);
        activeQuestKey.Add(quest.QuestID);

    }

    private void RemoveQuest(IQUEST quest)
    {
        activeQuest.Remove(quest);
        activeQuestKey.Remove(quest.QuestID);
    }

    public void OnInteract(GameObject subject)
    {

        for (int i = 0; i < activeQuest.Count; i++)
        {
            for (int j = 0; j < activeQuest[i].Flags.Length; j++)
            {
                if (CheckFlag(activeQuest[i].Flags[j], QuestObjetive.talk))
                {
                    if(activeQuest[i].Flags[j].Target == subject)
                    {
                        activeQuest[i].Flags[j].Progress();
                        CheckQuestStatus(activeQuest[i]);
                    }
                }                         
            }
        }
    }

    private void CheckQuestStatus(IQUEST quest)
    {
        for (int i = 0; i < quest.Flags.Length; i++)
        {
            if (!quest.Flags[i].Completed)
                return;
        }
        CompleteQuest(quest);
    }

    private void CompleteQuest(IQUEST quest)
    {
        RemoveQuest(quest);
        //rewards and other not implemented stuff
    }



    private bool CheckFlag(IQUEST.QuestFlag flag, QuestObjetive objetive)
    {
        if (flag.Objetive == objetive)
            return true;

        return false;
    }
    public void OnKill(GameObject subject)
    {

    }
    public void OnPick(GameObject subject)
    {

    }

    public static void OnFlagCompleted()
    {

    }
}
