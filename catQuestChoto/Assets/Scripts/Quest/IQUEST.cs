using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestObjetive
{
    kill,
    collect,
    talk
}

[System.Serializable]
public class IQUEST {

    [SerializeField] string questID;
    public string QuestID { get { return questID; } }
    [SerializeField] string questDescription;
    [SerializeField] string dependanceID;
    [SerializeField] float xpReward;
    [SerializeField] Iitem itemReward;
    [SerializeField] QuestFlag[] questRequirements;
    public QuestFlag[] Flags { get { return questRequirements; } }
    [SerializeField] int suggestedLvl;
    

    [System.Serializable]
    public class QuestFlag
    {

        [SerializeField] QuestObjetive objetive;
        public QuestObjetive Objetive { get { return objetive; } }
        [SerializeField] GameObject questTargetRef;
        public GameObject Target { get { return questTargetRef; } }
        [SerializeField] int amount;

        bool completed = false;
        public bool Completed { get { return completed; } }
        public void Progress()
        {
            amount--;
            if (amount <= 0)
                CompletRequirement();
        }
        public void CompletRequirement()
        {
            completed = true;
        }
    }

}

