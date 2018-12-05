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

    static private PoolManager myPoolManager;
    static private ItemFactory instance = null;
    private ItemFactory()
    {
        myPoolManager = PoolManager.Instance;
        GameObject model = (GameObject)Resources.Load("ItemPrefab/Armor");
        myPoolManager.AddPool(model, 10, "Armor", true);
        model = (GameObject)Resources.Load("ItemPrefab/Weapon");
        myPoolManager.AddPool(model, 5, "Weapon", true);
        model = (GameObject)Resources.Load("ItemPrefab/QuestItem");
        myPoolManager.AddPool(model, 5, "QuestItem", true);
        model = (GameObject)Resources.Load("ItemPrefab/Consumables");
        myPoolManager.AddPool(model, 5, "Consumables", true);
    }
    static public ItemFactory Instance
    {
        get
        {
            if (instance == null|| myPoolManager ==null)
            {
                instance = new ItemFactory();               
            }

            return instance;
        }        
    }

    public void GenerateLoot(ItemTier tier, Vector3 dropSpot)
    {
        Iitem item = Generate(tier);  
        GameObject drop = myPoolManager.RequestToPool(item.GetType().ToString(), dropSpot, Quaternion.Euler(0, 0, 0));
        drop.GetComponent<ItemComponent>().SetStats(item);
        drop.GetComponent<dropPhysics>().Spawn();
        drop.name = drop.GetComponent<ItemComponent>().GetName();
    }

    public void GenerateItem(ItemTier tier, ItemType type, string ID, Vector3 dropSpot)
    {
        Iitem item = Generate(type,tier,ID);
        GameObject drop = myPoolManager.RequestToPool(item.GetType().ToString(), dropSpot, Quaternion.Euler(0, 0, 0));
        drop.GetComponent<ItemComponent>().SetStats(item);
        drop.GetComponent<dropPhysics>().Spawn();
        drop.name = drop.GetComponent<ItemComponent>().GetName();
    }

    public void DropItem(Iitem item, Vector3 dropSpot)
    {
        GameObject drop = myPoolManager.RequestToPool(item.GetType().ToString(), dropSpot, Quaternion.Euler(0, 0, 0));
        drop.GetComponent<ItemComponent>().SetStats(item);
        drop.GetComponent<dropPhysics>().Spawn();
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
        if(tier == ItemTier.Tier3)
        {
            roll = 2; // there are only weapons tier3
        }

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
