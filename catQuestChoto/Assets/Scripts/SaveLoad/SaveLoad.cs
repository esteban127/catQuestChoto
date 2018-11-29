using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoad : MonoBehaviour {

    string currentDirectory = "C:/Users/Core-i5/Desktop/facu/Practica profecional 2/CatQuestChoto/catQuestChoto/Assets/Resources/Saves/Test/"; //test
    public string SaveDirectory { get { return currentDirectory; } }

    static private SaveLoad instance = null;
    static public SaveLoad Instance { get { return instance; } }

    private void Awake()
    {        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);
    }
    
    private bool newDirectory(string name)
    {
        string path = Application.dataPath;
        path += ("/Resources/Saves/" + name );
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(path);
            currentDirectory = path + "/";
            return true;
        }
        return false;           
    }
    public void NewCharacter(string characterName, c_class cClass)
    {
        string path = Application.dataPath;
        path += ("/Resources/Json/Character/" + cClass +".Json");
        CharacterActor player = JsonUtility.FromJson<CharacterActor>(File.ReadAllText(path));
        player.Name = characterName;
        if (newDirectory(characterName))
        {
            path = currentDirectory + "/Stats.Json";
            string playerSave = JsonUtility.ToJson(player);
            File.WriteAllText(path, playerSave);
        }
    }
    public void DeleteSave(string characterName)
    {
        string path = Application.dataPath;
        path += ("/Resources/Saves/" + characterName);
        File.Delete(path);
        Directory.Delete(path);
    }
    public bool LoadDirectory(string name)
    {
        string path = Application.dataPath;
        path += ("/Resources/Saves/" + name);
        if (File.Exists(path))
        {
            currentDirectory = path + "/";
            return true;
        }
        return false;
    }
    public CharacterActor[] getAllCharacters()
    {
        string path = Application.dataPath;
        path += ("/Resources/Saves/");
        string[] directions = Directory.GetDirectories(path);
        CharacterActor[] characters = new CharacterActor[directions.Length];
        for (int i = 0; i < directions.Length; i++)
        {
            characters[i] = JsonUtility.FromJson<CharacterActor>(File.ReadAllText((directions[i])+"/Stats.Json"));
        }
        return characters;
    }

}
