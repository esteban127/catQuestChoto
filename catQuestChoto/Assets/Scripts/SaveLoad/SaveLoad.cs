using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;


public class SaveLoad : MonoBehaviour {

    string currentDirectory;
    public string SaveDirectory { get { return currentDirectory; } }
    CharacterActor currentPlayer;
    public c_class currentClass { get { return currentPlayer.Class; } }
    static private SaveLoad instance = null;
    static public SaveLoad Instance { get { return instance; } }
    public delegate void SaveDelegate();
    public static event SaveDelegate BeforeClosing;
    private void Awake()
    {        
        if (instance == null)
        {
            instance = this;
            CheckSaveFolder();
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }        
    }

    private void CheckSaveFolder()
    {
        string path = Application.dataPath;
        path += ("/Resources/Saves");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path); 
        }        
    }

    private bool newDirectory(string directoryName)
    {
        string path = Application.dataPath;
        path += ("/Resources/Saves/" + directoryName);
        if (!Directory.Exists(path))
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
        currentPlayer = JsonUtility.FromJson<CharacterActor>(File.ReadAllText(path));
        currentPlayer.Name = characterName;
        if (newDirectory(characterName))
        {
            path = currentDirectory + "/Stats.Json";
            string playerSave = JsonUtility.ToJson(currentPlayer);            
            File.WriteAllText(path, playerSave);
            StatGame();
        }
    }
    public void DeleteSave(string characterName)
    {
        string path = Application.dataPath;
        path += ("/Resources/Saves/" + characterName);        
        Directory.Delete(path,true);
    }
    public void LoadPlayer(string characterName)
    {
        if (LoadDirectory(characterName))
        {
            currentPlayer = JsonUtility.FromJson<CharacterActor>(File.ReadAllText(currentDirectory + "Stats.Json"));
            StatGame();
        }
    }  
    public bool LoadDirectory(string DirectorynName)
    {
        string path = Application.dataPath;
        path+=("/Resources/Saves/" + DirectorynName);
        if (Directory.Exists(path))
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
    private void StatGame()
    {
        SceneManager.LoadScene("Town", LoadSceneMode.Single);
    }
    public void ChangeScene(string sceneName)
    {
        BeforeClosing();
        CleanDelegate();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    public void QuitApplication()
    {
        BeforeClosing();
        CleanDelegate();
        Application.Quit();
    }

    private void CleanDelegate()
    {
        Delegate[] functions = BeforeClosing.GetInvocationList();
        for (int i = 0; i < functions.Length; i++)
        {
            BeforeClosing -= (SaveDelegate)functions[i];
        }
    }
}
