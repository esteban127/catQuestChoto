using UnityEngine;
using System.Collections.Generic;
using System.IO;

public enum FileType
{
    Character,
    Consumable,
    Armor,
    Weapon,
    QItem,
    AtackAbility,
    HealAbility
}

public class UJsonCreator : MonoBehaviour
{
    //aca van las clases de los json, como y que queres que muestre, estas mismas usarias para el json utility
    //NO TIENEN QUE ESTAR ACA, SI LAS TENES EN OTRO LADO ESTA BIEN

   
    //los enum de cada tipo
   
    public FileType fType;

    //tenes un objeto de cada tipo, para que se muestre
    public Weapon jsonWeapon;
    public AtackAbility jsonAtAbility;
    public HealAbility jsonHeAbility;
    public QuestItem jsonQItem;
    public Armor jsonArmor;
    public Consumables jsonConsumable;
    public CharacterActor jsonCharacter;

    public string VerifyJSONCreation()
    {
        return p_message;
    }

    string p_message;
    public void CreateJSON()
    {
        string path = Application.dataPath;
        string jsonString;
        switch (fType)
        {
            case FileType.HealAbility:
                jsonString = JsonUtility.ToJson(jsonHeAbility);
                p_message = "Heal ability creation Ok";
                path += "/Resources/Json/Ability/" + jsonHeAbility.AbilityClass + "/HealAbility/" + jsonHeAbility.Name + ".Json";

                break;
            case FileType.AtackAbility:
                jsonString = JsonUtility.ToJson(jsonAtAbility);
                p_message = "Atack ability creation Ok";
                path += "/Resources/Json/Ability/"+jsonAtAbility.AbilityClass+"/AtackAbility/" + jsonAtAbility.Name + ".Json";
                break;
            case FileType.QItem:
                jsonString = JsonUtility.ToJson(jsonQItem);
                p_message = "Quest Item creation Ok";
                path += "/Resources/Json/Items/" + jsonQItem.Tier + "/QuestItem/" + jsonQItem.ID + ".Json";
                break;
            case FileType.Armor:
                jsonString=JsonUtility.ToJson(jsonArmor);                
                p_message = "Armor Creation OK";
                path += "/Resources/Json/Items/"+ jsonArmor.Tier +"/Armor/" + jsonArmor.ID + ".Json";
                break;
            case FileType.Character:
                jsonString = JsonUtility.ToJson(jsonCharacter);
                p_message = "Character Creation OK";
                path += "/Resources/Json/Character/" + jsonCharacter.Name + ".Json";
                break;
            case FileType.Consumable:
                jsonString = JsonUtility.ToJson(jsonConsumable);
                p_message = "Consumable Creation OK";
                path += "/Resources/Json/Items/" + jsonConsumable.Tier + "/Consumable/" + jsonConsumable.ID + ".Json";
                break;
            case FileType.Weapon:
                jsonString = JsonUtility.ToJson(jsonWeapon);
                p_message = "Weapon Creation OK";
                path += "/Resources/Json/Items/" + jsonWeapon.Tier + "/Weapon/" + jsonWeapon.ID + ".Json";
                break;
            default:
                p_message = "Se pudrio todo";
                jsonString = "Fatal Error";
                path += "/Resources/Json/Corrupted/failure.Json";
                break;
        }
        //aca va el FILE IO        
        if (!File.Exists(path))
            File.WriteAllText(path, jsonString);
        //si hay exception cambias el pmessage a todo mal
        //si todo sale bien dejas el siguiente pmessage
       
    }
}