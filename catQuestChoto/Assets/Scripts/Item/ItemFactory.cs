using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum ItemType
{
    Weapon,
    Armor,
    Consumable,    
}


public class ItemFactory{    


    static private ItemFactory instance = null;
    private ItemFactory() { }
    static public ItemFactory Instance()
    {
        if (instance == null)
            instance = new ItemFactory();

        return instance;
    }

    public GameObject Generate(FileType type, string itemID)
    {
        GameObject Item = new GameObject();
        string path = Application.dataPath;
        switch (type)
        {
            case FileType.Armor:
                Item.AddComponent<ArmorManager>();
                path += "/Resources/Json/Armor/" + itemID + ".Json";
                Item.GetComponent<ArmorManager>().SetStats(JsonUtility.FromJson<Armor>(File.ReadAllText(path)));
                break;
            case FileType.Character:                
                /*path += "/Resources/Json/Character/" + itemID + ".Json";
                JsonUtility.FromJson<Character>(File.ReadAllText(path));*/
                break;
            case FileType.Consumable:
                Item.AddComponent<ConsumableManager>();
                path += "/Resources/Json/Consumable/" + itemID + ".Json";
                Item.GetComponent<ConsumableManager>().SetStats(JsonUtility.FromJson<Consumables>(File.ReadAllText(path)));
                break;
            case FileType.Weapon:
                Item.AddComponent<WeaponManager>();
                path += "/Resources/Json/Weapon/" + itemID + ".Json";
                Item.GetComponent<WeaponManager>().SetStats(JsonUtility.FromJson<Weapon>(File.ReadAllText(path)));
                break;
            default:                
                path += "/Resources/Json/Corrupted/failure.Json";
                break;
        }



        return Item;
    }


}
