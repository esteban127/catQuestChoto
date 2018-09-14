using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponSet
{
    Fist,
    SwordAndShield,
    Bow,
    TwoHandedSword,
    DualBlades
}

public enum EquipmentSlot
{
    mainHand,
    offHand,
    helmet,
    chest,
    pants,
    ring1,
    ring2,
    amulet,
    boots,
    gloves
}


public class InventoryManager : MonoBehaviour {



    Iitem[,] inventorySpots;
    Iitem[] equipedItems;
    
    private bool checkInventorySpace(int[] checkSize)
    {
        return false;
    }

    public bool checkFit(int[] size, int[,] position) 
    {
        return false;
    }

    public Iitem checkEquipment (EquipmentSlot slotToCheck)
    {
        return null;
    }
    public bool EquipItem (EquipmentSlot stopToEquip, Iitem itemToEquip)
    {
        return false;
    }
    public bool TryToAddItemToTheInventory (Iitem itemToAdd)
    {
        //return CheckInventorySpace(itemToAdd.Size);
        return false;
    }
    public bool TryToAddItemToTheInventory(Iitem itemToAdd, int[,] positionToAdd)
    {
        return false;
    }
    public void UseFromInventory (int[,] position)
    {
    }
    public void RemoveFromInventory(int[,] position)
    {        
    }
}
