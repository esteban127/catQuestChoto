using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum WeaponSet
{
    Fist,
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

[RequireComponent(typeof(GridLayoutGroup))]
public class InventoryManager : MonoBehaviour {

    Iitem[,] inventorySpots;
    int[,][] refInventory;
    Iitem[] equipedItems;
    InventoryInterface Iinteface;
    RectTransform ItemHolded;
    WeaponSet currentWeaponSet;
    [SerializeField] Button[] equipSlot;

    [SerializeField] Button buttonPrefab;
    [SerializeField] int col;
    [SerializeField] int row;
    [SerializeField] GameObject itemFramePrefab;

    static private InventoryManager instance = null;
    static public InventoryManager Instance { get { return instance; } }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Iinteface = new InventoryInterface(gameObject);
            inventorySpots = new Iitem[col, row];
            refInventory = new int[col,row][];
            equipedItems = new Iitem[10];
            InitializeRefInv(col,row);            
            //LoadInventory()
            Iinteface.CreateInventory(col, row, buttonPrefab);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void PickItem(GameObject item, ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
                TryToAddItemToTheInventory(item.GetComponent<WeaponManager>().GiveStats());
            break;
            case ItemType.Armor:
                TryToAddItemToTheInventory(item.GetComponent<ArmorManager>().GiveStats());
                break;
            case ItemType.Consumable:
                TryToAddItemToTheInventory(item.GetComponent<ConsumableManager>().GiveStats());
                break;
        }
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
    public bool EquipItem(EquipmentSlot slotToEquip, Iitem itemToEquip, int[] itemToEquipSpot)
    {
        Iitem equipedItem = checkEquipment(slotToEquip);
        if (equipedItem != null)
        {            
            if (!TryToAddItemToTheInventory(equipedItem))                       
            {
                return false;
            }
            Destroy(equipSlot[(int)slotToEquip].transform.GetChild(0).gameObject);
        }
        RemoveFromInventory(itemToEquipSpot,true);
        equipedItems[(int)slotToEquip] = itemToEquip;
        checkWeaponInstance();
        return true;
    }
    public void UnEquipeItem(EquipmentSlot slot)
    {
        equipedItems[(int)slot] = null;
        checkWeaponInstance();
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
            Destroy(equipSlot[(int)slotToEquip].transform.GetChild(0).gameObject);
        }
        equipedItems[(int)slotToEquip] = itemToEquip;
        checkWeaponInstance();
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
                    case weaponType.OneHandedSword:
                        currentWeaponSet = WeaponSet.SwordAndShield;
                        break;
                    case weaponType.Staf:
                        currentWeaponSet = WeaponSet.Staff;
                        break;
                    case weaponType.TwoHandedSword:
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
        Debug.Log(currentWeaponSet);
        
    }
    private bool checkItemTypeForSlot(EquipmentSlot slot, Iitem item)
    {
      
        if (typeof(Weapon) == item.GetType())
        {
            Weapon weapon = (Weapon)item;
            switch (weapon.WeaponType)
            {
                case weaponType.OneHandedSword:
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
                case weaponType.Staf:
                case weaponType.TwoHandedSword:
                case weaponType.Bow:
                    if (slot == EquipmentSlot.mainHand)
                    {
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
                case armorType.shield:
                    if (slot == EquipmentSlot.offHand && currentWeaponSet != WeaponSet.TwoHandedSword && currentWeaponSet != WeaponSet.Bow && currentWeaponSet != WeaponSet.Staff)
                        return true;
                    break;

            }
        }
        return false;
    }
    public bool TryToAddItemToTheInventory (Iitem itemToAdd)
    {
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
    public void UseFromInventory (int[] position)
    {
        Iitem itemToUse = inventorySpots[position[0], position[1]];
        if (typeof(Consumables) == itemToUse.GetType())
        {
            //ConsumableManager consume();
            RemoveFromInventory(position,true);
        }
        else
        {
            if (typeof(Weapon) == itemToUse.GetType())
            {
                if (!EquipItem(EquipmentSlot.mainHand, itemToUse, position))
                {
                    Debug.LogError("CannotEquipThat");
                }
            }
            else if (typeof(Armor) == itemToUse.GetType())
            {
                Armor item = (Armor)itemToUse;
                switch (item.ArmorType)
                {
                    case armorType.helmet:
                        EquipItem(EquipmentSlot.helmet, itemToUse, position);
                        break;
                    case armorType.chest:
                        EquipItem(EquipmentSlot.chest, itemToUse, position);
                        break;
                    case armorType.pants:
                        EquipItem(EquipmentSlot.pants, itemToUse, position);
                        break;
                    case armorType.boots:
                        EquipItem(EquipmentSlot.boots, itemToUse, position);
                        break;
                    case armorType.gloves:
                        EquipItem(EquipmentSlot.gloves, itemToUse, position);
                        break;
                    case armorType.amulet:
                        EquipItem(EquipmentSlot.amulet, itemToUse, position);
                        break;
                    case armorType.ring:
                        if (equipedItems[(int)EquipmentSlot.ring2] == null)
                        {
                            EquipItem(EquipmentSlot.ring2, itemToUse, position);
                        }
                        else
                        {
                            EquipItem(EquipmentSlot.ring1, itemToUse, position);
                        }
                        break;

                }                
            }
        }

        
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
    public void OnButtonClicked(int col, int row)
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
    public void OnButtonClicked(EquipmentSlot slot)
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
                EquipItem(slot, ItemHolded.GetComponent<ItemOnInventoryManager>().getItem());
                ItemHolded.GetComponent<ItemOnInventoryManager>().Equip(equipSlot[(int)slot].gameObject);
                ItemHolded = null;
            }
        }

    }

   

    class InventoryInterface
    {

        Button[,] inventorySlot;
        GridLayoutGroup grid;
        GameObject parent;
        float cellSize;

        public InventoryInterface(GameObject reference)
        {
            parent = reference;
            grid = reference.GetComponent<GridLayoutGroup>();
        }
        public void CreateInventory(int col, int row, Button prefab)
        {

            cellSize = parent.GetComponent<RectTransform>().rect.width / col;
            grid.cellSize = new Vector2(cellSize,cellSize);
            Debug.Log(cellSize);
            inventorySlot = new Button[col, row];

            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    Button instance = Instantiate(prefab);
                    instance.transform.SetParent(parent.transform);
                    instance.GetComponent<buttonManager>().SetPos(i, j);
                    inventorySlot[i, j] = instance;
                }
            }
        }

        public Button GetButton(int col, int row)
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


