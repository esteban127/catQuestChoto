using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum itemTipe
{
    Weapon,
    Armor,
    Consumable,    
}


public class ItemFactory {    


    static private ItemFactory instance = null;
    private ItemFactory() { }
    static public ItemFactory Instance()
    {
        if (instance == null)
            instance = new ItemFactory();

        return instance;
    }

    /*public GameObject generateItem(itemTipe tipe, int itemID)
    {
        JsonUtility.


    }*/


}
