using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum c_class
{
    warrior,
    archer,
    mage
}


[System.Serializable]
public class CharacterActor : FriendlyActor {
    

    [SerializeField] c_class characterClass;
    public c_class Class { get { return characterClass; } }
    [SerializeField] float experience;
    public float Experience { get { return experience; } set { experience = value; } }
    //[SerializeField] skilltree skilltree?
    
}
