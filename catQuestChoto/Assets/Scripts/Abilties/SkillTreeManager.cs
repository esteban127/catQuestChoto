using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

enum AbilityType
{
    HealAbility,
    AtackAbility
}

public enum AbilityInTree
{    
    Tree0Ab0,
    Tree0Ab1,
    Tree0Ab2,
    Tree0Ab3,
    Tree0Ab4,
    Tree1Ab0,
    Tree1Ab1,
    Tree1Ab2,
    Tree1Ab3,
    Tree1Ab4,
    WeaponAbility,
    empty
}


public class SkillTreeManager : MonoBehaviour {

    [SerializeField] GameObject SkillTreeButtonPrefab;
    [SerializeField] GameObject EquipedAbilitiesParent;
    IAbility[] skillsInTrees;
    [SerializeField] SkillTreeButton[] skillButtonsInTrees;
    [SerializeField] LvlUpSkillButton[] lvlUpSkillsButton;
    [SerializeField] Text SkillTreeTitle1;
    [SerializeField] Text SkillTreeTitle2;
    AbilitySystem aSystem;
    [SerializeField] Text SkillPointText;
    AtackAbility weaponAbility;
    InventoryManager iManager;
    SaveLoad sLManager;
    Clock timer;
    SkillsSave savedState;
    AbilityInTree[] abilityInSlot;
    EquipedAbilities eAbilitiesFrame;
    [SerializeField] draginSpell selectedSpell;
    int skillPoints = 0;
    private void Start()
    {
        sLManager = SaveLoad.Instance;
        aSystem = AbilitySystem.Instance;
        iManager = InventoryManager.Instance;
        LoadSystem.OnEndLoading += EndLoad;
        SaveLoad.BeforeClosing += Save;
        iManager.OnWeaponChange += actualizeWeaponAbility;
        iManager.OnStatChange += actualizateCDR;
        abilityInSlot = new AbilityInTree[aSystem.NumOfSpells];
        eAbilitiesFrame = new EquipedAbilities(EquipedAbilitiesParent);
        eAbilitiesFrame.CreateAbilities(aSystem.NumOfSpells, SkillTreeButtonPrefab, 3);            
        setSkillButtonsPos();    
        SetDelegates();
    }
    private void OnDisable()
    {
        LoadSystem.OnEndLoading -= EndLoad;
    }
    private void EndLoad()
    {
        Load(sLManager.currentClass);
        actualizeWeaponAbility();
        gameObject.SetActive(false);
    }   

    private void actualizateCDR()
    {
        for (int i = 0; i < aSystem.NumOfSpells; i++)
        {
            if (abilityInSlot[i] != AbilityInTree.empty)
            {
                aSystem.SetAbilityCDR((skillsInTrees[(int)abilityInSlot[i]]));
            }
        }
    }
    

    private void setSkillButtonsPos()
    {
        for (int i = 0; i < skillButtonsInTrees.Length; i++)
        {
            skillButtonsInTrees[i].SetPos(i);
        }
    }

    private void SetDelegates()
    {
        for (int i = 0; i < skillButtonsInTrees.Length; i++)
        {
            skillButtonsInTrees[i].OnLeftClick += OnLeftClickTreeSkill;
            skillButtonsInTrees[i].OnRightClick += OnRightClickTreeSkill;
            skillButtonsInTrees[i].OnMouseEnter += OnPointerEnterTreeSkill;
            skillButtonsInTrees[i].OnMouseExit += OnPointerExit;
        }
        for (int i = 0; i < lvlUpSkillsButton.Length; i++)
        {
            lvlUpSkillsButton[i].OnClik += OnClickLvlUpSkillButton;
        }
        for (int i = 0; i < aSystem.NumOfSpells; i++)
        {
            eAbilitiesFrame.GetSlot(i).OnLeftClick += OnLeftClickEquipedSkill;
            eAbilitiesFrame.GetSlot(i).OnRightClick += OnRightClickEquipedSkill;
            eAbilitiesFrame.GetSlot(i).OnMouseEnter += OnPointerEnterEquipedSkill;
            eAbilitiesFrame.GetSlot(i).OnMouseExit += OnPointerExit;
        }
    }

