using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;



public class QuestManager : MonoBehaviour {

    [SerializeField] List<IQUEST> questList;
    [SerializeField] GameObject activeQuestDisplay;
    [SerializeField] Text questDescriptionText;
    [SerializeField] int rowInActiveQuestDisplay;
    [SerializeField] GameObject QuestButtonPrefab;
    [SerializeField] CharacterStats playerStats;
    SaveLoad sLManager;
    List<IQUEST> activeQuest;
    List<string> activeQuestKey;
    QuestSave saveState;
    int[] flagsTrack;
    public List<string> ActiveQuestKey { get { return activeQuestKey; } }
    QuestInterface qInterface;
    static private QuestManager instance = null;
    static public QuestManager Instance { get { return instance; } }
    IQUEST questSelected;
    InventoryManager iManager;

    public delegate void QuestDelegate();
    public QuestDelegate OnNewQuestStart;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            activeQuest = new List<IQUEST>();
            activeQuestKey = new List<string>();            
            qInterface = new QuestInterface(activeQuestDisplay, questDescriptionText, rowInActiveQuestDisplay, QuestButtonPrefab);                     
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        sLManager = SaveLoad.Instance;
        iManager = InventoryManager.Instance;
        LoadSystem.OnEndLoading += EndLoad;
        SaveLoad.BeforeClosing += Save;
    }
    private void OnDisable()
    {
        LoadSystem.OnEndLoading -= EndLoad;
    }

    private void EndLoad()
    {
        Load();
        gameObject.SetActive(false);
    }

    void AddQuest(IQUEST quest)
    {
        activeQuest.Add(quest);
        if(OnNewQuestStart!=null)
            OnNewQuestStart();
        activeQuestKey.Add(quest.QuestID);
        qInterface.InitializeQuest(quest);

    }

    private void RemoveQuest(IQUEST quest)
    {
        activeQuest.Remove(quest);
        activeQuestKey.Remove(quest.QuestID);
        qInterface.RemoveQuest(quest);
    }

    public bool OnInteract(IACTOR actor)
    {

        for (int i = 0; i < activeQuest.Count; i++)
        {
            for (int j = 0; j < activeQuest[i].Flags.Length; j++)
            {
                if (CheckFlag(activeQuest[i].Flags[j], QuestObjetive.talk))
                {
                    if(activeQuest[i].Flags[j].Target == actor.Name)
                    {
                        if (!activeQuest[i].Flags[j].Completed)
                        {
                            activeQuest[i].Flags[j].Progress();
                            CheckQuestStatus(activeQuest[i]);
                            return true;
                        }
                    }
                }                         
            }
        }
        return false;
    }
    public bool OnKill(IACTOR actor)
    {
        for (int i = 0; i < activeQuest.Count; i++)
        {
            for (int j = 0; j < activeQuest[i].Flags.Length; j++)
            {
                if (CheckFlag(activeQuest[i].Flags[j], QuestObjetive.kill))
                {
                    if (activeQuest[i].Flags[j].Target == actor.Name)
                    {
                        if (!activeQuest[i].Flags[j].Completed)
                        {
                            activeQuest[i].Flags[j].Progress();
                            CheckQuestStatus(activeQuest[i]);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    public bool OnPick(Iitem item)
    {
        for (int i = 0; i < activeQuest.Count; i++)
        {
            for (int j = 0; j < activeQuest[i].Flags.Length; j++)
            {
                if (CheckFlag(activeQuest[i].Flags[j], QuestObjetive.collect))
                {
                    if (activeQuest[i].Flags[j].Target == item.Name)
                    {
                        if (!activeQuest[i].Flags[j].Completed)
                        {
                            int amount = iManager.CheckAmountInInventory(item);                            
                            activeQuest[i].Flags[j].setProgress(amount);
                            if (activeQuest[i].Flags[j].ReqAmount <= amount)
                                iManager.RemoveAmountFromInventory(item,activeQuest[i].Flags[j].ReqAmount);
                            CheckQuestStatus(activeQuest[i]);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }


    private void CheckQuestStatus(IQUEST quest)
    {
        if(quest == questSelected)
            qInterface.SetDesription(quest);
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
        for (int i = 0; i < questList.Count; i++)
        {
            if(questList[i].Dependance == quest.QuestID)
            {
                AddQuest(questList[i]);
                
            }
        }
        playerStats.addXp(quest.XpReward);   
    }



    private bool CheckFlag(IQUEST.QuestFlag flag, QuestObjetive objetive)
    {
        if (flag.Objetive == objetive)
            return true;

        return false;
    }   
    
    public void OnQuestSelected(IQUEST quest)
    {
        questSelected = quest;
        qInterface.SetDesription(quest);
    }

    public void Save()
    {
        string path = sLManager.SaveDirectory + "Quest.json";
        saveState.Save(activeQuest);
        string save = JsonUtility.ToJson(saveState);
        File.WriteAllText(path, save);
    }

    private void Load()
    {
        string path = sLManager.SaveDirectory + "Quest.json";
        if (File.Exists(path))
        {
            saveState = JsonUtility.FromJson<QuestSave>(File.ReadAllText(path));
        }
        else
        {
            AddQuest(questList[0]);
            saveState = new QuestSave();
        }
        LoadQuest();
    }

    private void LoadQuest()
    {
        IQUEST[] questArray = saveState.Load();
        for (int i = 0; i < questArray.Length; i++)
        {
            AddQuest(questArray[i]);
        }
    }

    class QuestInterface
    {
        List<GameObject> questList;
        GameObject activeQuestFrame;
        GameObject prefab;
        GridLayoutGroup grid;
        Text questDescription;
        public QuestInterface(GameObject activeQuest, Text questDescriptionText, int rowCount, GameObject buttonPrefab)
        {
            questList = new List<GameObject>();
            activeQuestFrame = activeQuest;
            grid = activeQuest.GetComponent<GridLayoutGroup>();
            questDescription = questDescriptionText;
            prefab = buttonPrefab;
            SetGridSize(rowCount);
        }

        private void SetGridSize(int rowCount)
        {            
            grid.cellSize = new Vector2(activeQuestFrame.GetComponent<RectTransform>().rect.width, (activeQuestFrame.GetComponent<RectTransform>().rect.height - ((rowCount - 1) * 3)) / rowCount);          
        }

        public void InitializeQuest(IQUEST quest)
        {
            GameObject instance = Instantiate(prefab, activeQuestFrame.transform);
            instance.GetComponent<QuestButtonManager>().setQuest(quest);
            instance.GetComponentInChildren<Text>().text = quest.QuestID;
            questList.Add(instance);

        }
        public void SetDesription(IQUEST quest)
        {
            string questText = "";
            questText += "<b>" + quest.QuestID + "</b>\n";
            questText += "Suggested Level: " + quest.SuggestedLevel + "\n\n";
            questText += "Descritpion: \n";
            questText += quest.Description +"\n\n";
            questText += "<b>Objetive:</b>\n";
            for (int i = 0; i < quest.Flags.Length; i++)
            {
                if (quest.Flags[i].Completed)                
                    questText += "<color=grey>";
                      
                questText += quest.Flags[i].Objetive + " " + quest.Flags[i].Target + " : " + quest.Flags[i].CurrentAmount + " / " + quest.Flags[i].ReqAmount+"\n";
                if (quest.Flags[i].Completed)
                    questText += "</color>";
            }
            if (quest.XpReward > 0)
                questText += "\nReward: " + quest.XpReward + " Xp";

            questDescription.text = questText;
        }

        public bool RemoveQuest (IQUEST quest)
        {
            string iD = quest.QuestID;
            for (int i = 0; i < questList.Count; i++)
            {
                if(questList[i].GetComponent<QuestButtonManager>().getQuest().QuestID == iD)
                {
                    Destroy(questList[i], 0.1f);
                    questList.Remove(questList[i]);
                    return true;
                }
            }
            Debug.LogError("QuestButton " + iD + " not found");
            return false;
        }

    }

    [System.Serializable]
    class QuestSave
    {

        [SerializeField]string[] quests;
        
        public QuestSave()
        {
            quests = new string[0];
        }

        public void Save(List<IQUEST> activeQuest)
        {
            quests = new string[activeQuest.Count];
            for (int i = 0; i < activeQuest.Count; i++)
            {
                quests[i] = JsonUtility.ToJson(activeQuest[i]);
            }
        }
        public IQUEST[] Load()
        {
            IQUEST[] questArray = new IQUEST[quests.Length];
            for (int i = 0; i < quests.Length; i++)
            {
                questArray[i] = JsonUtility.FromJson<IQUEST>(quests[i]);
            }
            return questArray;
        }

    }
}
