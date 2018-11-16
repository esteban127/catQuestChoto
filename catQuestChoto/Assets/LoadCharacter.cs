using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class LoadCharacter : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] string characterName;

    private void Awake()
    {
        Load();
    }

    private void Load()
    {
        
        string path = Application.dataPath + "/Resources/Json/Character/" + characterName + ".Json";
        CharacterActor stats = JsonUtility.FromJson<CharacterActor>(File.ReadAllText(path));
        UnityEngine.Object model = Resources.Load("CharacterPrefab/" + stats.Class);
        Instantiate(model, player.transform);
        player.GetComponent<CharacterStats>().loadCharacter(stats);
    }
}
