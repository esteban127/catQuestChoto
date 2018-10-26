using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {

    [SerializeField] string[] dialogueTree;
    Dictionary<string, int> dictionary;
    [SerializeField] string[] dKey;
    private void Start()
    {
        dictionary = new Dictionary<string, int>(dKey.Length);
        for (int i = 0; i < dKey.Length; i++)
        {
            dictionary.Add(dKey[i], i);
        }
    }

    public string getDialogue(string key)
    {
        return dialogueTree[dictionary[key]];        
    }

    public bool CheckKey(string key)
    {
        return (dictionary.ContainsKey(key));
    }
}
