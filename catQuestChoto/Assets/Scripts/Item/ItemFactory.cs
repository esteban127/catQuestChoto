using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum ItemType
{
    Weapon,
    Armor,
    Consumable,
    QuestItem
}

public class ItemFactory{    


    static private ItemFactory instance = null;
    private ItemFactory() { }
    static public ItemFactory Instance
    {get
        {
            if (instance == null)
                instance = new ItemFactory();

            return instance;
        }        
    }

    public void GenerateLoot(ItemTier tier, Vector3 dropSpot)
    {
        Iitem item = Generate(tier);
        Object model = Resources.Load("ItemPrefab/" + item.GetType());
        GameObject drop = (GameObject)GameObject.Instantiate(model, dropSpot, Quaternion.Euler(0, 0, 0));
        drop.GetComponent<ItemComponent>().SetStats(item);
        drop.name = drop.GetComponent<ItemComponent>().GetName();
    }

    public void GenerateItem(ItemTier tier, ItemType type, string ID, Vector3 dropSpot)
    {
        Iitem item = Generate(type,tier,ID);
        Object model = Resources.Load("ItemPrefab/" + item.GetType());
        GameObject drop = (GameObject)GameObject.Instantiate(model, dropSpot, Quaternion.Euler(0, 0, 0));
        drop.GetComponent<ItemComponent>().SetStats(item);
        drop.name = drop.GetComponent<ItemComponent>().GetName();
    }

    public void DropItem(Iitem item, Vector3 dropSpot)
    {
        Object model = Resources.Load("ItemPrefab/" + item.GetType());
        GameObject drop = (GameObject)GameObject.Instantiate(model, dropSpot, Quaternion.Euler(0,0,0));
        drop.GetComponent<ItemComponent>().SetStats(item);
        drop.name = drop.GetComponent<ItemComponent>().GetName();
    }

    private Iitem Generate(ItemType type, ItemTier tier, string itemID)
    {
        
        string path = Application.dataPath +"/Resources/Json/Items/" + tier;
        switch (type)
        {
            case ItemType.Armor:                
                path += "/Armor/" + itemID + ".Json";
                Armor aStats = JsonUtility.FromJson<Armor>(File.ReadAllText(path));
                aStats.SetRandomProperty();
                return aStats;                
           
            case ItemType.Consumable:
                
                path += "/Consumable/" + itemID + ".Json";
                return (JsonUtility.FromJson<Consumables>(File.ReadAllText(path)));
                
            case ItemType.Weapon:
                
                path += "/Weapon/" + itemID + ".Json";
                Weapon wStats = JsonUtility.FromJson<Weapon>(File.ReadAllText(path));
                wStats.SetRandomProperty();
                return wStats;
            case ItemType.QuestItem:

                path += "/QuestItem/" + itemID + ".Json";
                return (JsonUtility.FromJson<QuestItem>(File.ReadAllText(path)));

            default:
                Debug.Log("Missing Type");
                return null;                
        }

        

         
    }

    private Iitem Generate(ItemTier tier)
    {
        
        string path = Application.dataPath + "/Resources/Json/Items/" + tier; 
        string[] fileArray;
        int roll = Random.Range(0, 3);
        switch (roll)
        {
            case 0:
                path += "/Consumable/";
                fileArray = Directory.GetFiles(path, "*.Json");
                return (JsonUtility.FromJson<Consumables>(File.ReadAllText(fileArray[Random.Range(0,fileArray.Length)])));
                
            case 1:
                path += "/Armor/";
                fileArray = Directory.GetFiles(path, "*.Json");                
                Armor aStats = JsonUtility.FromJson<Armor>(File.ReadAllText(fileArray[Random.Range(0, fileArray.Length)]));
                aStats.SetRandomProperty();
                return aStats;
                
            case 2:
                path += "/Weapon/";
                fileArray = Directory.GetFiles(path, "*.Json");                
                Weapon wStats = JsonUtility.FromJson<Weapon>(File.ReadAllText(fileArray[Random.Range(0, fileArray.Length)]));
                wStats.SetRandomProperty();
                return wStats;

            default:
                Debug.LogError("Fail with the roll?");
                return null;

        }


        
    }


}
