using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UJsonCreator))]
public class JsonCreatorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        UJsonCreator myScript = (UJsonCreator)target;
        SerializedObject serializedObj = new SerializedObject(myScript);

        switch (myScript.fType)
        {
            case FileType.HealAbility:
                DrawPropertiesExcluding(serializedObj, new string[] { "jsonCharacter", "jsonConsumable", "jsonWeapon", "jsonArmor", "jsonQItem", "jsonAtAbility" });
                break;
            case FileType.AtackAbility:
                DrawPropertiesExcluding(serializedObj, new string[] { "jsonCharacter", "jsonConsumable", "jsonWeapon", "jsonArmor", "jsonHeAbility", "jsonQItem" });
                break;
            case FileType.QItem:
                DrawPropertiesExcluding(serializedObj, new string[] { "jsonCharacter", "jsonConsumable", "jsonWeapon", "jsonArmor", "jsonHeAbility", "jsonAtAbility" });
                break;
            case FileType.Armor:
                DrawPropertiesExcluding(serializedObj, new string[] { "jsonCharacter", "jsonConsumable", "jsonWeapon", "jsonQItem", "jsonHeAbility", "jsonAtAbility" });
                break;
            case FileType.Character:
                DrawPropertiesExcluding(serializedObj, new string[] { "jsonArmor", "jsonConsumable", "jsonWeapon", "jsonQItem", "jsonHeAbility", "jsonAtAbility" });
                break;
            case FileType.Consumable:
                DrawPropertiesExcluding(serializedObj, new string[]{"jsonArmor", "jsonCharacter", "jsonWeapon", "jsonQItem", "jsonHeAbility", "jsonAtAbility" });
                break;
            case FileType.Weapon:
                DrawPropertiesExcluding(serializedObj, new string[] { "jsonArmor", "jsonCharacter", "jsonConsumable", "jsonQItem", "jsonHeAbility", "jsonAtAbility" });
                break;
            default:
                break;
        }

        if (GUILayout.Button("Create JSON"))
        {
            myScript.CreateJSON();
        }

        GUILayout.Label(myScript.VerifyJSONCreation());
        serializedObj.ApplyModifiedProperties();
    }
}