    private void OnPointerExit(int pos)
    {
        aSystem.HideToolTip();
    }
    private void OnPointerEnterEquipedSkill(int pos)
    {

        if (abilityInSlot[pos] != AbilityInTree.empty)
        {
            aSystem.ShowToolTip(skillsInTrees[(int)abilityInSlot[pos]]);
        }


    }
    private void OnPointerEnterTreeSkill(int pos)
    {
        aSystem.ShowToolTip(skillsInTrees[pos]);
    }
    private void OnRightClickEquipedSkill(int pos)
    {
        if (abilityInSlot[pos] != AbilityInTree.empty)
        {
            UnequipAbility(pos);
        }
    }
    private void OnLeftClickEquipedSkill(int pos)
    {
        if (abilityInSlot[pos] != AbilityInTree.empty)
        {
            if (selectedSpell.Picked)
            {
                EquipAbility(pos, selectedSpell.Release());
            }
            if (!selectedSpell.Picked)
            {
                selectedSpell.Drag(skillsInTrees[(int)abilityInSlot[pos]].AbilitySprite, abilityInSlot[pos]);
                UnequipAbility(pos);
            }
        }
        else
        {
            if (selectedSpell.Picked)
            {
                EquipAbility(pos, selectedSpell.Release());
            }
        }
    }
    private void OnLeftClickTreeSkill(int pos)
    {
        if (selectedSpell.Picked)
        {
            selectedSpell.Release();
        }
        else
        {
            if (skillsInTrees[pos].Level > 0)
                selectedSpell.Drag(skillsInTrees[pos].AbilitySprite, (AbilityInTree)pos);
        }
    }
    private void OnRightClickTreeSkill(int pos)
    {
        if (skillsInTrees[pos].Level > 0)
        {

            TryToEquipAbility((AbilityInTree)pos);

        }
    }

    private bool TryToEquipAbility(AbilityInTree refAbility)
    {
        for (int i = 0; i < abilityInSlot.Length; i++)
        {
            if (abilityInSlot[i] == AbilityInTree.empty)
            {
                EquipAbility(i, refAbility);
                return true;
            }
        }
        return false;
    }

    private void EquipAbility(int pos, AbilityInTree refAbility)
    {
        aSystem.SetAbility(pos, skillsInTrees[(int)refAbility]);
        abilityInSlot[pos] = refAbility;
        eAbilitiesFrame.GetSlot(pos).SetSprite(skillsInTrees[(int)refAbility].AbilitySprite, skillsInTrees[(int)refAbility].Loked);
        actualizateCDR();
    }
    private void UnequipAbility(int pos)
    {
        IAbility empty = LoadAbility("Empty", AbilityType.AtackAbility,abilityClass.Weapon);

        eAbilitiesFrame.GetSlot(pos).SetSprite(empty.AbilitySprite, false);
        aSystem.SetAbility(pos, empty);
        abilityInSlot[pos] = AbilityInTree.empty;
    }

    private void OnClickLvlUpSkillButton(int pos)
    {
        skillsInTrees[pos].Level++;
        skillButtonsInTrees[pos].TogleSprite(false);
        SetSkillPoints(skillPoints - 1);

    }
    public void AddSkillPoint()
    {
        SetSkillPoints(skillPoints + 1);
    }

    private void SetSkillPoints(int cSkillPoints)
    {
        skillPoints = cSkillPoints;
        SkillPointText.text = skillPoints.ToString();
        if (skillPoints <= 0)
        {
            DesactivateAllLvlUp();
        }
        else
        {
            ActivateLvlUp();
        }
    }

    private void ActivateLvlUp()
    {        
        for (int i = 0; i < lvlUpSkillsButton.Length; i++)
        {
            switch ((AbilityInTree)i)
            {
                case AbilityInTree.Tree0Ab0:
                case AbilityInTree.Tree1Ab0:
                    lvlUpSkillsButton[i].Interactable(true);
                    break;
                case AbilityInTree.Tree0Ab1:
                    if (skillsInTrees[(int)AbilityInTree.Tree0Ab0].Level > 0)
                    {
                        lvlUpSkillsButton[i].Interactable(true);
                    }
                    break;
                case AbilityInTree.Tree1Ab1:
                    if (skillsInTrees[(int)AbilityInTree.Tree1Ab0].Level > 0)
                    {
                        lvlUpSkillsButton[i].Interactable(true);
                    }
                    break;
                case AbilityInTree.Tree0Ab2:
                case AbilityInTree.Tree1Ab2:
                case AbilityInTree.Tree0Ab3:
                case AbilityInTree.Tree0Ab4:
                case AbilityInTree.Tree1Ab3:
                case AbilityInTree.Tree1Ab4:
                    if (skillsInTrees[i - 2].Level > 0)
                    {
                        lvlUpSkillsButton[i].Interactable(true);
                    }
                    break;
            }
        }
    }

