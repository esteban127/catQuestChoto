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
    public string Description { get { return questDescription; } }
    [SerializeField] string dependanceID;
    public string Dependance { get { return dependanceID; } }
    [SerializeField] float xpReward;
    public float XpReward { get { return xpReward; } }
    [SerializeField] Iitem[] itemReward;
    public Iitem[] ItemReward { get { return itemReward; } }
    [SerializeField] QuestFlag[] questRequirements;
    public QuestFlag[] Flags { get { return questRequirements; } }
    [SerializeField] int suggestedLvl;
    public int SuggestedLevel{get{ return suggestedLvl; } }

    [System.Serializable]
    public class QuestFlag
    {

        [SerializeField] QuestObjetive objetive;
        public QuestObjetive Objetive { get { return objetive; } }
        [SerializeField] string questTargetRef;
        public string Target { get { return questTargetRef; } }
        [SerializeField] int amount;
        public int ReqAmount { get { return amount; } }
        int currentAmount;
        public int CurrentAmount { get { return currentAmount; } }
        bool completed = false;
        public bool Completed { get { return completed; } }
        public void Progress()
        {
            currentAmount++;
            if (currentAmount >=amount)
                CompletRequirement();
        }
        public void setProgress(int progress)
        {
            currentAmount = progress;
            if (currentAmount >= amount)
                CompletRequirement();
        }
        public void CompletRequirement()
        {
            completed = true;
        }
    }

}

