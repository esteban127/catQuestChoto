using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum WeaponSet
{
    Fist,
    OneHanded,
    SwordAndShield,
    Bow,
    TwoHandedSword,
    Staff,
    DualBlades
}

public enum EquipmentSlot
{
    mainHand,
    offHand,
    helmet,
    chest,
    pants,
    boots,
    gloves,
    amulet,
    ring1,
    ring2
}

public struct equipmentStats
{
    public itemstats stats;
    public float defense;
    public float baseMinDamage;
    public float baseMaxDamage;
    public float critDmg;
    public float baseCritChance;
}

[RequireComponent(typeof(GridLayoutGroup))]
public class InventoryManager : MonoBehaviour {

    Iitem[,] inventorySpots;
    int[,][] refInventory;
    Iitem[] equipedItems;
    InventoryInterface Iinteface;    
    RectTransform ItemHolded;
    WeaponSet currentWeaponSet;
    equipmentStats currentEquipmentBonus;
    [SerializeField] GameObject[] equipSlot;
    [SerializeField] GameObject toolTip;
    [SerializeField] Text toolTipSizeText;
    [SerializeField] Text toolTipVisualText;
    [SerializeField] Text equipmentBonusText;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] int col;
    [SerializeField] int row;
    [SerializeField] GameObject itemFramePrefab;
    ItemFactory iFactory;


    static private InventoryManager instance = null;
    static public InventoryManager Instance { get { return instance; } }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Iinteface = new InventoryInterface(gameObject);
            currentEquipmentBonus.stats = new itemstats();
            inventorySpots = new Iitem[col, row];
            refInventory = new int[col,row][];
            equipedItems = new Iitem[10];
            InitializeRefInv(col,row);            
            //LoadInventory()
            Iinteface.CreateInventory(col, row, buttonPrefab);
            calculateEquipmentBonus();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        iFactory = ItemFactory.Instance();
    }

    public void PickItem(GameObject item)
    {
        Debug.Log("EHhh");
        TryToAddItemToTheInventory(item.GetComponent<ItemComponent>().GiveStats());           
    }
    private void InitializeRefInv(int col, int row)
    {
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                refInventory[i, j] = new int[2];
                refInventory[i, j][0] = -1;
            }
        }
    }
    private int[] CheckFit(int[] itemSize)
    {
        
        int[] checkingPos = new int[2];
        for (int i = itemSize[0]-1; i < inventorySpots.GetLength(0); i++)
        {
            checkingPos[0] = i;
            for (int j = itemSize[1]-1; j < inventorySpots.GetLength(1); j++)
            {
                checkingPos[1] = j;
                if (CheckFitInPos(itemSize, checkingPos))
                    return checkingPos;
            }
        }
        checkingPos[0] = -1;
        checkingPos[1] = -1;
        return checkingPos;
    }
    public bool CheckFitInPos(int[] size, int[] position) 
    {

        
        for (int i = position[0] - (size[0]-1); i < position[0]+1; i++)
        {
            if(i >= 0 && i < refInventory.GetLength(0))
            {            
                for (int j = position[1] - (size[1] - 1); j < position[1]+1; j++)
                {
                    if ( j >= 0 &&  j < refInventory.GetLength(1))
                    {
                        if (refInventory[i, j][0] >= 0)
                            return false;
                    }                      
                    else                        
                        return false;
                }
                
            }
            else
            {
            return false;
            }
        }
        return true;
    }
    public Iitem checkEquipment (EquipmentSlot slotToCheck)
    {
        return equipedItems[(int)slotToCheck];
    }    
    public void UnEquipeItem(EquipmentSlot slot)
    {
        equipedItems[(int)slot] = null;
        if (slot == EquipmentSlot.mainHand || slot == EquipmentSlot.offHand)
            checkWeaponInstance();
        calculateEquipmentBonus();
    }
    public bool EquipItem(EquipmentSlot slotToEquip, Iitem itemToEquip)
    {
        Iitem equipedItem = checkEquipment(slotToEquip);
        if (equipedItem != null)
        {
            if (!TryToAddItemToTheInventory(equipedItem))
            {
                return false;
            }
            Debug.Log("destroy");
            Destroy(equipSlot[(int)slotToEquip].transform.GetChild(0).gameObject);
        }
        equipedItems[(int)slotToEquip] = itemToEquip;        
        if (slotToEquip == EquipmentSlot.mainHand || slotToEquip == EquipmentSlot.offHand)
            checkWeaponInstance();

        calculateEquipmentBonus();
        return true;
    }

    private void checkWeaponInstance()
    {

        if (equipedItems[(int)EquipmentSlot.offHand] != null)
        {
            if (equipedItems[(int)EquipmentSlot.offHand].GetType() == typeof(Weapon))
            {
                currentWeaponSet = WeaponSet.DualBlades;
            }
            else
            {
                currentWeaponSet = WeaponSet.SwordAndShield;
            }
        }
        else
        {
            if (equipedItems[(int)EquipmentSlot.mainHand] != null)
            {
                Weapon weapon = (Weapon)equipedItems[(int)EquipmentSlot.mainHand];
                switch (weapon.WeaponType)
                {
                    case weaponType.OneHanded:
                        currentWeaponSet = WeaponSet.OneHanded;
                        break;
                    case weaponType.MagicStaf:
                        currentWeaponSet = WeaponSet.Staff;
                        break;
                    case weaponType.TwoHanded:
                        currentWeaponSet = WeaponSet.TwoHandedSword;
                        break;
                    case weaponType.Bow:
                        currentWeaponSet = WeaponSet.Bow;
                        break;
                }
            }            
            else
            {
                currentWeaponSet = WeaponSet.Fist;
            }
            
        }
        
    }

    private void calculateEquipmentBonus()
    {
        float defense = 0.0f;
        float critDmg = 1.0f;
        float baseMinDmg = 0.0f;
        float baseMaxDamage = 0.0f;
        float baseCritChance = 0.0f;
        itemstats tempStats = new itemstats();
        for (int i = 0; i < equipedItems.Length; i++)
        {
            if (equipedItems[i] != null)
            {
                tempStats += equipedItems[i].stats;
                if (equipedItems[i].GetType() == typeof(Armor))
                {
                    defense += ((Armor)equipedItems[i]).Defense;
                    tempStats += ((Armor)equipedItems[i]).AditionalStats;
                }
                else
                {
                    tempStats += ((Weapon)equipedItems[i]).AditionalStats;
                    critDmg *= ((Weapon)equipedItems[i]).CritDmg;
                    baseCritChance += ((Weapon)equipedItems[i]).BaseCritChance;
                    baseMinDmg += ((Weapon)equipedItems[i]).BaseMinDamage;
                    baseMaxDamage += ((Weapon)equipedItems[i]).BaseMaxDamage;
                }
            }
        }
        currentEquipmentBonus.stats = tempStats;
        currentEquipmentBonus.defense = defense;
        currentEquipmentBonus.critDmg = critDmg;
        currentEquipmentBonus.baseCritChance = baseCritChance;
        currentEquipmentBonus.baseMinDamage = baseMinDmg;
        currentEquipmentBonus.baseMaxDamage = baseMaxDamage;

        WriteEquipmentBonusText();
    }

    private void WriteEquipmentBonusText()
    {
        string text = "";
        text += ("Defense : " + currentEquipmentBonus.defense + "\n");
        text += (currentWeaponSet + "\n");
        text += ("Damage : " + currentEquipmentBonus.baseMinDamage + "-" + currentEquipmentBonus.baseMaxDamage + "\n");
        text += ("Base critical chance: " + (int)(currentEquipmentBonus.baseCritChance * 100) + "%\n");
        text += ("Critical multiplier: x" + (int)(currentEquipmentBonus.critDmg * 100) + "%\n");
        text += StatsTooltipText(currentEquipmentBonus.stats);

        equipmentBonusText.text = text;
    }

    private bool checkItemTypeForSlot(EquipmentSlot slot, Iitem item)
    {
      
        if (typeof(Weapon) == item.GetType())
        {
            Weapon weapon = (Weapon)item;
            switch (weapon.WeaponType)
            {
                case weaponType.OneHanded:
                    if (slot == EquipmentSlot.mainHand)
                    {
                        return true;
                    }
                    else
                    {
                        if (slot == EquipmentSlot.offHand)
                        {
                            if (currentWeaponSet != WeaponSet.TwoHandedSword && currentWeaponSet != WeaponSet.Bow && currentWeaponSet != WeaponSet.Staff)
                                return true;
                        }
                    }
                    break;
                case weaponType.MagicStaf:
                case weaponType.TwoHanded:
                case weaponType.Bow:
                    if (slot == EquipmentSlot.mainHand)
                    {
                        if (currentWeaponSet != WeaponSet.SwordAndShield && currentWeaponSet != WeaponSet.DualBlades)
                            return true;
                        
                    }
                    break;
            }           
        }
        else if (typeof(Armor) == item.GetType())
        {
            Armor armor = (Armor)item;
            switch (armor.ArmorType)
            {
                case armorType.helmet:
                    if (slot == EquipmentSlot.helmet)
                    {
                        return true;
                    }
                    break;
                case armorType.chest:
                    if (slot == EquipmentSlot.chest)
                    {
                        return true;
                    }
                    break;
                case armorType.pants:
                    if (slot == EquipmentSlot.pants)
                    {
                        return true;
                    }
                    break;
                case armorType.boots:
                    if (slot == EquipmentSlot.boots)
                    {
                        return true;
                    }
                    break;
                case armorType.gloves:
                    if (slot == EquipmentSlot.gloves)
                    {
                        return true;
                    }
                    break;
                case armorType.amulet:
                    if (slot == EquipmentSlot.amulet)
                    {
                        return true;
                    }
                    break;
                case armorType.ring:
                    if (slot == EquipmentSlot.ring1||slot == EquipmentSlot.ring2)
                    {
                        return true;
                    }
                    break;
                case armorType.offHand:
                    if (slot == EquipmentSlot.offHand && currentWeaponSet != WeaponSet.TwoHandedSword && currentWeaponSet != WeaponSet.Bow && currentWeaponSet != WeaponSet.Staff)
                        return true;
                    break;

            }
        }
        return false;
    }
    public bool TryToAddItemToTheInventory (Iitem itemToAdd)
    {
        Debug.Log("Ahhh");

        int[] emptyPos = CheckFit(itemToAdd.Size);
        if (emptyPos[0] >= 0)
        {
            AddItemToTheInventory(itemToAdd, emptyPos,true);
            return true;
        }
        Debug.Log("Inventory Full");
        return false;
    }
    public void AddItemToTheInventory(Iitem itemToAdd, int[] positionToAdd, bool newItem)
    {
        inventorySpots[positionToAdd[0], positionToAdd[1]] = itemToAdd;
        for (int i = 0; i < itemToAdd.Size[0]; i++)
        {
            for (int j = 0; j < itemToAdd.Size[1]; j++)
            {
                refInventory[positionToAdd[0] - i, positionToAdd[1] - j][0] = positionToAdd[0];
                refInventory[positionToAdd[0] - i, positionToAdd[1] - j][1] = positionToAdd[1];
            }
        }
        if(newItem)
            Iinteface.CreateItemFrame(positionToAdd[0], positionToAdd[1], itemToAdd, itemFramePrefab);
    }
   
    public void RemoveFromInventory(int[] position, bool deleteFrame)
    {
        Iitem itemToDelete = inventorySpots[position[0], position[1]];
        for (int i = 0; i < itemToDelete.Size[0]; i++)
        {
            for (int j = 0; j < itemToDelete.Size[1]; j++)
            {
                refInventory[position[0] - i, position[1] - j][0] = -1;
                refInventory[position[0] - i, position[1] - j][1] = -1;
            }
        }
        if(deleteFrame)
            Iinteface.DeleteFrame(position[0], position[1]);
    }
    public void LoadInventory()
    {

    } //pendiente
    public bool SaveInventory()
    {
        return false;
    } //pendiente

    private bool UseFromInventory(int[] pos)
    {
        GameObject item = Iinteface.GetButton(pos[0], pos[1]).transform.GetChild(0).gameObject;
        Iitem itemToUse = item.GetComponent<ItemOnInventoryManager>().getItem();
        if (typeof(Consumables) == itemToUse.GetType())
        {
            //ConsumableManager consume();
            RemoveFromInventory(pos, true);
            return true;
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                if (equipedItems[i] == null)
                {
                    if (checkItemTypeForSlot((EquipmentSlot)i, itemToUse))
                    {
                        if(EquipItem((EquipmentSlot)i, itemToUse))
                        {
                            RemoveFromInventory(pos, false);
                            item.GetComponent<ItemOnInventoryManager>().Equip(equipSlot[i]);
                            return true;

                        }                        
                    }
                }
                
            }
        }
        return false;

    }
    private bool UseFromInventory(EquipmentSlot slot)
    {        
        if (!TryToAddItemToTheInventory(equipedItems[(int)slot]))
        {
            return false;
        }
        UnEquipeItem(slot);
        Destroy(equipSlot[(int)slot].transform.GetChild(0).gameObject);        
        return true;
    }

    public void OnLeftButtonClicked(int col, int row)
    {
        int[] pos = new int[2];
        pos[0] = col;
        pos[1] = row;
       
        
        if (ItemHolded == null)
        {
            if (refInventory[col, row][0] >= 0)
            {
                RemoveFromInventory(pos, false);
                Iinteface.GetButton(col, row).GetComponentInChildren<ItemOnInventoryManager>().Draw();
                ItemHolded = Iinteface.GetButton(col, row).transform.GetChild(0).GetComponent<RectTransform>();
                ItemHolded.SetParent(transform.parent);
            }
        }
        else
        {
            if (CheckFitInPos(ItemHolded.GetComponent<ItemOnInventoryManager>().getItem().Size, pos))
            {
                    AddItemToTheInventory(ItemHolded.GetComponent<ItemOnInventoryManager>().getItem(), pos, false);
                    ItemHolded.GetComponent<ItemOnInventoryManager>().Release(Iinteface.GetButton(col, row).gameObject);
                    ItemHolded = null;
            }
               
        }
              
    }
    public void OnLeftButtonClicked(EquipmentSlot slot)
    {
        
        if (ItemHolded == null)
        {
            if (equipedItems[(int)slot] != null)
            {
                UnEquipeItem(slot);
                equipSlot[(int)slot].GetComponentInChildren<ItemOnInventoryManager>().Draw();
                ItemHolded = equipSlot[(int)slot].transform.GetChild(0).GetComponent<RectTransform>();
                ItemHolded.SetParent(transform.parent);
            }
        }
        else
        {
            if(checkItemTypeForSlot(slot, ItemHolded.GetComponent<ItemOnInventoryManager>().getItem()))
            { 
                if(EquipItem(slot, ItemHolded.GetComponent<ItemOnInventoryManager>().getItem()))
                {
                    ItemHolded.GetComponent<ItemOnInventoryManager>().Equip(equipSlot[(int)slot].gameObject);
                    ItemHolded = null;
                }
                
            }
        }

    }
    public void OnRightButtonClicked(int col, int row)
    {
        int[] pos = new int[2];
        pos[0] = col;
        pos[1] = row;
       
        if (refInventory[col, row][0] >= 0)
        {        
            UseFromInventory(pos);
        }
    }
    public void OnRightButtonClicked(EquipmentSlot slot)
    {
        if (equipedItems[(int)slot] != null)
            UseFromInventory(slot);
        
    }

    public void ClickedOutOfInventory()
    {
        if (ItemHolded != null)
        {
            Debug.Log("Adios");
            DropItem(ItemHolded.GetComponent<ItemOnInventoryManager>().getItem());
            Destroy(ItemHolded.gameObject);
            ItemHolded = null;
        }
    }

    private void DropItem(Iitem item)
    {
        Vector3 v3 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 5));
       iFactory.DropItem(item,v3);
    }

    public void MouseOver(int col, int row)
    {
        if (refInventory[col, row][0] >= 0)
        {
            ShowToolTip(Iinteface.GetButton(col, row).GetComponentInChildren<ItemOnInventoryManager>().getItem());
        }
    }
    public void MouseOver(EquipmentSlot slot)
    {
        if (equipedItems[(int)slot] != null)
        {
            ShowToolTip(equipedItems[(int)slot]);
        }
            
    }
    private void ShowToolTip(Iitem item)
    {
        toolTip.SetActive(true);
        float xPos = Input.mousePosition.x - 3;
        float yPos = Input.mousePosition.y;

        float tooltipWidth = toolTipSizeText.GetComponent<RectTransform>().rect.width;
        float tooltipHeight = toolTipSizeText.GetComponent<RectTransform>().rect.height;

        xPos -= (tooltipWidth / 2 + 5);
        if (yPos + tooltipHeight + 10 < Screen.height)
        {
            yPos = yPos + tooltipHeight / 2 + 10;
        }
        else
        {
            yPos = yPos - tooltipHeight / 2 - 5;
        }     
        toolTipSizeText.text = GenerateToolTipText(item);
        toolTipVisualText.text = toolTipSizeText.text;
        toolTip.transform.position = new Vector2(xPos, yPos);
    }

    private string GenerateToolTipText(Iitem item)
    {
        string text = "";
        switch  (item.Tier)
        {
            case ItemTier.Tier0:
                text += "<color=white>";
                break;
            case ItemTier.Tier1:
                text += "<color=blue>";
                break;
            case ItemTier.Tier2:
                text += "<color=yellow>";
                break;
            case ItemTier.Tier3:
                text += "<color=orange>";
                break;
        }
        text += ("<b>" +"<size=18>"+ item.Name + "</size>"+ "</b>" + "</color>" + "          ");

        if(typeof(Armor) == item.GetType())
        {
            text += (((Armor)item).ArmorType + "\n");
            text += (((Armor)item).Defense>0?"Defense : " + ((Armor)item).Defense + "\n" :"" );
        }
        if (typeof(Weapon) == item.GetType())
        {
            text += (((Weapon)item).WeaponType + "\n");
            text += ("Damage : " + ((Weapon)item).BaseMinDamage + " - " + ((Weapon)item).BaseMaxDamage + "\n");
            text += ("Base critical chance: " + (int)(((Weapon)item).BaseCritChance * 100) + "%\n");
            text += ("Critical multiplier: x" + (int)(((Weapon)item).CritDmg * 100) + "%\n");
        }
        if (typeof(Consumables) == item.GetType())
        {
            text += "Consumable \n"; 
            text += ((Consumables)item).Restoration? "Restoration   " : "Effect   ";
            text += (((Consumables)item).Duration > 0 ? ((Consumables)item).Duration + " s \n" : "Instant\n");
        }
        string effects = StatsTooltipText(item.stats);

        if (effects != "")
            text += ("Effects: \n" + effects);

        if(typeof(Weapon) == item.GetType())
        {
            text += "Aditional effects: <color=cyan> \n";
            text += StatsTooltipText(((Weapon)item).AditionalStats);
            text += "</color>";
        }
        if (typeof(Armor) == item.GetType())
        {
            text += "Aditional effects: <color=cyan> \n";
            text += StatsTooltipText(((Armor)item).AditionalStats);
            text += "</color>";
        }
        
            text += ("\n <i>"+item.Description+ "</i>");
        return text;
    }

    private string StatsTooltipText(itemstats itemstats)
    {
        string statText = "";
        if (itemstats.Health > 0)
            statText += ("Healt: " + itemstats.Health + "\n");
        if (itemstats.Mana > 0)
            statText += ("Mana: " + itemstats.Mana + "\n");
        if (itemstats.Strength > 0)
            statText += ("Strength: " + itemstats.Strength + "\n");
        if (itemstats.Constitution > 0)
            statText += ("Constitution: " + itemstats.Constitution + "\n");
        if (itemstats.Dextery > 0)
            statText += ("Dextery: " + itemstats.Dextery + "\n");
        if (itemstats.Inteligence > 0)
            statText += ("Inteligence: " + itemstats.Inteligence + "\n");                      
        if (itemstats.Luck > 0)
            statText += ("Luck: " + itemstats.Luck + "\n");
        if (itemstats.Precision > 0)
            statText += ("Precision: " + (int)(itemstats.Precision*100) + "%\n");
        if (itemstats.Dodge > 0)
            statText += ("Dodge: " + (int)(itemstats.Dodge * 100) + "%\n");
        if (itemstats.CritChance > 0)
            statText += ("Critical chance: " + (int)(itemstats.CritChance * 100) + "%\n");
        if (itemstats.ColdownReduction > 0)
            statText += ("Coldown reduction: " + (int)(itemstats.ColdownReduction * 100) + "%\n");
        if (itemstats.HealthRegen > 0)
            statText += ("Healt regeneration: " + itemstats.HealthRegen + "\n");
        if (itemstats.ManaRegen > 0)
            statText += ("Mana regeneration: " + itemstats.ManaRegen + "\n");
        return statText;
    }

    public void HideToolTip()
    {
        toolTip.SetActive(false);
    }
   

    class InventoryInterface
    {

        GameObject[,] inventorySlot;
        GridLayoutGroup grid;
        GameObject parent;
        float cellSize;
        
        public InventoryInterface(GameObject reference)
        {
            parent = reference;
            grid = reference.GetComponent<GridLayoutGroup>();
        }
        public void CreateInventory(int col, int row, GameObject prefab)
        {

            cellSize = parent.GetComponent<RectTransform>().rect.width / col;
            grid.cellSize = new Vector2(cellSize,cellSize);            
            inventorySlot = new GameObject[col, row];

            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    GameObject instance = Instantiate(prefab);
                    instance.transform.SetParent(parent.transform);
                    instance.GetComponent<buttonManager>().SetPos(i, j);
                    inventorySlot[i, j] = instance;
                }
            }
        }

        public GameObject GetButton(int col, int row)
        {
            return inventorySlot[col, row];
        }

        public void CreateItemFrame(int col, int row, Iitem item, GameObject framePrefab)
        {
            GameObject frame = Instantiate(framePrefab);
            frame.transform.SetParent(GetButton(col, row).transform);
            frame.GetComponent<ItemOnInventoryManager>().Initialize(item,cellSize);
        }

        public void DeleteFrame(int col, int row)
        {
            Destroy(GetButton(col, row).transform.GetChild(0).gameObject);           
        }


    }
}