    private void DesactivateAllLvlUp()
    {
        for (int i = 0; i < lvlUpSkillsButton.Length; i++)
        {
            lvlUpSkillsButton[i].Interactable(false);
        }
    }

    private void actualizeWeaponAbility()
    {
        IAbility currentWeaponAbility = LoadAbility(iManager.CurrentWeaponSet.ToString(), AbilityType.AtackAbility,abilityClass.Weapon);
        skillsInTrees[10] = currentWeaponAbility;
        skillButtonsInTrees[10].SetSprite(currentWeaponAbility.AbilitySprite, false);
        for (int i = 0; i < aSystem.NumOfSpells; i++)
        {
            if (abilityInSlot[i] == AbilityInTree.WeaponAbility)
            {
                EquipAbility(i, AbilityInTree.WeaponAbility);
            }
        }
        CheckAllAbilityesForWeapon(iManager.CurrentWeaponSet);
        ActualziateAbilitiesInBar();
    }

    private void ActualziateAbilitiesInBar()
    {
        for (int i = 0; i < aSystem.NumOfSpells; i++)
        {
            if (abilityInSlot[i] != AbilityInTree.empty)
                EquipAbility(i, abilityInSlot[i]);
        }
    }

    private void CheckAllAbilityesForWeapon(WeaponSet currentWeaponSet)
    {
        bool unloked = false;
        for (int i = 0; i < skillsInTrees.Length; i++)
        {

            for (int j = 0; j < skillsInTrees[i].WeaponReq.Length; j++)
            {
                if (skillsInTrees[i].WeaponReq[j] == currentWeaponSet)
                {
                    skillsInTrees[i].Unlock();
                    unloked = true;
                }
                if (j == skillsInTrees[i].WeaponReq.Length - 1 && !unloked)
                {
                    skillsInTrees[i].Lock();
                }
            }
            unloked = false;

        }
    }
    private int[] GetAbilityLvls()
    {
        int[] lvls = new int[10];
        for (int i = 0; i < 10; i++)
        {
            lvls[i] = skillsInTrees[i].Level;
        }
        return lvls;
    }

    public void Save()
    {
        Debug.Log("Save");
        string path = sLManager.SaveDirectory + "/Abilities.json";
        savedState.Save(abilityInSlot,skillPoints,GetAbilityLvls());
        string save = JsonUtility.ToJson(savedState);

        File.WriteAllText(path, save);
    }

    

    //Load stuff
    public void Load(c_class cClass)
    {
        string path = sLManager.SaveDirectory + "/Abilities.json";
        if (File.Exists(path))
        {
            savedState = JsonUtility.FromJson<SkillsSave>(File.ReadAllText(path));
        }
        else
        {
            savedState = new SkillsSave(aSystem.NumOfSpells, 10);
        }
        LoadAbilityForClass(cClass);
        LoadAbilitySetUp();
        LoadAbilityLevels();
        LoadSkillPoints();
    }

    private void LoadAbilityForClass(c_class cClass)
    {        
        switch (cClass)
        {
            case c_class.Mage:
                SkillTreeTitle1.text = "Water";
                SkillTreeTitle2.text = "Fire";
                break;
            case c_class.Warrior:
                SkillTreeTitle1.text = "Defense";
                SkillTreeTitle2.text = "Atack";
                break;
            case c_class.Archer:
                SkillTreeTitle1.text = "Hunter";
                SkillTreeTitle2.text = "Thief";
                break;
        }

        IAbility[] classAbilities = new IAbility[10];
        skillsInTrees = new IAbility[11];
        string path = Application.dataPath + "/Resources/Json/Ability/" + cClass + "/"+ AbilityType.AtackAbility+"/";
        string[] allAtackAbilitiesNames = Directory.GetFiles(path, "*.Json");
        path = Application.dataPath + "/Resources/Json/Ability/" + cClass + "/" + AbilityType.HealAbility + "/";
        string[] allHealAbilitiesNames = Directory.GetFiles(path, "*.Json");

        for (int i = 0; i < allAtackAbilitiesNames.Length; i++)
        {
            classAbilities[i] = LoadAbility(allAtackAbilitiesNames[i], AbilityType.AtackAbility); 
        }
        for (int i = 0; i < allHealAbilitiesNames.Length; i++)
        {
            classAbilities[i+allAtackAbilitiesNames.Length] = LoadAbility(allHealAbilitiesNames[i], AbilityType.HealAbility);
        }
        for (int i = 0; i < 10; i++)
        {
            skillsInTrees[classAbilities[i].PosInTree] = classAbilities[i];
        }
    }

   

    private void LoadAbilitySetUp()
    {
        abilityInSlot = savedState.AbilityInSlot;

    }
    private void LoadSkillPoints()
    {
        SetSkillPoints(savedState.SkillPoints);
    }
    private void LoadAbilityLevels()
    {
        int[] lvls = savedState.AbilityLvl;
        for (int i = 0; i < lvls.Length; i++)
        {
            skillsInTrees[i].Level = lvls[i];
            skillButtonsInTrees[i].SetSprite(skillsInTrees[i].AbilitySprite, skillsInTrees[i].Level > 0 ? false : true);
        }        
    }
    private IAbility LoadAbility(string abilityName, AbilityType type, abilityClass AbClass)
    {
        IAbility ability;
        string path = Application.dataPath + "/Resources/Json/Ability/" +AbClass + "/" + type + "/" + abilityName + ".Json";
        switch (type)
        {
            case AbilityType.AtackAbility:
                ability= (JsonUtility.FromJson<AtackAbility>(File.ReadAllText(path)));
                break;
            case AbilityType.HealAbility:
                ability = (JsonUtility.FromJson<HealAbility>(File.ReadAllText(path)));
                break;
            default:
                ability = ((JsonUtility.FromJson<AtackAbility>(File.ReadAllText(path))));
                break;  
        }
        ability.Initialize();
        return ability;
    }
    private IAbility LoadAbility(string path, AbilityType type)
    {
        IAbility ability;        
        switch (type)
        {
            case AbilityType.AtackAbility:
                ability = (JsonUtility.FromJson<AtackAbility>(File.ReadAllText(path)));
                break;
            case AbilityType.HealAbility:
                ability = (JsonUtility.FromJson<HealAbility>(File.ReadAllText(path)));
                break;
            default:
                ability = ((JsonUtility.FromJson<AtackAbility>(File.ReadAllText(path))));
                break;
        }
        ability.Initialize();
        return ability;
    }

    class EquipedAbilities
    {
        SkillTreeButton[] skillSlot;
        GridLayoutGroup grid;
        GameObject parent;

        public EquipedAbilities(GameObject reference)
        {
            parent = reference;
            grid = reference.GetComponent<GridLayoutGroup>();
        }

        public void CreateAbilities(int col, GameObject prefab, float separation)
        {

            Vector2 cellSize = new Vector2((parent.GetComponent<RectTransform>().rect.width - (separation * (col - 1))) / col, parent.GetComponent<RectTransform>().rect.height);
            grid.cellSize = cellSize;
            skillSlot = new SkillTreeButton[col];

            for (int i = 0; i < col; i++)
            {               
                GameObject instance = Instantiate(prefab);
                instance.transform.SetParent(parent.transform);
                instance.GetComponent<SkillTreeButton>().SetPos(i);
                skillSlot[i] = instance.GetComponent<SkillTreeButton>();

            }
        }
        public SkillTreeButton GetSlot(int slot)
        {
            return skillSlot[slot];
        }
    }
    [System.Serializable]
    public class SkillsSave
    {

        [SerializeField] AbilityInTree[] abilityInSlot;
        public AbilityInTree[] AbilityInSlot { get { return abilityInSlot; } }
        [SerializeField] int skillPoints;
        public int SkillPoints { get { return skillPoints; } }
        [SerializeField] int[] abilityLvl;
        public int[] AbilityLvl { get { return abilityLvl; } }

        public SkillsSave(int abInSlotSize,int totalAbilitiesInTree)
        {
            abilityInSlot = new AbilityInTree[abInSlotSize];
            abilityLvl = new int[totalAbilitiesInTree];
            for (int i = 0; i < abInSlotSize; i++)
            {
                abilityInSlot[i] = AbilityInTree.empty;
            }
        }

        public void Save(AbilityInTree[] AbSl, int SP, int[] AbLvl)
        {
            abilityInSlot = new AbilityInTree[AbSl.Length];
            abilityLvl = new int[AbLvl.Length];
            skillPoints = SP;
            for (int i = 0; i < AbSl.Length; i++)
            {
                abilityInSlot[i] = AbSl[i];
            }
            for (int i = 0; i < AbLvl.Length; i++)
            {
                abilityLvl[i] = AbLvl[i];
            }
        }
    }
}
